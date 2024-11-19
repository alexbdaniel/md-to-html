using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Conversion.Configuration;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class ConfigurationOptions
{
    public const string Key = "Configuration";

    public const string AppFileSystemName = "md-to-html";

    public static readonly DirectoryInfo ConfigurationDirectory = ConfigurationUtilities.GetConfigurationDirectory();

    [Required]
    [FileExists]
    public string StyleSheetFilePath { get; init; } = Path.Combine(ConfigurationDirectory.FullName, "stylesheets", "stackedit-toc-modified.css");
}