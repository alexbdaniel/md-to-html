using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Conversion.Tests.Creation;

public class DirectoryContextTest
{
    private readonly ITestOutputHelper testConsole;

    public DirectoryContextTest(ITestOutputHelper testConsole)
    {
        this.testConsole = testConsole;
    }

    [Fact]
    public void GetCommandDirectory()
    {
        var paths = new Dictionary<string, string>()
        {
            {"Directory.GetCurrentDirectory()", Directory.GetCurrentDirectory()},
            {"AppDomain.Currentdomain.BaseDirectory", AppDomain.CurrentDomain.BaseDirectory},
            { "this.GetType().Assembly.Location", this.GetType().Assembly.Location},
        };

        foreach (var (command, result) in paths)
        {
            testConsole.WriteLine($"{command}:\n\"{result}\"");
        }
        
        
    }
}