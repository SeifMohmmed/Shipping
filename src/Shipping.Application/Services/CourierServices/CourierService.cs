using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shipping.Application.Abstraction.Courier;
using Shipping.Application.Abstraction.Courier.DTO;
using Shipping.Domain.Constants;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.CourierServices;
internal class CourierService(ILogger<CourierService> logger,
    IUnitOfWork unitOfWork,
    UserManager<ApplicationUser> userManager,
    IMapper mapper) : ICourierService
{
    //Get All Couriers
    public async Task<IEnumerable<CourierDTO>> GetAllAsync(PaginationParameters pramter)
    {
        logger.LogInformation("Retrieving all couriers with pagination: {@Pagination}", pramter);

        var couriers =
                await unitOfWork.GetRepository<ApplicationUser, string>().GetAllAsync(pramter);

        return mapper.Map<IEnumerable<CourierDTO>>(couriers);
    }

    //Get Courier By Branch
    public async Task<IEnumerable<CourierDTO>> GetCourierByBranch(int orderId)
    {
        logger.LogInformation("Fetching couriers for OrderId {OrderId} by branch", orderId);

        var order = await unitOfWork.GetOrderRepository().GetByIdAsync(orderId);

        if (order is null)
            throw new NotFoundException(nameof(Order), orderId.ToString());

        var Couriers = await userManager.GetUsersInRoleAsync(UserRole.Courier);

        var couriersInBranch = Couriers.Where(c => c.BranchId == order!.BranchId);

        var courierDTO = mapper.Map<IEnumerable<CourierDTO>>(couriersInBranch);

        return courierDTO;
    }

    //Get Courier By Region
    public async Task<IEnumerable<CourierDTO>> GetCourierByRegion(int orderId, PaginationParameters pramter)
    {
        logger.LogInformation("Fetching couriers for OrderId {OrderId} by region", orderId);

        var order = await unitOfWork.GetOrderRepository().GetByIdAsync(orderId);

        if (order is null)
            return Enumerable.Empty<CourierDTO>();

        var couriers = await userManager.GetUsersInRoleAsync(UserRole.Courier);

        var couriersInRegion = couriers.Where(c => c.RegionId == order.RegionId).ToList();

        if (couriersInRegion.Count == 0)
        {
            logger.LogInformation("No couriers found in region {RegionId}. Searching in special courier regions.", order.RegionId);

            // No couriers in the order's region; check special regions
            var specialRegions = await unitOfWork.GetRepository<SpecialCourierRegion, int>().GetAllAsync(pramter);
            var relevantSpecialRegions = specialRegions.Where(r => r.RegionId == order.RegionId).ToList();

            // Extract unique courier IDs from relevant special regions
            var specialCourierIds = relevantSpecialRegions.Select(s => s.CourierId).Distinct().ToList();

            // Get couriers associated with the special regions
            var couriersInSpecialRegion = couriers.Where(c => specialCourierIds.Contains(c.Id)).ToList();

            return mapper.Map<IEnumerable<CourierDTO>>(couriersInSpecialRegion);
        }

        logger.LogInformation("Found {Count} couriers in RegionId {RegionId}", couriersInRegion.Count, order.RegionId);

        // Return couriers in the order's region
        return mapper.Map<IEnumerable<CourierDTO>>(couriersInRegion);
    }
}
