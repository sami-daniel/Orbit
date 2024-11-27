namespace Orbit.Models;

public partial class User
{
    public uint UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string UserProfileName { get; set; } = null!;

    public string? UserDescription { get; set; }

    public byte[]? UserProfileImageByteType { get; set; }

    public byte[]? UserProfileBannerImageByteType { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<User> Followers { get; set; } = new List<User>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        User user = (User)obj;

        return UserId == user.UserId
               && UserName == user.UserName
               && UserEmail == user.UserEmail
               && UserPassword == user.UserPassword
               && UserProfileName == user.UserProfileName
               && UserDescription == user.UserDescription
               && UserProfileImageByteType == user.UserProfileImageByteType
               && UserProfileBannerImageByteType == user.UserProfileBannerImageByteType;
    }

    public override int GetHashCode() 
    {
        return HashCode.Combine(UserId,
                                UserName,
                                UserEmail,
                                UserPassword,
                                UserProfileName,
                                UserDescription,
                                UserProfileImageByteType,
                                UserProfileBannerImageByteType);
    }
}
