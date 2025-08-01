using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Abstraction.Auth;
using Shipping.Application.Abstraction.User;
using Shipping.Domain.Entities;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(IUserService userService,
    IAuthService authService) : ControllerBase
{

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
    {
        var result = await authService.RegisterAsync(model);

        if (!result.IsAuthenticated)
            return BadRequest(result.Message);

        SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] TokenRequestModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await authService.GetTokenAsync(model);

        if (!result.IsAuthenticated)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [HttpGet("refreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var result = await authService.RefreshTokenAsync(refreshToken);

        if (!result.IsAuthenticated)
            return BadRequest(result);

        SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

        return Ok(result);
    }

    private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = expires.ToLocalTime(),
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.None
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}
