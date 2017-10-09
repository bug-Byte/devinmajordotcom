$(document).ready(function () {

    $(document).on("click", ".menu li a", function () {

        $(".menu li a").removeClass('active');

        $(this).addClass('active');

    });

});