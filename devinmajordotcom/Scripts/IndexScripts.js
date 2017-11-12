$(document).ready(function () {

    $(".work-wrapper").hover(function () {
        $(this).find('.glyphicon').addClass('blueGlyphicon', 600);
    }, function () {
        $(this).find('.glyphicon').removeClass('blueGlyphicon', 600);
    });

    $(".masthead-nav").find('li').each(function () {
        $(this).click(function () {
            $('.masthead-nav li').removeClass('active');
            var divToShow = $(this).data('activediv');
            $(this).addClass('active');
            $(".inner.cover").hide();
            $(this).show();
            $(divToShow).fadeIn(500);
        });
    });

});

function MailSuccess(data) {
    debugger;
    $("#bootsnackAlertContainer").bootsnack({
        alertType: 'success',
        message: 'Your email was successfully sent to the Administrator of this site!'
    });
}

function MailFailure(data) {
    debugger;
    $("#bootsnackAlertContainer").bootsnack({
        alertType: 'error',
        message: 'Your email was not sent! Please try again in about 5 minutes.' + data
    });
}