namespace TacoParserV2.Models;

public interface ITrackable
{
    string Name { get; set; }
    Point Location { get; set; }
}