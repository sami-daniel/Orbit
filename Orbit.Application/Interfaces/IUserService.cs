using Orbit.Application.Dtos.Requests;
using Orbit.Application.Dtos.Responses;

namespace Orbit.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> AddUserAsync(UserAddRequest userAddRequest);

        [Obsolete("Esse metodo retorna uma lista completa de todos os usuários. A iteração sobre essa coleção causará instabilidade e uso excessivo de recursos. Use FindAsync em vez de GetAllUsersAsync")]
        Task<IEnumerable<UserResponse>> GetAllUsersAsync();

        [Obsolete("Esse metodo retorna uma lista completa de todos os usuários. A iteração sobre essa coleção causará instabilidade e uso excessivo de recursos. Use FindAsync em vez de GetAllUsersAsync()")]
        Task<IEnumerable<UserResponse>> GetAllUsersAsync(params string[] navProperties);

        Task<IEnumerable<UserResponse>> FindUsersAsync(object conditions);

        Task<IEnumerable<UserResponse>> FindUsersAsync(object conditions, params string[] navProperties);
    }
}
