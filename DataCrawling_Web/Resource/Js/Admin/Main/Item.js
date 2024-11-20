$(function () {
    $('#gPathBar h2').text('상단컨텐츠(module_ty)');

    GetSector($('#hidMenuType0').attr('data-idx'));

    $(document).on("click", ".listWrap", function (e) {
        $(this).toggleClass('on');
    });

    $(document).on("click", "#dvLyrMenuType li", function (e) {
        var txt = $(this).find('button').text();
        var idx = $(this).data('idx');
        if ($('#hidMenuType0').val() != txt) {
            $('#MenuGroup0 .textCategory').text($(this).find('button').text());

            $('#hidMenuType0').val(txt);
            $('#hidMenuType0').attr('data-idx', idx);

            GetSector($('#hidMenuType0').attr('data-idx'));
        }
        $('#SectorGroup0').removeClass('on');
    });

    $(document).on("click", "#dvLyrSectorType li", function (e) {
        var txt = $(this).find('button').text();
        var idx = $(this).data('idx');
        if ($('#hidSectorType0').val() != txt) {
            $('#SectorGroup0 .textCategory').text($(this).find('button').text());

            $('#hidSectorType0').val(txt);
            $('#hidSectorType0').attr('data-idx', idx);

            GetContent();
        }
    });

    // 컨텐츠 신규등록
    $('#regist').click(function () {
        location.href = '/Admin/Content?status=new';
    });
});

function GetSector(idx) {
    $.ajax({
        async: false,
        type: "post",
        url: '/Main/GetSector',
        data: {
            idx: idx,
        },
        success: function (data) {
            const obj = JSON.parse(data);
            $('#hidSectorType0').attr('data-idx', obj[0].CODE);
            $('#hidSectorType0').val(obj[0].SECTOR);
            $('#btnSectorType0 .textCategory').text(obj[0].SECTOR);

            $('#dvLyrSectorType .customSelectList').empty();
            obj.forEach((a, i) => {
                let temp =
                    `<li id="idSectorType" data-idx="${obj[i].CODE}" class="customTypeItem">
                            <button type="button">${obj[i].SECTOR}</button>
                    </li>`;

                $('#SectorGroup0 .customSelectList').append(temp);
            });

            GetContent();
        },
        error: function (response) {
            alert(response);
        }
    });
}

function GetContent() {
    $('.col-table tbody').load('/Main/GetContent', {
        m_idx: $('#hidMenuType0').attr('data-idx'),
        code: $('#hidSectorType0').attr('data-idx')
    });
}