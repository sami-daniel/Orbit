using Orbit.Domain.Entities;
using Orbit.Infrastructure.Data.Contexts;
using Orbit.Infrastructure.Repository.Core.Implementations;
using Orbit.Infrastructure.Repository.Interfaces;

namespace Orbit.Infrastructure.Repository.Implementations;

/// <summary>
/// Implementação do repositório de Posts.
/// </summary>
public class PostRepository : Repository<Post>, IPostRepository
{
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="PostRepository"/> com o contexto de banco de dados fornecido.
    /// </summary>
    /// <param name="context">O <see cref="ApplicationDbContext"/> a ser fornecido.</param>
    public PostRepository(ApplicationDbContext context) : base(context)
    {
    }
}
