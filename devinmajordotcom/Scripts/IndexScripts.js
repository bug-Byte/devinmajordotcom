﻿var firstRun = true;
var saveButtonPressed = false;
var days = new Array('Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday');
var months = new Array('January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December');
var canvas = document.querySelector("canvas");
var ctx;

if(canvas != null) {
    canvas.width = window.innerWidth;
    canvas.height = window.innerHeight;
    ctx = canvas.getContext("2d");
    
}

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

if(canvas != null) {
    for (var i = 0; i < canvas.width * canvas.height / (85 * 85) ; i++) {
        balls.push(new Ball(Math.random() * canvas.width, Math.random() * canvas.height));
    }
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

function ResetActiveImage(el) {
    $(".BackgroundImage").attr("activeimage", false);
    $(".previewImage").attr("activepreview", false);
    $(el).parent().parent().find('.previewImage:first').attr("activepreview", true);
    $(el).parent().parent().find('.BackgroundImage:first').attr("activeimage", true);
}

function InitializeModal(ID, TITLE) {
    $('.modal-title').text(TITLE);
    $(ID).modal(
        {
            backdrop: 'static',
            keyboard: false
        }
    );
}

function removeDisable() {
    $('body').data('loading-disabled', false);
}

$(window).bind("resize", function () {
    var w = $(window).width();
    var h = $(window).height();
    $("#particles").width(w + "px");
    $("#particles").height(h + "px");
});

function removeDisable() {
    $('body').data('loading-disabled', false);
}

function UpdateFavoritesModal(data) {
    $('#favoritesFormContainer').html(data);
}

function UpdateBlogPostsModal(data) {
    $('#blogPostFormContainer').html(data);
    RefreshTinyMce();
}

function SettingsUpdate(data) {
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'success',
        message: 'Your settings were successfully updated!'
    });
    $("html").html(data);
    $('.preloader').fadeOut(1000); // set duration in brackets   
}

function UpdateFavoritesSuccess() {
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'success',
        message: 'Your favorites were successfully updated!'
    });
}

function UpdateFavorites(data) {
    $("#formModalAddEdit").modal('toggle');
    $("#favoritesListContainer").empty();
    $("#favoritesListContainer").html(data);
}

function UpdateBlogPosts(data) {
    $("#formModalAddEdit").modal('toggle');
    $("#blogPostContainer").empty();
    $("#blogPostContainer").html(data);
    MomentAllDateTimes();
}

function SettingsUpdateFailure() {
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'error',
        message: 'Your settings were not successfully updated! Please try again.'
    });
}

function MomentAllDateTimes() {

    $(".datetime").each(function () {
        var date = $(this).html();
        var timeString = moment(date).format("h:mm:ss a");
        var dateString = moment(date).format("dddd MMMM Do, Y");
        $(this).html(dateString + ", at " + timeString);
    });

    $(".datetimepicker").each(function () {
        var date = $(this).val();
        var dateString = moment(date).format("MM/DD/YYYY");
        $(this).val(dateString);
    });

    $(".datetime, .datetimepicker").removeClass('datetime').removeClass('datetimepicker');

}

