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

document.addEventListener("DOMContentLoaded", function () {
  const firstStep = document
    .getElementById("divSignIn")
    .querySelector(".first-step");
  const lastStep = document
    .getElementById("divSignIn")
    .querySelector(".last-step");
  const backBtn = document.getElementById("backBtn");

  // Inicialmente esconda a �ltima etapa
  lastStep.style.display = "none";

  // Handler para o bot�o de avan�ar na primeira etapa
  document
    .getElementById("nextBtn")
    .addEventListener("click", function (event) {
      event.preventDefault(); // Previne o comportamento padr�o de submiss�o do formul�rio

      // Verifica��es de validade podem ser adicionadas aqui antes de avan�ar
      if (validateFirstStep()) {
        firstStep.style.display = "none";
        lastStep.style.display = "block";
      } else {
        alert("Por favor, preencha todos os campos corretamente.");
      }
    });

  backBtn.addEventListener("click", function () {
    firstStep.style.display = "block";
    lastStep.style.display = "none";
  });

  // Fun��o para validar a primeira etapa
  function validateFirstStep() {
    const name = document.getElementById("name").value.trim();
    const email = document.getElementById("email").value.trim();
    const dayBirthday = document.getElementById("DayBirthday").value.trim();
    const monthBirthday = document.getElementById("MonthBirthday").value.trim();
    const yearBirthday = document.getElementById("YearBirthday").value.trim();

    // Exemplo de valida��o simples
    return (
      name !== "" &&
      email !== "" &&
      dayBirthday !== "" &&
      monthBirthday !== "" &&
      yearBirthday !== ""
    );
  }
});
