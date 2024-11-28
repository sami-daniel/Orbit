using System.Collections.Frozen;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Orbit.Models;
using Orbit.Services.Exceptions;
using Orbit.Services.Helpers;
using Orbit.Services.Interfaces;
using Orbit.UnitOfWork.Interfaces;

namespace Orbit.Services.Implementations;

/// <summary>
/// Service to manage operations related to users.
/// </summary>
public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work for repository access.</param>
    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Adds a new user asynchronously.
    /// </summary>
    /// <param name="user">The User object to be added.</param>
    /// <exception cref="UserAlredyExistsException">Thrown when a user with the same identifier already exists.</exception>
    public async Task AddUserAsync(User user)
    {

        try
        {
            UserServiceHelpers.ValidateUser(user);
        }
        catch (Exception)
        {
            throw;
        }

        user.UserEmail = user.UserEmail.ToLower();

        try
        {
            await _unitOfWork.UserRepository.InsertAsync(user);
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateException ex)
        when (ex.InnerException!.Message.Contains(user.UserEmail, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new UserAlredyExistsException($"The user with email {user.UserEmail} already exists!");
        }
        catch (DbUpdateException ex)
        when (ex.InnerException!.Message.Contains(user.UserName, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new UserAlredyExistsException($"The user with identifier {user.UserName} already exists!");
        }

    }

    /// <summary>
    /// Gets all users asynchronously.
    /// </summary>
    /// <param name="filter">Expression to filter the users.</param>
    /// <param name="orderBy">Function to order the users.</param>
    /// <param name="includeProperties">Properties to be included in the query.</param>
    /// <returns>A collection of User objects.</returns>
    public async Task<IEnumerable<User>> GetAllUserAsync(Expression<Func<User, bool>>? filter = null, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, string includeProperties = "")
    {
        return await _unitOfWork.UserRepository.GetAsync(filter, orderBy, includeProperties);
    }

    /// <summary>
    /// Gets a user by their identifier asynchronously.
    /// </summary>
    /// <param name="userIdentifier">The identifier of the user (username or email).</param>
    /// <returns>The corresponding User object or null if not found.</returns>
    public async Task<User?> GetUserByIdentifierAsync(string userIdentifier)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userIdentifier, nameof(userIdentifier));
        IEnumerable<User>? users = null;

        if (userIdentifier.Contains('@'))
        {
            users = await _unitOfWork.UserRepository.GetAsync(u => u.UserEmail.ToLower() == userIdentifier);
        }
        else
        {
            users = await _unitOfWork.UserRepository.GetAsync(u => u.UserName == userIdentifier);
        }

        return users.FirstOrDefault();
    }

    /// <summary>
    /// Updates an existing user asynchronously.
    /// </summary>
    /// <param name="userIdentifier">The identifier of the user (username or email).</param>
    /// <param name="updatedUser">The User object with updated information.</param>
    /// <exception cref="UserAlredyExistsException">Thrown when a user with the same identifier already exists.</exception>
    /// <exception cref="UserNotFoundException">Thrown when the user with the provided identifier is not found.</exception>
    public async Task UpdateUserAsync(string userIdentifier, User updatedUser)
    {
        IEnumerable<User>? users = null;

        try
        {
            UserServiceHelpers.ValidateUser(updatedUser);
        }
        catch (Exception)
        {
            throw;
        }

        if (userIdentifier.Contains('@'))
        {
            users = await _unitOfWork.UserRepository.GetAsync(u => u.UserEmail == userIdentifier);
        }

        users = await _unitOfWork.UserRepository.GetAsync(u => u.UserName == userIdentifier);

        var user = users.FirstOrDefault();

        if (user != null)
        {
            user.UserEmail = updatedUser.UserEmail;
            user.UserDescription = updatedUser.UserDescription;
            user.UserProfileImageByteType = updatedUser.UserProfileImageByteType;
            user.UserProfileName = updatedUser.UserProfileName;
            user.UserPassword = updatedUser.UserPassword;
            user.UserName = updatedUser.UserName;

            using (var transaction = await _unitOfWork.StartTransactionAsync())
            {
                try
                {
                    await _unitOfWork.CompleteAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();

<<<<<<< HEAD
                    throw new UserAlredyExistsException("The user with this identifier already exists!");
=======
                    throw new UserAlredyExistsException("O usuário com esse identificador já está em uso!");
>>>>>>> 35189761d3d1ce18be8ee88be049a8f9dcaf53a6
                }
            }
        }
        else
        {
            throw new UserNotFoundException("The user with this identifier was not found!");
        }
    }

    public async Task FollowUserAsync(string followerUsername, string userToBeFollowedUserName)
    {
        var follower = await _unitOfWork.UserRepository.GetAsync(u => u.UserName == followerUsername, includeProperties: "Users");
        var followedUser = await _unitOfWork.UserRepository.GetAsync(u => u.UserName == userToBeFollowedUserName, includeProperties: "Followers");

        if (!follower.Any())
        {
            throw new UserNotFoundException($"The user with username '{followerUsername} not exists on data source.");
        }

        if (!followedUser.Any())
        {
            throw new UserNotFoundException($"The user with username '{userToBeFollowedUserName} not exists on data source.");
        }

        using (var transaction = await _unitOfWork.StartTransactionAsync())
        {
            try
            {
                followedUser.First().Followers.Add(follower.First());
                follower.First().Users.Add(followedUser.First());
                await _unitOfWork.CompleteAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException)
            {
                // Fail quietly
                await transaction.RollbackAsync();
            }
        }
    }

    public async Task UnfollowUserAsync(string followerUsername, string userToBeUnfollowedUserName)
    {
        var follower = await _unitOfWork.UserRepository.GetAsync(u => u.UserName == followerUsername, includeProperties: "Users");
        var followedUser = await _unitOfWork.UserRepository.GetAsync(u => u.UserName == userToBeUnfollowedUserName, includeProperties: "Followers");

        if (!follower.Any())
        {
            throw new UserNotFoundException($"The user with username '{followerUsername} not exists on data source.");
        }

        if (!followedUser.Any())
        {
            throw new UserNotFoundException($"The user with username '{userToBeUnfollowedUserName} not exists on data source.");
        }

        using (var transaction = await _unitOfWork.StartTransactionAsync())
        {
            try
            {
                followedUser.First().Followers.Remove(follower.First());
                follower.First().Users.Remove(followedUser.First());
                await _unitOfWork.CompleteAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException)
            {
                // Fail quietly
                await transaction.RollbackAsync();
            }
        }
    }
}
