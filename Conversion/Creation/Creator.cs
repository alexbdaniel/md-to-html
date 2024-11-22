using Conversion.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Conversion.Creation;

public class Creator
{
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly ILogger logger;

    public Creator(IServiceScopeFactory serviceScopeFactory, ILogger<Creator> logger)
    {
        this.serviceScopeFactory = serviceScopeFactory;
        this.logger = logger;
    }

    public async Task CreateAsync(CommandLineOptions args)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        
        var converter = scope.ServiceProvider.GetRequiredService<Converter>();
        
        string html = await converter.ConvertAsync(args.MarkdownFilePath);

        string outputFullPath = Saver.GetFullPath(args.OutputDirectoryName);
        await Saver.SaveAsync(html, outputFullPath, logger);

        if (args.OpenOnCompletion || args.OpenWithBrowser)
            FileOpener.OpenFile(outputFullPath, args.OpenWithBrowser);
    }
}