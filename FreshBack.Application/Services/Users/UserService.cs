using AutoMapper;
using FreshBack.Application.Dtos.Settings.Users;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Settings.Users;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Common.Tokens.Interfaces;
using FreshBack.Domain.Interfaces.Repositories.Settings.Users;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Roles;
using FreshBack.Domain.Models.Settings.Users;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FreshBack.Application.Services.Users;

public class UserService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    ITokensService tokensService) :
    BaseService<UserDto, UserDto, UserDto, UserDto, User, int>(userRepository,
        unitOfWork, mapper), IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly ITokensService _tokensService = tokensService;

    public override async Task<ResultDto<UserDto>> CreateAsync(UserDto userDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create User",
            action: async () =>
            {
                var user = _mapper.Map<User>(userDto);
                var createResult = await _userManager.CreateAsync(user, userDto.Password!);

                if (!createResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"User creation failed: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                }

                var role = await _roleManager.FindByIdAsync(userDto.RoleId.ToString())
                    ?? throw new InvalidOperationException("Role not found");
                var roleResult = await _userManager.AddToRoleAsync(user, role.Name!);

                if (!roleResult.Succeeded)
                {
                    await _userManager.DeleteAsync(user);

                    throw new InvalidOperationException(
                        $"Role assignment failed: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                }

                return userDto;
            });
    }

    public async Task<ResultDto<UserDto>> GetUserByRoleAsync(int userId, int roleId)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get User by Role",
            action: async () =>
            {
                var role = await _roleManager.FindByIdAsync(roleId.ToString())
                    ?? throw new InvalidOperationException("Role not found");
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
                var user = usersInRole.FirstOrDefault(u => u.Id == userId)
                    ?? throw new InvalidOperationException("User not found in specified role");
                var userWithIncludes = await _userRepository.GetAsync(user.Id);

                return _mapper.Map<UserDto>(userWithIncludes);
            });
    }

    public async Task<ResultDto<IEnumerable<UserDto>>> GetAllUsersByRoleAsync(int roleId)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get All Users by Role",
            action: async () =>
            {
                var role = await _roleManager.FindByIdAsync(roleId.ToString())
                    ?? throw new InvalidOperationException("Role not found");
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
                var userIds = usersInRole.Select(u => u.Id).ToList();
                var usersWithIncludes = await _userRepository.GetAllAsync();

                return _mapper.Map<IEnumerable<UserDto>>(usersWithIncludes);
            });
    }

    public override async Task<ResultDto<UserDto>> UpdateAsync(UserDto newUserDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Update User",
            action: async () =>
            {
                var existingUser = await _userManager.FindByIdAsync(newUserDto.Id.ToString())
                    ?? throw new InvalidOperationException("User not found");

                _mapper.Map(newUserDto, existingUser);

                if (string.IsNullOrEmpty(existingUser.SecurityStamp))
                {
                    existingUser.SecurityStamp = Guid.NewGuid().ToString();
                }

                var result = await _userManager.UpdateAsync(existingUser);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"User update failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                return newUserDto;
            });
    }

    public async Task<ResultDto<LoggedInDto>> LoginAsync(LoginDto model)
    {
        return await ExecuteServiceCallAsync(
            operationName: "User Login",
            action: async () =>
            {
                var user = await AuthenticateUserAsync(model)
                    ?? throw new InvalidOperationException("Authentication failed");
                var roles = await _userManager.GetRolesAsync(user);
                var role = await _roleManager.FindByNameAsync(roles.FirstOrDefault()!)
                    ?? throw new InvalidOperationException("Role not found");

                return new LoggedInDto
                {
                    UserId = user.Id,
                    RoleId = role.Id,
                    Token = await GetToken(user)
                };
            });
    }

    public async Task<ResultDto<bool>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Reset Password",
            action: async () =>
            {
                var user = await _userManager.FindByNameAsync(resetPasswordDto.UserName)
                    ?? throw new InvalidOperationException("User not found");
                var result = await _userManager.ChangePasswordAsync(
                    user,
                    resetPasswordDto.OldPassword,
                    resetPasswordDto.NewPassword);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Password reset failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                return true;
            });
    }

    public async Task<ResultDto<bool>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Forgot Password",
            action: async () =>
            {
                var user = await _userManager.FindByNameAsync(forgotPasswordDto.UserName)
                    ?? throw new InvalidOperationException("User not found");
                var removeResult = await _userManager.RemovePasswordAsync(user);

                if (!removeResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Password removal failed: {string.Join(", ", removeResult.Errors.Select(e => e.Description))}");
                }

                var addResult = await _userManager.AddPasswordAsync(user, forgotPasswordDto.NewPassword);

                if (!addResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Password set failed: {string.Join(", ", addResult.Errors.Select(e => e.Description))}");
                }

                return true;
            });
    }

    private async Task<User?> AuthenticateUserAsync(LoginDto model)
    {
        var result = await _signInManager.PasswordSignInAsync(
            model.UserName,
            model.Password,
            isPersistent: false,
            lockoutOnFailure: false);

        return result.Succeeded ? await _userManager.FindByNameAsync(model.UserName) : null;
    }

    private async Task<string> GetToken(User user)
    {
        var claims = new List<TokenClaim>
        {
            new("userId", user.Id.ToString()),
            new("userName", user.UserName ?? string.Empty),
            new("email", user.Email ?? string.Empty)
        };

        var userRoles = await _userManager.GetRolesAsync(user);

        foreach (var role in userRoles)
        {
            claims.Add(new TokenClaim(ClaimTypes.Role, role));
        }

        return await _tokensService.GenerateToken(claims)
            ?? throw new InvalidOperationException("Token generation failed");
    }
}
