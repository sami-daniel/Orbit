using Orbit.Models;
using Orbit.Services.Interfaces;
using Orbit.UnitOfWork.Interfaces;

namespace Orbit.Services.Implementations;

public class LikeService : ILikeService
{
    // Private readonly field for the unit of work to interact with repositories
    private readonly IUnitOfWork _unitOfWork;

    // Constructor to initialize the LikeService with a unit of work
    public LikeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Method to get all likes from a specific user
    public async Task<IEnumerable<Like>> GetLikesFromUser(string username) 
    {
        // Retrieve all likes by the user, including associated user data
        return await _unitOfWork.LikeRepository.GetAsync(l => l.User.UserName == username, includeProperties: "User,");
    }

    // Method to like a post
    public async Task LikePost(uint postID, string username)
    {
        // Fetch user and post data
        var users = await _unitOfWork.UserRepository.GetAsync(f => f.UserName == username);
        var posts = await _unitOfWork.PostRepository.GetAsync(p => p.PostId == postID);

        // Check if the post exists, throw exception if not found
        if (!posts.Any())
        {
            throw new ArgumentException("Post not found");
        }

        // Insert a new like for the post by the user
        await _unitOfWork.LikeRepository.InsertAsync(new Like
        {
            PostId = postID,
            UserId = users.First().UserId
        });

        // Commit the transaction to the database
        await _unitOfWork.CompleteAsync();
    }

    // Method to remove a like from a post
    public async Task UnlikePost(uint postID, string username)
    {
        // Fetch user and post data
        var users = await _unitOfWork.UserRepository.GetAsync(f => f.UserName == username);
        var posts = await _unitOfWork.PostRepository.GetAsync(p => p.PostId == postID);
        
        // Check if the post exists, throw exception if not found
        if (!posts.Any())
        {
            throw new ArgumentException("Post not found");
        }

        // Retrieve the like to be removed
        var like = await _unitOfWork.LikeRepository.GetAsync(l => l.PostId == postID && l.UserId == users.First().UserId);

        // Check if the like exists, throw exception if not found
        if (!like.Any())
        {
            throw new ArgumentException("Like not found");
        }

        // Delete the like from the repository
        _unitOfWork.LikeRepository.Delete(like.First());

        // Commit the transaction to the database
        await _unitOfWork.CompleteAsync();
    }
}
