using Orbit.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Orbit.Application.Dtos.Requests
{
    public class UserAddRequest
    {
        [Required(ErrorMessage = "Insira o nome do usuário!")]
        [StringLength(100, ErrorMessage = "O nome do usuário deve ter no máximo 100 caracteres.")]
        [Display(Name = "Name")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Insira o email do usuário!")]
        [StringLength(200, ErrorMessage = "O email do usuário deve ter no máximo 200 caracteres.")]
        [EmailAddress(ErrorMessage = "O email do usuário não é válido.")]
        [Display(Name = "Email")]
        public string UserEmail { get; set; } = null!;

        [Required(ErrorMessage = "Insira a data de nascimento do usuário!")]
        [DataType(DataType.Date, ErrorMessage = "Formato de data inválido.")]
        [Display(Name = "Data de Nascimento")]
        public DateTime UserDateOfBirth { get; set; }

        [Required(ErrorMessage = "Insira a senha do usuário!")]
        [StringLength(200, ErrorMessage = "A senha do usuário deve ter no máximo 200 caracteres.")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$", ErrorMessage = "A senha deve conter pelo menos uma letra minúscula, uma letra maiúscula, um caractere especial (@$!%*?&)")]
        [Display(Name = "Senha")]
        public string UserPassword { get; set; } = null!;


        public User ToUser() => new User
        {
            UserName = UserName,
            UserDateOfBirth = UserDateOfBirth,
            UserEmail = UserEmail,
            UserPassword = UserPassword
        };
    }
}
