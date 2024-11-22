using System;
using System.Runtime.InteropServices;
using Conversion.Creation;
using JetBrains.Annotations;
using Xunit;

namespace Conversion.Tests.Creation;

[TestSubject(typeof(Saver))]
public class SaverTest
{
    private static readonly bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    [Theory]
    [InlineData("/Documents", false, true)]
    [InlineData("Documents", false, true)]
    [InlineData(".", false, true)]
    public void FullPathCheckReturnsExpectedResult(string path, bool expectedFull, bool expectedValid)
    {
      
        
        if (expectedValid)
        {
            Assert.Equal(expected: expectedFull, path.IsFullPath());
        }
        else if (expectedValid == false)
        {
            Assert.Throws<Exception>(() => path.IsFullPath());
        }
        
        Assert.Fail($"{path} did not match any test conditon.");
       
        
       
    }
}