
function MomentAllDateTimes() {

    $(".datetime").each(function () {
        var date = $(this).html();
        var dateString = moment(date).format("dddd MMMM Do, Y");
        $(this).html(dateString);
    });

    $(".datetime").removeClass('datetime');

}

function AjaxSuccess(data) {
    $("#sendEmail").prop('disabled', false);
    $(".mailSpinner").fadeOut(500);
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

// ISOTOPE FILTER

$(document).ready(function() {

    MomentAllDateTimes();

    $(document).on("submit", "#dropMeALineEmailForm", function () {
        $("#sendEmail").prop('disabled', true);
        $(".mailSpinner").fadeIn(500);
    });

	if ( $('.iso-box-wrapper').length > 0 ) { 

	    var $container 	= $('.iso-box-wrapper'), 
	    	$imgs 		= $('.iso-box img');



	    $container.imagesLoaded(function () {

	    	$container.isotope({
				layoutMode: 'fitRows',
				itemSelector: '.iso-box'
	    	});

	    	$imgs.load(function(){
	    		$container.isotope('reLayout');
	    	})

	    });

	    //filter items on button click

	    $('.filter-wrapper li a').click(function(){

	        var $this = $(this), filterValue = $this.attr('data-filter');

			$container.isotope({ 
				filter: filterValue,
				animationOptions: { 
				    duration: 750, 
				    easing: 'linear', 
				    queue: false, 
				}              	 
			});	            

			// don't proceed if already selected 

			if ( $this.hasClass('selected') ) { 
				return false; 
			}

			var filter_wrapper = $this.closest('.filter-wrapper');
			filter_wrapper.find('.selected').removeClass('selected');
			$this.addClass('selected');

	      return false;
	    }); 

	}

});


// MAIN NAVIGATION

 $('.main-navigation').onePageNav({
        scrollThreshold: 0.2, // Adjust if Navigation highlights too early or too late
        scrollOffset: 75, //Height of Navigation Bar
        filter: ':not(.external)',
        changeHash: true
    }); 

    /* NAVIGATION VISIBLE ON SCROLL */
    mainNav();
    $(window).scroll(function () {
        mainNav();
    });

    function mainNav() {
        var top = (document.documentElement && document.documentElement.scrollTop) || document.body.scrollTop;
        if (top > 40) $('.sticky-navigation').stop().animate({
            "opacity": '1',
            "top": '0'
        });
        else $('.sticky-navigation').stop().animate({
            "opacity": '0',
            "top": '-75'
        });
    }


// HIDE MOBILE MENU AFTER CLIKING ON A LINK

    $('.navbar-collapse a').click(function(){
        $(".navbar-collapse").collapse('hide');
    });
