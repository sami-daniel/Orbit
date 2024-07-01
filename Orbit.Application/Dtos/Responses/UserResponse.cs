using Orbit.Domain.Entities;

namespace Orbit.Application.Dtos.Responses
{
    public class UserResponse
    {
        public uint UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
        public string UserProfileName { get; set; } = null!
        public bool IsPrivateProfile { get; set; } = null!
        public byte[]? UserImageByteType { get; set; }
        public string? UserDescription { get; set; }
        public ICollection<UserResponse> Followers { get; set; }
        public ICollection<UserResponse> Users { get; set; }
    }

    public static class UserExtensions
    {
        public static UserResponse ToUserResponse(this User user)
        {
            return user.ToUserResponseInternal([]);
        }

        private static UserResponse ToUserResponseInternal(this User user, List<uint> processedUserIds)
        {
            if (processedUserIds.Contains(user.UserId))
            {
                return new UserResponse
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    UserEmail = user.UserEmail,
                    UserPassword = user.UserPassword,
                    UserProfileName = user.UserProfileName,
                    IsPrivateProfile = user.IsPrivateProfile,
                    UserImageByteType = user.UserImageByteType,
                    UserDescription = user.UserDescription,
                    Followers = [],
                    Users = []
                };
            }

            processedUserIds.Add(user.UserId);

            UserResponse response = new()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                UserPassword = user.UserPassword,
                UserProfileName = user.UserProfileName,
                IsPrivateProfile = user.IsPrivateProfile,
                UserDescription = user.UserDescription,
                UserImageByteType = user.UserImageByteType,
                Followers = user.Followers.Select(u => u.ToUserResponseInternal(new List<uint>(processedUserIds))).ToList(),
                Users = user.Users.Select(u => u.ToUserResponseInternal(new List<uint>(processedUserIds))).ToList()
            };

            return response;
        }
    }

}
