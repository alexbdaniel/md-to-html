#nullable enable
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Conversion.Configuration;
using Conversion.Creation;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Conversion.Tests.Creation;

[TestSubject(typeof(Converter))]
public class ConverterTest
{
    private readonly ITestOutputHelper  console;

    public ConverterTest(ITestOutputHelper  console)
    {
        this.console = console;
    }

    [Fact]
    public async Task SandboxDownloadConfigurationFiles()
    {
        const string saveDirectoryName = "";
        const string latestUrl = "https://github.com/alexbdaniel/md-to-html-templates/releases/latest";

        
        
        var handler = new HttpClientHandler()
        {
            UseProxy = false
        };
        
        using var client = new HttpClient(handler);

        Uri latestReleaseGeneric = new Uri(latestUrl);
        var response1 = await client.GetAsync(latestReleaseGeneric, HttpCompletionOption.ResponseHeadersRead);
        response1.EnsureSuccessStatusCode();
        
        Uri? latestRelease = response1.RequestMessage?.RequestUri;
        if (latestRelease == null)
            return;

        string tag = latestRelease.Segments.Last().Replace("/", "");
        string profile = latestRelease.Segments[1].Replace("/", "");
        string repository = latestRelease.Segments[2].Replace("/", "");

        Uri downloadUri = new UriBuilder
        {
            Scheme = latestRelease.Scheme,
            Host = latestRelease.Host,
            Port = latestRelease.Port,
            Path = $"{profile}/{repository}/archive/refs/tags/{tag}.zip"
        }.Uri;
        
        var downloadResponse = await client.GetAsync(downloadUri);

        await HandleSaveAsync(downloadResponse.Content);
        //https://github.com/alexbdaniel/md-to-html-templates/archive/refs/tags/1.0.2.zip

    }

    public async Task HandleSaveAsync(HttpContent content, bool overwrite = false)
    {
        
        DirectoryInfo tempDir = Directory.CreateTempSubdirectory();
        ZipFile.ExtractToDirectory(await content.ReadAsStreamAsync(), tempDir.FullName, Encoding.UTF8, false);
        
        DirectoryInfo configurationDirectory = ConfigurationOptions.ConfigurationDirectory;
        DirectoryInfo contentsDirectory = tempDir.GetDirectories().First();
        
        DirectoryInfo[] directories = contentsDirectory.GetDirectories();
        foreach (var directory in directories)
        {
            string destDirName = Path.Combine(configurationDirectory.FullName, directory.Name);
            if (overwrite && Directory.Exists(destDirName))
            {
                Directory.Delete(destDirName, true);
                directory.MoveTo(destDirName);
            }
            else if (Directory.Exists(destDirName) == false)
            {
                directory.MoveTo(destDirName);
            }
        }

        FileInfo[] files = contentsDirectory.GetFiles();
        foreach (var file in files)
        {
            file.MoveTo(configurationDirectory.FullName, overwrite);
        }
        
        tempDir.Delete(true);
    }


    
    public static Uri AppendUri(Uri uri, params string[] paths)
    {
        return new Uri(paths.Aggregate(uri.AbsoluteUri, (current, path) => $"{current.TrimEnd('/')}/{path.TrimStart('/')}"));
    }
}