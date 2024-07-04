using Orbit.Application.Extensions;
using Orbit.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Orbit.Application.Dtos.Requests
{
    public class UserAddRequest
    {
        [Required(ErrorMessage = "Insira o nome do usuário!")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "O nome do usuário deve ter no máximo 100 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "O nome do usuário só pode conter letras, números e underline.")]
        [Display(Name = "Nome de Usuário")]
        public string UserName { get; set; } = null!;
        [Required(ErrorMessage = "Insira o email do usuário!")]
        [StringLength(200, ErrorMessage = "O email do usuário deve ter no máximo 200 caracteres.")]
        [EmailAddress(ErrorMessage = "O email do usuário não é válido.")]
        [Display(Name = "Email")]
        public string UserEmail { get; set; } = null!;
        [Required(ErrorMessage = "Insira a senha do usuário!")]
        [StringLength(200, ErrorMessage = "A senha do usuário deve ter no máximo 200 caracteres.")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$", ErrorMessage = "A senha deve conter pelo menos uma letra minúscula, uma letra maiúscula, um caractere especial (@$!%*?&)")]
        [Display(Name = "Senha")]
        public string UserPassword { get; set; } = null!;
        [Required(ErrorMessage = "Insira o nome de perfil do usuário!")]
        [StringLength(200, ErrorMessage = "O nome de perfil dever ter no máximo 200 caracteres.")]
        [Display(Name = "Nome do Perfil")]
        public string UserProfileName { get; set; } = null!;
        [Display(Name = "Perfil Privado")]
        public string IsPrivateProfile { get; set; } = null!;
        
        public User ToUser()
        {
            return new User
            {
                UserName = UserName,
                UserEmail = UserEmail,
                UserPassword = UserPassword,
                UserProfileName = UserProfileName,
                IsPrivateProfile = IsPrivateProfile.ToLong()
            };
        }
    }
}
