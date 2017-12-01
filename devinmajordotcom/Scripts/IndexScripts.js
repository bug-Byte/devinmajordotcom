$(document).ready(function () {
    
    ConnectToSignalRPerformanceHub();

    $(".portfolioPanelHeading").on("click", function () {
        var toggler = $(this).children("span");
        toggler.toggleClass("glyphicon-collapse-up");
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

    $(".mediaSortable").sortable({
        handle: ".move",
        stop: function (e, ui) {
            $('td.drag', ui.item).click();
        }
    });

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

    tinymce.init({
        selector: '.tinymce',
        theme: 'modern'
    });

});

function AjaxSuccess(data) {
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'success',
        message: 'Your email was successfully sent to the Administrator of this site!'
    });
}

function AjaxFailure(data) {
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'error',
        message: 'Your email was not sent! Please try again in about 5 minutes.' + data
    });
}

function ConnectToSignalRPerformanceHub() {

    var performanceHub = $.connection.performanceHub;

    performanceHub.client.updatePerformanceCounters = function (nextCpuValue, nextRamValue, temp, drives) {

        document.getElementById('driveCounters').innerHTML = "";
        var diskCountersHtml = "";

        for (var x = 0; x < drives.length; x++) {
            var drive = drives[x];
            var driveData = drive.split(",");
            diskCountersHtml += "<div class='row'><div class='col-md-2'>" + driveData[0] + "</div><div class='col-md-2'>" + driveData[1] + "</div><div class='col-md-2'>" + driveData[2] + "</div><div class='col-md-2'>" + driveData[3] + "</div><div class='col-md-2'>" + driveData[4] + "</div><div class='col-md-2'>" + driveData[5] + "</div></div>";
        }

        document.getElementById('cpuCounter').innerHTML = nextCpuValue;
        document.getElementById('ramCounter').innerHTML = nextRamValue;
        document.getElementById('tempCounter').innerHTML = temp;
        document.getElementById('driveCounters').innerHTML = diskCountersHtml;

        

    };

    $.connection.hub.start().done(function () {
        performanceHub.server.SendPerformanceMonitoring();
    }).fail(function (reason) {
        $("#ajaxAlertContainer").bootsnack({
            alertType: 'error',
            message: 'SignalR is not running.'
        });
    });

}