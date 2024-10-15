$(document).ready(() => {
    const hostMessages = [];
    const hostname = $("#host").val();
    const guestname = $("#guest").val()

    $.ajaxSetup({
        async: false
    });
    $.ajax({
        url: "load-messages",
        type: "GET",
        data: {
            username: hostname,
            with: guestname
        },
        success: function(data) {
            data.$values.forEach(element => {
                const { content, timeStamp, to } = element;
                hostMessages.push({
                    message: content,
                    timeStamp: new Date(timeStamp),
                    to
                });
            });
        }
    });

    const guestMessages = [];
    $.ajax({
        url: "load-messages",
        type: "GET",
        data: {
            username: guestname,
            with: hostname
        },
        success: function(data) {
            data.$values.forEach(element => {
                const { content, timeStamp, to } = element;
                guestMessages.push({
                    message: content,
                    timeStamp: new Date(timeStamp),
                    to
                });
            });
        }
    });

    const enqueuedMessages = hostMessages.concat(guestMessages).sort((m1, m2) => new Date(m1.timeStamp) - new Date(m2.timeStamp));
    enqueuedMessages.forEach(m => {
        const { to, message, timeStamp } = m;
        let stamp = dayjs(timeStamp).format("HH:mm:ss").toString();
        if (to === guestname) {
            $('.messages').append('<div class="user-message"><strong>Eu:</strong>' + '<span>' + message + '</span>' + stamp + '</div>');
        } else {
            $('.messages').append('<div class="contact-message"><strong>' + guestname + ':</strong>' + '<span>' + message + '</span>' + stamp + '</div>');
        }
      });
    scrollToBottom();
});

function scrollToBottom(){
  const messageDiv = document.getElementById("message");
  messageDiv.scrollTop = messageDiv.scrollHeight;
}
