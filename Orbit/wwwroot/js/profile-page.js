// Handle the click event for opening the edit modal
$("#openEditDiv").on("click", function() {
    // Fade in the modal container and then the edit form within it
    $(".modal-container-edit").fadeIn(500, function() {
        $(".container-edit").fadeIn(1000); // Fade in the edit container with a delay
        // Trim any extra spaces in the value of the description textarea
        $("#descricao-textarea").val($("#descricao-textarea").val().trim());
    });
    // Disable page scrolling while the modal is open
    $("body").css("overflow", "hidden");
});

// Handle the click event for closing the edit modal
$("#closeBtn").on("click", function() {
    // Hide the modal container
    $(".modal-container-edit").hide();
    // Re-enable page scrolling
    $("body").css("overflow", "auto");
});