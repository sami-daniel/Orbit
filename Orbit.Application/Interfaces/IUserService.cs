using Orbit.Application.Dtos.Requests;
using Orbit.Application.Dtos.Responses;

namespace Orbit.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> AddUserAsync(UserAddRequest userAddRequest);
        Task<IEnumerable<UserResponse>> GetAllUsersAsync();
    }
}
