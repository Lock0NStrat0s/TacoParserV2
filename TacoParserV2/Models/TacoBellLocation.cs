namespace TacoParserV2.Models;

public class TacoBellLocation : ITrackable
{
    public string Name { get; set; }
    public Point Location { get; set; }
}
