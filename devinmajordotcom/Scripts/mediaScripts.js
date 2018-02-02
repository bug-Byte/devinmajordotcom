var newWidth = $('.site-wrapper').width() - 74.67;

$(document).ready(function () {

    $(document).on("click", ".menu li a", function () {
        var id = "#" + $(this).data('framename');
        $(".menu li a").removeClass('active');
        $(this).addClass('active');
        $('iframe').hide();
        $(id).fadeIn(500);
    });

    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });

    //$('#nav1').bind("mouseenter", function () {
    //    $('#site-wrapper').animate({ left: '74.67px', width: newWidth }, 500);
    //    $('#nav').animate({ width: '20vw' }, 500);
    //});

    //$('#nav').bind("mouseleave", function () {
    //    $('#site-wrapper').animate({ left: '0px', width: '100%' }, 500);
    //    $('#nav').animate({ width: '0px' }, 500);
    //});

});

$(window).on("resize", function () {
    newWidth = $('.site-wrapper').width() - 74.67;
});