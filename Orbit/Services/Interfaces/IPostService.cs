using Orbit.Data.Contexts;
using Orbit.Models;

namespace Orbit.Services.Interfaces;

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

    /// <summary>
    /// Obtém uma lista de postagens paginadas.
    /// </summary>
    /// <param name="skip">A quantidade de postagens a serem ignoradas.</param>
    /// <param name="take">A quantidade de postagens a serem retornadas.</param>
    /// <param name="applicationDbContext">O contexto do banco de dados.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma lista de postagens.</returns>
    Task<IEnumerable<Post>> GetPaginatedPostAsync(int skip, int take);

    /// <summary>
    /// Obtém uma lista de postagens de um usuário, de acordo com suas preferencias.
    /// </summary>
    /// <param name="userName">O nome do usuário.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma lista de postagens.</returns>
    Task<IEnumerable<Post>> GetPostsRandomizedByUserPreferenceAsync(string username);

    /// <summary>
    /// Obtém uma postagem por seu identificador.
    /// </summary>
    /// <param name="postId">O identificador da postagem.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém a postagem. Nulo caso o post não exista.</returns>
    Task<Post?> GetPostByIdAsync(uint postId);
}
