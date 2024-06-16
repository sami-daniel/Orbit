document.addEventListener("DOMContentLoaded", function () {
    const modalBackground = document.getElementById("modalBackground");
    const divSignIn = document.getElementById("divSignIn");
    const divLogin = document.getElementById("divLogin");
    // Supondo que o bot�o de fechar tenha o id 'closeBtn'
    const closeButton = document.getElementById("closeBtn");
    const closeButtonLogin = document.getElementById("closeBtnLogin");

    closeButtonLogin.addEventListener("click", function () {
        modalBackground.classList.remove("visible");
        modalBackground.classList.add("hidden");
    });

    closeButton.addEventListener("click", function () {
        event.preventDefault();
    });

    // Fun��o para abrir um modal espec�fico
    function openModal(modal) {
        modalBackground.classList.remove("hidden");
        modal.classList.remove("hidden");
        modal.classList.add("visible");

        // Certifique-se de que a outra div � fechada
        if (modal === divSignIn) {
            divLogin.classList.remove("visible");
            divLogin.classList.add("hidden");
        } else if (modal === divLogin) {
            divSignIn.classList.remove("visible");
            divSignIn.classList.add("hidden");
        }
    }

    // Fun��o para fechar todos os modais
    function closeModal() {
        modalBackground.classList.add("hidden");
        [divSignIn, divLogin].forEach((modal) => {
            modal.classList.remove("visible");
            modal.classList.add("hidden");
        });
    }

    // Listener para o bot�o de "Cadastrar"
    document.getElementById("openSignIn").addEventListener("click", function () {
        openModal(divSignIn);
    });

    // Listener para o bot�o de "Entrar"
    document.getElementById("openLogin").addEventListener("click", function () {
        openModal(divLogin);
    });

    // Adiciona o listener para o bot�o de fechar
    closeButton.addEventListener("click", function () {
        closeModal();
    });
});

