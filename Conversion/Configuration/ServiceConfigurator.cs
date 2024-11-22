using Conversion.Creation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Conversion.Configuration;

public static class ServiceConfigurator
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, HostApplicationBuilder builder, CommandLineOptions args)
    {
        services.ConfigureOptions(builder);
        services.ConfigureLogging(builder, args);
        
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

    private static IServiceCollection ConfigureLogging(this IServiceCollection services, HostApplicationBuilder builder, CommandLineOptions args)
    {
        LogEventLevel defaultLevel;
        
        try
        {
           defaultLevel = (LogEventLevel)((int)LogEventLevel.Warning - args.Verbosity);
        }
        catch
        {
            defaultLevel = LogEventLevel.Warning;
        }

        int max = Enum.GetValues<LogEventLevel>().Cast<int>().Max();
        int min = Enum.GetValues<LogEventLevel>().Cast<int>().Min();

        if ((int)defaultLevel < min || (int)defaultLevel > max)
        {
            defaultLevel = LogEventLevel.Verbose;
            Console.WriteLine($"An invalid verbosity was set. Log level will now be set to {nameof(LogEventLevel.Verbose)}.");
        }
        
        var logger = new LoggerConfiguration()
            .WriteTo.Console(restrictedToMinimumLevel: defaultLevel)
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentUserName()
            .MinimumLevel.Override("Microsoft", defaultLevel)
            .MinimumLevel.Override("System", defaultLevel)
            .CreateLogger();

        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(logger));
        
        
        return services;
    }
}