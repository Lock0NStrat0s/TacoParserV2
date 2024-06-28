using System;
using TacoParserV2;
using Xunit;

namespace TacoParserTests;

public class TacoParserTests
{
    [Fact]
    public void ShouldReturnNonNullObject()
    {
        //Arrange
        var tacoParser = new TacoParser();

        //Act
        var actual = tacoParser.Parse("34.073638,-84.677017,Taco Bell Acwort...");

        //Assert
        Assert.NotNull(actual);

    }

    [Theory]
    [InlineData("34.073638,-84.677017,Taco Bell Acwort...", -84.677017)]
    [InlineData("34.035985,-84.683302,Taco Bell Acworth...", -84.683302)]
    [InlineData("34.087508,-84.575512,Taco Bell Acworth...", -84.575512)]
    //Add additional inline data. Refer to your CSV file.
    public void ShouldParseLongitude(string line, double expected)
    {
        // TODO: Complete the test with Arrange, Act, Assert steps below.
        //       Note: "line" string represents input data we will Parse 
        //       to extract the Longitude.  
        //       Each "line" from your .csv file
        //       represents a TacoBell location

        //Arrange
        var tacoParser = new TacoParser();
        //Act
        var actual = tacoParser.Parse(line).Location.Longitude;
        //Assert
        Assert.Equal(expected, actual);
    }


    //TODO: Create a test called ShouldParseLatitude
    [Theory]
    [InlineData("34.073638,-84.677017,Taco Bell Acwort...", 34.073638)]
    [InlineData("34.035985,-84.683302,Taco Bell Acworth...", 34.035985)]
    [InlineData("34.087508,-84.575512,Taco Bell Acworth...", 34.087508)]
    public void ShouldParseLatitude(string line, double expected)
    {
        //Arrange
        var tacoParser = new TacoParser();
        //Act
        var actual = tacoParser.Parse(line).Location.Latitude;
        //Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("34.073638,-84.677017,Taco Bell Acwort...", "Taco Bell Acwort...")]
    [InlineData("34.035985,-84.683302,Taco Bell Acworth...", "Taco Bell Acworth...")]
    [InlineData("34.087508,-84.575512,Taco Bell Acworth...", "Taco Bell Acworth...")]
    public void ShouldParseStoreName(string line, string expected)
    {
        //Arrange
        var tacoParser = new TacoParser();
        //Act
        var actual = tacoParser.Parse(line).Name;
        //Assert
        Assert.Equal(expected, actual);
    }
}
