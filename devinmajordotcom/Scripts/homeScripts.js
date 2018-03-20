var days = new Array('Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday');
var months = new Array('January', 'February', 'March', 'April', 'May', 'June', 'Jully', 'August', 'September', 'October', 'November', 'December');

$(document).ready(function () {

    "use strict";
    
    $('.preloader').fadeOut(1000); // set duration in brackets   
    
    if ($('#currentDateAndTime').length > 0) {
        DisplayDateTime();
    }

    $("#settingsWindowButton").draggable().popover({
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

    $('.custom-navbar a, #home a').on('click', function(event) {
    var $anchor = $(this);
        $('html, body').stop().animate({
        scrollTop: $($anchor.attr('href')).offset().top - 49
        }, 1000);
        event.preventDefault();
    });

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
            $.getJSON("http://api.openweathermap.org/data/2.5/weather?lat=" + latitude + "&lon=" + longitude + "&units=metric" + '&appid=1e1dfc63a4f20867543bd3d8fbb81a30', function (data) {	// add '&units=imperial' to get U.S. measurements
                var currWeather = new Array();								// create array to hold our weather response data
                currWeather['currTemp'] = Math.round(data.main.temp);				// current temperature
                currWeather['highTemp'] = Math.round(data.main.temp_max);			// today's high temp
                currWeather['lowTemp'] = Math.round(data.main.temp_min);			// today's low temp
                currWeather['humidity'] = Math.round(data.main.humidity);			// humidity (in percent)

                currWeather['description'] = data.weather[0].description;				// short text description (ie. rain, sunny, etc.)
                currWeather['icon'] = "http://openweathermap.org/img/w/" + data.weather[0].icon + ".png";	// 50x50 pixel png icon
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
                //}

                console.log(data);												// log weather data for reference (json format) 
                $("#weather").html(response);									// write current weather to textarea
            });
        } else {
            return false;														// respond w/error if no address entered
        }
    }


});

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