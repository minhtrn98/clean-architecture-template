using Microsoft.Extensions.Configuration;

namespace Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static T GetRequiredSection<T>(this IConfiguration configuration, string key)
        => configuration.GetRequiredSection(key).Get<T>()!;
}
