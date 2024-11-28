using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orbit.DTOs.Requests;
using Orbit.Models;
using Orbit.Services.Interfaces;

namespace Orbit.Controllers;

// Ensures that the controller actions require authorization (authenticated users)
[Authorize]
public class PostController : Controller
{
    private readonly IPostService _postService;  // Service for post-related operations
    private readonly IMapper _mapper;  // AutoMapper to map between DTOs and models

    // Constructor to initialize the service and mapper
    public PostController(IPostService postService, IMapper mapper)
    {
        _postService = postService;
        _mapper = mapper;
    }

    // POST: [controller]/create-post - Creates a new post
    [HttpPost("[controller]/create-post")]
    public async Task<IActionResult> CreatePost([FromForm] PostAddRequest postAddRequest, [FromForm] IFormFile? imageOrVideo, [FromForm] string returnTo)
    {
        // Check if the model is valid; if not, return an error message
        if (!ModelState.IsValid)
        {
            var errors = string.Join(',', ModelState.Values.SelectMany(e => e.Errors.Select(e => e.ErrorMessage)));
            return BadRequest(errors);
        }

        // Handle image or video uploads
        if (imageOrVideo != null)
        {    
            // Check the file type and assign it accordingly
            if (imageOrVideo.ContentType == "image/png" || imageOrVideo.ContentType == "image/jpg" || imageOrVideo.ContentType == "image/jpeg" || imageOrVideo.ContentType == "image/gif")
            {
                postAddRequest.PostImageByteType = imageOrVideo;
            }
            else if (imageOrVideo.ContentType == "video/mp4" || imageOrVideo.ContentType == "video/mkv" || imageOrVideo.ContentType == "video/avi" || imageOrVideo.ContentType == "video/webm")
            {
                postAddRequest.PostVideoByteType = imageOrVideo;
            }
            else
            {
                return BadRequest("Invalid file type");
            }
        }

        // Map the request data to the Post model
        var post = _mapper.Map<Post>(postAddRequest);

        // Handle empty byte arrays (set them to null if empty)
        if (post.PostImageByteType != null && post.PostImageByteType!.Length == 0)
        {
            post.PostImageByteType = null;
        }

        if (post.PostVideoByteType != null && post.PostVideoByteType!.Length == 0)
        {
            post.PostVideoByteType = null;
        }
        
        try
        {
            // Add the post using the service layer
            await _postService.AddPostAsync(post, postAddRequest.PostOwnerName);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); // Handle errors in post creation
        }

        return RedirectPermanent(returnTo); // Redirect to the specified location after creating the post
    }

    // GET: [controller]/get-post-image/{postID} - Fetches the post's image
    [HttpGet("[controller]/get-post-image/{postID}")]
    public async Task<IActionResult> GetPostImage(uint postID)
    {
        var post = await _postService.GetPostByIdAsync(postID);
    
        if (post == null)
        {
            return NotFound("Post not found"); // Return a 404 if the post is not found
        }

        if (post.PostImageByteType == null)
        {
            return NotFound("Post has no image"); // Return a 404 if there is no image
        }

        return File(post.PostImageByteType, "image/jpeg"); // Return the post image
    }

    // GET: [controller]/get-post-video/{postID} - Fetches the post's video
    [HttpGet("[controller]/get-post-video/{postID}")]
    public async Task<IActionResult> GetPostVideo(uint postID)
    {
        var post = await _postService.GetPostByIdAsync(postID);
    
        if (post == null)
        {
            return NotFound("Post not found"); // Return a 404 if the post is not found
        }

        if (post.PostVideoByteType == null)
        {
            return NotFound("Post has no video"); // Return a 404 if there is no video
        }

        return File(post.PostVideoByteType, "video/mp4"); // Return the post video
    }
}
