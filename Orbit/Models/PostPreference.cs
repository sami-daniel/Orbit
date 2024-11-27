namespace Orbit.Models;

public class PostPreference
{
    public uint PreferenceId { get; set; }
    public string PreferenceName { get; set; } = null!;
    
    public uint PostId { get; set; }
    public Post Post { get; set; } = null!;
}