using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddOptionsAndValidate<T>(
        this IServiceCollection services,
        IConfigurationManager configuration,
        string name
    ) where T : class
        => services
            .AddOptions<T>()
            .Bind(configuration.GetRequiredSection(name))
            .ValidateDataAnnotations()
            .ValidateOnStart();

    public static void AddOptionsAndValidate<T>(
        this IServiceCollection services,
        IConfiguration configuration,
        string name
    ) where T : class
        => services
            .AddOptions<T>()
            .Bind(configuration.GetRequiredSection(name))
            .ValidateDataAnnotations()
            .ValidateOnStart();
}
