using Microsoft.EntityFrameworkCore;
using Orbit.Data.Contexts;
using Orbit.Models;
using Orbit.Services.Exceptions;
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

    public async Task<IEnumerable<Post>> GetPostsRandomizedByUserPreferenceAsync(string username, ApplicationDbContext context)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == username)
                   ?? throw new UserNotFoundException($"The user with the username {username} was not found.");

        var userPreferences = await context.UserPreferences
                          .Where(up => up.UserId == user.UserId)
                          .ToListAsync();

        if (userPreferences.Count == 0)
        {
            // In case that user has no preferences, return the last 50 posts, randomized.
            return await context.Posts
                   .Take(50)
                   .OrderBy(p => Guid.NewGuid())
                   .ToListAsync();
        }
        
        var posts = context.Database.SqlQuery<Post>($"""
            SELECT p.* FROM post p
            JOIN post_preference USING(post_id)
            WHERE post_preference.preference_name = IN({string.Join(',', userPreferences.Select(up => up.PreferenceName))})
            ORDER BY RAND()
            LIMIT 50
        """);

        if (posts.Count() < 50)
        {
            posts = context.Database.SqlQuery<Post>($"""
                SELECT * FROM 
                (SELECT p.* FROM post p
                JOIN post_preference USING(post_id)
                WHERE post_preference.preference_name IN({string.Join(',', userPreferences.Select(up => up.PreferenceName))})
                UNION
                SELECT p.* FROM post p
                JOIN post_preference USING(post_id)
                WHERE post_preference.preference_name IN({string.Join(',', userPreferences.Select(up => up.PreferenceName))}))
                AS Result 
                ORDER BY RAND()
                LIMIT 50
            """);
        }

        return await posts.ToListAsync();
    }
}
