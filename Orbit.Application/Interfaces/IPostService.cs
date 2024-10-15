using Orbit.Domain.Entities;
using Orbit.Infrastructure.Data.Contexts;

namespace Orbit.Application.Interfaces;

/// <summary>
/// Interface que define contratos para o serviço de Postagem
/// </summary>
public interface IPostService
{
    /// <summary>
    /// Adiciona um novo post ao sistema.
    /// </summary>
    /// <param name="post">O post a ser adicionado.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    Task AddPostAsync(Post post, string postOwnerName);

    Task<IEnumerable<Post>> GetPaginatedPostAsync(int skip, int take, ApplicationDbContext applicationDbContext);
}
