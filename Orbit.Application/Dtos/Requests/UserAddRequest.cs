using Orbit.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Orbit.Application.Dtos.Requests
{
    public class UserAddRequest
    {
        [Required(ErrorMessage = "Insira o nome do usuário!")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "O nome do usuário deve ter no máximo 100 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "O nome do usuário só pode conter letras, números e underline.")]
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
        public DateOnly UserDateOfBirth { get; set; }

        [Required(ErrorMessage = "Insira a senha do usuário!")]
        [StringLength(200, ErrorMessage = "A senha do usuário deve ter no máximo 200 caracteres.")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$", ErrorMessage = "A senha deve conter pelo menos uma letra minúscula, uma letra maiúscula, um caractere especial (@$!%*?&)")]
        [Display(Name = "Senha")]
        public string UserPassword { get; set; } = null!;

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Range(1, 31, ErrorMessage = "O campo {0} deve estar entre 1 e 31.")]
        [Display(Name = "Dia")]
        public int Day { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Range(1, 12, ErrorMessage = "O campo {0} deve estar entre 1 e 12.")]
        [Display(Name = "Mês")]
        public int Month { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Range(1, 9999, ErrorMessage = "O campo {0} deve estar entre 1 e 9999.")]
        [Display(Name = "Ano")]
        public int Year { get; set; }

        public User ToUser() => new User
        {
            UserName = UserName,
            UserDateOfBirth = new DateOnly(Year, Month, Day),
            UserEmail = UserEmail,
            UserPassword = UserPassword
        };
    }
}
