$(document).ready(function () {
  $('#search-input').on('input', () => {


    $.ajax(
      {
        url: 'user/search',
        method: 'GET',
        contentType: 'application/json',
        data: { username: $('#search-input').val() },
        success: function (data) {
          var html = '';
          $.each(data.$values, (_, user) => {
            html += `<li class="li-user">
                              <a href="user/${user.userName}?returnTo=${window.location.href}" style="display: flex; width:100%; align-items:center;">
                              <div style='border-radius: 100%; width:40px; height:40px; overflow:hidden; margin-right: 15px;'>
                                <img src='user/get-profile-image?userName=${user.userName}' style="width:100%; height:100%; object-fit:cover;" />
                              </div>
                              ${user.userName}
                              </a>
                          </li>`
          })
          document.getElementById('ul-users').innerHTML = html;
        },
        error: function (xhr, status, error) {
          console.log(error);
        }
      }
    )
  });

  $('#search-input').keyup((input) => {
    if ($("#search-input").val() === "") {
      $('#ul-users').addClass("none")
    }
    else {
      $('#ul-users').removeClass("none")
    }
  });
});
