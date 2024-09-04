using System.Drawing;
using System.Linq.Expressions;
using System.Reflection;
using Orbit.Application.Exceptions;
using Orbit.Application.Interfaces;
using Orbit.Domain.Entities;
using Orbit.Infrastructure.UnitOfWork.Interfaces;

namespace Orbit.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task AddUserAsync(User user)
        {

        }
        public Task<User> GetAllUserAsync(Expression<Func<User, bool>>? filter = null, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, string includeProperties = "") => throw new NotImplementedException();
        public Task<User?> GetUserByIdentifierAsync(string userIdentifier) => throw new NotImplementedException();
        public Task UpdateUserAsync(string userIdentifier, User user) => throw new NotImplementedException();

        private static void ValidateUser(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            ArgumentNullException.ThrowIfNullOrWhiteSpace(user.UserName);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(user.UserProfileName);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(user.UserPassword);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(user.UserEmail);

            if (user.UserName.Length > 255)
            {
                throw new ArgumentException("O nome de usuário não pode ter mais de 255 caracteres.");
            }

            if (user.UserEmail.Length > 255)
            {
                throw new ArgumentException("O email do usuário não pode ter mais de 255 caracteres.");
            }

            if (user.UserProfileName.Length > 255)
            {
                throw new ArgumentException("O nome do perfil do usuário não pode ter mais de 255 caracteres.");
            }

            if (user.UserPassword.Length > 255)
            {
                throw new ArgumentException("A senha do usuário não pode ter mais de 255 caracteres.");
            }

            if (user.UserDescription != null && user.UserDescription.Length > 65535)
            {
                throw new ArgumentException("A descrição do usuário não pode ter mais de 65535 caracteres.");
            }

            // Verifing valid images
            if (user.UserImageByteType != null)
            {
                try
                {
                    using (var ms = new MemoryStream(user.UserImageByteType))
                    {
#pragma warning disable CA1416 // Validate platform compatibility
                        Image.FromStream(ms);
#pragma warning restore CA1416 // Validate platform compatibility
                    }
                }
                catch (Exception)
                {
                    throw new InvalidImageException("A imagem do usuário é inválida.");
                }
            }

            if (user.UserBannerByteType != null)
            {
                try
                {
                    using (var ms = new MemoryStream(user.UserBannerByteType))
                    {
#pragma warning disable CA1416 // Validate platform compatibility
                        Image.FromStream(ms);
#pragma warning restore CA1416 // Validate platform compatibility
                    }
                }
                catch (Exception)
                {
                    throw new InvalidImageException("A imagem de banner do usuário é inválida.");
                }
            }
        }
    }
}
