$(function () {
    GetGroupUser($('#isPage').val());

    $(document).on("click", ".listWrap", function (e) {
        $(this).toggleClass('on');
    });

    $('#dvLyrManagementType li').click(function () {
        var txt = $(this).find('button').text();
        var idx = $(this).data('idx');

        if ($('#hidManagementType').val() != txt) {
            $('.textCategory').text($(this).find('button').text());

            $('#hidManagementType').val(txt);
            $('#hidManagementType').attr('data-idx', idx);

            GetGroupUser($('#isPage').val());
        }
    });

    $(document).on("click", "#dvLyrGroupType li", function (e) {
        var txt = $(this).find('button').text();
        var idx = $(this).data('idx');
        var m_idx = $(this).parents('.paramtrCls').data('idx');
        var id = $(this).parents('.layerBox').siblings('.hiddenResult').attr('id');

        if ($('#' + id).val() != txt) {
            var ele = $(this).parents('.customSelect').find('.textCategory');
            var result = confirm('[권한변경] ' + $('#' + id).val() + '에서 ' + txt + '로 변경하시겠습니까?');
            if (result) {
                $.ajax({
                    type: "post",
                    url: '/Member/SetAuthority',
                    dataType: "json",
                    data: {
                        IDX: $('#' + id).data('idx'),
                        GROUP_ID: idx,
                        M_IDX: m_idx
                    },
                    success: function (data) {
                        if (data.success) {
                            location.reload();
                        }
                    },
                    error: function (response) {
                        alert(response);
                    }
                });
            }
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
        GROUP_ID: $('#hidManagementType').attr('data-idx'),
        Page: page,
        SearchTxt: $('#searchTxt').val()
    });
}