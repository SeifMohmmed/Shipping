using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstraction.User;
using Shipping.Application.Abstraction.User.DTO;
using Shipping.Domain.Entities;
using Shipping.Domain.Repositories;

namespace Shipping.Infrastructure.UserServices;
public class UserService(UserManager<ApplicationUser> userManager,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IUserService
{
    // Add Courier
    public async Task<string> AddCourierAsync(AddCourierDTO addCourierDTO, CancellationToken cancellationToken = default)
    {
        if (await userManager.Users.AnyAsync(u => u.Email.Equals(addCourierDTO.Email)))
            return "Another user with the same Email is already exist";

        var user = mapper.Map<ApplicationUser>(addCourierDTO);
        var result = await userManager.CreateAsync(user, addCourierDTO.Password);

        if (!result.Succeeded)
            return string.Join(",", result.Errors.Select(e => e.Description));

        await userManager.AddToRoleAsync(user, addCourierDTO.RoleName);

        foreach (var item in addCourierDTO.SpecialCourierRegions)
        {
            item.CourierId = user.Id;
        }

        var specailCourierRegion = mapper.Map<List<SpecialCourierRegion>>(addCourierDTO);

        await unitOfWork.GetSpecialCourierRegionRepository().AddRangeAsync(specailCourierRegion);

        return string.Empty;

    }

    // Add Employee
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

    // Add Merchant
    public async Task<string> AddMerchantAsync(AddMerchantDTO addMerchantDTO, CancellationToken cancellationToken = default)
    {
        if (await userManager.Users.AnyAsync(u => u.Email.Equals(addMerchantDTO.Email)))
            return "Another user with the same Email is already exist";

        var user = mapper.Map<ApplicationUser>(addMerchantDTO);

        var result = await userManager.CreateAsync(user, addMerchantDTO.Password);

        if (!result.Succeeded)
            return string.Join(",", result.Errors.Select(e => e.Description));

        await userManager.AddToRoleAsync(user, addMerchantDTO.RoleName);

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

    //Get Account Profile
    public async Task<AccountProfileDTO?> GetAccountProfileAsync(string userId, CancellationToken cancellationToken = default)
    {
        var accountDetails = await userManager.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (accountDetails == null)
            return null;

        return mapper.Map<AccountProfileDTO>(accountDetails);
    }
}
