$(document).ready(function () {
    // When the DOM is fully loaded, set up an event listener for the window's unload event
    $(window).on('beforeunload', function () {
        // Reset the registration form to its default state
        $('#reg-form')[0].reset();
        // Reset the login form to its default state
        $('#log-form')[0].reset();
    });
});
