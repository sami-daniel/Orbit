using Orbit.Models;
using Orbit.Services.Interfaces;
using Orbit.UnitOfWork.Interfaces;

namespace Orbit.Services.Implementations;

public class LikeService : ILikeService
{
<<<<<<< HEAD
    // Private readonly field for the unit of work to interact with repositories
    private readonly IUnitOfWork _unitOfWork;

    // Constructor to initialize the LikeService with a unit of work
=======
    private readonly IUnitOfWork _unitOfWork;

>>>>>>> 35189761d3d1ce18be8ee88be049a8f9dcaf53a6
    public LikeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

<<<<<<< HEAD
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
=======
    public async Task<IEnumerable<Like>> GetLikesFromUser(string username) 
    {
        return await _unitOfWork.LikeRepository.GetAsync(l => l.User.UserName == username, includeProperties: "User,");
    }

    public async Task LikePost(uint postID, string username)
    {
        var users = await _unitOfWork.UserRepository.GetAsync(f => f.UserName == username);
        var posts = await _unitOfWork.PostRepository.GetAsync(p => p.PostId == postID);

>>>>>>> 35189761d3d1ce18be8ee88be049a8f9dcaf53a6
        if (!posts.Any())
        {
            throw new ArgumentException("Post not found");
        }

<<<<<<< HEAD
        // Insert a new like for the post by the user
=======
>>>>>>> 35189761d3d1ce18be8ee88be049a8f9dcaf53a6
        await _unitOfWork.LikeRepository.InsertAsync(new Like
        {
            PostId = postID,
            UserId = users.First().UserId
        });

<<<<<<< HEAD
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
=======
        await _unitOfWork.CompleteAsync();
    }

    public async Task UnlikePost(uint postID, string username)
    {
        var users = await _unitOfWork.UserRepository.GetAsync(f => f.UserName == username);
        var posts = await _unitOfWork.PostRepository.GetAsync(p => p.PostId == postID);
        
>>>>>>> 35189761d3d1ce18be8ee88be049a8f9dcaf53a6
        if (!posts.Any())
        {
            throw new ArgumentException("Post not found");
        }

<<<<<<< HEAD
        // Retrieve the like to be removed
        var like = await _unitOfWork.LikeRepository.GetAsync(l => l.PostId == postID && l.UserId == users.First().UserId);

        // Check if the like exists, throw exception if not found
=======
        var like = await _unitOfWork.LikeRepository.GetAsync(l => l.PostId == postID && l.UserId == users.First().UserId);

>>>>>>> 35189761d3d1ce18be8ee88be049a8f9dcaf53a6
        if (!like.Any())
        {
            throw new ArgumentException("Like not found");
        }

<<<<<<< HEAD
        // Delete the like from the repository
        _unitOfWork.LikeRepository.Delete(like.First());

        // Commit the transaction to the database
        await _unitOfWork.CompleteAsync();
    }
}
=======
        _unitOfWork.LikeRepository.Delete(like.First());
        await _unitOfWork.CompleteAsync();
    }
}
>>>>>>> 35189761d3d1ce18be8ee88be049a8f9dcaf53a6
