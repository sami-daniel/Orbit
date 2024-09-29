$(document).ready(() => {
  $('#profileImage').change(() => {
      var formData = new FormData($('#profile-image-form')[0]);

      $.ajax({
          url: '/user/UploadProfileImage',
          type: 'POST',
          data: formData,
          processData: false,
          contentType: false,
          success: function () {
              location.reload();
          }
      });
  });
});

$(document).ready(() => {
  $('#backgroundImg').change(() => {
      var formData = new FormData($('#form-banner')[0]);

      $.ajax({
          url: '/user/UploadBannerImage',
          type: 'POST',
          data: formData,
          processData: false,
          contentType: false,
          success: function () {
              location.reload();
          }
      });
  });
});

$(document).ready(() => {
  $('#submit-edit').click(() => {
      event.preventDefault();

      var formData = new FormData($('#form-to-edit')[0]);
      formData.append('id', '@Model.UserName');
      $.ajax({
          url: '/user/UpdateProfile',
          type: 'PUT',
          data: formData,
          processData: false,
          contentType: false,
          success: function (response) {
              alert('Perfil atualizado com sucesso!');
          },
          error: function (xhr, status, error) {
              alert('Ocorreu um erro ao atualizar o perfil: ', xhr.responseText);
          }
      });
  });
});
