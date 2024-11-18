namespace Orbit.Models;

public class Message
{
    public string Content { get; set; } = null!;
    public string To { get; set; } = null!;
    public DateTime TimeStamp { get; set; }
}