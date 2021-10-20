; (function ($) {
    "use strict";

    function myFunction() {
        // Get the navbar
        var navbar = document.getElementById("navbars");
       // console.log(navbar);
        // Get the offset position of the navbar
        var sticky = 300;
        if (window.pageYOffset >= sticky) {
            navbar.classList.add("sticky")
        } else {
            navbar.classList.remove("sticky");
        }
    }
    $(document).ready(function () {

        /**-----------------------------
         *  Navbar fix
         * ---------------------------*/
        $(document).on('click', '.navbar-area .navbar-nav li.menu-item-has-children>a', function (e) {
            e.preventDefault();
        })

        /*------------------
           back to top
       ------------------*/
        $(document).on('click', '.back-to-top', function () {
            $("html,body").animate({
                scrollTop: 0
            }, 2000);
        });

        /*----------------------
            Search Popup
        -----------------------*/
        var bodyOvrelay = $('#body-overlay');
        var searchPopup = $('#search-popup');

        $(document).on('click', '#body-overlay', function (e) {
            e.preventDefault();
            bodyOvrelay.removeClass('active');
            searchPopup.removeClass('active');
        });
        $(document).on('click', '#search', function (e) {
            e.preventDefault();
            searchPopup.addClass('active');
            bodyOvrelay.addClass('active');
        });
        // When the user scrolls the page, execute myFunction
       // window.onscroll = function () { myFunction() };
        // Add the sticky class to the navbar when you reach its scroll position. Remove "sticky" when you leave the scroll position
       

    });

    // humberger toggle menu
    $(document).ready(function(){
        $("#menuToggle #menu .submenu").click(function(){
            $(".dropdown").slideToggle();
        });
    });

    var lastScrollTop = "";
    $(window).on("scroll", function() {
        /*---------------------------------------
        sticky menu activation && Sticky Icon Bar
        -----------------------------------------*/
        var st = $(this).scrollTop();
        var mainMenuTop = $(".navbar-area");
        if ($(window).scrollTop() > 1000) {
            if (st > lastScrollTop) {
                // hide sticky menu on scrolldown
                mainMenuTop.removeClass("nav-fixed");
            } else {
                // active sticky menu on scrollup
                mainMenuTop.addClass("nav-fixed");
            }
        } else {
            mainMenuTop.removeClass("nav-fixed ");
        }
        lastScrollTop = st;
        
        var ScrollTop = $('.back-to-top');
        if ($(window).scrollTop() > 1000) {
            ScrollTop.fadeIn(1000);
        } else {
            ScrollTop.fadeOut(1000);
        }
    });


    $(window).on('load', function () {

        /*-----------------
            preloader
        ------------------*/
        var preLoder = $("#preloader");
        preLoder.fadeOut(0);

        /*-----------------
            back to top
        ------------------*/
        var backtoTop = $('.back-to-top')
        backtoTop.fadeOut();

        /*---------------------
            Cancel Preloader
        ----------------------*/
        $(document).on('click', '.cancel-preloader a', function (e) {
            e.preventDefault();
            $("#preloader").fadeOut(2000);
        });

    });



    
   

   

})(jQuery);