$(document).ready(function () {

    // Start
    

    var ms_ie = false;
    var ua = window.navigator.userAgent;
    var old_ie = ua.indexOf('MSIE ');
    var new_ie = ua.indexOf('Trident/');

    if ((old_ie > -1) || (new_ie > -1)) {
        ms_ie = true;
    }

    if (!ms_ie) {
        $("#particles").show();
        if (canvas != null) {
            loop();
        }
    }

    InitializePieCharts();
    
    ConnectToSignalRPerformanceHub();

    setTimeout(setupHandlebarsHelpers, 50);

    MomentAllDateTimes();

    $(document).find(".settingsWindowButton").each(function () {

        $(this).draggable({
            stop: function () {
                var icon = $(this).find("i");
                if ($(icon).hasClass("glyphicon-resize-small")) {
                    $(this).popover('show');
                }
            }
        }).popover({
            html: 'true',
            animation: true,
            trigger: "click",
            placement: 'auto bottom',
            container: 'body'
        }).click(function () {
            //$("#settingsWindow").toggle(500);
            var icon = $(this).find("i");
            if ($(icon).hasClass("glyphicon-cog")) {
                $(icon).removeClass("glyphicon-cog");
                $(icon).addClass("glyphicon-resize-small");
            } else {
                $(icon).removeClass("glyphicon-resize-small");
                $(icon).addClass("glyphicon-cog");
            }
        });

    });
      
    $(document).on('keydown', 'input[data-val-remote]', function () {
        if ($(this).data('val-remote') != undefined) {
            $('body').data('loading-disabled', true);
        } else {
            $('body').data('loading-disabled', false);
        } setTimeout(removeDisable, 1000);
    });

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

    $(document).find('.masthead-nav li').each(function () {
        $(this).click(function () {
            $('.masthead-nav li').removeClass('active');
            var divToShow = $(this).data('activediv');
            $(this).addClass('active');
            $(".inner.cover").hide();
            $(this).show();
            $(divToShow).fadeIn(500);
        });
    });

    $(".landingPageLink.active").click();

    $(".date").datetimepicker({
        format: "L",
        allowInputToggle: true
        //locale: moment.locale("en-us")
    });

    InitializeMediaDashboardEventHandlers();

    RefreshTinyMce();

});

function InitializeMediaDashboardEventHandlers() {
    $('button[role="iconpicker"]').iconpicker().change(function () {
        var icon = $(this).children("input:first").val();
        $(this).attr("data-icon", icon);
        if(icon.indexOf("fa") !== -1) {
            $(this).closest(".handlebarsItem").find('.iconContainer').html("<span class='fa " + $(this).children("input:first").val() + "'></span>");
        }
        else {
            $(this).closest(".handlebarsItem").find('.iconContainer').html("<span class='glyphicon " + $(this).children("input:first").val() + "'></span>");
        }
        //check for fa or glyphicon
        
    });
    $(".mediaSortable").sortable({
        connectWith: '.viewport',
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
            $(this).closest(".handlebarsItem").find('.titleContainer').html($(this).val());
        }
        if($(this).prop("tagName") == "TEXTAREA") {
            $(this).html($(this).val());
        }

    });
    $(".toggler").change(function () {
        var id = $(this).attr("id");
        if (document.getElementById(id).hasAttribute("checked")) {
            $(this).removeAttr("checked");
        } else {
            $(this).attr("checked", "true");
        }
    });
    $('.customColorPicker').each(function() {
        $(this).colorpicker({
            inline: false,
            format: false
        })
        .on('change', function (e) {
            $(this).attr('data-color', e.value);
            $(this).find(".form-control").attr("value", e.value);
            $(this).find(".form-control").val(e.value);

            if ($(this).hasClass('contactSiteLink')) {
                $(this).parent().parent().parent().css('background-color', e.color.toRgbString() + " !important");
            }

        });
    });
    RefreshTinyMce();
    $('#skillCarousel').carousel({
        interval: false,
        wrap: false
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

function RefreshTinyMce() {
    tinymce.remove();
    tinymce.init({
        setup:function(ed) {
            ed.on('change', function(e) {
                $("#" + ed.id).closest('textarea').html(ed.getContent());
            });
        },
        selector: '.tinymce',
        theme: 'modern',
        branding: false,
        plugins: [
            "advlist autolink lists link image charmap print preview anchor",
            "searchreplace visualblocks code fullscreen",
            "insertdatetime media table contextmenu paste imagetools wordcount"
        ],
        toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image",
        image_title: true,
        // enable automatic uploads of images represented by blob or data URIs
        automatic_uploads: true,
        // URL of our upload handler (for more details check: https://www.tinymce.com/docs/configure/file-image-upload/#images_upload_url)
        // images_upload_url: 'postAcceptor.php',
        // here we add custom filepicker only to Image dialog
        file_picker_types: 'image',
        // and here's our custom image picker
        file_picker_callback: function (cb, value, meta) {
            var input = document.createElement('input');
            input.setAttribute('type', 'file');
            input.setAttribute('accept', 'image/*');

            // Note: In modern browsers input[type="file"] is functional without 
            // even adding it to the DOM, but that might not be the case in some older
            // or quirky browsers like IE, so you might want to add it to the DOM
            // just in case, and visually hide it. And do not forget do remove it
            // once you do not need it anymore.

            input.onchange = function () {
                var file = this.files[0];

                var reader = new FileReader();
                reader.onload = function () {
                    // Note: Now we need to register the blob in TinyMCEs image blob
                    // registry. In the next release this part hopefully won't be
                    // necessary, as we are looking to handle it internally.
                    var id = 'blobid' + (new Date()).getTime();
                    var blobCache = tinymce.activeEditor.editorUpload.blobCache;
                    var base64 = reader.result.split(',')[1];
                    var blobInfo = blobCache.create(id, file, base64);
                    blobCache.add(blobInfo);

                    // call the callback and populate the Title field with the file name
                    cb(blobInfo.blobUri(), { title: file.name });
                };
                reader.readAsDataURL(file);
            };

            input.click();
        }
    });
}

function HideLoginModal(data) {

    $('#LoginModal').modal('hide');
    $('#mainLogin').replaceWith('<li class="landingPageLink" data-activediv="#appmanager"><a><span class="fa fa-cog"></span>&nbsp;Settings</a></li>');
    $('#mainContainer').append(data);
    $('#appmanager').fadeIn(500);
    InitializeMediaDashboardEventHandlers();
    RefreshTinyMce();
    $(".masthead-nav").find('li[data-activediv="#appmanager"]').each(function () {
        $(this).click(function () {
            $('.masthead-nav li').removeClass('active');
            var divToShow = $(this).data('activediv');
            $(this).addClass('active');
            $(".inner.cover").hide();
            $(this).show();
            $(divToShow).fadeIn(500);
        });
    });

}

function LoginFailure() {
    var errors = { Password: "Could not log in. You are either not an administrator, or you typed your credentials incorrectly. Please try again!" };
    var $validator = $(".loginForm").validate();
    $validator.showErrors(errors);
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

function UpdateMyHomeSettingsAjaxSuccess() {
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'success',
        message: 'Your settings have been successfully updated! Please go to the page to see any changes you made.'
    });
}

function ManageBannerLinksAjaxFailure() {
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'error',
        message: 'Your main links were not updated! Please try again in about 5 minutes.' + data
    });
}

