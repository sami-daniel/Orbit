document.addEventListener("DOMContentLoaded", function() {
    const divEdit = document.getElementById("divEditProfile");
    const closeButton = document.getElementById("closeBtn");

    closeButton.addEventListener("click", function() {
        event.preventDefault();
        closeModal(divEdit);
    });

    function openModal(modal) {
        modal.classList.remove("hidden", "fade-out");
        modal.classList.add("visible", "fade-in");
    }

    function closeModal(modal) {
        modal.classList.remove("visible", "fade-in");
        modal.classList.add("fade-out");

        setTimeout(() => {
            modal.classList.add("hidden");
            modal.classList.remove("fade-out");
        }, 300);
    }

    document.getElementById("openEditDiv").addEventListener("click", function() {
        openModal(divEdit);
    });

    closeButton.addEventListener("click", function() {
        closeModal(divEdit);
    });
});