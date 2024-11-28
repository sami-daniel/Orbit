namespace Orbit.DTOs.Responses;

public class UserResponse
{
    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string UserEmail { get; set; } = null!;

    /// <summary>
    /// Gets or sets the profile name of the user.
    /// </summary>
    public string UserProfileName { get; set; } = null!;

    /// <summary>
    /// Gets or sets a flag indicating whether the user's profile is private.
    /// </summary>
    public bool IsPrivateProfile { get; set; } = false;

    /// <summary>
    /// Gets or sets the byte array representing the user's profile image.
    /// </summary>
    public byte[]? UserProfileImageByteType { get; set; }

    /// <summary>
    /// Gets or sets the byte array representing the user's profile banner image.
    /// </summary>
    public byte[]? UserProfileBannerImageByteType { get; set; }

    /// <summary>
    /// Gets or sets a description of the user.
    /// </summary>
    public string? UserDescription { get; set; }

    /// <summary>
    /// Gets or sets a collection of followers of the user.
    /// </summary>
    public ICollection<UserResponse> Followers { get; set; } = [];

    /// <summary>
    /// Gets or sets a collection of users that the user is following.
    /// </summary>
    public ICollection<UserResponse> Users { get; set; } = [];
}
