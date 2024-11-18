using Frontliners.Common.InfraStructure.Interfaces;
using Frontliners.Identity.Domain.Entities;

namespace Frontliners.Identity.InfraStructure.Interfaces;

public interface ITokenService : IBaseService
{
    string GenerateToken(User user);
}