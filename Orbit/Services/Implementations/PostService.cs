using Microsoft.EntityFrameworkCore;
using Orbit.Data.Contexts;
using Orbit.Models;
using Orbit.Services.Helpers;
using Orbit.Services.Interfaces;
using Orbit.UnitOfWork.Interfaces;

namespace Orbit.Services.Implementations;

public class PostService : IPostService
{
    private readonly IUnitOfWork _unitOfWork;

    public PostService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddPostAsync(Post post, string postOwnerName)
    {
        try
        {
            PostServiceHelpers.ValidatePost(post);
        }
        catch (Exception)
        {
            throw;
        }
        var postOwner = await _unitOfWork.UserRepository.GetAsync(f => f.UserName == postOwnerName);

        if (!postOwnerName.Any())
        {
            throw new ArgumentException("User not found");
        }

        post.User = postOwner.First();
        post.UserId = postOwner.First().UserId;
        await _unitOfWork.PostRepository.InsertAsync(post);
    }

    public async Task<IEnumerable<Post>> GetPaginatedPostAsync(int skip, int take, ApplicationDbContext applicationDbContext)
    {
        return await applicationDbContext.Posts
        .OrderBy(p => p.PostId)
        .Skip(skip)
        .Take(take)
        .ToListAsync();


    }
}
