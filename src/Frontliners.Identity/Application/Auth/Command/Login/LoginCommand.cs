using FluentResults;
using Frontliners.Identity.Application.Auth.Dto;
using MediatR;

namespace Frontliners.Identity.Application.Auth.Command.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;