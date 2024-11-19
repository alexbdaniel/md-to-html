using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Conversion.Creation;

public static class FileOpener
{
    public static void OpenFile(string url, bool withBrowser)
    {
        if (withBrowser == false)
        {
            OpenWithDefault(url);
            return;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            OpenWithBrowser(url);
            return;
        }
        
        Console.Error.WriteLine("Unable to open with browser on non-Windows operating systems.");
    }
    
    [SupportedOSPlatform("Windows")]
    private static void OpenWithBrowser(string fileFullPath)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            const string firefoxPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
            Process.Start(firefoxPath, fileFullPath);
            return;
        }
    }

    private static void OpenWithDefault(string url)
    {
        //Uri uri = new Uri(fileFullPath, UriKind.Absolute);
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            url = url.Replace("&", "^&");
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            return;
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", url);
            return;
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", url);
            return;
        }
        
    }
}