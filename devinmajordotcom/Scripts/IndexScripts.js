var firstRun = true;
var saveButtonPressed = false;

var canvas = document.querySelector("canvas");
canvas.width = window.innerWidth;
canvas.height = window.innerHeight;
var ctx = canvas.getContext("2d");
var TAU = 2 * Math.PI;
times = [];
var balls = [];
var lastTime = Date.now();
var mouseX = -1e9, mouseY = -1e9;

function loop() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    update();
    draw();
    requestAnimationFrame(loop);
}

function Ball(startX, startY, startVelX, startVelY) {
    this.x = startX || Math.random() * canvas.width;
    this.y = startY || Math.random() * canvas.height;
    this.vel = {
        x: startVelX || Math.random() * 2 - 1,
        y: startVelY || Math.random() * 2 - 1
    };
    this.update = function (canvas) {
        if (this.x > canvas.width + 50 || this.x < -50) {
            this.vel.x = -this.vel.x;
        }
        if (this.y > canvas.height + 50 || this.y < -50) {
            this.vel.y = -this.vel.y;
        }
        this.x += this.vel.x;
        this.y += this.vel.y;
    };
    this.draw = function (ctx, can) {
        ctx.beginPath();
        ctx.globalAlpha = .4;
        ctx.fillStyle = '#448fda';
        ctx.arc((0.5 + this.x) | 0, (0.5 + this.y) | 0, 3, 0, TAU, false);
        ctx.fill();
    }
}

for (var i = 0; i < canvas.width * canvas.height / (85 * 85) ; i++) {
    balls.push(new Ball(Math.random() * canvas.width, Math.random() * canvas.height));
}

function update() {
    var diff = Date.now() - lastTime;
    for (var frame = 0; frame * 10.6667 < diff; frame++) {
        for (var index = 0; index < balls.length; index++) {
            balls[index].update(canvas);
        }
    }
    lastTime = Date.now();
}

document.addEventListener('mousemove', function (event) {
    mouseX = event.clientX;
    mouseY = event.clientY;
});

function distMouse(ball) {
    return Math.hypot(ball.x - mouseX, ball.y - mouseY);
}

function draw() {
    ctx.globalAlpha = 1;
    ctx.fillStyle = '#0B1C1E';
    ctx.fillRect(0, 0, canvas.width, canvas.height);
    for (var index = 0; index < balls.length; index++) {
        var ball = balls[index];
        ball.draw(ctx, canvas);
        ctx.beginPath();
        for (var index2 = balls.length - 1; index2 > index; index2 += -1) {
            var ball2 = balls[index2];
            var dist = Math.hypot(ball.x - ball2.x, ball.y - ball2.y);
            if (dist < 100) {
                ctx.strokeStyle = "#448fda";
                ctx.globalAlpha = 1 - (dist > 100 ? .8 : dist / 150);
                ctx.lineWidth = "2px";
                ctx.moveTo((0.5 + ball.x) | 0, (0.5 + ball.y) | 0);
                ctx.lineTo((0.5 + ball2.x) | 0, (0.5 + ball2.y) | 0);
            }
        }
        ctx.stroke();
    }
}

