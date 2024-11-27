using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orbit.Data.Contexts;
using Orbit.Extensions;
using Orbit.Models;

namespace Orbit.Controllers;

[Authorize]
public class LikeController : Controller
{
    private readonly ApplicationDbContext _context;

    public LikeController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("[controller]/like-post/{postID}")]
    public async Task<IActionResult> LikePost(uint postID)
    {
        var username = HttpContext.Session.GetObject<User>("User")!.UserName;
        var users = await _context.Users.Where(f => f.UserName == username).ToListAsync();
        var posts = await _context.Posts.Where(p => p.UserId == postID).ToListAsync();

        if (!posts.Any())
        {
            return NotFound();
        }

        await _context.Likes.AddAsync(new Like 
        {
            PostId = postID,
            UserId = users.First().UserId
        });

        return NoContent();
    }
}