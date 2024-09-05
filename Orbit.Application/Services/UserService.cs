using System.Linq.Expressions;
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

        public async Task AddUserAsync(User user)
        {
            try
            {
                UserServiceHelpers.ValidateUser(user);
            }
            catch (Exception)
            {
                throw;
            }

            using (var transaction = await _unitOfWork.StartTransactionAsync())
            {
                try
                {
                    await _unitOfWork.UserRepository.InsertAsync(user);
                    await _unitOfWork.CompleteAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();

                    throw new UserAlredyExistsException("O usuário com esse identificador já existe!");
                }
            }
        }

        public async Task<IEnumerable<User>> GetAllUserAsync(Expression<Func<User, bool>>? filter = null, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, string includeProperties = "")
        {
            return await _unitOfWork.UserRepository.GetAsync(filter, orderBy, includeProperties);
        }

        public async Task<User?> GetUserByIdentifierAsync(string userIdentifier)
        {
            IEnumerable<User>? users = null;

            if (userIdentifier.Contains('@'))
            {
                users = await _unitOfWork.UserRepository.GetAsync(u => u.UserEmail ==  userIdentifier);
            }

            users = await _unitOfWork.UserRepository.GetAsync(u => u.UserName == userIdentifier);

            return users.FirstOrDefault();
        }
        public async Task UpdateUserAsync(string userIdentifier, User updatedUser)
        {
            IEnumerable<User>? users = null;

            if (userIdentifier.Contains('@'))
            {
                users = await _unitOfWork.UserRepository.GetAsync(u => u.UserEmail == userIdentifier);
            }

            users = await _unitOfWork.UserRepository.GetAsync(u => u.UserName == userIdentifier);

            var user = users.FirstOrDefault();

            if (user != null)
            {
                user.UserEmail = updatedUser.UserEmail;
                user.UserDescription = updatedUser.UserDescription;
                user.UserProfileImageByteType = updatedUser.UserProfileImageByteType;
                user.UserProfileName = updatedUser.UserProfileName;
                user.UserPassword = updatedUser.UserPassword;
                user.UserName = updatedUser.UserName;

                using (var transaction = await _unitOfWork.StartTransactionAsync())
                {
                    try
                    {
                        await _unitOfWork.CompleteAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();

                        throw new UserAlredyExistsException("O usuário com esse identificador já existe!");
                    }
                }
            }
            else
            {
                throw new UserNotFoundException("O usuário com esse identificador não foi encontrado!");
            }
        }
    }
}
