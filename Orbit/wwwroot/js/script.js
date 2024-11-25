$(document).ready(function() {
    $(".container")
        .fadeIn(1000, function() {
            $(this).css("display", "flex");
            $(this).find("label").fadeIn(2000, function() {
                $(this).siblings("button").css("display", "block").animate({opacity: 1}, 1000);
            });
        });
});

$("#btnShow").click(function(){
    $(".modal-container").fadeIn(1000,function (){
        $(this).css("display", "grid");
        $(this).find(".modal-section").fadeIn(2000, function() {
            $(this).find(".login-div").fadeIn(1000);
        });
    });
});

$("#signUpDiv").click(function (){
    $(".login-div").fadeOut(1000, function() {
        $(".signIn-div").fadeIn(1000);
    });
});

$(".btnHide").click(function(){
    $(".modal-container").hide(function() {
        $(".login-div").show();
    });
});



