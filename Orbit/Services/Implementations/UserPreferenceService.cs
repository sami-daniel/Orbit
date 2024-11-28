using Microsoft.EntityFrameworkCore;
using Orbit.Data.Contexts;
using Orbit.Models;
using Orbit.Services.Exceptions;
using Orbit.Services.Interfaces;

namespace Orbit.Services.Implementations;

public class UserPreferenceService : IUserPreferenceService
{
    private readonly ApplicationDbContext _context;

    public UserPreferenceService(ApplicationDbContext context)
    {
        _context = context;
    }

<<<<<<< HEAD
    /// <summary>
    /// Updates a user's preferences based on the likes they have given to posts.
    /// </summary>
    /// <param name="userName">The username of the user whose preferences are to be updated.</param>
    /// <param name="postID">The ID of the post that is being interacted with.</param>
    /// <exception cref="UserNotFoundException">Thrown when the user is not found in the database.</exception>
    /// <exception cref="PostNotFoundException">Thrown when the post is not found in the database.</exception>
=======
>>>>>>> 35189761d3d1ce18be8ee88be049a8f9dcaf53a6
    public async Task UpdateUserPreferenceAsync(string userName, uint postID)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName);

        if (user == null)
        {
            throw new UserNotFoundException($"The user with the username {userName} has not found.");
        }

        var post = await _context.Posts
            .FirstOrDefaultAsync(p => p.PostId == postID);

        if (post == null)
        {
            throw new PostNotFoundException($"The post with postID {postID} has not found.");
        }

        var likes = await _context.Likes
            .Where(l => l.UserId == user.UserId)
            .Include(l => l.Post)
            .ThenInclude(p => p.PostPreferences)
            .ToListAsync();

        var postPreferences = await _context.PostPreferences
            .Where(pf => pf.PostId == postID)
            .ToListAsync();

        if (!postPreferences.Any())
        {
            return;
        }

        foreach (var like in likes)
        {
            var postPreferenceLikeBased = like.Post.PostPreferences
                .FirstOrDefault(p => postPreferences.Any(pf => pf.PreferenceName == p.PreferenceName));

            if (postPreferenceLikeBased == null)
            {
                continue;
            }

            var matchingPostPreferences = postPreferences
                .Where(pf => pf.PreferenceName == postPreferenceLikeBased.PreferenceName)
                .ToList();

            foreach (var preference in matchingPostPreferences)
            {
                var preferenceLikesCount = likes.Count(l => l.Post.PostPreferences
                    .Any(p => p.PreferenceName == preference.PreferenceName));

                if (preferenceLikesCount >= 3)
                {
                    var existingPreference = await _context.UserPreferences
                        .FirstOrDefaultAsync(up => up.UserId == user.UserId && up.PreferenceName == preference.PreferenceName);

                    if (existingPreference == null)
                    {
                        var userPreference = new UserPreference
                        {
                            UserId = user.UserId,
                            PreferenceName = preference.PreferenceName
                        };

                        await _context.UserPreferences.AddAsync(userPreference);
                    }
                }
            }
        }

        await _context.SaveChangesAsync();
    }
<<<<<<< HEAD
}
=======

}
>>>>>>> 35189761d3d1ce18be8ee88be049a8f9dcaf53a6
