using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shipping.Application.Abstraction.Auth;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Shipping.Application.Services.AuthServices;
public class AuthService(UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IOptions<JWT> jwt) : IAuthService
{
    private readonly JWT _jwt = jwt.Value;

    //Register User
    public async Task<AuthModel> RegisterAsync(RegisterModel model)
    {
        if (await userManager.FindByEmailAsync(model.Email) is not null)
            return new AuthModel { Message = "Email is Already registered!" };

        var user = new ApplicationUser
        {
            UserName = model.Username,
            Email = model.Email,
            FullName = model.FirstName + " " + model.LastName,
        };

        var result = await userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var errors = string.Empty;

            foreach (var error in result.Errors)
                errors += $"{error.Description}, ";

        }

        var allowedRoles = new[] { "Merchant", "Courier" };
        if (!allowedRoles.Contains(model.Role))
        {
            return new AuthModel { Message = "Registration with this role is not allowed." };
        }


        // ensure role exists
        if (!await roleManager.RoleExistsAsync(model.Role))
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = model.Role, NormalizedName = model.Role.ToUpper() });
        }


        await userManager.AddToRoleAsync(user, model.Role);

        var jwtSecurityToken = await CreateJwtToken(user);

        return new AuthModel
        {
            Email = user.Email,
            ExpiresOn = jwtSecurityToken.ValidTo,
            IsAuthenticated = true,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            Username = user.UserName
        };
    }

    //Get Token
    public async Task<AuthModel> GetTokenAsync(TokenRequestModel model)
    {
        var authModel = new AuthModel();

        var user = await userManager.FindByEmailAsync(model.Email);

        if (user is null || !await userManager.CheckPasswordAsync(user, model.Password))
        {
            authModel.Message = "Email or Password is incorrect!";
            return authModel;
        }

        var jwtSecurityToken = await CreateJwtToken(user);
        var rolesList = await userManager.GetRolesAsync(user);

        authModel.IsAuthenticated = true;
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        authModel.Email = user.Email;
        authModel.Username = user.UserName;
        authModel.ExpiresOn = jwtSecurityToken.ValidTo;
        authModel.Roles = rolesList.ToList();

        if (user.RefreshTokens.Any(t => t.IsActive))
        {
            var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
            authModel.RefreshToken = activeRefreshToken.Token;
            authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
        }
        else
        {
            var refreshToken = GenerateRefreshToken();
            authModel.RefreshToken = refreshToken.Token;
            authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
            user.RefreshTokens.Add(refreshToken);
            await userManager.UpdateAsync(user);
        }

        return authModel;
    }

    //Add Role
    public async Task<string> AddRoleAsync(AddRoleModel model)
    {
        var user = await userManager.FindByIdAsync(model.UserId);

        if (user is null || !await roleManager.RoleExistsAsync(model.Role))
            return "Invalid user ID or Role";

        if (await userManager.IsInRoleAsync(user, model.Role))
            return "User already assigned to this role";

        var result = await userManager.AddToRoleAsync(user, model.Role);

        return result.Succeeded ? string.Empty : "Sonething went wrong";

    }

    //Return RefreshToken
    public async Task<AuthModel> RefreshTokenAsync(string token)
    {
        var authModel = new AuthModel();

        var user =
            await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

        if (user is null)
        {
            authModel.Message = "Invalid token";
            return authModel;
        }

        var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

        if (!refreshToken.IsActive)
        {
            authModel.Message = "Inactive token";
            return authModel;
        }

        refreshToken.RevokedOn = DateTime.UtcNow;

        var newRefreshToken = GenerateRefreshToken();
        user.RefreshTokens.Add(newRefreshToken);
        await userManager.UpdateAsync(user);

        var jwtToken = await CreateJwtToken(user);
        authModel.IsAuthenticated = true;
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        authModel.Email = user.Email;
        authModel.Username = user.UserName;

        var roles = await userManager.GetRolesAsync(user);
        authModel.Roles = roles.ToList();
        authModel.RefreshToken = newRefreshToken.Token;
        authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

        return authModel;
    }


    //Revoke Token
    public async Task<bool> RevokeTokenAsync(string token)
    {
        var user =
            await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

        if (user is null)
            return false;

        var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

        if (!refreshToken.IsActive)
            return false;

        refreshToken.RevokedOn = DateTime.UtcNow;

        await userManager.UpdateAsync(user);

        return true;
    }

    //Generate JWTToken
    private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        var userClaims = await userManager.GetClaimsAsync(user);
        var roles = await userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();

        foreach (var role in roles)
            roleClaims.Add(new Claim("roles", role));

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }

    //===============================================================================================
    private RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var generator = new RNGCryptoServiceProvider();

        generator.GetBytes(randomNumber);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            ExpiresOn = DateTime.UtcNow.AddDays(10),
            CreatedOn = DateTime.UtcNow
        };
    }
}
