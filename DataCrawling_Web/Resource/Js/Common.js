$(document).ready(function () {
    $('.navigation .left-menu li').mouseover(function () {
        if ($(this).index() > 0) {
            $('.navigation .left-menu-list li').removeClass('active');
            $(this).addClass('active');
        }
    });

    $('.navigation .left-menu li').mouseleave(function () {
        $('.navigation .left-menu-list li').removeClass('active');
    });
});