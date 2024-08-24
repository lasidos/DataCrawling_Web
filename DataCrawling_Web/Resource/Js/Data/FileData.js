$(function () {
    $('.tab-cont').load("/Data/SelTabView",
        {
            tab: $('.tab-list li').eq(0).data('tab')
        });

    $('.tab-list li').click(function () {
        $('.tab-list li').removeClass('on');
        $(this).addClass('on');

        $('.tab-cont').load("/Data/SelTabView",
        {
            tab: $(this).data('tab')
        });
    });
});