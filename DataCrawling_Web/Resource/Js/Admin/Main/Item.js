$(function () {
    $('#gPathBar h2').text('상단컨텐츠(module_ty)');

    $('#regist').click(function () {
        location.href = '/Admin/Content?status=new';
    });

    $('tbody tr td:not(:nth-child(5)):not(:last-child)').click(function () {
        var idx = $(this).parent('tr').data('idx');
        location.href = '/Admin/Content?idx=' + idx + '&status=view';
    });

    $('tbody tr .edit').click(function (e) {
        var idx = $(this).parents('tr').data('idx');
        location.href = '/Admin/Content?idx=' + idx + '&status=edit';
    });
});