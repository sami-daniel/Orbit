using Orbit.Models;
using Orbit.Services.Interfaces;
using Orbit.UnitOfWork.Interfaces;

namespace Orbit.Services.Implementations;

public class LikeService : ILikeService
{
    private readonly IUnitOfWork _unitOfWork;

    public LikeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Like>> GetLikesFromUser(string username) 
    {
        return await _unitOfWork.LikeRepository.GetAsync(l => l.User.UserName == username, includeProperties: "User,");
    }

    public async Task LikePost(uint postID, string username)
    {
        var users = await _unitOfWork.UserRepository.GetAsync(f => f.UserName == username);
        var posts = await _unitOfWork.PostRepository.GetAsync(p => p.PostId == postID);

        if (!posts.Any())
        {
            throw new ArgumentException("Post not found");
        }

        await _unitOfWork.LikeRepository.InsertAsync(new Like
        {
            PostId = postID,
            UserId = users.First().UserId
        });

        await _unitOfWork.CompleteAsync();
    }

    public async Task UnlikePost(uint postID, string username)
    {
        var users = await _unitOfWork.UserRepository.GetAsync(f => f.UserName == username);
        var posts = await _unitOfWork.PostRepository.GetAsync(p => p.PostId == postID);
        
        if (!posts.Any())
        {
            throw new ArgumentException("Post not found");
        }

        var like = await _unitOfWork.LikeRepository.GetAsync(l => l.PostId == postID && l.UserId == users.First().UserId);

        if (!like.Any())
        {
            throw new ArgumentException("Like not found");
        }

        _unitOfWork.LikeRepository.Delete(like.First());
        await _unitOfWork.CompleteAsync();
    }
}