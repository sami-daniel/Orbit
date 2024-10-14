using Orbit.Domain.Entities;
using Orbit.Infrastructure.Data.Contexts;
using Orbit.Infrastructure.Repository.Core.Implementations;
using Orbit.Infrastructure.Repository.Interfaces;

namespace Orbit.Infrastructure.Repository.Implementations;

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
