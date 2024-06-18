$(document).ready(function () {
    $(window).on('beforeunload', function () {
        $('#reg-form')[0].reset();
        $('#log-form')[0].reset();
    });
});