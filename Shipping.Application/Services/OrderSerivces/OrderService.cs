using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    //--------------------------------------------------------------------------


    // Get all orders
    public async Task<IEnumerable<OrderWithProductsDTO>> GetOrdersAsync(PaginationParameters pramter)
    {
        var orders =
            await _unitOfWork.GetOrderRepository().GetAllAsync(pramter);

        var ordersDTO = await GetMerchantName(orders);

        return ordersDTO;
    }


    // Get order by id
    public async Task<OrderWithProductsDTO> GetOrderAsync(int id)
    {
        var findOrder = await _unitOfWork.GetOrderRepository().GetByIdAsync(id);

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
    public async Task AddAsync(AddOrderDTO DTO)
    {
        decimal shippingCost = 0;
        decimal orderCost = DTO.OrderCost;
        decimal totalWeight = DTO.TotalWeight;

        var IsOutOfCityShipping = DTO.IsOutOfCityShipping;

        var city = await _unitOfWork.GetRepository<CitySetting, int>().GetByIdAsync(DTO.Id);
        var pickUpShippingCost = city!.PickUpShippingCost;

        decimal standardShippingCost = city!.StandardShippingCost;

        var AllweightSetting = await _unitOfWork.GetWeightSettingRepository().GetAllWeightSetting();
        var weightSetting = AllweightSetting.FirstOrDefault();
        decimal maxWeight = weightSetting!.MaxWeight;
        decimal costPerKG = weightSetting!.CostPerKg;


        var SpecialCityCost = await _unitOfWork.GetSpecialCityCostRepository()
            .GetCityCostByMarchantId(DTO.MerchantName, DTO.City);

        if (SpecialCityCost is not null)
        {
            if (totalWeight > 0 && totalWeight <= maxWeight)
            {
                if (IsOutOfCityShipping == true)
                    shippingCost += SpecialCityCost.Price * 1.1m;
                shippingCost += SpecialCityCost.Price;
            }
            else if (totalWeight > maxWeight)
            {
                decimal ExcessWeight = totalWeight - maxWeight;
                if (IsOutOfCityShipping == true)
                    shippingCost += SpecialCityCost.Price * 1.1m + (ExcessWeight * costPerKG);
                shippingCost += SpecialCityCost.Price + (ExcessWeight * costPerKG);
            }
        }
        else
        {
            if (totalWeight > 0 && totalWeight <= maxWeight)
            {
                if (IsOutOfCityShipping)
                    shippingCost += standardShippingCost * 1.1m;
                shippingCost += standardShippingCost;
            }
            else if (totalWeight > maxWeight)
            {
                decimal ExcessWeight = totalWeight - maxWeight;
                if (IsOutOfCityShipping)
                    shippingCost += standardShippingCost * 1.1m + (ExcessWeight * costPerKG);
                shippingCost += standardShippingCost + (ExcessWeight * costPerKG);
            }
        }

        var ShippingType = await _unitOfWork.GetRepository<ShippingType, int>().GetByIdAsync(DTO.ShippingId);

        if (ShippingType is not null)
        {
            shippingCost += ShippingType.BaseCost;
        }

        DTO.ShippingCost = shippingCost;
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
        await _unitOfWork.CompleteAsync();

        #region TODO : Adding Order Report
        // Retrieve the saved order to get the correct ID
        //var savedOrder = await _unitOfWork.GetOrderRepository().GetByIdAsync(orderEntity.Id);

        // Create the order report When Add New order
        //var orderReportDto = new OrderReportDTO
        //{
        //    OrderId = savedOrder.Id,
        //    ReportDate = DateTime.UtcNow
        //};

        //await _unitOfWork.GetOrderReportRepository().AddAsync(_mapper.Map<OrderReport>(orderReportDto));
        //await _unitOfWork.CompleteAsync();
        #endregion

    }


    //Update Order
    public async Task UpdateAsync(UpdateOrderDTO DTO)
    {
        var orderRepo = _unitOfWork.GetOrderRepository();

        var existingOrder = await orderRepo.GetByIdAsync(DTO.Id);

        if (existingOrder is null)
            throw new KeyNotFoundException($"Order with ID {DTO.Id} not found.");

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


}
