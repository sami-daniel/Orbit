using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Orbit.Domain.Entities;
using System.Collections.ObjectModel;

namespace Orbit.Application.Dtos.Responses
{
    public class UserReponse
    {
        public uint UserId { get; private set; }

        public string UserName { get; private set; } = null!;

        public string UserEmail { get; private set; } = null!;

        public DateOnly UserDateOfBirth { get; private set; }

        public string UserPassword { get; private set; } = null!;

        public string? UserDescription { get; private set; }

        public byte[]? UserImageByteType { get; private set; }

        public string? UserProfileName { get; private set; }

        public virtual ICollection<UserReponse> Followers { get; private set; } = new List<UserReponse>();

        public virtual ICollection<UserReponse> Users { get; private set; } = new List<UserReponse>();

        public UserReponse(uint userId, string userName, string userEmail, DateOnly userDateOfBirth, string userPassword, string? userDescription, byte[]? userImageByteType, string? userProfileName, ICollection<UserReponse> followers, ICollection<UserReponse> users)
        {
            UserId = userId;
            UserName = userName;
            UserEmail = userEmail;
            UserDateOfBirth = userDateOfBirth;
            UserPassword = userPassword;
            UserDescription = userDescription;
            UserImageByteType = userImageByteType;
            UserProfileName = userProfileName;
            Followers = followers;
            Users = users;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(UserReponse))
            {
                return false;
            }

            var userResponse = obj as UserReponse;
#pragma warning disable CS8602
            return userResponse.UserId == UserId
                   && userResponse.UserName == UserName
                   && userResponse.UserEmail == UserEmail
                   && userResponse.UserDateOfBirth == UserDateOfBirth
                   && userResponse.UserPassword == UserPassword
                   && userResponse.UserDescription == UserDescription
                   && userResponse.UserProfileName == UserProfileName
                   && userResponse.UserImageByteType == UserImageByteType;
#pragma warning restore CS8602
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class UserExtensions
    {
        public static UserReponse ToUserResponse(this User user) =>
            new UserReponse(userId: user.UserId,
                            userName: user.UserName,
                            userEmail: user.UserEmail,
                            userDateOfBirth: user.UserDateOfBirth,
                            userPassword: user.UserPassword,
                            userDescription: user.UserDescription,
                            userImageByteType: user.UserImageByteType,
                            userProfileName: user.UserProfileName,
                            followers: new Collection<UserReponse>(user.Followers.Select(user => user.ToUserResponse()).ToList()),
                            users: new Collection<UserReponse>(user.Users.Select(user => user.ToUserResponse()).ToList()));
    }
}
