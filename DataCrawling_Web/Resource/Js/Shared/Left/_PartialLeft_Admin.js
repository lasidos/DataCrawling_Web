$(function () {
    $('aside > nav > ul > li').click(function () {
        var on = $(this).hasClass('gCurrent');
        $('aside > nav > ul > li').removeClass('gCurrent');
        if (on) {
            $(this).removeClass('gCurrent');
        } else {
            $(this).addClass('gCurrent');
        }
    });
});