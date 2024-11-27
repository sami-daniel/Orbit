$("#btn-show").click(function(){
    $(".post-modal").fadeIn(500, function(){
        $(".post-form").fadeIn(500);
    });
});

$(".btn-close").click(function(){
    $(".post-modal").fadeOut(300);
});
