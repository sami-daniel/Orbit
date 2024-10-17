$(document).ready(function() {
    const messageConnection = new signalR.HubConnectionBuilder()
        .withUrl("/chathub")
        .build();

    const notificationConnection = new signalR.HubConnectionBuilder()
        .withUrl("/notification")
        .build();

    notificationConnection.start().then(function() {
        console.log("Connected");
    }).catch(function(err) {
        return console.error(err.toString());
    });
    messageConnection.start().then(function() {
        console.log("Connected");
    }).catch(function(err) {
        return console.error(err.toString());
    });

    messageConnection.on("ReceiveChatMessage", function(_, message) {
        const timeStamp = new Date(); // FIXME
        let stamp = dayjs(timeStamp).format("HH:mm:ss").toString();
        $('.messages').append('<div class="contact-message"><strong>' + $('#guest').val() + ':</strong>' + '<span>' + message + '</span>' + stamp + '</div>');
        scrollToBottom();
    });

    $('#btn-submit').click((event) => {
        event.preventDefault();
        var message = $('#input-message').val();
        message = message.trim()
        if (message != null && message) {
            const timeStamp = new Date();
            let stamp = dayjs(timeStamp).format("HH:mm:ss").toString();
            $('.messages').append('<div class="user-message"><strong>Eu:</strong>' + '<span>' + message + '</span>' + stamp + '</div>');
            messageConnection.invoke("SendChatMessage", $("#guest").val(), message)
                .then(() => {
                    $.ajaxSetup({
                        async: true
                    });
                    $.ajax({
                        url: '/chat/save-message',
                        type: 'POST',
                        data: {
                            message: {
                                content: message,
                                to: $("#guest").val(),
                                timeStamp: new Date().toLocaleString()
                            },
                            from: $("#host").val()
                        }
                    });
                })
                .catch(function(err) {
                    console.error(err.toString());
                    alert("Erro ao enviar mensagem");
                })
                .finally(() => {
                    $('#input-message').val('');
                });
            notificationConnection.invoke("SendNotification", $("#guest").val(), $("#host").val());
        }
        scrollToBottom();
    });

    $(document).keydown(function(event) {
        if (event.key === 'Enter') {
            $('#btn-submit').click();
        }
    });
});

function scrollToBottom(){
    const messageDiv = document.getElementById("message");
    messageDiv.scrollTop = messageDiv.scrollHeight;
}
