using MiniValidation;

namespace Conversion.Configuration;

public static class OptionsValidator
{
    public static bool Validate<TModel>(TModel model)
    {
        bool valid = MiniValidator.TryValidate(model, out IDictionary<string, string[]> errors);

        if (valid)
            return valid;
        
        Console.WriteLine($"{typeof(TModel).Name} has one or more validation errors:");
        foreach (var entry in errors)
        {
            Console.WriteLine($"  {entry.Key}:");
            foreach (var error in entry.Value)
            {
                Console.WriteLine($"  - {error}");
            }
        }
        
        Environment.Exit(1);
        
        return valid;
    }

    public static bool ValidateCommandLineOptions(CommandLineOptions options)
    {
        var errors = new Dictionary<string, string[]>();

        if (!File.Exists(options.MarkdownFilePath))
        {
            errors.Add(nameof(options.MarkdownFilePath), [$"Could not find file at \"{options.MarkdownFilePath}\"."]);
        }

        bool valid = errors.Count == 0;
        if (valid)
            return valid;
        
        Console.WriteLine($"One or more of the command line arguments supplied are invalid:");
        foreach (var entry in errors)
        {
            Console.WriteLine($"  {entry.Key}:");
            foreach (var error in entry.Value)
            {
                Console.WriteLine($"  - {error}");
            }
        }
        
        Environment.Exit(1);
        
        return valid;
        
    }
}