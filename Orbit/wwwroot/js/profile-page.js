$("#openEditDiv").on("click", function() {
    $(".modal-container-edit").fadeIn(500, function() {
        $(".container-edit").fadeIn(1000);
        $("#descricao-textarea").val($("#descricao-textarea").val().trim());
    });
    $("body").css("overflow", "hidden");
});

$("#closeBtn").on("click", function() {
    $(".modal-container-edit").hide();
    $("body").css("overflow", "auto");
});

