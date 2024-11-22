using System.Reflection;
using CommandLine;
using Conversion.Configuration;
using Conversion.Creation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Conversion;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var parser = new Parser(configuration =>
        {
            configuration.GetoptMode = true;
        });
        
        var parserResults = parser.ParseArguments<CommandLineOptions>(args);

        await parserResults.WithParsedAsync(RunAsync);
        // Console.WriteLine($"Exit code = {Environment.ExitCode}");
    }

    private static async Task RunAsync(CommandLineOptions args)
    {
        bool argsValid = OptionsValidator.ValidateCommandLineOptions(args);
        
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        builder.Configuration
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", false)
            .SetEnvironmentNameFromAppSettings(ref builder)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName.ToLower()}.json", true)
            .AddUserSecrets(Assembly.GetExecutingAssembly())
            .AddEnvironmentVariables();

        builder.Services.Configure<HostOptions>(options =>
        {
            options.ServicesStartConcurrently = true;
            options.ServicesStopConcurrently = true;
        });
        
        var services = builder.Services;
        
        services.ConfigureServices(builder, args);
        
        await using var provider = services.BuildServiceProvider();
        
        var creator = provider.GetRequiredService<Creator>();
        await creator.CreateAsync(args);
        
        // await application.RunAsync().ConfigureAwait(false);
    }

    private static int HandleArgsError(IEnumerable<Error> errors)
    {
        int result = -2;
        Error[] enumerable = errors as Error[] ?? errors.ToArray();
        Console.WriteLine("errors {0}", enumerable.Count());
        if (enumerable.Any(error => error is HelpRequestedError or VersionRequestedError))
            result = -1;
        Console.WriteLine("Exit code {0}", result);
        return result;
    }

  
    
    private static IConfigurationBuilder SetEnvironmentNameFromAppSettings(this IConfigurationBuilder configurationManager, ref HostApplicationBuilder builder)
    {
        string environmentName = builder.Configuration
            .GetSection("Configuration")
            .GetValue<string>("Environment") ?? "Production";
        
        builder.Environment.EnvironmentName = environmentName;
        
        return configurationManager;
    }
}