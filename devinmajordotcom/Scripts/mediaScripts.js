var newWidth = $('.site-wrapper').width() - 74.67;

$(document).ready(function () {

    $(document).on("click", ".menu li a", function () {
        var id = "#" + $(this).data('framename');
        $(".menu li").removeClass('active');
        $(this).parent().addClass('active');
        $('iframe').hide();
        $(id).fadeIn(500);
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

$(window).on("resize", function () {
    newWidth = $('.site-wrapper').width() - 74.67;
});