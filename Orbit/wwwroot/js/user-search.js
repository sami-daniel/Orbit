$(document).ready(function () {
    $('#searchBar').on('input' ,() => {
        $.ajax(
            {
                url: 'Profile/Search',
                method: 'GET',
                contentType: 'application/json',
                data: { username: $('#searchBar').val() },
                success: function (data) {
                    var html = '';
                    data.forEach((user) => {
                        html += `<a href='Profile/watch/${user.userName}' class='results' id='result'>${user.userName}</a>`
                    })
                    document.getElementById('search-box').innerHTML = html;
                },
                error: function (xhr, status, error) {
                    console.log(error);
                }
            }
        )
    }); 
});