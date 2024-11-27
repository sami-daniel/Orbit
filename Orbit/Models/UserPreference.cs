namespace Orbit.Models;

public class UserPreference
{
    public uint PreferenceId { get; set; }
    public string PreferenceName { get; set; } = null!;

    public uint UserId { get; set; }
    public User User { get; set; } = null!;
}