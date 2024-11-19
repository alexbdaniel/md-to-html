using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Conversion.Configuration;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
public class ConfigurationOptions
{
    public const string Key = "Configuration";

    public const string AppFileSystemName = "md-to-html";

    public static readonly DirectoryInfo ConfigurationDirectory = ConfigurationUtilities.GetConfigurationDirectory();

    private readonly string? styleSheetFilePath;

    [FileExists]
    public string StyleSheetFilePath
    {
        get => styleSheetFilePath ?? Path.Combine(ConfigurationDirectory.FullName, "stylesheets", "stackedit-toc-modified.css");
        init => styleSheetFilePath = String.IsNullOrWhiteSpace(value) ? Path.Combine(ConfigurationDirectory.FullName, "stylesheets", "stackedit-toc-modified.css") : value;
    }
    // public string StyleSheetFilePath { get; init; } = Path.Combine(ConfigurationDirectory.FullName, "stylesheets", "stackedit-toc-modified.css");
}