$(document).ready(function () {

    $(document).on("click", ".menu li a", function () {
        $(".menu li a").removeClass('active');
        $(this).addClass('active');
    });

    $(document).on("hover", "#menu", function() {
        $("#menu").toggleClass("toggle");
    });

});