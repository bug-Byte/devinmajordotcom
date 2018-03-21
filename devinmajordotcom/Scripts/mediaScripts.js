var newWidth = $('.site-wrapper').width() - 74.67;

function removeDisable() {
    $('body').data('loading-disabled', false);
}

$(document).ready(function () {

    $(document).on("click", ".menu li[data-type='link'] a", function () {
        var id = "#" + $(this).data('framename');
        $(".menu li").removeClass('active');
        $(this).parent().addClass('active');
        $('iframe').hide();
        $(id).fadeIn(500);
    });

    $(document).on('keydown', 'input[data-val-remote]', function () {
        if ($(this).data('val-remote') != undefined) {
            $('body').data('loading-disabled', true);
        } else {
            $('body').data('loading-disabled', false);
        } setTimeout(removeDisable, 1000);
    });

    $(document).on("click", "#mainLogin", function () {
        $("#LoginModal").modal();
    });

    $('ul li').hover(
	    function () {
	        title = $(this).attr('title');
	        $(this).attr({ 'title': '' });
	    },
	    function () {
	        $(this).attr({ 'title': title });
	    }
    );

    $('.sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
        if($("#sidebar").hasClass("active")) {
            $(this).children(":first").removeClass("glyphicon-resize-small");
            $(this).children(":first").addClass("glyphicon-resize-full");
        }
        else {
            $(this).children(":first").removeClass("glyphicon-resize-full");
            $(this).children(":first").addClass("glyphicon-resize-small");
        }
    });

});

function HideLoginModal(data) {
    debugger;
    $('#LoginModal').modal('hide');
    $("#wrapper").replaceWith(data);
}

$(window).on("resize", function () {
    newWidth = $('.site-wrapper').width();
});