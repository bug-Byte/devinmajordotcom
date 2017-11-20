$(document).ready(function () {

    $(document).ready(function () {
        $(".portfolioPanelHeading").on("hide.bs.collapse", function () {
            var toggler = $(this).find("glyphicon-collapse-up");
            toggler.removeClass("glyphicon-collapse-up");
            toggler.removeClass("glyphicon-collapse-down");
        });
        $(".portfolioPanelHeading").on("show.bs.collapse", function () {
            var toggler = $(this).find("glyphicon-collapse-down");
            toggler.removeClass("glyphicon-collapse-down");
            toggler.removeClass("glyphicon-collapse-up");
        });
    });

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

    $('button[role="iconpicker"]').iconpicker();

    $(".sortable").sortable();

    $(document).on("click", ".css-checkbox", function() {
        var currentValue = $(this).val();
        var newValue = "";
        if (currentValue == "True") {
            newValue = "False";
        } else {
            newValue = "True";
        }
        $(this).val(newValue);
        $(this).attr("data-val", newValue.replace("T","t").replace("F","f"));
        $(this).toggleClass("checked");
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