function ManageBannerLinksAjaxSuccess() {
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'success',
        message: 'Your main links have been successfully updated! Refresh the page to see any changes take effect.'
    });
}

function ManageSiteLinksAjaxFailure() {
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'error',
        message: 'Your application links were not updated! Please try again in about 5 minutes.' + data
    });
}

function ManageSiteLinksAjaxSuccess() {
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'success',
        message: 'Your application links have been successfully updated! Refresh the page to see any changes take effect.'
    });
}

function ManageMainSettingsAjaxSuccess() {
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'success',
        message: 'Your main settings have been successfully updated! Refresh the page to see any changes take effect.'
    });
}

function ManageMainSettingsAjaxFailure() {
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'error',
        message: 'Your main settings were not updated! Please try again in about 5 minutes.' + data
    });
}

function RemoveMediaLink(id) {
    var number = id.split("_")[1];
    if (!$(this).hasClass("newLinkInput")) {
        var linksToChange = $(".newLinkInput");
    }
    var element = document.getElementById($(id).parent().parent().parent().attr('id'));
    element.parentNode.removeChild(element);
    //$(".hiddenInput_" + number).remove();
    ManageMediaAjaxSuccess();
}

function RemoveBannerLink(id) {
    var number = id.split("_")[1];
    if (!$(this).hasClass("newLinkInput")) {
        var linksToChange = $(".newLinkInput");
    }
    var element = document.getElementById($(id).parent().parent().parent().attr('id'));
    element.parentNode.removeChild(element);
    //$(".hiddenInput_" + number).remove();
    ManageBannerLinksAjaxSuccess();
}

