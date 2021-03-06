﻿var newWidth = $('.site-wrapper').width() - 74.67;

var links;

function removeDisable() {
    $('body').data('loading-disabled', false);
}

function createIframe(url, id) {
    var i = document.createElement("iframe");
    i.src = url;
    i.scrolling = "auto";
    i.frameborder = "0";
    i.id = id.toString().replace("#","");
    i.class = "z-iframe";
    i.allowfullscreen = "true";
    i.webkitallowfullscreen = "true";
    i.sandbox = "allow-forms allow-same-origin allow-pointer-lock allow-scripts allow-popups allow-modals";
    //if (link.IsDefault) {
    //    i.style = "width: 100%; min-height: 100%; z-index: -1; border: none;";
    //} else {
    //    i.style = "width: 100%; min-height: 100%; z-index: -1; border: none; display:none;";
    //}
    i.style = "width: 100%; min-height: 100%; z-index: -1; border: none; display:none;";
    i.onload = function () { $('.preloader').fadeOut(500); };
    document.getElementById("content").appendChild(i);
};

$(document).ready(function () {

    $(document).on("click", ".modalSubmit", function () {
        if ($(".loginForm").valid()) {
            $('.spinner').fadeIn(500);
        }
    });

    $(document).on("click", ".menu li[data-type='link'] a", function () {
        $('iframe').hide();
        $(".menu li").removeClass('active');
        $(this).parent().addClass('active');
        var id = "#" + $(this).data('framename');
        var url = $(this).data('url');
        if ($(id).length == 0) {
            $('.preloader').fadeIn(500);
            createIframe(url, id);
        }
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

function LoginFailure() {
    var $validator = $(".loginForm").validate();
    var errors = { Password: "Could not log in. You are either not an administrator, or you typed your credentials incorrectly. Please try again!" };
    $validator.showErrors(errors);
}

function HideLoginModal(data) {
    $('#LoginModal').modal('hide');
    $("#wrapper").replaceWith(data);
}

$(window).on("resize", function () {
    newWidth = $('.site-wrapper').width();
});