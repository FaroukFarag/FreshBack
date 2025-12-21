using FreshBack.Application.Dtos.Settings.Users;
using FreshBack.Application.Interfaces.Settings.Users;
using FreshBack.Domain.Models.Settings.Users;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Users;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService)
    : BaseController<IUserService, UserDto, UserDto, UserDto, UserDto, User,
        int>(userService)
{
    private readonly IUserService _userService = userService;

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto loginDto)
        => await HandleLoginAsync(loginDto);

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var result = await _userService.ResetPasswordAsync(resetPasswordDto);

        return HandleResult(result.Succeeded, "Password reset successfully.", "Failed to reset password. User not found or invalid request.");
    }

    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto request)
    {
        var result = await _userService.ForgotPasswordAsync(request);

        return HandleResult(result.Succeeded, "Password reset successfully.", "Failed to reset password. User not found or invalid request.");
    }

    private async Task<IActionResult> HandleLoginAsync(LoginDto loginDto)
    {
        var loggedInDto = await _userService.LoginAsync(loginDto);

        return loggedInDto is null
            ? Unauthorized(new { Message = "Invalid login attempt" })
            : Ok(loggedInDto);
    }

    private IActionResult HandleResult(bool result, string successMessage, string errorMessage)
    {
        return result
            ? Ok(new { Message = successMessage })
            : BadRequest(new { Message = errorMessage });
    }
}
