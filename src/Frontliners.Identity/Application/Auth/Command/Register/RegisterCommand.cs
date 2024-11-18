using FluentResults;
using MediatR;

namespace Frontliners.Identity.Application.Auth.Command.Register;


public sealed record RegisterCommand(string Email,
    string Password,
    string FirstName,
    string LastName) : IRequest<Result<Unit>>;