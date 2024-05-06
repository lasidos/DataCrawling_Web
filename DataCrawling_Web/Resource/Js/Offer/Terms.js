$(function () {
    $('#lb_agree_check-all').change(function () {
        if ($('#lb_agree_check-all').prop("checked")) {
            $('.devCheckbox').prop("checked", true);
        } else {
            $('.devCheckbox').prop("checked", false);
        }
    });

    $('.devCheckbox').change(function () {
        if ($('.devCheckbox[name="lb_agree"]:checked').length == 2) {
            $('#lb_agree_check-all').prop("checked", true);
        } else {
            $('#lb_agree_check-all').prop("checked", false);
        }
    });

    $('.btn_regist.devRequest button').click(function () {
        if ($('.devCheckbox[name="lb_agree"]:checked').length == 2) {

            $.post("/Offer/functionname", { Term: "agree", type: $('#type').val() }, function (data) {
                location.href = '/Offer/Request';
            });
        } else {
            alert('의뢰서는 약관동의 후에 진행하실수 있습니다.');
        }
    });
});