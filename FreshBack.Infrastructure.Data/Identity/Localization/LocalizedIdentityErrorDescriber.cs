using FreshBack.Domain.Resources.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace FreshBack.Infrastructure.Data.Identity.Localization;

public class LocalizedIdentityErrorDescriber(IStringLocalizer<IdentityErrors> localizer) :
    IdentityErrorDescriber
{
    private readonly IStringLocalizer<IdentityErrors> _localizer = localizer;

    public override IdentityError DuplicateEmail(string email)
        => Describe(nameof(DuplicateEmail), email);

    public override IdentityError InvalidEmail(string? email)
        => Describe(nameof(InvalidEmail), email!);

    public override IdentityError DuplicateUserName(string userName)
        => Describe(nameof(DuplicateUserName), userName);

    public override IdentityError PasswordTooShort(int length)
        => Describe(nameof(PasswordTooShort), length);

    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        => Describe(nameof(PasswordRequiresUniqueChars), uniqueChars);

    public override IdentityError PasswordRequiresDigit()
        => Describe(nameof(PasswordRequiresDigit));

    public override IdentityError PasswordRequiresUpper()
        => Describe(nameof(PasswordRequiresUpper));

    public override IdentityError PasswordRequiresLower()
        => Describe(nameof(PasswordRequiresLower));

    public override IdentityError PasswordRequiresNonAlphanumeric()
        => Describe(nameof(PasswordRequiresNonAlphanumeric));

    public override IdentityError InvalidRoleName(string? role)
        => Describe(nameof(InvalidRoleName), role!);

    public override IdentityError DuplicateRoleName(string role)
        => Describe(nameof(DuplicateRoleName), role);

    public override IdentityError UserAlreadyInRole(string role)
        => Describe(nameof(UserAlreadyInRole), role);

    public override IdentityError UserNotInRole(string role)
        => Describe(nameof(UserNotInRole), role);

    private IdentityError Describe(string code, params object[] args)
    {
        var message = _localizer[code, args];
        return new IdentityError { Code = code, Description = message };
    }
}
