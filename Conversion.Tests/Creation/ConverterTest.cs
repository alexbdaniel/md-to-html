using System;
using Conversion.Creation;
using JetBrains.Annotations;
using Xunit;

namespace Conversion.Tests.Creation;

[TestSubject(typeof(Converter))]
public class ConverterTest
{

    [Fact]
    public void METHOD()
    {
        string appDataDirectoryName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    }
}