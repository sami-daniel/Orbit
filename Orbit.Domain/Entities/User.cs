namespace Orbit.Domain.Entities;

public partial class User
{
    public uint UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public string UserPassword { get; set; } = null!;
    public string UserProfileName { get; set; } = null!
    public bool IsPrivateProfile { get; set; } = null!
    public string? UserDescription { get; set; }
    public byte[]? UserImageByteType { get; set; }
    public virtual ICollection<User> Followers { get; set; } = [];
    public virtual ICollection<User> Users { get; set; } = [];
}
