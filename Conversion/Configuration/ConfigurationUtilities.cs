namespace Conversion.Configuration;

public static class ConfigurationUtilities
{
    /// <summary>
    /// Gets or creates the configuration directory for the application.
    /// </summary>
    /// <returns>Configuration directory</returns>
    public static DirectoryInfo GetConfigurationDirectory()
    {
        string parentConfigDirName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string targetDirName = Path.Combine(parentConfigDirName, ConfigurationOptions.AppFileSystemName);

        return Directory.CreateDirectory(targetDirName);
    }
}