function RemoveSiteLink(id) {
    var number = id.split("_")[1];
    if (!$(this).hasClass("newLinkInput")) {
        var linksToChange = $(".newLinkInput");
    }
    var element = document.getElementById($(id).parent().parent().parent().attr('id'));
    element.parentNode.removeChild(element);
    //$(".hiddenInput_" + number).remove();
    ManageSiteLinksAjaxSuccess();
}

function RemoveContactLink(id) {
    var number = id.split("_")[1];
    if (!$(this).hasClass("newLinkInput")) {
        var linksToChange = $(".newLinkInput");
    }
    var element = document.getElementById($(id).parent().parent().parent().attr('id'));
    element.parentNode.removeChild(element);
    //$(".hiddenInput_" + number).remove();
    ManagePortfolioContactLinksAjaxSuccess();
}

function RemoveProject(id) {
    var number = id.split("_")[1];
    if (!$(this).hasClass("newLinkInput")) {
        var linksToChange = $(".newLinkInput");
    }
    var element = document.getElementById($(id).parent().parent().parent().attr('id'));
    element.parentNode.removeChild(element);
    //$(".hiddenInput_" + number).remove();
    ManagePortfolioAjaxSuccess();
}

function ManageMediaAjaxSuccess(data) {
    saveButtonPressed = true;
    updateLinks();
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'success',
        message: 'Your media dashboard links have been updated!'
    });
}


function ManagePortfolioAjaxSuccess(data) {
    saveButtonPressed = true;
    updateLinks();
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'success',
        message: 'Your portfolio has been updated!'
    });
}

function ManagePortfolioContactLinksAjaxSuccess(data) {
    saveButtonPressed = true;
    updateLinks();
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'success',
        message: 'Your contact links have been updated!'
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
        message: 'Something went wrong! Your portfolio has not been updated.'
    });
}

function RemoveSkill(classname, type) {
    
    var element = document.getElementsByClassName(classname.replace(".",""))[0];
    element.parentNode.removeChild(element);
    ManagePortfolioSkillsAjaxSuccess();

    if (type == 'workSkill') {
        var originalNumber = classname.split("_")[1];
        var newID = $('li[data-slide-to="' + originalNumber + '"]').attr("id");
        var Lielement = document.getElementById(newID);
        Lielement.parentNode.removeChild(Lielement);
        $('li[data-target="#myCarousel"]:first').addClass("active");
        $(".item:first").addClass("active");

    }

}

