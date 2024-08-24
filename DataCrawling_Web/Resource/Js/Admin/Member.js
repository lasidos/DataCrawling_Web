$(function () {
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

            GetGroupUser(idx);
        }
    });

    $(".common-modal .close-button").on("click", function (e) {
        e.preventDefault();
        $('.common-modal.service-modal').removeClass('active');
    });

    $(document).on("click", ".btn-edit", function (e) {
        e.preventDefault();

        var idx = $(this).closest('tr').attr('data-idx');
        $('.common-modal.service-modal').addClass('active');
    });

    $(document).on("click", ".reset", function (e) {
        e.preventDefault();

        var idx = $(this).closest('tr').attr('data-idx');
        alert(idx + '리셋');
    });
});

function GetGroupUser(idx) {
    $.ajax({
        async: false,
        type: "post",
        url: '/Member/GetGroupUser',
        data: {
            GROUP_ID: idx
        },
        success: function (data) {
            $('.col-table tbody').empty();
            const obj = JSON.parse(data);
            obj.forEach((a, i) => {
                let temp =
                    `<tr class="paramtrCls" data-idx="${obj[i].IDX}">`
                    + `<td>${obj[i].OrderNo}</td>`
                    + `<td>${obj[i].User_ID}</td>`
                    + `<td>${obj[i].User_Name}</td>`
                    + `<td>${obj[i].Phone}</td>`
                    + `<td>${obj[i].Gender}</td>`
                    + `<td>${obj[i].GROUP_NAME}</td>`
                    + `<td>${obj[i].DESCRIPTION}</td>`
                    + `<td>${obj[i].LastLoginDateST}</td>`
                    + `<td>${obj[i].RegistDateST}</td>`
                    + `<td><span class="btn btn-edit">그룹설정</span></td>`
                    + `<td><span class="btn reset">초기화</span></td></tr>`;
                $('.col-table tbody').append(temp);
            });
        },
        error: function (response) {
            alert(response);
        }
    });
}