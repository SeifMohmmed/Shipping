using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shipping.Application.Abstraction.User;
using Shipping.Application.Abstraction.User.DTO;
using Shipping.Domain.Entities;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.UserSerivces;
public class UserService(ILogger<UserService> logger,
     IUserContext userContext,
    IUserStore<ApplicationUser> userStore,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IUserService
{
    //Get Accout Profile Data
    public async Task<AccountProfileDTO?> GetAccountProfileAsync(string userId, CancellationToken cancellationToken = default)
    {
        var accountDetails = await userManager.Users.FirstAsync(u => u.Id == userId);

        return mapper.Map<AccountProfileDTO>(accountDetails);
    }

    //Update User Details
    public async Task UpdateUserDetails(UpdateUserDetails request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();

        logger.LogInformation("Updating user: {UserId}, with {@Request}", user!.Id, request);

        var dbUser = await userStore.FindByIdAsync(user!.Id, cancellationToken);

        if (dbUser is null)
            throw new KeyNotFoundException($"User with ID {user.Id} not found.");

        dbUser.FullName = request.FullName;
        dbUser.IsDeleted = request.IsDeleted;
        dbUser.Address = request.Address;
        dbUser.StoreName = request.StoreName;
        dbUser.RegionId = request.RegionId;
        dbUser.PickupPrice = request.PickupPrice;
        dbUser.CanceledOrder = request.CanceledOrder;
        dbUser.DeductionTypes = request.DeductionTypes;
        dbUser.BranchId = request.BranchId;
        dbUser.DeductionCompanyFromOrder = request.DeductionCompanyFromOrder;
        dbUser.PhoneNumber = request.PhoneNumber;

        await userStore.UpdateAsync(dbUser, cancellationToken);
    }

    //Assign User Roles
    public async Task AssignUserRoles(AssignUserRoles request)
    {
        logger.LogInformation("Assigning user role: {@Request}", request);

        var user = await userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new KeyNotFoundException($"User with Email {request.UserEmail} not found.");

        var role = await roleManager.FindByNameAsync(request.RoleName)
            ?? throw new KeyNotFoundException($"User with Role {request.RoleName} not found.");

        await userManager.AddToRoleAsync(user, role.Name!);
    }

    //Unassign User Roles
    public async Task UnassignUserRoles(UnassignUserRoles request)
    {
        logger.LogInformation("Unassigning user role: {@Request}", request);

        var user = await userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new KeyNotFoundException($"User with Email {request.UserEmail} not found.");

        var role = await roleManager.FindByNameAsync(request.RoleName)
            ?? throw new KeyNotFoundException($"User with Role {request.RoleName} not found.");

        await userManager.RemoveFromRoleAsync(user, role.Name!);

    }

    // Add Employee ==> Registers a new employee user in the system.
    public async Task<string> AddEmployeeAsync(AddEmployeeDTO addEmployeeDTO, CancellationToken cancellationToken = default)
    {
        if (await userManager.Users.AnyAsync(u => u.Email.Equals(addEmployeeDTO.Email)))
            return "Another user with the same Email is already exist";

        var user = mapper.Map<ApplicationUser>(addEmployeeDTO);
        var result = await userManager.CreateAsync(user, addEmployeeDTO.Password);

        if (!result.Succeeded)
            return string.Join(",", result.Errors.Select(e => e.Description));

        await userManager.AddToRoleAsync(user, addEmployeeDTO.RoleName);

        return string.Empty;
    }

    // Add Merchant ==> Creates a new merchant user, assigns the merchant role, and stores any special city delivery costs.
    public async Task<string> AddMerchantAsync(AddMerchantDTO addMerchantDTO, CancellationToken cancellationToken = default)
    {
        if (await userManager.Users.AnyAsync(u => u.Email.Equals(addMerchantDTO.Email)))
            return "Another user with the same Email is already exist";

        var user = mapper.Map<ApplicationUser>(addMerchantDTO);
        var result = await userManager.CreateAsync(user, addMerchantDTO.Password);

        if (!result.Succeeded)
            return string.Join(",", result.Errors.Select(e => e.Description));

        await userManager.AddToRoleAsync(user, addMerchantDTO.RoleName); // Merchant Role

        if (addMerchantDTO.SpecialCityCosts is not null)
        {
            foreach (var specialCityCosts in addMerchantDTO.SpecialCityCosts)
            {
                specialCityCosts.MerchantId = user.Id;
            }

            var specialCityCost = mapper.Map<List<SpecialCityCost>>(addMerchantDTO.SpecialCityCosts);
            await unitOfWork.GetSpecialCityCostRepository().AddRangeAsync(specialCityCost);
        }

        return string.Empty;
    }

    // Add Courier ==> Creates a new courier user, assigns the courier role, and stores their special regions.
    public async Task<string> AddCourierAsync(AddCourierDTO addCourierDTO, CancellationToken cancellationToken = default)
    {
        if (await userManager.Users.AnyAsync(u => u.Email.Equals(addCourierDTO.Email)))
            return "Another user with the same Email is already exist";

        var user = mapper.Map<ApplicationUser>(addCourierDTO);
        var result = await userManager.CreateAsync(user, addCourierDTO.Password);

        if (!result.Succeeded)
            return string.Join(",", result.Errors.Select(e => e.Description));

        await userManager.AddToRoleAsync(user, addCourierDTO.RoleName); // Courier Role

        foreach (var item in addCourierDTO.SpecialCourierRegions)
        {
            item.CourierId = user.Id;
        }

        var specialCourierRegion = mapper.Map<List<SpecialCourierRegion>>(addCourierDTO.SpecialCourierRegions);
        await unitOfWork.GetSpecialCourierRegionRepository().AddRangeAsync(specialCourierRegion);

        return string.Empty;
    }
}
