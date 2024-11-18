using Frontliners.Common.InfraStructure.Interfaces;

namespace Frontliners.Common.InfraStructure.Claims;

public interface ICurrentUserService : IBaseService
{
    string? UserId { get; }
    
    List<string> Roles { get; }
}
