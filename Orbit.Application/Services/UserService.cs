using Orbit.Application.Dtos.Requests;
using Orbit.Application.Dtos.Responses;
using Orbit.Application.Helpers;
using Orbit.Application.Interfaces;
using Orbit.Infrastructure.Repositories.Interfaces;

namespace Orbit.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserReponse> AddUserAsync(UserAddRequest userAddRequest)
        {
            ArgumentNullException.ThrowIfNull(nameof(userAddRequest));

            if (!ValidationHelper.IsValid(userAddRequest))
            {
                throw new ArgumentException("Dados invalidos para o usuario!");
            }

            var users = await _unitOfWork.User.FindAsync(user => user.UserEmail == userAddRequest.UserEmail);

            if(users.Any())
            {
                throw new ArgumentException("E-mail já cadastrado anteriormente!");
            }

            var user = userAddRequest.ToUser();

            await _unitOfWork.User.AddAsync(user);

            _unitOfWork.Complete();

            return user.ToUserReponse();
        }

        public async Task<UserReponse?> GetUserByUserIdAsync(int userId)
        {
            if (userId <= 0)
            {
                return null;
            }

            var user = await _unitOfWork.User.FindAsync(user => user.UserId == userId);

            if (user == null)
            {
                return null;
            }

            return user.FirstOrDefault(user => user.UserId == userId)?.ToUserReponse();
        }
    }
}