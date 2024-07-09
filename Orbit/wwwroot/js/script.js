document.addEventListener("DOMContentLoaded", function () {
    const modalBackground = document.getElementById("modalBackground");
    const divSignIn = document.getElementById("divSignIn");
    const divLogin = document.getElementById("divLogin");
    // Supondo que o botão de fechar tenha o id 'closeBtn'
    const closeButton = document.getElementById("closeBtn");
    const closeButtonLogin = document.getElementById("closeBtnLogin");

    closeButtonLogin.addEventListener("click", function () {
        closeModal(divLogin);
    });

    closeButton.addEventListener("click", function () {
        event.preventDefault();
        closeModal(divSignIn);
    });

    // Função para abrir um modal específico
    function openModal(modal) {
        modalBackground.classList.remove("hidden");
        modal.classList.remove("hidden", "fade-out");
        modal.classList.add("visible", "fade-in");

        // Certifique-se de que a outra div é fechada
        if (modal === divSignIn) {
            divLogin.classList.remove("visible", "fade-in");
            divLogin.classList.add("hidden");
        } else if (modal === divLogin) {
            divSignIn.classList.remove("visible", "fade-in");
            divSignIn.classList.add("hidden");
        }
    }

    // Função para fechar todos os modais
    function closeModal(modal) {
        modal.classList.remove("visible", "fade-in");
        modal.classList.add("fade-out");

        setTimeout(() => {
            modal.classList.add("hidden");
            modal.classList.remove("fade-out");
            modalBackground.classList.add("hidden");
        }, 300); // Atraso igual à duração da animação fadeOut
    }

    // Listener para o botão de "Cadastrar"
    document.getElementById("openSignIn").addEventListener("click", function () {
        openModal(divSignIn);
    });

    // Listener para o botão de "Entrar"
    document.getElementById("openLogin").addEventListener("click", function () {
        openModal(divLogin);
    });

    // Adiciona o listener para o botão de fechar
    closeButton.addEventListener("click", function () {
        closeModal(divSignIn);
    });

    AOS.init();
});
