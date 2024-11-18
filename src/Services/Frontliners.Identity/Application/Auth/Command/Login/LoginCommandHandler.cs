using FluentResults;
using Frontliners.Common.Mongo.Repository;
using Frontliners.Identity.Application.Auth.Dto;
using Frontliners.Identity.Domain.Entities;
using Frontliners.Identity.Domains.Enum;
using Frontliners.Identity.InfraStructure;
using Frontliners.Identity.InfraStructure.Helper;
using Frontliners.Identity.InfraStructure.Interfaces;
using MediatR;

namespace Frontliners.Identity.Application.Auth.Command.Login;

public sealed class LoginCommandHandler(IMongoRepository<User> userRepository,ITokenService tokenService)
    : IRequestHandler<LoginCommand, Result<LoginResponse>>

{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user= await userRepository.GetAsync(x => x.Email == request.Email, cancellationToken);
        
        if (user is null)
            return Result.Fail<LoginResponse>(IdentityErrorMessage.InvalidCredentials);
        
        if(PasswordManagement.CheckPassword(request.Password,user.Password) == false)
            return Result.Fail<LoginResponse>(IdentityErrorMessage.InvalidCredentials);

        if (user.Status == Status.Inactive)
            return Result.Fail<LoginResponse>(IdentityErrorMessage.UserIsInactive);
        
        
        var token = tokenService.GenerateToken(user);
        
        return Result.Ok(new LoginResponse(token));
        
    }
    
}