using System.Diagnostics.CodeAnalysis;

namespace Conversion.Creation;

[SuppressMessage("ReSharper", "ConvertIfStatementToConditionalTernaryExpression")]
public static class Saver
{
    public static async Task SaveAsync(string html, string saveFullPath)
    {
        await using var writer = new StreamWriter(saveFullPath);

        await writer.WriteAsync(html);

    }
    
    public static string GetFullPath(string saveDirectoryName)
    {
        DirectoryInfo directory;

        if (string.IsNullOrWhiteSpace(saveDirectoryName) || saveDirectoryName == "Desktop directory")
        {
            directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
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
}