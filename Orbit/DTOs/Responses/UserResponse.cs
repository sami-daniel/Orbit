namespace Orbit.DTOs.Responses
{
    public class UserResponse
    {
        public uint UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string UserProfileName { get; set; } = null!;
        public bool IsPrivateProfile { get; set; } = false;
        public byte[]? UserImageByteType { get; set; }
        public byte[]? UserBannerByteType { get; set; }
        public string? UserDescription { get; set; }
        public ICollection<UserResponse> Followers { get; set; } = [];
        public ICollection<UserResponse> Users { get; set; } = [];
    }
}
