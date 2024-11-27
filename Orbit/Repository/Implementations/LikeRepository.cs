using Orbit.Data.Contexts;
using Orbit.Models;
using Orbit.Repository.Core.Implementations;
using Orbit.Repository.Interfaces;

namespace Orbit.Repository.Implementations;

/// <summary>
/// Implementação do repositório de Likes.
/// </summary>
public class LikeRepository : Repository<Like>, ILikeRepository
{
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="LikeRepository"/> com o contexto de banco de dados fornecido.
    /// </summary>
    /// <param name="context">O <see cref="ApplicationDbContext"/> a ser fornecido.</param>
    public LikeRepository(ApplicationDbContext context) : base(context)
    {
    }
}
