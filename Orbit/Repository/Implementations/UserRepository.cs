using Orbit.Data.Contexts;
using Orbit.Models;
using Orbit.Repository.Core.Implementations;
using Orbit.Repository.Interfaces;

namespace Orbit.Repository.Implementations;

/// <summary>
/// Implementação do repositório de Usuários.
/// </summary>
public class UserRepository : Repository<User>, IUserRepository
{
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="UserRepository"/> com o contexto de banco de dados fornecido.
    /// </summary>
    /// <param name="context">O <see cref="ApplicationDbContext"/> a ser fornecido.</param>
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
}
