$(function () {
    $('#gPathBar h2').text('상단컨텐츠(module_ty)');

    // 컨텐츠 신규등록
    $('#regist').click(function () {
        location.href = '/Admin/Content?status=new';
    });

    // 컨텐츠 보기
    $('tbody tr td:not(:nth-child(5)):not(:last-child)').click(function () {
        var idx = $(this).parent('tr').data('idx');
        location.href = '/Admin/Content?idx=' + idx + '&status=view';
    });

    // 컨텐츠 수정
    $('tbody tr .edit').click(function (e) {
        var idx = $(this).parents('tr').data('idx');
        location.href = '/Admin/Content?idx=' + idx + '&status=edit';
    });
});