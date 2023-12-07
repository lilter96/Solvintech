using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solvintech.Application.DTO.Token;
using Solvintech.Application.DTO.User;
using Solvintech.Application.Services.Users;
using Solvintech.Shared.Utils;

namespace Solvintech.Presentation.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(
        IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
    {
        var result = await _userService.RegisterAsync(registerUserDto);

        return result.Match<IActionResult, UserDto>(
            onSuccess: Ok,
            onFailure: BadRequest);
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginUserDto loginUserDto)
    {
        var result = await _userService.LoginAsync(loginUserDto);

        return result.Match<IActionResult, UserDto>(
            onSuccess: Ok,
            onFailure: BadRequest);
    }
    
    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> GenerateToken(RefreshTokenDto refreshTokenDto)
    {
        var result = await _userService.RefreshTokenAsync(refreshTokenDto);

        return result.Match<IActionResult, RefreshTokenDto>(
            onSuccess: Ok,
            onFailure: error => StatusCode(StatusCodes.Status500InternalServerError, error));
    }
}