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
            var usernames = await _unitOfWork.User.FindAsync(user => user.UserName == userAddRequest.UserName);

            if (users.Any())
            {
                throw new ArgumentException("E-mail já cadastrado anteriormente!");
            }

            if (usernames.Any())
            {
                throw new ArgumentException("Username já cadastrado anteriormente!");
            }
            var user = userAddRequest.ToUser();

            await _unitOfWork.User.AddAsync(user);

            _unitOfWork.Complete();

            return user.ToUserReponse();
        }

        public async Task<IEnumerable<UserReponse>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.User.GetAllAsync();
            var usersResponses = new List<UserReponse>();
            foreach (var user in users)
            { 
                usersResponses.Add(user.ToUserReponse());
            }

            return usersResponses;
        }
    }
}