using Orbit.Application.Dtos.Requests;
using Orbit.Application.Dtos.Responses;

namespace Orbit.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserReponse> AddUserAsync(UserAddRequest userAddRequest);
        Task<UserReponse?> GetUserByUserIdAsync(int userId);
        Task<IEnumerable<UserReponse>> GetAllUsersAsync();
    }
}
