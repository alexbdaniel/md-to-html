using Conversion.Creation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Conversion.Configuration;

public static class ServiceConfigurator
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, HostApplicationBuilder builder)
    {
        services.ConfigureOptions(builder);

        services.AddScoped<Converter>();
        services.AddSingleton<Creator>();

        return services;
    }
    
    private static IServiceCollection ConfigureOptions(this IServiceCollection services, HostApplicationBuilder builder)
    {
        services.AddOptions<ConfigurationOptions>().Bind(builder.Configuration.GetSection(ConfigurationOptions.Key))
            .ValidateDataAnnotations()
            .Validate(OptionsValidator.Validate)
            .ValidateOnStart();

        return services;
    }
}