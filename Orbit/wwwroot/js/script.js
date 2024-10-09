document.addEventListener("DOMContentLoaded", function() {
    const modalBackground = document.getElementById("modalBackground");
    const divSignIn = document.getElementById("divSignIn");
    const divLogin = document.getElementById("divLogin");
    const closeButton = document.getElementById("closeBtn");
    const closeButtonLogin = document.getElementById("closeBtnLogin");

    closeButtonLogin.addEventListener("click", function() {
        closeModal(divLogin);
    });

    closeButton.addEventListener("click", function() {
        event.preventDefault();
        closeModal(divSignIn);
    });

    function openModal(modal) {
        modalBackground.classList.remove("hidden");
        modal.classList.remove("hidden", "fade-out");
        modal.classList.add("visible", "fade-in");

        if (modal === divSignIn) {
            divLogin.classList.remove("visible", "fade-in");
            divLogin.classList.add("hidden");
        } else if (modal === divLogin) {
            divSignIn.classList.remove("visible", "fade-in");
            divSignIn.classList.add("hidden");
        }
    }

    function closeModal(modal) {
        modal.classList.remove("visible", "fade-in");
        modal.classList.add("fade-out");

        setTimeout(() => {
            modal.classList.add("hidden");
            modal.classList.remove("fade-out");
            modalBackground.classList.add("hidden");
        }, 300);
    }

    document.getElementById("openSignIn").addEventListener("click", function() {
        openModal(divSignIn);
    });

    document.getElementById("openLogin").addEventListener("click", function() {
        openModal(divLogin);
    });

    closeButton.addEventListener("click", function() {
        closeModal(divSignIn);
    });

    AOS.init();
});