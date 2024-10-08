const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

connection.start().then(function() {
    console.log("Connected");
}).catch(function(err) {
    console.error(err.toString());
});

let recipient = null;

$('.contact-user').click((event) => {
    recipient = $(event.currentTarget).data('user-id');
    $(event.currentTarget).addClass('active-btn');
});

connection.on("ReceiveMessage", function(user, userChecker, message) {
    if (recipient == userChecker) {
        $('.messages').append('<div class="contact-message"><strong>' + user + ':</strong> ' + message + '</div>');
    }
});

$('#btn-submit').click((event) => {
    event.preventDefault();
    var message = $('#input-message').val();
    if (message != null && message != '') {
        $('.messages').append('<div class="user-message"><strong>Eu:</strong>' + message + '</div>');
    }
    if (recipient) {
        let user = $('#sender').val();
        connection.invoke("SendMessage", recipient, user, message)
            .catch(function(err) {
                return console.error(err.toString());
            });
        $('#input-message').val('');
    } else {
        alert('Selecione um contato para enviar a mensagem');
    }
});