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
                              <a href="user/${user.userName}?returnTo=${window.location.href}">${user.userName}</a>
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
