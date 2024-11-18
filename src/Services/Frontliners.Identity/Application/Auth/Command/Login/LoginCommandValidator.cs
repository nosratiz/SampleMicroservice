using FluentValidation;
using Frontliners.Identity.InfraStructure;

namespace Frontliners.Identity.Application.Auth.Command.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(IdentityErrorMessage.EmailIsRequired)
            .EmailAddress().WithMessage(IdentityErrorMessage.InvalidEmail);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(IdentityErrorMessage.PasswordIsRequired)
            .MinimumLength(6).WithMessage(IdentityErrorMessage.PasswordLength);
    }
}