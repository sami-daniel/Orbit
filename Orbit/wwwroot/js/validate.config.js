$(document).ready(function () {
    $('#reg-form').validate({
        rules: {
            UserName: {
                required: true,
                minlength: 5,
                maxlength: 100,
                pattern: "^[a-zA-Z0-9_]*$",
                remote: {
                    url: 'Account/CheckUsername',
                    type: 'post',
                    data: {
                        username: function () {
                            return $('#name').val();
                        }
                    }
                }
            },
            UserEmail: {
                required: true,
                email: true,
                maxlength: 200,
                remote: {
                    url: 'Account/CheckEmail',
                    type: 'post',
                    data: {
                        email: function () {
                            return $('#email').val();
                        }
                    }
                }
            },
            UserPassword: {
                required: true,
                maxlength: 200,
                pattern: "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$",
                equalTo: "#repeatpassword"
            },
            terms: {
                required: true
            },
            Day: {
                required: true,
                range: [1, 31]
            },
            Month: {
                required: true,
                range: [1, 12]
            },
            Year: {
                required: true,
                range: [1, 9999]
            }
        },
        messages: {
            UserName: {
                required: "Insira o nome do usuário!",
                minlength: "O nome do usuário deve ter no mínimo 5 caracteres.",
                maxlength: "O nome do usuário deve ter no máximo 100 caracteres.",
                pattern: "O nome do usuário só pode conter letras, números e underline.",
                remote: "O nome de usuário já foi cadastrado anteriormente! \nGentileza escolher outro."
            },
            UserEmail: {
                required: "Insira o email do usuário!",
                email: "O email do usuário não é válido.",
                maxlength: "O email do usuário deve ter no máximo 200 caracteres.",
                remote: "Email já cadastrado!"
            },
            UserPassword: {
                required: "Insira a senha do usuário!",
                maxlength: "A senha do usuário deve ter no máximo 200 caracteres.",
                pattern: "A senha deve conter pelo menos uma letra minúscula, uma letra maiúscula, um caractere especial (@$!%*?&) e ter pelo menos 8 caracteres.",
                equalTo: "As senhas não coincidem."
            },
            repeatpassword: {
                required: "Repita a senha digitada!"
            },
            terms: {
                required: "Você deve concordar com nossos termos e políticas de privacidade antes de cadastrar."
            },
            Day: {
                required: "O campo Dia é obrigatório.",
                range: "O campo Dia deve estar entre 1 e 31."
            },
            Month: {
                required: "O campo Mês é obrigatório.",
                range: "O campo Mês deve estar entre 1 e 12."
            },
            Year: {
                required: "O campo Ano é obrigatório.",
                range: "O campo Ano deve estar entre 1 e 9999."
            }
        }
    });
});