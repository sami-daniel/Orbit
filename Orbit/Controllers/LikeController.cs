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

// The controller is authorized, meaning only authenticated users can access it.
[Authorize]
public class LikeController : Controller
{
<<<<<<< HEAD
    // Declare dependencies for services related to liking posts, user preferences, and posts themselves.
=======
>>>>>>> 35189761d3d1ce18be8ee88be049a8f9dcaf53a6
    private readonly ILikeService _likeService;
    private readonly IPostService _postService;
    private readonly IUserPreferenceService _userPreferenceService;

<<<<<<< HEAD
    // Constructor to inject the services into the controller.
    public LikeController(ILikeService likeService, IUserPreferenceService userPreferenceService, IPostService postService)
    {
        _likeService = likeService;  // Initialize the like service.
        _postService = postService;  // Initialize the post service.
        _userPreferenceService = userPreferenceService;  // Initialize the user preference service.
    }

    // HTTP GET action that handles liking and unliking a post by the user.
    [HttpGet("[controller]/like-post/{postID}")]
    public async Task<IActionResult> LikePost(uint postID)
    {
        // Get the username of the currently authenticated user from session.
        var username = HttpContext.Session.GetObject<UserResponse>("User")!.UserName;
        
        // Retrieve the post by its ID.
        var post = await _postService.GetPostByIdAsync(postID);
        
        // Get the list of posts the user has liked.
        var likes = await _likeService.GetLikesFromUser(username);
        
        // Check if the user has already liked the post.
        if (likes.Where(l => l.PostId == postID).Any())
        {
            // If the user has already liked the post, "unlike" it and return a success message.
=======
    public LikeController(ILikeService likeService, IUserPreferenceService userPreferenceService, IPostService postService)
    {
        _likeService = likeService;
        _postService = postService;
        _userPreferenceService = userPreferenceService;
    }

    [HttpGet("[controller]/like-post/{postID}")]
    public async Task<IActionResult> LikePost(uint postID)
    {
        var username = HttpContext.Session.GetObject<UserResponse>("User")!.UserName;
        var post = await _postService.GetPostByIdAsync(postID);
        
        var likes = await _likeService.GetLikesFromUser(username);
        
        if (likes.Where(l => l.PostId == postID).Any())
        {
>>>>>>> 35189761d3d1ce18be8ee88be049a8f9dcaf53a6
            await _likeService.UnlikePost(postID, username);
            return Ok("Post unliked");
        }

<<<<<<< HEAD
        // If the user has not liked the post, "like" it and update user preferences.
=======
>>>>>>> 35189761d3d1ce18be8ee88be049a8f9dcaf53a6
        await _likeService.LikePost(postID, username);
        await _userPreferenceService.UpdateUserPreferenceAsync(username, postID);   

        // Return HTTP 204 No Content, indicating the action was successful but no content is returned.
        return NoContent();
    }
}
