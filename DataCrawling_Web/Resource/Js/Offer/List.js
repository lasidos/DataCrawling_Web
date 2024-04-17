$(function () {
    $('.modal').removeClass('active');

    $('.mtcTplTab .tabItems li').click(function () {
        $('.mtcTplTab .tabItems li').removeClass('on');
        $(this).addClass('on');

        $('.listWrap').load("/Offer/GetTabData",
            {
                Tab: $(this).val()
            });
    });
});