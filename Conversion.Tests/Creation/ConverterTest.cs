#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Conversion.Creation;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

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

        string tempFileFullPath = Path.GetTempFileName();

        var downloadResponse = await client.GetAsync(downloadUri);
        await using (var stream = new FileStream(tempFileFullPath, FileMode.OpenOrCreate))
        {
            await downloadResponse.Content.CopyToAsync(stream);
            stream.Close();
            // console.WriteLine(downloadUri.ToString());
        }
        
        HandleTempFile(tempFileFullPath);
        
        
        
        //https://github.com/alexbdaniel/md-to-html-templates/archive/refs/tags/1.0.2.zip

    }

    private void HandleTempFile(string tempFileFullPath)
    {
        string newFullPath = Path.ChangeExtension(tempFileFullPath, ".zip");
        new FileInfo(tempFileFullPath).MoveTo(newFullPath);
        
        console.WriteLine(newFullPath);

    }
    
    public static Uri AppendUri(Uri uri, params string[] paths)
    {
        return new Uri(paths.Aggregate(uri.AbsoluteUri, (current, path) => $"{current.TrimEnd('/')}/{path.TrimStart('/')}"));
    }
}