using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Shipping.Application.Abstraction.Courier;
using Shipping.Application.Abstraction.Courier.DTO;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.CourierServices;
internal class CourierService(IUnitOfWork unitOfWork,
    UserManager<ApplicationUser> userManager,
    IMapper mapper) : ICourierService
{
    //Get All Couriers
    public async Task<IEnumerable<CourierDTO>> GetAllAsync(PaginationParameters pramter)
    => mapper.Map<IEnumerable<CourierDTO>>(await unitOfWork.GetRepository<ApplicationUser, string>().GetAllAsync(pramter));

    //Get Courier By Branch
    public async Task<IEnumerable<CourierDTO>> GetCourierByBranch(int OrderId)
    {
        var order = await unitOfWork.GetOrderRepository().GetByIdAsync(OrderId);
        var Couriers = await userManager.GetUsersInRoleAsync(DefaultRole.Courier);
        var couriersInBranch = Couriers.Where(c => c.BranchId == order!.BranchId);
        var courierDTO = mapper.Map<IEnumerable<CourierDTO>>(couriersInBranch);

        return courierDTO;
    }

    //Get Courier By Region
    public async Task<IEnumerable<CourierDTO>> GetCourierByRegion(int OrderId, PaginationParameters pramter)
    {
        var order = await unitOfWork.GetOrderRepository().GetByIdAsync(OrderId);

        if (order is null)
            return Enumerable.Empty<CourierDTO>();

        var couriers = await userManager.GetUsersInRoleAsync(DefaultRole.Courier);

        var couriersInRegion = couriers.Where(c => c.RegionId == order.RegionId).ToList();

        if (couriersInRegion.Count == 0)
        {
            // No couriers in the order's region; check special regions
            var specialRegions = await unitOfWork.GetRepository<SpecialCourierRegion, int>().GetAllAsync(pramter);
            var relevantSpecialRegions = specialRegions.Where(r => r.RegionId == order.RegionId).ToList();

            // Extract unique courier IDs from relevant special regions
            var specialCourierIds = relevantSpecialRegions.Select(s => s.CourierId).Distinct().ToList();

            // Get couriers associated with the special regions
            var couriersInSpecialRegion = couriers.Where(c => specialCourierIds.Contains(c.Id)).ToList();

            return mapper.Map<IEnumerable<CourierDTO>>(couriersInSpecialRegion);
        }

        // Return couriers in the order's region
        return mapper.Map<IEnumerable<CourierDTO>>(couriersInRegion);
    }
}
