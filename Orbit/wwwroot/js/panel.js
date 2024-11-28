$("#btn-show").click(function(){
    $(".post-modal").fadeIn(500, function(){
        $(".post-form").fadeIn(500);
    });
});

$(".btn-close").click(function(){
    $(".post-modal").fadeOut(300);
});

$(".video-content").attr("controls",true);

$("#showAsideLeft").click(function(){
    $("#asideLeft").show();
});

$("#closeAsideLeft").click(function() {
    $("#asideLeft").hide();
});

$("#showAsideRight").click(function(){
    $("#asideRight").show();
});

$("#closeAsideRight").click(function() {
    $("#asideRight").hide();
});

$(window).resize(function () {
    if ($(window).width() > 1200) {
        // Mostrar as asides automaticamente em telas maiores que 768px
        $("#asideRight").show();
        $("#asideLeft").show();
    }
    else if($(window).width() > 600 && $(window).width() < 1200){
        $("#asideRight").hide();
        $("#asideLeft").show();
    }
});
