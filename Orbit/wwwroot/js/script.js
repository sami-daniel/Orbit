document.addEventListener("DOMContentLoaded", function() {
    const modalBackground = document.getElementById("modalBackground");
    const divSignIn = document.getElementById("divSignIn");
    const divLogin = document.getElementById("divLogin");
<<<<<<< HEAD

=======
>>>>>>> chat
    const closeButton = document.getElementById("closeBtn");
    const closeButtonLogin = document.getElementById("closeBtnLogin");

    closeButtonLogin.addEventListener("click", function() {
        closeModal(divLogin);
    });

    closeButton.addEventListener("click", function() {
        event.preventDefault();
        closeModal(divSignIn);
    });

<<<<<<< HEAD

=======
>>>>>>> chat
    function openModal(modal) {
        modalBackground.classList.remove("hidden");
        modal.classList.remove("hidden", "fade-out");
        modal.classList.add("visible", "fade-in");

<<<<<<< HEAD

=======
>>>>>>> chat
        if (modal === divSignIn) {
            divLogin.classList.remove("visible", "fade-in");
            divLogin.classList.add("hidden");
        } else if (modal === divLogin) {
            divSignIn.classList.remove("visible", "fade-in");
            divSignIn.classList.add("hidden");
        }
    }

<<<<<<< HEAD

=======
>>>>>>> chat
    function closeModal(modal) {
        modal.classList.remove("visible", "fade-in");
        modal.classList.add("fade-out");

        setTimeout(() => {
            modal.classList.add("hidden");
            modal.classList.remove("fade-out");
            modalBackground.classList.add("hidden");
<<<<<<< HEAD
        }, 300); 
    }


    document.getElementById("openSignIn").addEventListener("click", function () {
        openModal(divSignIn);
    });

    document.getElementById("openLogin").addEventListener("click", function () {
        openModal(divLogin);
    });


    closeButton.addEventListener("click", function () {
=======
        }, 300);
    }

    document.getElementById("openSignIn").addEventListener("click", function() {
        openModal(divSignIn);
    });

    document.getElementById("openLogin").addEventListener("click", function() {
        openModal(divLogin);
    });

    closeButton.addEventListener("click", function() {
>>>>>>> chat
        closeModal(divSignIn);
    });

    AOS.init();
});