function setupHandlebarsHelpers() {

    $(document).on('click', '#addNewBannerLink', function () {
        var bannerLinkTemplateSource = $("#bannerLinkTemplateScript").html();
        var template = Handlebars.compile(bannerLinkTemplateSource);
        //renderTemplate(template, $(this).data('viewmodel'));
        var linkCount = $(".hiddenBannerLinkID").length + 1;
        var context = { newLinkCounter: linkCount, newID: (linkCount - 1) };
        renderBannerLinkTemplate(template, context);
    });

    $(document).on('click', '#addNewSiteLink', function () {
        var siteLinkTemplateSource = $("#siteLinkTemplateScript").html();
        var template = Handlebars.compile(siteLinkTemplateSource);
        //renderTemplate(template, $(this).data('viewmodel'));
        var linkCount = $(".hiddenSiteLinkID").length + 1;
        var context = { newLinkCounter: linkCount, newID: (linkCount - 1) };
        renderSiteLinkTemplate(template, context);
    });

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
        var linkCount = $(".hiddenTechSkillID").length + 1;
        var context = { newLinkCounter: linkCount, newID: (linkCount - 1) };
        renderTechSkillTemplate(template, context);
    });

    $(document).on('click', '#addNewWorkSkillLink', function () {
        $(".item.active").removeClass("active");
        $(".carousel-indicators li.active").removeClass("active");
        $(".mce-tinymce").each(function() {
            var id = $(this).attr("id");
            var element = document.getElementById(id);
            element.parentNode.removeChild(element);
        });
        var workSkillTemplateSource = $("#workSkillTemplateScript").html();
        var template = Handlebars.compile(workSkillTemplateSource);
        //renderTemplate(template, $(this).data('viewmodel'));
        var linkCount = $(".hiddenWorkSkillID").length + 1;
        var context = { newLinkCounter: linkCount, newID: (linkCount - 1) };
        renderWorkSkillTemplate(template, context);
    });

    $(document).on('click', '#addNewLanguageSkillLink', function () {
        var languageSkillTemplateSource = $("#languageSkillTemplateScript").html();
        var template = Handlebars.compile(languageSkillTemplateSource);
        //renderTemplate(template, $(this).data('viewmodel'));
        var linkCount = $(".language.hiddenSkillId").length + 1;
        var context = { newLinkCounter: linkCount, newID: (linkCount - 1) };
        renderLanguageSkillTemplate(template, context);
    });

    $(document).on('click', '#addNewContactLink', function () {
        var languageSkillTemplateSource = $("#contactLinkTemplateScript").html();
        var template = Handlebars.compile(languageSkillTemplateSource);
        //renderTemplate(template, $(this).data('viewmodel'));
        var linkCount = $(".hiddenContactLinkId").length + 1;
        var context = { newLinkCounter: linkCount, newID: (linkCount - 1) };
        renderContactLinkTemplate(template, context);
    });

}

function renderMediaDashboardLinkTemplate(template, data) {
    var html = template(data);
    document.getElementById("mediaDashboardLinksList").innerHTML += html;
    InitializeMediaDashboardEventHandlers();
}

function renderBannerLinkTemplate(template, data) {
    var html = template(data);
    document.getElementById("bannerLinksList").innerHTML += html;
    InitializeMediaDashboardEventHandlers();
}

function renderSiteLinkTemplate(template, data) {
    var html = template(data);
    document.getElementById("siteLinksList").innerHTML += html;
    InitializeMediaDashboardEventHandlers();
}

function renderTechSkillTemplate(template, data) {
    var html = template(data);
    document.getElementById("techSkillsList").innerHTML += html;
    InitializeMediaDashboardEventHandlers();
}

function renderWorkSkillTemplate(template, data) {
    var html = template(data);
    document.getElementById("workSkillList").innerHTML += html;
    document.getElementsByClassName("carousel-indicators")[0].innerHTML += '<li data-target="#myCarousel" id="indicator_' + data.newID + '" data-slide-to="' + data.newID + '" class="active"></li>';
    InitializeMediaDashboardEventHandlers();
}

function renderLanguageSkillTemplate(template, data) {
    var html = template(data);
    document.getElementById("languageSkillContainer").innerHTML += html;
    InitializeMediaDashboardEventHandlers();
}

