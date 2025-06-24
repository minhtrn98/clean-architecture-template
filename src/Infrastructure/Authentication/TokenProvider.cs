using System.Security.Claims;
using System.Text;
using Application.Abstractions.Authentication;
using Application.Common.Configuration;
using Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SharedKernel;

namespace Infrastructure.Authentication;

internal sealed class TokenProvider(
    IDateTimeProvider dateTimeProvider,
    IOptions<JwtSettings> jwtOptions
    ) : ITokenProvider
{
    public string Create(User user, IEnumerable<string> permissions)
    {
        JwtSettings jwtSettings = jwtOptions.Value;

        string secretKey = jwtSettings.Secret;
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(secretKey));

        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            ]),
            Expires = dateTimeProvider.UtcNow.AddMinutes(jwtSettings.ExpirationInMinutes),
            SigningCredentials = credentials,
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience,
            Claims = new Dictionary<string, object>
            {
                { "permission", permissions },
            }
        };

        JsonWebTokenHandler handler = new();

        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }
}
