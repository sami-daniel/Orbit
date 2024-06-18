$(document).ready(function () {
    $('#searchBar').on('input' ,() => {
        $.ajax(
            {
                url: 'Dashboard/Search',
                method: 'GET',
                contentType: 'application/json',
                data: { username: $('#searchBar').val() },
                success: function (data) {
                    var html = '';
                    data.forEach((user) => {
                        html += `<div class='results' id='result'>${user.userName}</div>`
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