using Orbit.Application.Dtos.Requests;
using Orbit.Application.Dtos.Responses;
using System.Linq.Expressions;

namespace Orbit.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> AddUserAsync(UserAddRequest userAddRequest);
        Task<IEnumerable<UserResponse>> GetAllUsersAsync();
        Task<IEnumerable<UserResponse>> GetAllUsersAsync(params string[] navProperties);
        Task<IEnumerable<UserResponse>> FindUsersAsync(Expression<Func<UserResponse, bool>> predicate);
    }
}
