$(document).ready(() => {
    // Initialize arrays to store host and guest messages
    const hostMessages = [];
    const hostname = $("#host").val(); // Get the host's username
    const guestname = $("#guest").val(); // Get the guest's username

    // Configure AJAX requests to be synchronous
    $.ajaxSetup({
        async: false
    });

    // Fetch messages sent by the host to the guest
    $.ajax({
        url: "load-messages", // Server endpoint to load messages
        type: "GET", // HTTP method
        data: {
            username: hostname, // Host's username
            with: guestname // Guest's username
        },
        success: function(data) {
            // Process each message and add it to the hostMessages array
            data.$values.forEach(element => {
                const { content, timeStamp, to } = element; // Extract message details
                hostMessages.push({
                    message: content,
                    timeStamp: new Date(timeStamp), // Convert timestamp to Date object
                    to // Message recipient
                });
            });
        }
    });

    // Initialize an array to store guest messages
    const guestMessages = [];

    // Fetch messages sent by the guest to the host
    $.ajax({
        url: "load-messages", // Server endpoint to load messages
        type: "GET", // HTTP method
        data: {
            username: guestname, // Guest's username
            with: hostname // Host's username
        },
        success: function(data) {
            // Process each message and add it to the guestMessages array
            data.$values.forEach(element => {
                const { content, timeStamp, to } = element; // Extract message details
                guestMessages.push({
                    message: content,
                    timeStamp: new Date(timeStamp), // Convert timestamp to Date object
                    to // Message recipient
                });
            });
        }
    });

    // Merge host and guest messages and sort them by timestamp
    const enqueuedMessages = hostMessages.concat(guestMessages).sort((m1, m2) => new Date(m1.timeStamp) - new Date(m2.timeStamp));

    // Display each message in the chat interface
    enqueuedMessages.forEach(m => {
        const { to, message, timeStamp } = m; // Destructure message properties
        let stamp = dayjs(timeStamp).format("HH:mm:ss").toString(); // Format the timestamp

        // Append the message to the appropriate section of the chat
        if (to === guestname) {
            $('.messages').append('<div class="user-message"><strong>Eu:</strong>' + '<span>' + message + '</span>' + stamp + '</div>');
        } else {
            $('.messages').append('<div class="contact-message"><strong>' + guestname + ':</strong>' + '<span>' + message + '</span>' + stamp + '</div>');
        }
    });

    // Scroll to the bottom of the message container
    scrollToBottom();
});

// Function to scroll to the bottom of the messages container
function scrollToBottom(){
    const messageDiv = document.getElementById("message"); // Get the messages container
    messageDiv.scrollTop = messageDiv.scrollHeight; // Set the scroll position to the bottom
}
