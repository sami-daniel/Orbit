using Orbit.Domain.Entities;

namespace Orbit.Application.Dtos.Responses
{
    public class UserReponse
    {
        public int UserId { get; private set; }

        public string UserName { get; private set; } = null!;

        public string UserEmail { get; private set; } = null!;

        public DateOnly UserDateOfBirth { get; private set; }

        public string UserPassword { get; private set; } = null!;

        public UserReponse(int userId, string userName, string userEmail, DateOnly userDateOfBirth, string userPassword)
        {
            UserId = userId;
            UserName = userName;
            UserEmail = userEmail;
            UserDateOfBirth = userDateOfBirth;
            UserPassword = userPassword;
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
                   && userResponse.UserPassword == UserPassword;
#pragma warning restore CS8602
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class UserExtensions
    {
        public static UserReponse ToUserReponse(this User user) =>
            new UserReponse(userId: user.UserId,
                            userName: user.UserName,
                            userEmail: user.UserEmail,
                            userDateOfBirth: user.UserDateOfBirth,
                            userPassword: user.UserPassword
                            );
    }
}
