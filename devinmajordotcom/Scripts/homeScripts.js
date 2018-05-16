var days = new Array('Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday');
var months = new Array('January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December');

function removeDisable() {
    $('body').data('loading-disabled', false);
}

function UpdateFavoritesModal(data) {
    $('#favoritesFormContainer').html(data);
}

function ResetActiveImage(el) {
    $(".BackgroundImage").attr("activeimage", false);
    $(".previewImage").attr("activepreview", false);
    $(el).parent().parent().find('.previewImage:first').attr("activepreview", true);
    $(el).parent().parent().find('.BackgroundImage:first').attr("activeimage", true);
}

function UpdateComments(data) {
    $(".tinymce").val('');
    tinyMCE.activeEditor.setContent('');
    $("#commentsContainer").empty();
    $("#commentsContainer").html(data);
    MomentAllDateTimes();
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'success',
        message: 'Your comment has been added. Thanks for your input!'
    });
}

function ManageCommentsAjaxFailure() {
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'error',
        message: 'Something went wrong! Your comment has not been removed, please try again.'
    });
}

function RemoveComment(id) {
    var element = document.getElementById($(id).parent().attr('id'));
    element.parentNode.removeChild(element);
    $("#ajaxAlertContainer").bootsnack({
        alertType: 'success',
        message: 'Your comment has been removed from the blog post!'
    });
}

