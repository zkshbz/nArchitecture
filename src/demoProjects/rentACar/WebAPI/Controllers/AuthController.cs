using Application.Features.Auths.Commands.Register;
using Core.Security.Dtos;
using Core.Security.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : BaseController
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
    {
        var registerCommand = new RegisterCommand()
        {
            UserForRegisterDto = userForRegisterDto,
            IpAddress = GetIpAddress()
        };

        var result = await Mediator.Send(registerCommand);
        SetRefreshTokenToCookie(result.RefreshToken);
       
        return Ok(result.AccessToken);
    }

    private void SetRefreshTokenToCookie(RefreshToken refreshToken)
    {
        var cookieOptions = new CookieOptions()
        {
            HttpOnly = true,
            Expires = DateTime.Now.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
    }
}