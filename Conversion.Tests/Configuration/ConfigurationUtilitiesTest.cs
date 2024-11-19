using Conversion.Configuration;
using JetBrains.Annotations;
using Xunit;

namespace Conversion.Tests.Configuration;

[TestSubject(typeof(ConfigurationUtilities))]
public class ConfigurationUtilitiesTest
{

    [Fact]
    public void ConfigurationDirectoryIsRetrieved()
    {
        var directory = ConfigurationUtilities.GetConfigurationDirectory();
        
        Assert.NotNull(directory);
    }
}