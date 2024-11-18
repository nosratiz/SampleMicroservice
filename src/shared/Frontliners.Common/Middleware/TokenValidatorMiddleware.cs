using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Frontliners.Common.InfraStructure;
using Frontliners.Common.InfraStructure.Extensions;
using Frontliners.Common.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Frontliners.Common.Middleware;

public class TokenValidatorMiddleware(RequestDelegate next, IOptions<JwtSettings> jwtSettings,ILogger<TokenValidatorMiddleware> logger)
{
    public async Task Invoke(HttpContext httpContext)
    {
        if (httpContext.HasAuthorization())
        {
            var token = httpContext.GetAuthorizationToken();
            
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings.Value.SecretKey);
            
            logger.LogInformation("Token: {token}", token);

            var validationParameters = new TokenValidationParameters
                
            {
                ClockSkew = TimeSpan.Zero, // default: 5 min
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateAudience = false, //default : false
                ValidateIssuer = false, //default : false

                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            };

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var claimsPrincipal = handler.ValidateToken(token, validationParameters, out _);
                httpContext.User = claimsPrincipal;
            }
            catch (Exception e)
            {
                await httpContext.WriteError(new ApiError(new Error(e.Message, null)),
                    StatusCodes.Status401Unauthorized);
                return;
            }
        }

        await next(httpContext);
    }
}