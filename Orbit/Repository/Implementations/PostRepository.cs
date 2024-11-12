using Orbit.Data.Contexts;
using Orbit.Models;
using Orbit.Repository.Core.Implementations;
using Orbit.Repository.Interfaces;

namespace Orbit.Repository.Implementations;

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
