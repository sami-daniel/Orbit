$(document).ready(function() {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chathub")
        .build();

    connection.start().then(function() {
        console.log("Connected");
    }).catch(function(err) {
        return console.error(err.toString());
    });

    connection.on("ReceiveChatMessage", function(user, message) {
        $('.messages').append('<div class="contact-message"><strong>' + user + ':</strong>' + '<span>' + message + '</span>' + '</div>');
    });

    $('#btn-submit').click((event) => {
        event.preventDefault();
        var message = $('#input-message').val();

        if (message != null && message != '') {
            $('.messages').append('<div class="user-message"><strong>Eu:</strong>' + '<span>' + message + '</span>' + '</div>');
            connection.invoke("SendChatMessage", $("#guest").val(), message)
                .catch(function(err) {
                    console.error(err.toString());
                    alert("Erro ao enviar mensagem");
                })
                .finally(() => {
                    $('#input-message').val('');
                });
        }
    });

    $(document).keydown(function(event) {
        if (event.key === 'Enter') {
            $('#btn-submit').click();
        }
    });
});