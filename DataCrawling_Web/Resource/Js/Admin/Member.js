$(function () {
    GetGroupUser($('#isPage').val());

    $('#dvStatType').click(function () {
        $(this).toggleClass('on');
    });

    $('#dvLyrJobType li').click(function () {
        var txt = $(this).find('button').text();
        var idx = $(this).data('idx');

        if ($('#hidStatType').val() != txt) {
            $('.txCate').text($(this).find('button').text());

            $('#hidStatType').val(txt);
            $('#hidStatType').attr('data-idx', idx);

            GetGroupUser($('#isPage').val());
        }
    });

    $(".common-modal .close-button").on("click", function (e) {
        e.preventDefault();
    });

    $(document).on("click", ".btn-edit", function (e) {
        e.preventDefault();

        var idx = $(this).closest('tr').attr('data-idx');
        var result = confirm('[초기화] 비밀번호는 초기번호 "qwer1234!"로 설정됩니다.');
        if (result) {
            alert(idx);
            $.ajax({
                type: "post",
                url: '/Member/ReSetPW',
                dataType: "json",
                data: {
                    M_IDX: idx
                },
                success: function (data) {
                    if (data.success) {
                        alert('해당 계정의 비밀번호가 초기화되었습니다.');
                    }
                },
                error: function (response) {
                    alert(response);
                }
            });
        }

        /*openCenteredWindow('/Admin/Member/GroupSet?GROUP_ID=' + idx, '새창', 600, 400);*/
    });

    $(document).on("click", ".page-item li.active a", function () {

    });

    $(document).on("click", ".reset", function (e) {
        e.preventDefault();

        var idx = $(this).closest('tr').attr('data-idx');
        alert(idx + '리셋');
    });
});

function GetGroupUser(page) {
    $('.col-table tbody').load('/Member/GetGroupUser', {
        GROUP_ID: $('#hidStatType').attr('data-idx'),
        Page: page
    });
}

function openCenteredWindow(url, title, width, height) {
    const screenWidth = window.screen.width;
    const screenHeight = window.screen.height;

    // 새 창의 위치 계산
    const left = (screenWidth - width) / 2;
    const top = (screenHeight - height) / 2;

    // 창 옵션 설정
    const options = `width=${width},height=${height},left=${left},top=${top},status=no,resizable=no,fullscreen=no`;

    // 새 창 열기
    window.open(url, title, options);
}