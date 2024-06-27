using Orbit.Domain.Entities;
using System.Collections.ObjectModel;

namespace Orbit.Application.Dtos.Responses
{
    public class UserResponse
    {
        public uint UserId { get; private set; }

        public string UserName { get; private set; } = null!;

        public string UserEmail { get; private set; } = null!;

        public DateOnly UserDateOfBirth { get; private set; }

        public string UserPassword { get; private set; } = null!;

        public string? UserDescription { get; private set; }

        public byte[]? UserImageByteType { get; private set; }

        public string? UserProfileName { get; private set; }

        public virtual ICollection<UserResponse> Followers { get; private set; } = new List<UserResponse>();

        public virtual ICollection<UserResponse> Users { get; private set; } = new List<UserResponse>();

        public UserResponse(uint userId, string userName, string userEmail, DateOnly userDateOfBirth, string userPassword, string? userDescription, byte[]? userImageByteType, string? userProfileName, ICollection<UserResponse> followers, ICollection<UserResponse> users)
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

            if (obj.GetType() != typeof(UserResponse))
            {
                return false;
            }

            var userResponse = obj as UserResponse;
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
        public static UserResponse ToUserResponse(this User user) =>
            new UserResponse(userId: user.UserId,
                             userName: user.UserName,
                             userEmail: user.UserEmail,
                             userDateOfBirth: user.UserDateOfBirth,
                             userPassword: user.UserPassword,
                             userDescription: user.UserDescription,
                             userImageByteType: user.UserImageByteType,
                             userProfileName: user.UserProfileName,
                             followers: new Collection<UserResponse>(user.Followers
                                 .Select(u => u.ToUserResponse())
                                 .ToList()),
                             users: new Collection<UserResponse>(user.Users
                                 .Select(u => u.ToUserResponse())
                                 .ToList()));
        // FIXME: A utilização de construtores para o mapeamento se deve ao fato de que,
        // o metodo ToUserResponse deve ser estatico, uma vez que a classe User
        // é criada via scaffolding, então sua integridade não é constante.
            
    }
}
