using FluentResults;
using MediatR;

namespace Frontliners.Order.Applications.Orders.Command.Submit;

public sealed record SubmitCommand : IRequest<Result>;