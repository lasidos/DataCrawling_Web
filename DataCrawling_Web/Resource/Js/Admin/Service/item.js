$(function () {
    $(document).on("click", ".listWrap", function (e) {
        $(this).toggleClass('on');
    });

    $(document).on("click", ".layerBox li", function (e) {
        var txt = $(this).find('button').text();
        var idx = $(this).data('idx');
        var ele = $(this).parents('.listWrap');
        if (ele.find('.hiddenResult').val() != txt) {
            ele.find('.textCategory').text($(this).find('button').text());

            ele.find('.hiddenResult').val(txt);
            ele.find('.hiddenResult').attr('data-idx', idx);
        }
    });

    $('.add-request').click(function () {
        var ele = $('.request tbody .paramtrCls:last-child');
        ele.clone().appendTo('.request tbody');
        $('.request tbody .paramtrCls:last-child').find('input').val('');
    });

    $('.add-response').click(function () {
        var ele = $('.response tbody .paramtrCls:last-child');
        ele.clone().appendTo('.response tbody');
        $('.response tbody .paramtrCls:last-child').find('input').val('');
    });

    $('.edit-area .confirm').click(function () {
        var data = [];
        var title = $('#title').val();
        var summary = $('#summary').val();
        var subTits = $('#subTits').val(); 
        var sector = $('#hidItem_SectorType1').val();
        var sector_idx = $('#hidItem_SectorType1').attr('data-idx');
        var d_type = $('#hidItem_D_TYPEType').val();
        var updateCycle = $('#updateCycle').val();
        var explane = $('#explane').text();
        
        // 요청 url
        var d_link = $('#d_link').val();
        var hidItem_R_TYPEType = $('#hidItem_R_TYPEType').val();

        // 요청변수
        var request = [];
        $('.request tbody tr').each(function (index, value) {
            request.push({
                param: $(value).find('#p-name').val(),
                type: $(value).find('#hidP_TYPEType').val(),
                need: $(value).find('#hidP_NEEDType').val(),
                explane: $(value).find('#p-explane').val()
            });
        });

        var response = [];
        $('.response tbody tr').each(function (index, value) {
            response.push({
                param: $(value).find('#p-name-e').val(),
                type: $(value).find('#hidR_TYPEType').val(),
                explane: $(value).find('#r-explane').val()
            });
        });

        data.push({
            title,
            summary,
            subTits,
            sector,
            sector_idx,
            d_type,
            updateCycle,
            explane,
            d_link,
            hidItem_R_TYPEType,
            request,
            response
        });

        $.ajax({
            url: '/Service/ServiceDataUpdate',
            type: 'POST',
            data: {
                data: JSON.stringify(data)
            },
            success: function (result) {
                if (result.success) {
                    alert('수정되었습니다.');
                } else {
                    alert('수정에 실패하였습니다.');
                }
            }
        });
    });

    $('.edit-area .cancel').click(function () {
        location.href = "/Admin/Service/API";
    });
});