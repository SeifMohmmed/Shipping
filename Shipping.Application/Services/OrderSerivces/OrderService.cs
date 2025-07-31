using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstraction.OrderReport.DTO;
using Shipping.Application.Abstraction.Orders.DTO;
using Shipping.Application.Abstraction.Orders.Service;
using Shipping.Domain.Entities;
using Shipping.Domain.Enums;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.OrderSerivces;
public class OrderService(IUnitOfWork _unitOfWork,
    IMapper _mapper,
    UserManager<ApplicationUser> _userManager,
    IHttpContextAccessor _httpContextAccessor) : IOrderService
{
    // Get all orders
    public async Task<IEnumerable<OrderWithProductsDTO>> GetOrdersAsync(PaginationParameters pramter)
    {
        var orders =
            await _unitOfWork.GetOrderRepository().GetAllAsync(pramter, p => p
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
        var findOrder = await _unitOfWork.GetOrderRepository().GetByIdAsync(id,
            include: p => p
            .Include(o => o.Branch)
            .Include(o => o.Region)
            .Include(o => o.CitySetting)
            .Include(o => o.Products));

        if (findOrder is null || findOrder.IsDeleted)
            return null;

        var orderDTO = _mapper.Map<OrderWithProductsDTO>(findOrder);

        var merchantName = await _userManager.FindByIdAsync(orderDTO.MerchantName);
        orderDTO.MerchantName = merchantName!.FullName;

        return orderDTO;
    }


    //  Get all waiting orders
    public async Task<IEnumerable<OrderWithProductsDTO>> GetAllWatingOrder(PaginationParameters pramter)
    {
        var orders = await _unitOfWork.GetOrderRepository().GetAllWatingOrder(pramter);

        var waitngOrderDTO = await GetMerchantName(orders);

        return waitngOrderDTO;
    }


    // Get all orders by status
    public async Task<IEnumerable<OrderWithProductsDTO>> GetOrdersByStatus(OrderStatus status, PaginationParameters pramter)
    {
        var orders = await _unitOfWork.GetOrderRepository().GetOrdersByStatus(status, pramter);

        var orderDTO = await GetMerchantName(orders);

        return orderDTO;
    }


    // Add new order And Calculate Shipping Cost And Create Order Report
    public async Task<OrderWithProductsDTO> AddAsync(AddOrderDTO DTO)
    {
        DTO.ShippingCost = await CalculateShippingCost(DTO);

        var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);

        if (currentUser is not null && await _userManager.IsInRoleAsync(currentUser, DefaultRole.Merchant))
        {
            DTO.status = OrderStatus.WaitingForConfirmation;
            DTO.MerchantName = currentUser.Id;

        }

        else
        {
            DTO.status = OrderStatus.Pending;

        }

        var orderEntity = _mapper.Map<Order>(DTO);
        await _unitOfWork.GetOrderRepository().AddAsync(orderEntity);
        await _unitOfWork.CompleteAsync(); // Save the Order to get its Id


        //Create the order report When Add New order
        var orderReportDto = new OrderReportDTO
        {
            OrderId = orderEntity.Id,
            ReportDate = DateTime.UtcNow,
            //ReportDetails = $"New order (ID: {orderEntity.Id}) was created by merchant '{currentUser.UserName}' for customer '{DTO.CustomerName}' with status '{DTO.status}' on {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC."
        };

        await _unitOfWork.GetOrderReportRepository().AddAsync(_mapper.Map<OrderReport>(orderReportDto));
        await _unitOfWork.CompleteAsync(); // Save the OrderReport

        return _mapper.Map<OrderWithProductsDTO>(orderEntity);

    }


    //Update Order
    public async Task UpdateAsync(int id, UpdateOrderDTO DTO)
    {
        if (DTO.Id != 0 && DTO.Id != id)
        {
            throw new ArgumentException("The Id in the DTO does not match the provided Id.");
        }

        var orderRepo = _unitOfWork.GetOrderRepository();

        var existingOrder = await orderRepo.GetByIdAsync(id,
            include: p => p
            .Include(o => o.Branch)
            .Include(o => o.Region)
            .Include(o => o.CitySetting)
            .Include(o => o.Products));

        if (existingOrder is null)
            throw new KeyNotFoundException($"Order with ID {id} not found.");

        _mapper.Map(DTO, existingOrder);


        orderRepo.UpdateAsync(existingOrder);
        await _unitOfWork.CompleteAsync();
    }


    //Delete Order
    public async Task DeleteAsync(int id)
    {
        var orderRepo = _unitOfWork.GetOrderRepository();

        var existingOrder = await orderRepo.GetByIdAsync(id);

        if (existingOrder is null)
            throw new KeyNotFoundException($"Order with ID {id} not found.");

        await orderRepo.DeleteAsync(id);
        await _unitOfWork.CompleteAsync();
    }


    // Assign order to courier
    public async Task AssignOrderToCourier(int OrderId, string courierId)
    {
        if (string.IsNullOrEmpty(courierId))
            throw new ArgumentException("CourierId cannot be null or empty.");

        var order = await _unitOfWork.GetOrderRepository().GetByIdAsync(OrderId);

        if (order is null)
            throw new KeyNotFoundException("Order not found.");

        order!.CourierId = courierId;

        var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);

        order!.EmployeeId = currentUser!.Id;
        order.Status = OrderStatus.DeliveredToCourier;

        _unitOfWork.GetOrderRepository().UpdateAsync(order);

        await _unitOfWork.CompleteAsync();

    }


    // Change order status
    public async Task ChangeOrderStatus(int id, OrderStatus orderStatus)
    {
        var order = await _unitOfWork.GetOrderRepository().GetByIdAsync(id);

        order!.Status = orderStatus;

        _unitOfWork.GetOrderRepository().UpdateAsync(order);

        await _unitOfWork.CompleteAsync();
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
        var ordersDTO = _mapper.Map<IEnumerable<OrderWithProductsDTO>>(orders);
        foreach (var order in ordersDTO)
        {
            var MerchantName = await _userManager.FindByIdAsync(order.MerchantName);
            order.MerchantName = MerchantName?.FullName ?? "اسم التاجر غير متاح";
        }
        return ordersDTO;
    }

    //Method To Calculate Shipping Cost
    private async Task<decimal> CalculateShippingCost(AddOrderDTO DTO)
    {
        decimal shippingCost = 0;
        decimal totalWeight = DTO.TotalWeight;
        bool isOutOfCity = DTO.IsOutOfCityShipping;

        var city = await _unitOfWork.GetRepository<CitySetting, int>().GetByIdAsync(DTO.City)
            ?? throw new KeyNotFoundException($"City with ID {DTO.City} not found.");

        var weightSetting = (await _unitOfWork.GetWeightSettingRepository().GetAllWeightSetting()).FirstOrDefault()
            ?? throw new Exception("Weight settings not configured.");

        decimal maxWeight = weightSetting.MaxWeight;
        decimal costPerKg = weightSetting.CostPerKg;

        var specialCityCost = await _unitOfWork.GetSpecialCityCostRepository()
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

        var shippingType = await _unitOfWork.GetRepository<ShippingType, int>().GetByIdAsync(DTO.ShippingId)
            ?? throw new KeyNotFoundException($"ShippingType with ID {DTO.ShippingId} not found.");

        shippingCost += shippingType.BaseCost;

        return shippingCost;
    }
}