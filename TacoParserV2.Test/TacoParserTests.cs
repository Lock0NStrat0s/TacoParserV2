using System;
using System.Diagnostics.Metrics;
using TacoParserV2;
using Xunit;
using static TacoParserV2.Responses.SingleResponseTacoBellLocationResponse;

namespace TacoParserTests;

public class TacoParserTests
{
    [Fact]
    public void ShouldReturnNonNullObject()
    {
        //Arrange
        var tacoParser = new TacoParser();
        //Act
        var actual = tacoParser.Parse("Marshall County AL 35950 United States of America,34.2739,-86.2064");
        //Assert
        Assert.NotNull(actual);
    }

    [Theory]
    [InlineData("Marshall County AL 35950 United States of America,34.2739,-86.2064", -86.2064)]
    [InlineData("1740 Gunter Avenue Mill Village Guntersville AL 35976 United States of America,34.3420432,-86.3082118", -86.3082118)]
    [InlineData("Taco Bell 710 9th Avenue North Skyview Bessemer AL 35020 United States of America,33.3936458,-86.9726679", -86.9726679)]
    public void ShouldParseLongitude(string line, double expected)
    {
        //Arrange
        var tacoParser = new TacoParser();
        //Act
        var actual = tacoParser.Parse(line).Location.Longitude;
        //Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("Marshall County AL 35950 United States of America,34.2739,-86.2064", 34.2739)]
    [InlineData("1740 Gunter Avenue Mill Village Guntersville AL 35976 United States of America,34.3420432,-86.3082118", 34.3420432)]
    [InlineData("Taco Bell 710 9th Avenue North Skyview Bessemer AL 35020 United States of America,33.3936458,-86.9726679", 33.3936458)]
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
    [InlineData("Marshall County AL 35950 United States of America,34.2739,-86.2064", "Marshall County AL 35950 United States of America")]
    [InlineData("1740 Gunter Avenue Mill Village Guntersville AL 35976 United States of America,34.3420432,-86.3082118", "1740 Gunter Avenue Mill Village Guntersville AL 35976 United States of America")]
    [InlineData("Taco Bell 710 9th Avenue North Skyview Bessemer AL 35020 United States of America,33.3936458,-86.9726679", "Taco Bell 710 9th Avenue North Skyview Bessemer AL 35020 United States of America")]
    public void ShouldParseStoreAddress(string line, string expected)
    {
        //Arrange
        var tacoParser = new TacoParser();
        //Act
        var actual = tacoParser.Parse(line).Name;
        //Assert
        Assert.Equal(expected, actual);
    }
}
