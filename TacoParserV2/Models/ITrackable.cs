namespace LoggingKata.Models;

public interface ITrackable
{
    string Name { get; set; }
    Point Location { get; set; }
}