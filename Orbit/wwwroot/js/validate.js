$(document).ready(function () {
    $("#reg-form").validate({
        rules: {
            UserEmail: {
                required: true,
                email: true,
                maxlength: 200,
                remote: {
                    url: "http://localhost:5216/Account/CheckEmail",
                    type: "POST",
                    data: {
                        email: function () {
                            return $('#email').val();
                        }
                    }
                }
            },
            UserName: {
                required: true,
                minlength: 5,
                maxlength: 100,
                pattern: /^[a-zA-Z0-9_]*$/,
                remote: {
                    url: "http://localhost:5216/Account/CheckUsername",
                    type: "POST",
                    data: {
                        username: function () {
                            return $('#uname').val();
                        }
                    }
                }
            },
            UserPassword: {
                required: true,
                maxlength: 200,
                pattern: /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$/
            },
            UserProfileName: {
                required: true,
                maxlength: 200
            },
            IsPrivateProfile: {
                required: false
            },
            terms: {
                required: true
            }
        },
        messages: {
            UserName: {
                required: "Insira o nome do usuário!",
                minlength: "O nome do usuário deve ter no mínimo 5 caracteres.",
                maxlength: "O nome do usuário deve ter no máximo 100 caracteres.",
                pattern: "O nome do usuário só pode conter letras, números e underline.",
                remote: "O nome de usuário já está cadastrado."
            },
            UserEmail: {
                required: "Insira o email do usuário!",
                email: "O email do usuário não é válido.",
                maxlength: "O email do usuário deve ter no máximo 200 caracteres.",
                remote: "O email já está cadastrado."
            },
            UserPassword: {
                required: "Insira a senha do usuário!",
                maxlength: "A senha do usuário deve ter no máximo 200 caracteres.",
                pattern: "A senha deve conter pelo menos uma letra minúscula, uma letra maiúscula, um número e um caractere especial (#?!@$ %^&*-)."
            },
            UserProfileName: {
                required: "Insira o nome de perfil do usuário!",
                maxlength: "O nome de perfil deve ter no máximo 200 caracteres."
            },
            terms: {
                required: "Deve concordar com os termos de usuário."
            }
        }
    });
});

$(document).ready(function () {
    const regForm = $('#reg-form');
    const firstStep = $("#divSignIn").find(".first-step");
    const lastStep = $("#divSignIn").find(".last-step");
    const backBtn = $("#backBtn");
    let onLastStep = false;
    lastStep.hide();

    regForm.find('input').prop('readonly', true);
    regForm.find('input').attr('onfocus', 'this.removeAttribute("readonly");this.select();')

    regForm.find('button').click((event) => {
        event.preventDefault();
    });

    $('#nextBtn').click(() => {
        if (regForm.valid()) {
            $('#email').val().trim();
            firstStep.hide();
            lastStep.show();
        }
    });

    $('#signInButton').click(() => {
        if (regForm.valid()) {
            regForm.submit();
        }
    });

    $('#backBtn').click(() => {
        lastStep.hide();
        firstStep.show();
    });
});