$(document).ready(function () {

    // Start
    loop();

    InitializePieCharts();
    
    ConnectToSignalRPerformanceHub();

    setTimeout(setupHandlebarsHelpers, 50);

    $(".portfolioPanelHeading").on("click", function () {
        var toggler = $(this).children("span");
        toggler.toggleClass("glyphicon-collapse-up");
    });

    $(document).on("click", "#mainLogin", function () {
        $("#LoginModal").modal();
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

    $(".date").datetimepicker({
        format: "L",
        allowInputToggle: true,
        locale: moment.locale("en-ca")
    });

    InitializeMediaDashboardEventHandlers();

    tinymce.init({
        selector: '.tinymce',
        theme: 'modern'
    });

});

function InitializeMediaDashboardEventHandlers() {
    $('button[role="iconpicker"]').iconpicker().change(function () {
        $(this).attr("data-icon", $(this).children("input:first").val());
        $(this).closest(".mediaDashboardLink").find('.iconContainer').html("<span class='fa " + $(this).children("input:first").val() + "'></span>");
    });
    $(".mediaSortable").sortable({
        handle: ".move",
        stop: function (e, ui) {
            $('td.drag', ui.item).click();
            var orderCount = 1;
            $('.hiddenOrder').each(function() {
                $(this).val(orderCount);
                orderCount++;
            });
        }
    });
    $(".form-control").on("change", function () {
        $(this).attr("value", $(this).val());
        if ($(this).hasClass('namer')) {
            $(this).closest(".mediaDashboardLink").find('.titleContainer').html($(this).val());
        }
    });
}

function updateLinks() {
    if (saveButtonPressed == true) {
        $(".newLinkInput").removeClass(".newLinkInput");
        saveButtonPressed = false;
    }
    else {
        saveButtonPressed = true;
    }
}

function HideAdminFirstRunModal() {
    $('#adminFirstRunFormModal').modal('hide');
}

function HideLoginModal() {
    $('#LoginModal').modal('hide');
    $('#mainLogin').replaceWith('<li class="landingPageLink" data-activediv="#appmanager"><a>Settings</a></li>');
    $('#appmanager').fadeIn(500);
}

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

function RemoveMediaLink(id) {
    var number = id.split("_")[1]; 
    if(!$(this).hasClass("newLinkInput")) {
        var linksToChange = $(".newLinkInput");
    }
    $(id).parent().parent().parent().remove();
    //$(".hiddenInput_" + number).remove();
    ManageMediaAjaxSuccess();
}

function ManageMediaAjaxSuccess(data) {
    saveButtonPressed = true;
    updateLinks();
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'success',
        message: 'Your media dashboard links have been updated!'
    });
}

function ManageMediaAjaxFailure(data) {
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'error',
        message: 'Something went wrong! Your media dashboard links have not been updated.'
    });
}

function ManagePortfolioSkillsAjaxSuccess(data) {
    saveButtonPressed = true;
    updateLinks();
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'success',
        message: 'Your portfolio skills have been updated!'
    });
}

function ManagePortfolioSkillsAjaxFailure(data) {
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'error',
        message: 'Something went wrong! Your portfolio skills have not been updated.'
    });
}

function RemoveSkill(id, type) {
    if (type == "languageSkill") {
        $(id).remove();
    }
    ManagePortfolioSkillsAjaxSuccess();
}

function setupHandlebarsHelpers() {

    $(document).on('click', '#addNewMediaDashboardLink', function () {
        var mediaDashboardLinkTemplateSource = $("#mediaDashboardLinkTemplateScript").html();
        var template = Handlebars.compile(mediaDashboardLinkTemplateSource);
        //renderTemplate(template, $(this).data('viewmodel'));
        var linkCount = $(".hiddenID").length + 1;
        var context = { newLinkCounter: linkCount, newID: (linkCount - 1) };
        renderMediaDashboardLinkTemplate(template, context);
    });

    $(document).on('click', '#addNewTechSkillLink', function () {
        var techSkillTemplateSource = $("#techSkillTemplateScript").html();
        var template = Handlebars.compile(techSkillTemplateSource);
        //renderTemplate(template, $(this).data('viewmodel'));
        var linkCount = $(".tech.hiddenSkillId").length + 1;
        var context = { newLinkCounter: linkCount, newID: (linkCount - 1) };
        renderTechSkillTemplate(template, context);
    });

    $(document).on('click', '#addNewLanguageSkillLink', function () {
        var languageSkillTemplateSource = $("#languageSkillTemplateScript").html();
        var template = Handlebars.compile(languageSkillTemplateSource);
        //renderTemplate(template, $(this).data('viewmodel'));
        var linkCount = $(".language.hiddenSkillId").length + 1;
        var context = { newLinkCounter: linkCount, newID: (linkCount - 1) };
        renderLanguageSkillTemplate(template, context);
    });

}

function renderMediaDashboardLinkTemplate(template, data) {
    var html = template(data);
    document.getElementById("mediaDashboardLinksList").innerHTML += html;
    InitializeMediaDashboardEventHandlers();
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