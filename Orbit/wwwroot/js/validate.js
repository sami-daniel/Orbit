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
                pattern: "O nome do usuário só pode conter letras, numeros e underline.",
                remote: "O nome de usúario ja foi cadastro anteriormente! \n Gentileza escolher outro."
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
                equalTo: "#repeatpassword"
            },
            repeatpassword: {
                required: "Repita a senha digitada!"
            },
            terms: {
                required: "Você deve concordar com nossos termos e póliticas de privacidades antes de cadastrar."
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

    $('#log-form').validate({
        rules: {
            email: {
                required: true
                /*
                ,
                remote: {
                    url: 'Account/CheckEmail',
                    type: 'post',
                    data: {
                        email: function () {
                            return $('#email-login').val();
                        }
                    }
                }
                O endpoint CheckEmail retorna resultados true ou false, como o validator
                do jQuery requer que seja para o remote retornar mensagens apropriadas de erro.
                Pórem, o endpoint CheckEmail é preparado para checar se um email já existe, retornando false
                e true caso contrario. Para esse caso, precisamos que retorne true se o email não existe,
                para assim o validator retornar a mensagem apropriada
                */
            },
            password: {
                required: true
            }
        },
        messages: {
            email: {
                required: 'Insira o email ou nome de usúario para login!'
                /*
                remote: 'Email não cadastrado!'
                */
                // FIX ME
            }
        }
    });

    const firstStep = $("#divSignIn").find(".first-step");
    const lastStep = $("#divSignIn").find(".last-step");
    const backBtn = $("#backBtn");
    let onLastStep = false;
    lastStep.hide();

    $('#nextBtn').click(function () {
        event.preventDefault();

        if ($('#reg-form').valid()) {
            trimmer();
            firstStep.hide();
            lastStep.show();
            onLastStep = true;
        }

    });

    $('#LoginButton').click(function () {
        event.preventDefault();

        if ($('#log-form').valid()) {
            document.getElementById("email-login").value.trim();
            document.getElementById("Password-Login").value.trim();
            $('#log-form').submit();
        }
    })

    $('#signInButton').click(function () {
        event.preventDefault();

        if ($('#reg-form').valid()) {
            document.getElementById('password').value.trim();
            $('#reg-form').submit();
        }
    });

    $(document).keypress(function (e) {
        if (e.which === 13) {
            event.preventDefault();

            if (onLastStep) {

                if ($('#reg-form').valid()) {
                    trimmer();
                    firstStep.hide();
                    lastStep.show();
                    onLastStep = true;
                }
            }

            else {

                if ($('#reg-form').valid()) {
                    document.getElementById('password').value.trim();
                    $('#signInButton').submit();
                }


            }
        }
    });

    backBtn.click(function () {
        event.preventDefault(); // Previne o comportamento padrão de submissão do 
        firstStep.show();
        lastStep.hide();
        onLastStep = false;
    });
});

function trimmer() {
    document.getElementById("name").value.trim();
    document.getElementById("email").value.trim();
    document.getElementById("DayBirthday").value.trim();
    document.getElementById("MonthBirthday").value.trim();
    document.getElementById("YearBirthday").value.trim();
   
}