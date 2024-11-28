using System.Linq.Expressions;
using Orbit.Models;

namespace Orbit.Services.Interfaces;

/// <summary>
/// Interface that defines contracts for user services.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves a user by identifier.
    /// The identifier can be an email or the user's username.
    /// </summary>
    /// <param name="userIdentifier">The user's identifier, which can be an email or the username.</param>
    /// <returns>A task representing the asynchronous operation. The task contains the user corresponding to the identifier.</returns>
    Task<User?> GetUserByIdentifierAsync(string userIdentifier);

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task contains a list of all users.</returns>
    Task<IEnumerable<User>> GetAllUserAsync(Expression<Func<User, bool>>? filter = null, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, string includeProperties = "");

    /// <summary>
    /// Adds a new user to the system.
    /// </summary>
    /// <param name="user">The user to be added.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddUserAsync(User user);

    /// <summary>
    /// Updates a user in the system.
    /// </summary>
    /// <param name="userIdentifier">The user's identifier, which can be an email or the username.</param>
    /// <param name="user">The <see cref="User"/> containing the updated profile data.</param>
    Task UpdateUserAsync(string userIdentifier, User user);

    /// <summary>
    /// Follows a user in the system.
    /// </summary>
    /// <param name="followerUsername">The identifier of the user who will follow.</param>
    /// <param name="userToBeFollowedUserName">The identifier of the user to be followed.</param>
    /// <returns></returns>
    Task FollowUserAsync(string followerUsername, string userToBeFollowedUserName);

    /// <summary>
    /// Unfollows a user in the system.
    /// </summary>
    /// <param name="followerUsername">The identifier of the user who will unfollow.</param>
    /// <param name="userToBeUnfollowedUserName">The identifier of the user to be unfollowed.</param>
    Task UnfollowUserAsync(string followerUsername, string userToBeUnfollowedUserName);
}
