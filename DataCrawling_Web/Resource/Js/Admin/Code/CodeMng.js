$(function () {
    $('#gPathBar h2').text('메뉴 관리');
    $('.page_code li:first-child').addClass('on');

    $('#dvStatType').click(function () {
        $(this).toggleClass('on');
    });

    $('#dvLyrJobType li').click(function () {
        var txt = $(this).find('button').text();
        var code = $(this).data('type');

        if ($('#hidStatType').val() != txt) {
            $('.txCate').text($(this).find('button').text());
            
            $('#hidStatType').val(txt);
            $('#hidStatType').attr('data-mtype', code);
            $('.menu-first li').removeClass('on');
            $('.menu-second li').removeClass('on');
            $('.menu-third li').removeClass('on');
            $('.menu-first ul').empty();
            $('.menu-second ul').empty();
            $('.menu-third ul').empty();

            GetMenu(code, -1, -1);
        }
    });

    // 메뉴 클릭
    $(document).on("click", ".code-list li", function (e) {
        e.preventDefault();

        var code = $('#hidStatType').attr('data-mtype');
        var txt = $(this).text();
        var idx = $(this).data('idx');
        var login = $(this).data('login');
        var visible = $(this).data('visible');
        var url = $(this).data('url');
        var order = $(this).data('order');

        var lv = $(this).closest('.code-list').data('lv');
        if (lv == 0) {
            $('.menu-first li').removeClass('on');
            $('.page_edit .sel-area .sel-val').text("-");
            $('.menu-second ul').empty();
            $('.menu-third ul').empty();
        } else if (lv == 1) {
            $('.menu-second li').removeClass('on');
            var temp = $('.menu-first li.on').text();
            $('.page_edit .sel-area .sel-val').text(temp);
            $('.menu-third ul').empty();
        } else if (lv == 2) {
            $('.menu-third li').removeClass('on');
            var temp = $('.menu-second li.on').text();
            $('.page_edit .sel-area .sel-val').text(temp);
        }
        $(this).addClass('on');

        $('.page_edit').show();
        if (txt == "[신규등록]") {
            $('.page_edit .area-title input').val('');
            $('.page_edit .url-title input').val('');
            $('.page_edit .order-title input').val('');
            $('.page_edit .rdb-login .login').prop("checked", true);
            $('.page_edit .rdb-visible .visible').prop("checked", true);
            $('.edit-new .edit_new').text('신규 등록');
            $('.edit-new .delete').hide();
        } else {
            $('.page_edit .area-title input').val(txt);
            $('.page_edit .url-title input').val(url);
            $('.page_edit .order-title input').val(order);

            if (login == 0) {
                $('.page_edit .rdb-login .unlogin').prop("checked", true);
            } else {
                $('.page_edit .rdb-login .login').prop("checked", true);
            }
            if (visible == 0) {
                $('.page_edit .rdb-visible .invisible').prop("checked", true);
            } else {
                $('.page_edit .rdb-visible .visible').prop("checked", true);
            }

            $('.edit-new .edit_new').text('수정');
            $('.edit-new .delete').show();

            GetMenu(code, idx, lv);
        }

        $('#idx').val(idx);
        if (lv == 0) {
            $('#parent_Id').val('-1');
        } else if (lv == 1) {
            $('#parent_Id').val($('.menu-first li.on').data('idx'));
        } else if (lv == 2) {
            $('#parent_Id').val($('.menu-second li.on').data('idx'));
        }
    });

    // 신규, 수정
    $('.edit-new .edit_new').click(function () {
        var m_type = $('#hidStatType').data('mtype');
        var idx = $('#idx').val();
        var parent_Id = $('#parent_Id').val();
        var title = $('.area-title input').val();
        var url = $('.page_edit .url-title input').val();
        var order = $('.page_edit .order-title input').val();
        var login = $('input[name=chk_login]:checked').val();
        var visible = $('input[name=chk_visible]:checked').val();

        $.ajax({
            async: false,
            type: "post",
            url: '/Code/AddMenu',
            data: {
                menuType: m_type,
                idx: idx,
                parent_Id: parent_Id,
                title: title,
                url: url,
                order: order,
                login: login,
                visible: visible
            },
            success: function (data) {
                location.href = "/Admin/Code/CodeMng";
            },
            error: function (response) {
                console.log(response.responseText);
            }
        });
    });

    // 삭제
    $('.edit-new .delete').click(function () {
        var idx = $('#idx').val();

        $.ajax({
            async: false,
            type: "post",
            url: '/Code/DeleteMenu',
            data: {
                idx: idx
            },
            success: function (data) {
                location.reload();
            },
            error: function (response) {
                console.log(response.responseText);
            }
        });
    });
});

function GetMenu(code, idx, lv) {
    $.ajax({
        async: false,
        type: "post",
        url: '/Code/GetMenu',
        data: {
            Menu_Type: code,
            Parent_Id: idx
        },
        success: function (data) {
            const obj = JSON.parse(data);
            obj.forEach((a, i) => {
                let temp =
                    `<li data-idx="${obj[i].Menu_Idx}" data-use="${obj[i].Use_Stat}" data-login="${obj[i].Login_Stat}" data-visible="${obj[i].Display_Stat}" data-url="${obj[i].Menu_URL}" data-order="${obj[i].Order_No}">${obj[i].Menu_Name}</li>`;

                if (lv == -1) {
                    $('.menu-first ul').append(temp);
                } else if (lv == 0) {
                    $('.menu-second ul').append(temp);
                } else if (lv == 1) {
                    $('.menu-third ul').append(temp);
                }
               
            });

            temp =
                `<li class="new_data" data-parent_Id="${idx}" data-idx="-1" data-use="1">[신규등록]</li>`;

            if (lv == -1) {
                $('.menu-first ul').append(temp);
            } else if (lv == 0) {
                $('.menu-second ul').append(temp);
            } else if (lv == 1) {
                $('.menu-third ul').append(temp);
            }
        },
        error: function (response) {
            alert(response);
        }
    });
}