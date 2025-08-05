using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shipping.Application.Abstraction.OrderReport.DTO;
using Shipping.Application.Abstraction.Orders.DTO;
using Shipping.Application.Abstraction.Orders.Service;
using Shipping.Domain.Constants;
using Shipping.Domain.Entities;
using Shipping.Domain.Enums;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.OrderSerivces;
public class OrderService(ILogger<OrderService> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    UserManager<ApplicationUser> userManager,
    IHttpContextAccessor httpContextAccessor) : IOrderService
{
    // Get all orders
    public async Task<IEnumerable<OrderWithProductsDTO>> GetOrdersAsync(PaginationParameters pramter)
    {
        logger.LogInformation("Fetching orders with pagination parameters: PageNumber {PageNumber}, PageSize {PageSize}", pramter.PageNumber, pramter.PageSize);

        var orders =
            await unitOfWork.GetOrderRepository().GetAllAsync(pramter, p => p
            .Include(o => o.Branch)
            .Include(o => o.Region)
            .Include(o => o.CitySetting)
            .Include(o => o.Products));

        var ordersDTO = await GetMerchantName(orders);

        return ordersDTO;
    }


    // Get order by id
    public async Task<OrderWithProductsDTO> GetOrderAsync(int id)
    {
        logger.LogInformation("Fetching order with ID {OrderId}", id);

        var findOrder = await unitOfWork.GetOrderRepository().GetByIdAsync(id,
            include: p => p
            .Include(o => o.Branch)
            .Include(o => o.Region)
            .Include(o => o.CitySetting)
            .Include(o => o.Products));

        if (findOrder is null)
            throw new NotFoundException(nameof(Order), id.ToString());

        var ordersDTO = await GetMerchantName(new[] { findOrder });

        return ordersDTO.First();
    }


    //  Get all waiting orders
    public async Task<IEnumerable<OrderWithProductsDTO>> GetAllWatingOrder(PaginationParameters pramter)
    {
        logger.LogInformation("Fetching waiting orders with pagination parameters: PageNumber {PageNumber}, PageSize {PageSize}", pramter.PageNumber, pramter.PageSize);

        var orders = await unitOfWork.GetOrderRepository().GetAllWatingOrder(pramter);

        var waitngOrderDTO = await GetMerchantName(orders);

        return waitngOrderDTO;
    }


    // Get all orders by status
    public async Task<IEnumerable<OrderWithProductsDTO>> GetOrdersByStatus(OrderStatus status, PaginationParameters pramter)
    {
        logger.LogInformation("Fetching orders with status {Status} and pagination: PageNumber {PageNumber}, PageSize {PageSize}", status, pramter.PageNumber, pramter.PageSize);

        var orders = await unitOfWork.GetOrderRepository().GetOrdersByStatus(status, pramter);

        if (orders is null)
            throw new NotFoundException(nameof(Order), status.ToString());

        var orderDTO = await GetMerchantName(orders);

        return orderDTO;
    }


    // Add new order And Calculate Shipping Cost And Create Order Report
    public async Task<OrderWithProductsDTO> AddAsync(AddOrderDTO DTO)
    {
        logger.LogInformation("Adding new order for customer {CustomerName}", DTO.CustomerName);

        if (string.IsNullOrEmpty(DTO.CustomerName))
        {
            throw new ArgumentException("CustomerName is required");
        }


        using var transaction = await unitOfWork.BeginTransactionAsync();

        try
        {

            var currentUser = await userManager.GetUserAsync(httpContextAccessor.HttpContext!.User);

            var isMerchant = currentUser is not null
                && await userManager.IsInRoleAsync(currentUser, UserRole.Merchant);


            if (currentUser is not null)
            {
                logger.LogDebug("Current user is {UserName}, ID: {UserId}", currentUser.UserName, currentUser.Id);
            }
            else
            {
                logger.LogWarning("No authenticated user found when creating order.");
            }

            var shippingCost = await CalculateShippingCost(DTO);


            var order = mapper.Map<Order>(DTO);

            order.ShippingCost = shippingCost;
            order.Status = isMerchant ? OrderStatus.WaitingForConfirmation : OrderStatus.Pending;
            order.MerchantId = isMerchant ? currentUser!.Id : null;

            await unitOfWork.GetOrderRepository().AddAsync(order);
            await unitOfWork.CompleteAsync(); // Save the Order to get its Id

            logger.LogInformation("Order ID {OrderId} created successfully", order.Id);


            //Create the order report When Add New order
            var orderReportDto = new OrderReportDTO
            {
                OrderId = order.Id,
                ReportDate = DateTime.UtcNow,
                ReportDetails = $"New order (ID: {order.Id}) was created by" +
                $" merchant '{currentUser.UserName}' for customer '{DTO.CustomerName}' with status '{DTO.status}' " +
                $"on {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC."
            };

            await unitOfWork.GetOrderReportRepository().AddAsync(mapper.Map<OrderReport>(orderReportDto));
            await unitOfWork.CompleteAsync(); // Save the OrderReport

            await transaction.CommitAsync();

            logger.LogInformation("Order report created for order ID {OrderId}", order.Id);


            return mapper.Map<OrderWithProductsDTO>(order);
        }



        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "Failed to add order for customer {CustomerName}", DTO.CustomerName);
            throw;
        }
    }


    //Update Order
    public async Task UpdateAsync(int id, UpdateOrderDTO DTO)
    {
        logger.LogInformation("Updating order with ID {OrderId}", id);

        IOrderRepository? orderRepo = unitOfWork.GetOrderRepository();

        var existingOrder = await orderRepo.GetByIdAsync(id,
            include: p => p
            .Include(o => o.Branch)
            .Include(o => o.Region)
            .Include(o => o.CitySetting)
            .Include(o => o.Products));

        if (existingOrder is null)
            throw new NotFoundException(nameof(Order), id.ToString());

        mapper.Map(DTO, existingOrder);


        orderRepo.UpdateAsync(existingOrder);
        await unitOfWork.CompleteAsync();
    }


    //Delete Order
    public async Task DeleteAsync(int id)
    {
        logger.LogInformation("Attempting to delete order with ID {OrderId}", id);

        var orderRepo = unitOfWork.GetOrderRepository();

        var existingOrder = await orderRepo.GetByIdAsync(id);

        if (existingOrder is null)
            throw new NotFoundException(nameof(Order), id.ToString());

        await orderRepo.DeleteAsync(id);
        await unitOfWork.CompleteAsync();
    }


    // Assign order to courier
    public async Task AssignOrderToCourier(int OrderId, string courierId)
    {
        logger.LogInformation("Assigning order {OrderId} to courier {CourierId}", OrderId, courierId);

        if (string.IsNullOrEmpty(courierId))
            throw new ArgumentException("CourierId cannot be null or empty.");

        var order = await unitOfWork.GetOrderRepository().GetByIdAsync(OrderId);

        if (order is null)
            throw new NotFoundException(nameof(Order), OrderId.ToString());

        order!.CourierId = courierId;

        var currentUser = await userManager.GetUserAsync(httpContextAccessor.HttpContext!.User);

        order!.EmployeeId = currentUser!.Id;
        order.Status = OrderStatus.DeliveredToCourier;

        unitOfWork.GetOrderRepository().UpdateAsync(order);

        await unitOfWork.CompleteAsync();

    }


    // Change order status
    public async Task ChangeOrderStatus(int id, OrderStatus orderStatus)
    {
        logger.LogInformation("Changing status of order {OrderId} to {Status}", id, orderStatus);

        var order = await unitOfWork.GetOrderRepository().GetByIdAsync(id);

        order!.Status = orderStatus;

        unitOfWork.GetOrderRepository().UpdateAsync(order);

        await unitOfWork.CompleteAsync();
    }


    // Change order status to Declined
    public async Task ChangeOrderStatusToDeclined(int id)
    {
        await ChangeOrderStatus(id, OrderStatus.Declined);
    }


    // Change order status to pending
    public async Task ChangeOrderStatusToPending(int id)
    {
        await ChangeOrderStatus(id, OrderStatus.Pending);
    }

    //=============================================================================================================
    // Method to get the merchant name for each order
    private async Task<IEnumerable<OrderWithProductsDTO>> GetMerchantName(IEnumerable<Order> orders)
    {
        logger.LogInformation("Fetching merchant names for {OrderCount} orders", orders.Count());

        var ordersDTO = mapper.Map<IEnumerable<OrderWithProductsDTO>>(orders);

        var merchantIds = ordersDTO.Select(o => o.MerchantName).Distinct().ToList();

        var merchants = await userManager.Users
        .Where(u => merchantIds.Contains(u.Id))
        .ToDictionaryAsync(u => u.Id, u => u.FullName);

        foreach (var order in ordersDTO)
        {
            order.MerchantName = merchants.TryGetValue(order.MerchantName, out var fullName)
                        ? fullName
                        : "اسم التاجر غير متاح";
        }

        return ordersDTO;
    }

    //Method To Calculate Shipping Cost
    private async Task<decimal> CalculateShippingCost(AddOrderDTO DTO)
    {
        decimal shippingCost = 0;
        decimal totalWeight = DTO.TotalWeight;
        bool isOutOfCity = DTO.IsOutOfCityShipping;

        var city = await unitOfWork.GetRepository<CitySetting, int>().GetByIdAsync(DTO.City)
            ?? throw new NotFoundException(nameof(CitySetting), DTO.City.ToString());

        var weightSetting = (await unitOfWork.GetWeightSettingRepository().GetAllWeightSetting()).FirstOrDefault()
            ?? throw new Exception("Weight settings not configured.");

        decimal maxWeight = weightSetting.MaxWeight;
        decimal costPerKg = weightSetting.CostPerKg;

        var specialCityCost = await unitOfWork.GetSpecialCityCostRepository()
            .GetCityCostByMarchantId(DTO.MerchantName, DTO.City);

        decimal baseCost = specialCityCost?.Price ?? city.StandardShippingCost;

        if (totalWeight <= maxWeight)
        {
            shippingCost += baseCost;
            if (isOutOfCity)
                shippingCost += baseCost * 0.1m; // 10% out-of-city fee
        }
        else
        {
            decimal excessWeight = totalWeight - maxWeight;
            shippingCost += baseCost + (excessWeight * costPerKg);
            if (isOutOfCity)
                shippingCost += baseCost * 0.1m;
        }

        var shippingType = await unitOfWork.GetRepository<ShippingType, int>().GetByIdAsync(DTO.ShippingId)
            ?? throw new NotFoundException(nameof(ShippingType), DTO.City.ToString());

        shippingCost += shippingType.BaseCost;

        return shippingCost;
    }
}