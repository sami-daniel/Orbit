$(document).ready(function() {
    // Hide the modal container on page load
    $(".modal-container").hide();

    // Fade in the main container
    $(".container")
        .fadeIn(1000, function() {
            // Set the container's display style to flex after fading in
            $(this).css("display", "flex");
            // Fade in all labels inside the container
            $(this).find("label").fadeIn(2000, function() {
                // Display buttons as block elements and animate their opacity
                $(this).siblings("button").css("display", "block").animate({opacity: 1}, 1000);
            });
        });
});

// Handle the click event to show the modal
$("#btnShow").click(function(){
    // Fade in the modal container
    $(".modal-container").fadeIn(1000, function (){
        // Set the modal container's display style to grid
        $(this).css("display", "grid");
        // Fade in the modal section
        $(this).find(".modal-section").fadeIn(2000, function() {
            // Fade in the login section within the modal
            $(this).find(".login-div").fadeIn(1000);
        });
    });
});

// Switch from login view to signup view
$("#signUpDiv").click(function (){
    // Fade out the login section
    $(".login-div").fadeOut(1000, function() {
        // Fade in the signup section after the login section is hidden
        $(".signUp-div").fadeIn(500);
    });
});

// Switch from signup view back to login view
$("#loginDiv").click(function (){
    // Fade out the signup section
    $(".signUp-div").fadeOut(1000, function() {
        // Fade in the login section after the signup section is hidden
        $(".login-div").fadeIn(500);
    });
});

// Handle the click event to hide the modal
$(".btnHide").click(function(){
    // Hide the modal container
    $(".modal-container").hide(function() {
        // Show the login section and hide the signup section when the modal is closed
        $(".login-div").show(function() {
            $(".signUp-div").hide();
        });
    });
});