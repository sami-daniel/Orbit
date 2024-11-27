using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Orbit.CustomDataAnnotations;

namespace Orbit.DTOs.Requests
{
    public class PostAddRequest
    {
        // Validação para garantir que o nome do proprietário do post não esteja vazio
        [Required(ErrorMessage = "O nome do post não pode ser vazio.")]
        [StringLength(100, ErrorMessage = "O nome do proprietário do post pode conter no máximo {1} caracteres.")]
        public string PostOwnerName { get; set; } = null!;

        // Validação para garantir que o conteúdo do post não seja vazio e tenha comprimento máximo de 65535 caracteres
        [Required(ErrorMessage = "O post não pode ser vazio.")]
        [MaxLength(65535, ErrorMessage = "O Post pode conter no máximo {0} caracteres.")]
        public string PostContent { get; set; } = null!;

        // Validação para arquivos de imagem, se enviados
        [FileExtensions(Extensions = "jpg,jpeg,png,gif", ErrorMessage = "O arquivo da imagem deve ser no formato JPG, JPEG, PNG ou GIF.")]
        [MaxFileSize(10 * 1024 * 1024, ErrorMessage = "O arquivo da imagem não pode ultrapassar 10 MB.")]
        public IFormFile? PostImageByteType { get; set; }

        // Validação para arquivos de vídeo, se enviados
        [FileExtensions(Extensions = "mp4,mkv,avi,webm", ErrorMessage = "O arquivo de vídeo deve ser no formato MP4, MKV, AVI ou WebM.")]
        [MaxFileSize(50 * 1024 * 1024, ErrorMessage = "O arquivo de vídeo não pode ultrapassar 50 MB.")]
        public IFormFile? PostVideoByteType { get; set; }
    }
}
