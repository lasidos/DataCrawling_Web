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

    $(document).on("click", ".btn-edit", function (e) {
        e.preventDefault();

        var idx = $(this).closest('tr').attr('data-idx');
        var result = confirm('[초기화] 비밀번호는 초기번호 "qwer1234!"로 설정됩니다.');
        if (result) {
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

        
    });

    $('.filter-area .btn-sch').click(function () {
        SearchTxt();
    });

    $('#searchTxt').keypress(function (e) {
        if (e.which == 13) {
            SearchTxt();
        }
    });

    $(document).on("click", ".reset", function (e) {
        e.preventDefault();

        var idx = $(this).closest('tr').attr('data-idx');
        alert(idx + '리셋');
    });
});

function SearchTxt() {
    if ($('#searchTxt').val() == '') {
        alert('검색어가 입력되지 않았습니다.');
        return;
    }
    GetGroupUser($('#isPage').val());
}

function GetGroupUser(page) {
    $('.col-table tbody').load('/Member/GetGroupUser', {
        GROUP_ID: $('#hidStatType').attr('data-idx'),
        Page: page,
        SearchTxt: $('#searchTxt').val()
    });
}