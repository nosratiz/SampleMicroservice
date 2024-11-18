using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Frontliners.Common.InfraStructure.Claims;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string? UserId { get; } = httpContextAccessor.HttpContext?.User.FindFirstValue("userId") ?? "9f77b3b5-e9ba-4796-9332-6ab2a84493f7";
    public List<string> Roles { get; } = httpContextAccessor.HttpContext?.User.FindFirstValue("Roles")?.Split(',').ToList() ??
                                         new List<string>();
}