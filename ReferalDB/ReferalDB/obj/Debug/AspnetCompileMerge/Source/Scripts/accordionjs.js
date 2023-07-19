$(document).ready(function(){
	
	// get version of browser, this to fix IE 6 bugs.
	var version = jQuery.browser.version; 
		
	// hide all accordian
	$('.accordion > div').hide();

	// attach accordion function to the titles
	 $('.accordion > h2, .accordion > h3').click(function(){
		
		// if section is already open, return false
		if($(this).is('.active')){
			$(this).next('div').slideUp('fast');
			$(this).parent().parent().find('.accordion > h2, .accordion > h3').removeClass("active");
			return false;
		}
		
		// open request and close open
		$(this).parent().parent().find('.accordion > div').slideUp('fast');
		$(this).parent().parent().find('.accordion > h2, .accordion > h3').removeClass("active");
		$(this).addClass("active");
  		$(this).next('div').slideToggle('fast');

		// fix IE 6 bug.
		if(jQuery.browser.msie && version < 7){
			$('.accordion div').addClass('iefix');
		}
		
		return false;
  });

	// navigation description for the homepage
	$('.about-desciption').show("slow").addClass('active');
	$("#about").hover(
		function(){ $('.home-desciption-wrapper div').hide("fast");$('.about-desciption').show("slow").addClass('active');}, 
		function(){ $('.about-desciption').hide("fast").removeClass('active');}
	);
	$("#ifeed").hover(
		function(){ $('.home-desciption-wrapper div').hide("fast");$('.ifeed-desciption').show("slow").addClass('active');}, 
		function(){ $('.ifeed-desciption').hide("fast").removeClass('active');}
	);
	$("#social-media").hover(
		function(){ $('.home-desciption-wrapper div').hide("fast");$('.social-media-desciption').show("slow").addClass('active');}, 
		function(){ $('.social-media-desciption').hide("fast").removeClass('active');}
	);

});

// extra caution just in case the $(document).ready fails to closed all the accodrion's on load.
window.onload = function(){
	$('.accordion > div').hide();
}