var firstRun = true;

$(document).ready(function () {

    InitializePieCharts();
    
    ConnectToSignalRPerformanceHub();

    setTimeout(setupHandlebarsHelpers, 50);

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
        $(this).attr("value", $(this).val());
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

function setupHandlebarsHelpers() {

    

    $(document).on('click', '#addNewMediaDashboardLink', function () {
        var mediaDashboardLinkTemplateSource = $("#mediaDashboardLinkTemplateScript").html();
        var template = Handlebars.compile(mediaDashboardLinkTemplateSource);
        //renderTemplate(template, $(this).data('viewmodel'));
        var linkCount = $(".mediaDashboardLink").length;
        var context = { newLinkCounter: linkCount };
        renderMediaDashboardLinkTemplate(template, context);
    });

    $(document).on('click', '#addNewTechSkillLink', function () {
        debugger;
        var techSkillTemplateSource = $("#techSkillTemplateScript").html();
        var template = Handlebars.compile(techSkillTemplateSource);
        //renderTemplate(template, $(this).data('viewmodel'));
        var newId = $(".techSkill_" + ($(".techSkill").length - 1)).closest("hiddenSkillId");
        var linkCount = $(".techSkill").length;
        var context = { newLinkCounter: linkCount, newID: newId };
        renderTechSkillTemplate(template, context);
    });

    $(document).on('click', '#addNewLanguageSkillLink', function () {
        var languageSkillTemplateSource = $("#languageSkillTemplateScript").html();
        var template = Handlebars.compile(languageSkillTemplateSource);
        //renderTemplate(template, $(this).data('viewmodel'));
        var newId = $(".languageSkill_" + ($(".languageSkill").length - 1)).closest("hiddenSkillId");
        var linkCount = $(".languageSkill").length;
        var context = { newLinkCounter: linkCount, newID: newId };
        renderLanguageSkillTemplate(template, context);
    });

}

function renderMediaDashboardLinkTemplate(template, data) {
    var html = template(data);
    document.getElementById("mediaDashboardLinksList").innerHTML += html;
    $('button[role="iconpicker"]').iconpicker();
    $(".form-control, .css-checkbox").on("change", function () {
        $(this).attr("value", $(this).val());
    });
}

function renderTechSkillTemplate(template, data) {
    var html = template(data);
    document.getElementById("techSkillContainer").innerHTML += html;
    $(".form-control, .css-checkbox").on("change", function () {
        $(this).attr("value", $(this).val());
    });
}

function renderLanguageSkillTemplate(template, data) {
    var html = template(data);
    document.getElementById("languageSkillContainer").innerHTML += html;
    $(".form-control, .css-checkbox").on("change", function () {
        $(this).attr("value", $(this).val());
    });
}

function UpdateCpuCounter(value, baseScale) {

    return (value / baseScale) * 100;

}

function UpdateRamCounter(value, baseScale) {

    return (value / baseScale) * 100;

}

function updateDriveCounterUsedSpace(value, baseScale) {

    return ((baseScale - value) / baseScale) * 100;

}

function ConnectToSignalRPerformanceHub() {

    var performanceHub = $.connection.performanceHub;

    performanceHub.client.updatePerformanceCounters = function (nextCpuValue, nextRamValue, temp, drives) {
        
        if (firstRun) {
            var diskCountersHtml = "<div class='row'>";
            for (var x = 0; x < drives.length; x++) {
                var drive = drives[x];
                var driveData = drive.split(",");
                var percentValue = updateDriveCounterUsedSpace(driveData[4], driveData[5]);
                diskCountersHtml += '<div class="col-sm-4"><div class="chart"><div id="disk_' + x + '" class="diskChart" data-percent="' + percentValue + '"><div class="percent">' + percentValue + '</div></div><br/><div class="chartlabel">' + driveData[1] + ' (' + driveData[0] + ')</div><div class="chartlabel">' + driveData[2] + ', ' + driveData[3] + '</div></div></div>';
                
            }
            diskCountersHtml += "</div>";
            document.getElementById('driveCounters').innerHTML = diskCountersHtml;
            $('.diskChart').easyPieChart({
                animate: 1000,
                size: 150,
                lineWidth: 25,
                lineCap: 'butt',
                scaleColor: false,
                trackColor: 'rgba(250,250,250,0.65)',
                barColor: function (percent) {
                    percent /= 100;
                    return "rgb(" + Math.round(255 * percent) + ", " + Math.round(255 * (1 - percent)) + ", " + Math.round(255 * (1 - percent)) + ")";
                },
                onStep: function (from, to, percent) {
                    $(this.el).find('.percent').text(Math.round(percent) + "% Used");
                }
            });
            
        } else {
            $(".diskChart").each(function () {
                var id = this.id.split("_")[1];
                var thisDrive = drives[id];
                var thisDriveData = thisDrive.split(",");
                var nextValue = thisDriveData[4];
                var baseScale = thisDriveData[5];
                $(this).data('easyPieChart').update(updateDriveCounterUsedSpace(nextValue, baseScale));
            });
        }

        var ramValues = nextRamValue.split(" / ");
        $("#cpuCounter").data('easyPieChart').update(UpdateCpuCounter(nextCpuValue, 100));
        $("#ramCounter").data('easyPieChart').update(UpdateRamCounter(ramValues[0], ramValues[1]));
        firstRun = false;
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
        lineWidth: 25,
        lineCap: 'butt',
        scaleColor: false,
        trackColor: 'rgba(250,250,250,0.65)',
        barColor: function (percent) {
            percent /= 100;
            return "rgb(" + Math.round(255 * percent) + ", " + Math.round(255 * (1 - percent)) + ", " + Math.round(255 * (1 - percent)) + ")";
        },
        onStep: function (from, to, percent) {
            $(this.el).find('.percent').text(Math.round(percent) + "%");
        }
    });

}