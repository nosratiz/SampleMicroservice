using Frontliners.Contract.Users;
using Frontliners.Order.Domain.Entities;
using Mapster;

namespace Frontliners.Order.InfraStructures.Profile;

public static class ProfileConfiguration
{
    public static void ConfigureProfile()
    {
        TypeAdapterConfig<UserCreated,User>.NewConfig()
            .Map(dest => dest.Id, src => src.UserId)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(des=>des.CreatedAt,src=>DateTime.Now)
        ;
    }
}

