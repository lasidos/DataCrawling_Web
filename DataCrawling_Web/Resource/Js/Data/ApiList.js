$(function () {
    $('.sort-button').click(function () {
        $(this).siblings('.sort-select-modal').toggleClass('active');
    });

    $('.sort-select-modal button').click(function () {
        var txt = $(this).text();
        console.log(txt);

        $(this).parents('.sort-select-modal').toggleClass('active');
    });

    console.log($('.giListRow .process-state .procItem input:checked').length);
})