using Orbit.Models;

namespace Orbit.Services.Interfaces;

public interface ILikeService
{
    /// <summary>
    /// Likes a post.
    /// </summary>
    /// <param name="postID">The ID of the post.</param>
    /// <param name="username">The username of the user liking the post.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task LikePost(uint postID, string username);

    /// <summary>
    /// Unlikes a post.
    /// </summary>
    /// <param name="postID">The ID of the post.</param>
    /// <param name="username">The username of the user unliking the post.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task UnlikePost(uint postID, string username);

    /// <summary>
    /// Retrieves the likes of a user on a post.
    /// </summary>
    /// <param name="username">The username of the user whose likes are being retrieved.</param>
    /// <returns>A task representing the asynchronous operation. The result contains a list of likes from the user.</returns>
    public Task<IEnumerable<Like>> GetLikesFromUser(string username);
}
