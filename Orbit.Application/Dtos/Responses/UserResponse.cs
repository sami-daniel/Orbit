using Orbit.Domain.Entities;
using System.Collections.ObjectModel;

namespace Orbit.Application.Dtos.Responses
{
    public class UserResponse
    {
        public uint UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public DateOnly UserDateOfBirth { get; set; }
        public string UserPassword { get; set; } = null!;
        public string? UserDescription { get; set; }
        public byte[]? UserImageByteType { get; set; }
        public string? UserProfileName { get; set; }
        public ICollection<UserResponse>? Followers { get; set; }
        public ICollection<UserResponse>? Users { get; set; }
    }

    public static class UserExtensions
    {
        public static UserResponse ToUserResponse(this User user)
        {
            return user.ToUserResponseInternal(new List<uint>());
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
                    UserDateOfBirth = user.UserDateOfBirth,
                    UserPassword = user.UserPassword,
                    UserDescription = user.UserDescription,
                    UserProfileName = user.UserProfileName,
                    UserImageByteType = user.UserImageByteType,
                    Followers = new List<UserResponse>(),
                    Users = new List<UserResponse>()
                };
            }

            processedUserIds.Add(user.UserId);

            var response = new UserResponse
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                UserDateOfBirth = user.UserDateOfBirth,
                UserPassword = user.UserPassword,
                UserDescription = user.UserDescription,
                UserProfileName = user.UserProfileName,
                UserImageByteType = user.UserImageByteType,
                Followers = user.Followers.Select(u => u.ToUserResponseInternal(new List<uint>(processedUserIds))).ToList(),
                Users = user.Users.Select(u => u.ToUserResponseInternal(new List<uint>(processedUserIds))).ToList()
            };

            return response;
        }
    }

}
