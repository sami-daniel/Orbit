using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orbit.Data.Contexts;
using Orbit.DTOs.Responses;
using Orbit.Extensions;
using Orbit.Models;
using Orbit.Services.Implementations;
using Orbit.Services.Interfaces;

namespace Orbit.Controllers;

[Authorize]
public class LikeController : Controller
{
    private readonly ILikeService _likeService;
    private readonly IPostService _postService;

    public LikeController(ILikeService likeService, IPostService postService)
    {
        _likeService = likeService;
        _postService = postService;
    }

    [HttpGet("[controller]/like-post/{postID}")]
    public async Task<IActionResult> LikePost(uint postID)
    {
        var username = HttpContext.Session.GetObject<UserResponse>("User")!.UserName;
        var post = await _postService.GetPostByIdAsync(postID);
        
        var likes = await _likeService.GetLikesFromUser(username);
        
        if (likes.Where(l => l.PostId == postID).Any())
        {
            await _likeService.UnlikePost(postID, username);
            return Ok("Post unliked");
        }

        await _likeService.LikePost(postID, username);

        return NoContent();
    }
}