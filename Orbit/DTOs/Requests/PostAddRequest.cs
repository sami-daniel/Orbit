using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Orbit.CustomDataAnnotations;

namespace Orbit.DTOs.Requests;

public class PostAddRequest
{
    // Validation to ensure that the post owner's name is not empty
    [Required(ErrorMessage = "The post owner name cannot be empty.")]
    public string PostOwnerName { get; set; } = null!;

    // Validation to ensure that the post content is not empty and has a maximum length of 65535 characters
    [Required(ErrorMessage = "The post cannot be empty.")]
    [MaxLength(65535, ErrorMessage = "The post can contain a maximum of {0} characters.")]
    public string PostContent { get; set; } = null!;

    // Validation for image files if uploaded
    [FileExtensions(Extensions = "jpg,jpeg,png,gif", ErrorMessage = "The image file must be in JPG, JPEG, PNG, or GIF format.")]
    [MaxFileSize(10 * 1024 * 1024, ErrorMessage = "The image file cannot exceed 10 MB.")]
    public IFormFile? PostImageByteType { get; set; }

    // Validation for video files if uploaded
    [FileExtensions(Extensions = "mp4,mkv,avi,webm", ErrorMessage = "The video file must be in MP4, MKV, AVI, or WebM format.")]
    [MaxFileSize(50 * 1024 * 1024, ErrorMessage = "The video file cannot exceed 50 MB.")]
    public IFormFile? PostVideoByteType { get; set; }
}
