$(document).ready(() => {
    // Set up an event listener for changes to the profile image input field
    $('#profileImage').change(() => {
        // Create a FormData object with the data from the profile image form
        var formData = new FormData($('#profile-image-form')[0]);

        // Send the profile image data to the server using AJAX
        $.ajax({
            url: '/user/upload-profile-image', // Server endpoint for profile image upload
            type: 'POST', // HTTP method
            data: formData, // Form data containing the image
            processData: false, // Do not process data (required for FormData)
            contentType: false, // Do not set content type (required for FormData)
            success: function () {
                location.reload(); // Reload the page on successful upload
            }
        });
    });
});

$(document).ready(() => {
    // Set up an event listener for changes to the background image input field
    $('#backgroundImg').change(() => {
        // Create a FormData object with the data from the banner form
        var formData = new FormData($('#form-banner')[0]);

        // Send the banner image data to the server using AJAX
        $.ajax({
            url: '/user/upload-banner-image', // Server endpoint for banner image upload
            type: 'POST', // HTTP method
            data: formData, // Form data containing the image
            processData: false, // Do not process data (required for FormData)
            contentType: false, // Do not set content type (required for FormData)
            success: function () {
                location.reload(); // Reload the page on successful upload
            }
        });
    });
});
