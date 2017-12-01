var firstRun = true;

$(document).ready(function () {

    InitializePieCharts();
    
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

function UpdateCpuCounter(value, baseScale) {

    return (value / baseScale) * 100;

}

function UpdateRamCounter(value, baseScale) {

    return (value / baseScale) * 100;

}

function updateDriveCounter(value, baseScale) {

    return (value / baseScale) * 100;

}

function ConnectToSignalRPerformanceHub() {

    var performanceHub = $.connection.performanceHub;

    performanceHub.client.updatePerformanceCounters = function (nextCpuValue, nextRamValue, temp, drives) {

        document.getElementById('driveCounters').innerHTML = "";
        var diskCountersHtml = "";
        var ramValues = nextRamValue.split(" / ");
        if (firstRun) {
            for (var x = 0; x < drives.length; x++) {
                var drive = drives[x];
                if (drive != undefined) {
                    firstRun = false;
                }
                var driveData = drive.split(",");
                var percentValue = updateDriveCounter(driveData[4], driveData[5]);
                diskCountersHtml += "<div class='row'><div class='col-md-2'>" + driveData[0] + "</div><div class='col-md-2'>" + driveData[1] + "</div><div class='col-md-2'>" + driveData[2] + "</div><div class='col-md-2'>" + driveData[3] + "</div><div class='col-md-2'>" + driveData[4] + "</div><div class='col-md-2'>" + driveData[5] + "</div></div>";
                diskCountersHtml += '<div id="disk_' + x + '" class="performancePieChart diskChart" data-percent="' + percentValue + '"><span class="percent">' + percentValue + '</span></div>';
            }
            document.getElementById('driveCounters').innerHTML = diskCountersHtml;
            $(".diskChart").each(function() {
                $(this).easyPieChart();
            });
            
        } else {
            debugger;
            $(".diskChart").each(function () {
                
                var id = this.id.split("_")[1];
                var thisDrive = drives[id];
                var thisDriveData = thisDrive.split(",");
                var nextValue = thisDriveData[4];
                var baseScale = thisDriveData[5];
                $(this).data('easyPieChart').update(updateDriveCounter(nextValue, baseScale));
            });
        }

        $("#cpuCounter").data('easyPieChart').update(UpdateCpuCounter(nextCpuValue, 100));
        $("#ramCounter").data('easyPieChart').update(UpdateRamCounter(ramValues[0], ramValues[1]));

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

function InitializePieCharts() {

    $('.performancePieChart').easyPieChart({
        animate: 1000,
        size: 150,
        lineWidth: 5,
        scaleColor: '#dfe0e0',
        trackColor: '#f2f2f2',
        barColor: function (percent) {
            percent /= 100;
            return "rgb(" + Math.round(255 * (1 - percent)) + ", " + Math.round(255 * percent) + ", 0)";
        },
        onStep: function (from, to, percent) {
            $(this.el).find('.percent').text(Math.round(percent));
        }
    });

}