$(document).ready(function() {
    // Initialize a SignalR connection for chat messages
    const messageConnection = new signalR.HubConnectionBuilder()
        .withUrl("/chathub") // Connect to the chat hub
        .build();

    // Initialize a SignalR connection for notifications
    const notificationConnection = new signalR.HubConnectionBuilder()
        .withUrl("/notification") // Connect to the notification hub
        .build();

    // Start the notification connection
    notificationConnection.start().then(function() {
        console.log("Connected"); // Log success when the connection is established
    }).catch(function(err) {
        return console.error(err.toString()); // Log error if connection fails
    });

    // Start the message connection
    messageConnection.start().then(function() {
        console.log("Connected"); // Log success when the connection is established
    }).catch(function(err) {
        return console.error(err.toString()); // Log error if connection fails
    });

    // Listen for incoming chat messages
    messageConnection.on("ReceiveChatMessage", function(_, message) {
        const timeStamp = new Date(); // Create a new timestamp
        let stamp = dayjs(timeStamp).format("HH:mm:ss").toString(); // Format the timestamp using dayjs
        // Append the received message to the messages container
        $('.messages').append('<div class="contact-message"><strong>' + $('#guest').val() + ':</strong>' + '<span>' + message + '</span>' + stamp + '</div>');
        scrollToBottom(); // Scroll to the bottom of the messages container
    });

    // Handle the submit button click event
    $('#btn-submit').click((event) => {
        event.preventDefault(); // Prevent default form submission
        var message = $('#input-message').val(); // Get the message from the input field
        message = message.trim(); // Trim any whitespace from the message
        if (message != null && message) { // Check if the message is not empty
            const timeStamp = new Date(); // Create a new timestamp
            let stamp = dayjs(timeStamp).format("HH:mm:ss").toString(); // Format the timestamp
            // Append the sent message to the messages container
            $('.messages').append('<div class="user-message"><strong>Eu:</strong>' + '<span>' + message + '</span>' + stamp + '</div>');
            // Invoke the SendChatMessage method on the SignalR hub
            messageConnection.invoke("SendChatMessage", $("#guest").val(), message)
                .then(() => {
                    // Configure and send an AJAX POST request to save the message
                    $.ajaxSetup({
                        async: true // Ensure the request is asynchronous
                    });
                    $.ajax({
                        url: '/chat/save-message', // Server endpoint to save the message
                        type: 'POST', // HTTP method
                        data: {
                            message: {
                                content: message, // Message content
                                to: $("#guest").val(), // Recipient
                                timeStamp: new Date().toLocaleString() // Formatted timestamp
                            },
                            from: $("#host").val() // Sender
                        }
                    });
                })
                .catch(function(err) {
                    console.error(err.toString()); // Log error if the send fails
                    alert("Erro ao enviar mensagem"); // Display an error alert
                })
                .finally(() => {
                    $('#input-message').val(''); // Clear the input field
                });
            // Invoke the SendNotification method on the SignalR hub
            notificationConnection.invoke("SendNotification", $("#guest").val(), `<a href='/chat/${$("#guest").val()}'>Nova mensagem de ${$("#host").val()}</a>`);
        }
        scrollToBottom(); // Scroll to the bottom of the messages container
    });

    // Trigger the send button when the 'Enter' key is pressed
    $(document).keydown(function(event) {
        if (event.key === 'Enter') {
            $('#btn-submit').click(); // Trigger a click event on the submit button
        }
    });
});

// Function to scroll to the bottom of the messages container
function scrollToBottom(){
    const messageDiv = document.getElementById("message"); // Get the messages container
    messageDiv.scrollTop = messageDiv.scrollHeight; // Set the scroll position to the bottom
}

// Get the button and container elements for the responsive contacts menu
const btnmenu = document.getElementById("btn_contacts");
const div = document.getElementById("contact_container_responsive");
const btnclose = document.getElementById("close");

// Add the 'active' class to show the contacts menu when the button is clicked
btnmenu.addEventListener("click", function(){
    div.classList.add("active");
});

// Remove the 'active' class to hide the contacts menu when the close button is clicked
btnclose.addEventListener("click", function(){
    div.classList.remove("active");
});