function RefreshTinyMce() {
    tinymce.remove();
    tinymce.init({
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

function InitializeModal(ID, TITLE) {
    $('.modal-title').text(TITLE);
    $(ID).modal(
        {
            backdrop: 'static',
            keyboard: false
        }
    );
    RefreshTinyMce();
}

function MomentAllDateTimes() {

    $(".datetime").each(function () {
        var date = $(this).html();
        var timeString = moment(date).format("h:mm:ss a");
        var dateString = moment(date).format("dddd MMMM Do, Y");
        $(this).html(dateString + ", at " + timeString);
    });

    $(".datetime").removeClass('datetime');

}

$(document).ready(function () {

    "use strict";

    $(document).on('keydown', 'input[data-val-remote]', function () {
        if ($(this).data('val-remote') != undefined) {
            $('body').data('loading-disabled', true);
        } else {
            $('body').data('loading-disabled', false);
        } setTimeout(removeDisable, 1000);
    });
    
    $('.preloader').fadeOut(1000); // set duration in brackets   
    
    if ($('#currentDateAndTime').length > 0) {
        DisplayDateTime();
    }

    MomentAllDateTimes();

    $(document).on("click", ".modalSubmit", function () {
        if ($(".loginForm").valid()) {
            $('.spinner').fadeIn(500);
        }
    });

    $("#settingsWindowButton").draggable({
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

    // MENU
    $('.navbar-collapse a').on('click',function(){
      $(".navbar-collapse").collapse('hide');
    });

    $(window).scroll(function() {
      if ($(".navbar").offset().top > 50) {
        $(".navbar-fixed-top").addClass("top-nav-collapse");
          } else {
            $(".navbar-fixed-top").removeClass("top-nav-collapse");
          }
    });

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(success);			// if geolocation supported, call function
    } else {
        $("#weather").html('Your browser does not support the Geolocation API.');
    }

    // MAGNIFIC POPUP
    $('.image-popup').magnificPopup({
        type: 'image',
        removalDelay: 300,
        mainClass: 'mfp-with-zoom',
        gallery:{
          enabled:true
        },
        zoom: {
        enabled: true, // By default it's false, so don't forget to enable it

        duration: 300, // duration of the effect, in milliseconds
        easing: 'ease-in-out', // CSS transition easing function

        // The "opener" function should return the element from which popup will be zoomed in
        // and to which popup will be scaled down
        // By defailt it looks for an image tag:
        opener: function(openerElement) {
        // openerElement is the element on which popup was initialized, in this case its <a> tag
        // you don't need to add "opener" option if this code matches your needs, it's defailt one.
        return openerElement.is('img') ? openerElement : openerElement.find('img');
        }
      }
    });

    //$('.custom-navbar a, #home a').on('click', function(event) {
    //var $anchor = $(this);
    //    $('html, body').stop().animate({
    //    scrollTop: $($anchor.attr('href')).offset().top - 49
    //    }, 1000);
    //    event.preventDefault();
    //});

    // function to get lat/long and plot on a google map
    function success(position) {
        var latitude = position.coords.latitude;							// set latitude variable
        var longitude = position.coords.longitude;						// set longitude variable

        var latLongResponse = 'Latitude: ' + latitude + ' / Longitude: ' + longitude;	// build string containing lat/long
        //$("#location-lat-long").val(latLongResponse);							// write lat/long string to input field

        getWeather(latitude, longitude);											// get weather for the lat/long
    }


    // function to get weather for an address
    function getWeather(latitude, longitude) {
        if (latitude != '' && longitude != '') {
            $("#weather").html("Retrieving Weather...");										// write temporary response while we get the weather
            $.getJSON("https://api.openweathermap.org/data/2.5/weather?lat=" + latitude + "&lon=" + longitude + "&units=metric" + '&appid=1e1dfc63a4f20867543bd3d8fbb81a30', function (data) {	// add '&units=imperial' to get U.S. measurements
                var currWeather = new Array();								// create array to hold our weather response data
                currWeather['currTemp'] = Math.round(data.main.temp);				// current temperature
                currWeather['highTemp'] = Math.round(data.main.temp_max);			// today's high temp
                currWeather['lowTemp'] = Math.round(data.main.temp_min);			// today's low temp
                currWeather['humidity'] = Math.round(data.main.humidity);			// humidity (in percent)

                currWeather['description'] = data.weather[0].description;				// short text description (ie. rain, sunny, etc.)
                currWeather['icon'] = "https://openweathermap.org/img/w/" + data.weather[0].icon + ".png";	// 50x50 pixel png icon
                currWeather['cloudiness'] = data.clouds.all;							// cloud cover (in percent)
                currWeather['windSpeed'] = Math.round(data.wind.speed);				// wind speed

                currWeather['windDegree'] = data.wind.deg;							// wind direction (in degrees)
                currWeather['windCompass'] = Math.round((currWeather['windDegree'] - 11.25) / 22.5);	// wind direction (compass value)

                currWeather['city'] = data.name;


                // array of direction (compass) names
                var windNames = new Array("North", "North Northeast", "Northeast", "East Northeast", "East", "East Southeast", "Southeast", "South Southeast", "South", "South Southwest", "Southwest", "West Southwest", "West", "West Northwest", "Northwest", "North Northwest");
                // array of abbreviated (compass) names
                var windShortNames = new Array("N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW");
                currWeather['windDirection'] = windNames[currWeather['windCompass']];	// convert degrees and find wind direction name


                var response = "<h3 style='color:white;'><img src='" + currWeather['icon'] + "'>" + currWeather['currTemp'] + " &deg;C</h3><div class=''><ul><li>" + currWeather['city'] + "</li><li>" + currWeather['description'] + "</li><li>High: " + currWeather['highTemp'] + " &deg;C, Low: " + currWeather['lowTemp'] + " &deg;C</li></ul></div>";

                //var response = "Current Weather: " + currWeather['currTemp'] + "\xB0 and " + currWeather['description'];
                //var spokenResponse = "It is currently " + currWeather['currTemp'] + " degrees and " + currWeather['description'];

                //if (currWeather['windSpeed'] > 0) {											// if there's wind, add a wind description to the response
                //    response = response + " with winds out of the " + windNames[currWeather['windCompass']] + " at " + currWeather['windSpeed'];
                //    spokenResponse = spokenResponse + " with winds out of the " + windNames[currWeather['windCompass']] + " at " + currWeather['windSpeed'];
                //    if (currWeather['windSpeed'] == 1) {
                //        response += " mile per hour";
                //        spokenResponse += " mile per hour";
                //    } else {
                //        response += " miles per hour";
                //        spokenResponse += " miles per hour";
                //    }
                //}										// log weather data for reference (json format) 
                $("#weather").html(response);									// write current weather to textarea
            });
        } else {
            return false;														// respond w/error if no address entered
        }
    }


});

function LoginFailure() {
    var $validator = $(".loginForm").validate();
    var errors = { Password: "Could not log in. You are either not an administrator, or you typed your credentials incorrectly. Please try again!" };
    $validator.showErrors(errors);
}

function HideLoginModal(data) {
    window.location.reload(true);
}

function DisplayDateTime() {
    var refresh = 1000;
    setTimeout('RefreshDateTime()', refresh);
}

function RefreshDateTime() {
    var date = new Date();
    var timeString = moment(date).format("h:mm:ss a");
    var dateString = moment(date).format("dddd MMMM Do, Y");
    var dateTimeString = "It's currently " + timeString + ", on " + dateString;
    document.getElementById('currentDateAndTime').innerHTML = dateTimeString;
    DisplayDateTime();
}