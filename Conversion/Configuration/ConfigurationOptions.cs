using System.ComponentModel.DataAnnotations;

namespace Conversion.Configuration;

public class ConfigurationOptions
{
    public const string Key = "Configuration";
    
    [Required]
    [FileExists]
    public required string StyleSheetFilePath { get; init; }
}