using Orbit.Models;

namespace Orbit.Services.Interfaces;

public interface ILikeService
{
    /// <summary>
    /// Curte um post.
    /// </summary>
    /// <param name="postID">ID do post.</param>
    /// <param name="username">Nome de usuário.</param>
    /// <returns>Uma tarefa que representa a operação assincrona.</returns>
    public Task LikePost(uint postID, string username);

    /// <summary>
    /// Descurte um post.
    /// </summary>
    /// <param name="postID">ID do post.</param>
    /// <param name="username">Nome de usuário.</param>
    /// <returns>Uma tarefa que representa a operação assincrona.</returns>
    public Task UnlikePost(uint postID, string username);

    /// <summary>
    /// Obtém as curtidas de um usuário em um post.
    /// </summary>
    /// <param name="username">Nome de usuário.</param>
    /// <returns>Uma tarefa que representa a operação assincrona.</returns>
    public Task<IEnumerable<Like>> GetLikesFromUser(string username);
}