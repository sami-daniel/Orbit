using System.ComponentModel.DataAnnotations;

namespace Orbit.DTOs.Requests;

public class UserAddRequest
{
    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    [Required(ErrorMessage = "Please enter the user's username!")]
    [StringLength(255, MinimumLength = 5, ErrorMessage = "The username must have at most {0} characters.")]
    [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "The username can only contain letters, numbers, and underscores.")]
    [Display(Name = "Username")]
    public string UserName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user's email.
    /// </summary>
    [Required(ErrorMessage = "Please enter the user's email!")]
    [StringLength(255, ErrorMessage = "The user's email must have at most {0} characters.")]
    [EmailAddress(ErrorMessage = "The user's email is not valid.")]
    [Display(Name = "Email")]
    public string UserEmail { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user's password.
    /// </summary>
    [Required(ErrorMessage = "Please enter the user's password!")]
    [StringLength(255, ErrorMessage = "The user's password must have at most {0} characters.")]
    [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$", ErrorMessage = "The password must contain at least one lowercase letter, one uppercase letter, and one special character (@$!%*?&)")]
    [Display(Name = "Password")]
    public string UserPassword { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user's profile name.
    /// </summary>
    [Required(ErrorMessage = "Please enter the user's profile name!")]
    [StringLength(255, ErrorMessage = "The profile name must have at most {0} characters.")]
    [Display(Name = "Profile Name")]
    public string UserProfileName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user's privacy setting for their profile.
    /// </summary>
    [Display(Name = "Private Profile")]
    public string? IsPrivateProfile { get; set; }
}
