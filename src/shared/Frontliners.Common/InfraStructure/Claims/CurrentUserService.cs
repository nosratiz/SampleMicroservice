using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Frontliners.Common.InfraStructure.Claims;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string? UserId { get; } = httpContextAccessor.HttpContext?.User.FindFirstValue("userId");
}