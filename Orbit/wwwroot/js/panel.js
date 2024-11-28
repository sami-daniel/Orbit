// Handle the click event for the "Show" button
$("#btn-show").click(function(){
    // Fade in the modal and then the form within it
    $(".post-modal").fadeIn(500, function(){
        $(".post-form").fadeIn(500);
    });
});

// Handle the click event for the "Close" button
$(".btn-close").click(function(){
    // Fade out the modal
    $(".post-modal").fadeOut(300);
});

// Enable video controls for all elements with the class "video-content"
$(".video-content").attr("controls", true);

// Show the left aside panel when the respective button is clicked
$("#showAsideLeft").click(function(){
    $("#asideLeft").show();
});

// Hide the left aside panel when the respective close button is clicked
$("#closeAsideLeft").click(function() {
    $("#asideLeft").hide();
});

// Show the right aside panel when the respective button is clicked
$("#showAsideRight").click(function(){
    $("#asideRight").show();
});

// Hide the right aside panel when the respective close button is clicked
$("#closeAsideRight").click(function() {
    $("#asideRight").hide();
});

// Adjust the visibility of the asides based on window resize events
$(window).resize(function () {
    if ($(window).width() > 1200) {
        // Show both asides automatically on screens wider than 1200px
        $("#asideRight").show();
        $("#asideLeft").show();
    }
    else if($(window).width() > 600 && $(window).width() < 1200){
        // Hide the right aside but show the left aside on screens between 600px and 1200px
        $("#asideRight").hide();
        $("#asideLeft").show();
    }
});
