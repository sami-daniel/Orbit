$(document).ready(() => {
  $('#profileImage').change(() => {
      var formData = new FormData($('#profile-image-form')[0]);

      $.ajax({
          url: '/user/upload-profile-image',
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
          url: '/user/upload-banner-image',
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
