namespace Orbit.Models;

public class ChatContext
{
    public User Host { get; set; } = null!;
    public User? Guest { get; set; }
}
