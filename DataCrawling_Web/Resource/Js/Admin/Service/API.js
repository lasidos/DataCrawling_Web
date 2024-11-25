$(function () {
    $('#gPathBar h2').text('API 관리');

    $('tbody tr').click(function () {
        var idx = $(this).attr('data-idx');
        console.log(idx);
        location.href = '/Admin/Service/item?type=api&view=sel&idx=' + idx;
    });

    $('tbody .edit').click(function (e) {
        e.stopPropagation();

        var idx = $(this).parents('tr').attr('data-idx');
        console.log(idx);
        location.href = '/Admin/Service/item?type=api&view=edit&idx=' + idx;
    });
});