function renderContactLinkTemplate(template, data) {
    var html = template(data);
    document.getElementById("contactLinksList").innerHTML += html;
    InitializeMediaDashboardEventHandlers();
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

    performanceHub.client.updatePerformanceCounters = function (cpuList, ramString, nextCpuValues, nextRamValue, temps, drives) {
        
        if (firstRun) {

            var diskCountersHtml = "";
            var usageCountersHtml = "";
            var tempCountersHtml = "";
            var ramCountersHtml = "";

            ramCountersHtml += '<div class="col-sm-4"><div class="chart"><div id="ramCounter" class="performancePieChart ramCounter percentage" data-percent="' + nextRamValue + '"><div class="percent">' + nextRamValue + '</div></div><div class="chartlabel">RAM Usage</div><div class="chartlabel ramChartLabel">' + ramString + '</div></div></div>';

            for (var x = 0; x < drives.length; x++) {
                var drive = drives[x];
                var driveData = drive.split(",");
                var percentValue = updateDriveCounterUsedSpace(driveData[4], driveData[5]);
                diskCountersHtml += '<div class="col-sm-4"><div class="chart"><div id="disk_' + x + '" class="diskChart" data-percent="' + percentValue + '"><div class="percent">' + percentValue + '</div></div><br/><div class="chartlabel">' + driveData[1] + ' (' + driveData[0] + ')</div><div class="chartlabel">' + driveData[2] + ', ' + driveData[3] + '</div></div></div>';
                
            }

            for (var x = 0; x < cpuList.length; x++) {
                var cpu = cpuList[x];
                var value1 = nextCpuValues[x];
                var value2 = temps[x];
                usageCountersHtml += '<div class="col-sm-4"><div class="chart"><div id="cpuCounter' + (x + 1) + '" class="performancePieChart cpuCounter percentage" data-percent="' + value1 + '"><div class="percent">' + value1 + '</div></div><div class="chartlabel">CPU ' + (x + 1) + ' Usage</div><div class="chartlabel">' + cpu + '</div></div></div>';
                tempCountersHtml += '<div class="col-sm-4"><div class="chart"><div id="tempCounter' + (x + 1) + '" class="performancePieChart tempCounter percentage temperature" data-percent="' + value2 + '"><div class="percent">' + value2 + '</div></div><div class="chartlabel">CPU ' + (x + 1) + ' Temp.</div><div class="chartlabel">' + cpu + '</div></div></div>';
            }

            document.getElementById('driveCounters').innerHTML = diskCountersHtml;
            document.getElementById('usageCounters').innerHTML = usageCountersHtml;
            document.getElementById('ramCounters').innerHTML = ramCountersHtml;
            document.getElementById('tempCounters').innerHTML = tempCountersHtml;

            $('.diskChart').easyPieChart({
                animate: 1000,
                size: 150,
                lineWidth: 25,
                lineCap: 'butt',
                scaleColor: false,
                trackColor: 'rgba(250,250,250,0.85)',
                barColor: function (percent) {
                    percent /= 100;
                    return "rgb(" + Math.round(255 * percent) + ", " + Math.round(255 * (1 - percent)) + ", " + Math.round(255 * (1 - percent)) + ")";
                },
                onStep: function (from, to, percent) {
                    $(this.el).find('.percent').text(Math.round(percent) + "% Used");
                }
            });

            $('.performancePieChart').easyPieChart({
                animate: 1000,
                size: 150,
                lineWidth: 25,
                lineCap: 'butt',
                scaleColor: false,
                trackColor: 'rgba(250,250,250,0.85)',
                barColor: function (percent) {
                    percent /= 100;
                    return "rgb(" + Math.round(255 * percent) + ", " + Math.round(255 * (1 - percent)) + ", " + Math.round(255 * (1 - percent)) + ")";
                },
                onStep: function (from, to, percent) {
                    if ($(this.el).hasClass("temperature")) {
                        $(this.el).find('.percent').text(Math.round(percent) + "°C");
                    } else {
                        $(this.el).find('.percent').text(Math.round(percent) + "%");
                    }

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

        var counter1 = 0;
        var counter2 = 0;
        var counter3 = 0;

        $(".cpuCounter").each(function () {
            $(this).data('easyPieChart').update(nextCpuValues[counter1]);
            counter1++;
        });

        $(".tempCounter").each(function () {
            $(this).data('easyPieChart').update(temps[counter2]);
            counter2++;
        });

        $(".ramCounter").each(function () {
            $(this).data('easyPieChart').update(nextRamValue);
            $(this).parent().find(".ramChartLabel:first").html(ramString);
            counter3++;
        });
        
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
        trackColor: 'rgba(250,250,250,0.85)',
        barColor: function (percent) {
            percent /= 100;
            return "rgb(" + Math.round(255 * percent) + ", " + Math.round(255 * (1 - percent)) + ", " + Math.round(255 * (1 - percent)) + ")";
        },
        onStep: function (from, to, percent) {
            if ($(this.el).hasClass("temperature")) {
                $(this.el).find('.percent').text(Math.round(percent) + "°C");
            } else {
                $(this.el).find('.percent').text(Math.round(percent) + "%");
            }
            
        }
    });

}