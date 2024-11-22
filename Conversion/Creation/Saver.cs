using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Conversion.Creation;

[SuppressMessage("ReSharper", "ConvertIfStatementToConditionalTernaryExpression")]
public static class Saver
{
    public static async Task SaveAsync(string html, string outputFullPath, ILogger logger)
    {
        await using var writer = new StreamWriter(outputFullPath);

        await writer.WriteAsync(html);
        
        logger.LogInformation("Saved to \"{outputFullPath}\"", outputFullPath);
    }
    
    public static string GetFullPath(string saveDirectoryName)
    {
        DirectoryInfo directory;

        
        

        if (string.IsNullOrWhiteSpace(saveDirectoryName) || saveDirectoryName.Trim() == ".")
        {
            directory = new DirectoryInfo(Environment.CurrentDirectory);

            //new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
        }
        else
        {
            directory = Directory.CreateDirectory(saveDirectoryName);
        }
        
        const string extension = "html";
        string baseName = DateTime.UtcNow.ToString("yyyyMMddTHHmmssK");
        
        string fullPath = Path.Combine(directory.FullName, $"{baseName}.{extension}");

        UInt16 count = 1;
        while (File.Exists(fullPath))
        {
            if (count > 1000)
                throw new Exception("Recursion limit exceeded");
            
            fullPath = Path.Combine(directory.FullName, $"{baseName}-{++count}.{extension}");
        }

        return fullPath;
    }

    /// <summary>
    /// Source: https://stackoverflow.com/a/47569899
    /// </summary>
    /// <param name="path"></param>
    /// <returns>True if the supplied path represents a full path.</returns>
    public static bool IsFullPath(this string path)
    {
        if (string.IsNullOrWhiteSpace(path) || path.IndexOfAny(Path.GetInvalidPathChars()) != -1 || !Path.IsPathRooted(path))
            return false;
    
        string? pathRoot = Path.GetPathRoot(path);
        if (pathRoot != null && pathRoot.Length <= 2 && pathRoot != "/") // Accepts X:\ and \\UNC\PATH, rejects empty string, \ and X:, but accepts / to support Linux
            return false;

        if (pathRoot != null && (pathRoot[0] != '\\' || pathRoot[1] != '\\'))
            return true; // Rooted and not a UNC path

        return pathRoot != null && pathRoot.Trim('\\').Contains('\\'); // A UNC server name without a share name (e.g "\\NAME" or "\\NAME\") is invalid
               
    }
}