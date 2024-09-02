document.addEventListener("DOMContentLoaded", function () {
    const divEdit = document.getElementById("divEditProfile");
    // Supondo que o botão de fechar tenha o id 'closeBtn'
    const closeButton = document.getElementById("closeBtn");

    closeButton.addEventListener("click", function () {
        event.preventDefault();
        closeModal(divEdit);
    });

    // Função para abrir um modal específico
    function openModal(modal) {
        modal.classList.remove("hidden", "fade-out");
        modal.classList.add("visible", "fade-in");
    }

    // Função para fechar todos os modais
    function closeModal(modal) {
        modal.classList.remove("visible", "fade-in");
        modal.classList.add("fade-out");

        setTimeout(() => {
            modal.classList.add("hidden");
            modal.classList.remove("fade-out");
        }, 300); // Atraso igual à duração da animação fadeOut
    }

    // Listener para o botão de "Cadastrar"
    document.getElementById("openEditDiv").addEventListener("click", function () {
        openModal(divEdit);
    });
  
    // Adiciona o listener para o botão de fechar
    closeButton.addEventListener("click", function () {
        closeModal(divEdit);
    });
});
