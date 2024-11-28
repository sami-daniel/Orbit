using Orbit.Data.Contexts;
using Orbit.Models;

namespace Orbit.Services.Interfaces;

/// <summary>
/// Interface that defines contracts for the Post service.
/// </summary>
public interface IPostService
{
    /// <summary>
    /// Adds a new post to the system.
    /// </summary>
    /// <param name="post">The post to be added.</param>
    /// <param name="postOwnerName">The name of the post's owner.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddPostAsync(Post post, string postOwnerName);

    /// <summary>
    /// Retrieves a list of paginated posts.
    /// </summary>
    /// <param name="skip">The number of posts to skip.</param>
    /// <param name="take">The number of posts to return.</param>
    /// <param name="applicationDbContext">The database context.</param>
    /// <returns>A task representing the asynchronous operation. The result of the task contains a list of posts.</returns>
    Task<IEnumerable<Post>> GetPaginatedPostAsync(int skip, int take);

    /// <summary>
    /// Retrieves a list of posts from a user, based on their preferences.
    /// </summary>
    /// <param name="userName">The username.</param>
    /// <returns>A task representing the asynchronous operation. The result of the task contains a list of posts.</returns>
    Task<IEnumerable<Post>> GetPostsRandomizedByUserPreferenceAsync(string username);

    /// <summary>
    /// Retrieves a post by its identifier.
    /// </summary>
    /// <param name="postId">The identifier of the post.</param>
    /// <returns>A task representing the asynchronous operation. The result of the task contains the post, or null if the post does not exist.</returns>
    Task<Post?> GetPostByIdAsync(uint postId);
}
