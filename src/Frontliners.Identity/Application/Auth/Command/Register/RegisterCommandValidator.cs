using FluentValidation;
using Frontliners.Common.Mongo.Repository;
using Frontliners.Identity.Domain.Entities;
using Frontliners.Identity.InfraStructure;

namespace Frontliners.Identity.Application.Auth.Command.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator(IMongoRepository<User> userRepository)
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(IdentityErrorMessage.InvalidEmail)
            .EmailAddress().WithMessage(IdentityErrorMessage.InvalidEmail)
            .MustAsync(async (email, _) =>
                await userRepository.ExistsAsync(x => x.Email == email)==false)
            .WithMessage(IdentityErrorMessage.EmailAlreadyExists);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(IdentityErrorMessage.PasswordIsRequired)
            .MinimumLength(6).WithMessage(IdentityErrorMessage.PasswordLength);

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(IdentityErrorMessage.FirstNameIsRequired);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(IdentityErrorMessage.LastNameIsRequired);
    }
}