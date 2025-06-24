using System.ComponentModel.DataAnnotations;

namespace Application.Common.Configuration;

public class JwtSettings
{
    public const string SectionName = "Jwt";

    [Required] public string Secret { get; init; }
    [Required] public string Issuer { get; init; }
    [Required] public string Audience { get; init; }
    [Required] public int ExpirationInMinutes { get; init; }
    [Required] public int RefreshTokenExpireMinutes { get; init; }
}
