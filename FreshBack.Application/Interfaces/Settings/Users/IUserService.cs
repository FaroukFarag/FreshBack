using FreshBack.Application.Dtos.Settings.Users;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.Settings.Users;

namespace FreshBack.Application.Interfaces.Settings.Users;

public interface IUserService : IBaseService<UserDto, UserDto, UserDto, UserDto,
    User, int>
{
    Task<ResultDto<UserDto>> GetUserByRoleAsync(int userId, int roleId);
    Task<ResultDto<IEnumerable<UserDto>>> GetAllUsersByRoleAsync(int roleId);
    Task<ResultDto<LoggedInDto>> LoginAsync(LoginDto model);
    Task<ResultDto<bool>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    Task<ResultDto<bool>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
}
