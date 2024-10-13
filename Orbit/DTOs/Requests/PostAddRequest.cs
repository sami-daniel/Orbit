using System.ComponentModel.DataAnnotations;

namespace Orbit.DTOs.Requests;

public class PostAddRequest
{
    [Required]
    public string PostOwnerName { get; set; } = null!;

    [Required(ErrorMessage = "O post não pode ser vazio.")]
    [MaxLength(65535, ErrorMessage = "O Post pode conter no máximo {0} caracteres.")]
    public string PostContent { get; set; } = null!;

    public IFormFile? PostImageByteType { get; set; }

    public IFormFile? PostVideoByteType { get; set; }
}
