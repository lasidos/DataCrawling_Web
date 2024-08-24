$(function () {
    $('.devCoTypeItem button').click(function (e) {
        e.preventDefault();
        $(this).parents('.selType').find('.txResult').text($(this).text());
        $(this).parents('.elWrap').addClass('ok').removeClass('on');
        var sel = $(this).parents('li').data('type');
        $(this).parents('.selType').find('.hidResult').val(sel);
    });

    $('.btnSelType').click(function (e) {
        var ele = null;
        if ($('.elWrap.on').length > 0) {
            ele = $('.elWrap.on').attr('id');
            if ($(this).parents('.elWrap').attr('id') != ele) {
                $('.elWrap.on').removeClass('on');
                $(this).parents('.elWrap').addClass('on');
            } else {
                $(this).parents('.elWrap').removeClass('on');
            }
        } else {
            $(this).parents('.elWrap').addClass('on');
        }
    });

    $('.devText').focus(function (e) {
        $('.elWrap').removeClass('on');
        $(this).parents('.elWrap').addClass('on ok');
    });

    $('.devText').blur(function (e) {
        if ($(this).hasClass('devText')) {
            if (CheckValidURL()) {
                $(this).parents('.elWrap').removeClass('on');
            };
        }
    });

    $(".devText").keypress(function (e) {
        if (e.keyCode && e.keyCode == 13) {
            e.preventDefault();
            if (CheckValidURL()) {
                $(this).parents('.elWrap').removeClass('on');
                $(this).blur();
            };
            return;
        }
    });

    $('.resist_content').click(function (e) {
        if (!$(e.target).hasClass('btnSelType') && !$(e.target).hasClass('txCate') && !$(e.target).hasClass('txResult')) {
            $('.btnSelType').parents('.elWrap').removeClass('on');
        }
    });

    $('.spec_add .txArea').focus(function () {
        $(this).parents('.spec_add').find('.ph').hide();
    });

    $('.spec_add .txArea').blur(function (e) {
        if ($(this).val() == "") {
            $('.spec_add .ph').show();
        }
    });

    $('.buttonAddUrl').click(function () {
        $("#uploadPortfolioFile").click();
    });

    $(document).on('click', '.buttonChoose', function (e) {
        e.preventDefault();
        if ($(this).next().hasClass('visible')) {
            $(this).next().removeClass('visible').addClass('hidden');
        } else {
            $(this).next().removeClass('hidden').addClass('visible');
        }
    });

    $(document).on('click', '.list.is-scroll li', function (e) {
        e.preventDefault();
        var sel = $(this).find('span').text();
        $(this).parents('.dropdown').find('.buttonChoose').addClass('on');
        $(this).parents('.dropdown').find('.buttonChoose .ChooseResult').text(sel);
        $(this).parents('.dropdown').find('.is-scroll').removeClass('visible').addClass('hidden');
    });

    $(document).on('click', '.someinfo .buttonDelete', function (e) {
        e.preventDefault();
        var ele = $(this).parents('li');
        var fileName = ele.find('.filename').text();

        $.ajax({
            type: "POST",
            url: "/TextUser/RemoveFile",
            data: {
                fileName: fileName
            },
            success: function (result) {
                if (result.msg != '') {
                    if (result.msg == "0") {
                        $('.total_size').text('');
                    } else {
                        $('.total_size').text(getfileSize(result.msg) + ' / 10MB');
                    }
                }
                $('.list.portfolioList').load("/Offer/GetViewData");
            },
            error: function (request, status, error) {
                console.log("code: " + request.status)
                console.log("message: " + request.responseText)
                console.log("error: " + error);
            }
        });
    });

    $('#uploadPortfolioFile').change(function (e) {
        var fileName = $(this).val();
        var fileSize = $(this)[0].files[0].size;
        if (fileSize > 1024 * 1024 * 10) {
            alert("첨부파일 용량은 최대 10MB까지 가능합니다.");
            $(this).val("");
            fileName = "";
            return;
        }

        var that = this,
            $source = $(e.currentTarget),
            $dropdown = $source.closest(".dropdown"),
            fileType = $source.data("value"),
            $container = $source.closest("[data-cid]"),
            id = $container.data("cid"),
            $fileEle = $container.find(":file");

        var $cloneFile = $fileEle.clone(true);
        $cloneFile.insertAfter($fileEle);

        const formData = new FormData();
        formData.append("image", $(this)[0].files[0]);

        $.ajax({
            type: "POST",
            url: "/TextUser/FileAppendAjax",
            processData: false,
            contentType: false,
            data: formData,
            success: function (result) {
                if (result.rc == 0) {
                    if (result.msg != '') {
                        $('.total_size').text(getfileSize(result.msg) + ' / 10MB');
                    }

                    $('.list.portfolioList').load("/Offer/GetViewData");
                } else {
                    alert(result.msg.replace("\\n", "\n"));
                }
            },
            error: function (request, status, error) {
                console.log("code: " + request.status)
                console.log("message: " + request.responseText)
                console.log("error: " + error);
            }
        });
    });

    $('.btn_regist.completed button').click(function () {
        var snStatType = $('#snStatType').text();
        var snPlanType = $('#snPlanType').text();
        var snPeriodType = $('#snPeriodType').text();
        var type = $('#type').val();
        var lb_reason = $('#lb_reason').val();
        if (snStatType == '') {
            alert('제작상태가 선택되지 않았습니다.');
            $('#btnStatType').focus();
            return;
        }
        if (snPlanType == '') {
            alert('기획상황이 선택되지 않았습니다.');
            $('#btnPlanType').focus();
            return;
        }
        if (snPeriodType == '') {
            alert('납기희망일이 선택되지 않았습니다.');
            $('#btnPeriodType').focus();
            return;
        }
        if (type == "S" && lb_reason == '') {
            alert('스크랩 대상 URL 또는 참고 서비스가 입력되지 않았습니다.');
            $('#lb_reason').focus();
            return;
        }

        if ($('#txtaContent').val() == "") {
            alert('상세정보가 입력되지 않았습니다.');
            $('#txtaContent').focus();
            return;
        }

        if ($('.someinfo').length > 0) {
            var emptyIdx = -1;
            for (var i = 0; i < $('.someinfo').length; i++) {
                var item = $('.someinfo').eq(i);
                if (item.find('.ChooseResult').text() == '') {
                    emptyIdx = i;
                    break;
                }
            }
            if (emptyIdx > -1) {
                alert((emptyIdx + 1) + '번째 파일의 분류가 선택되지 않았습니다.');
                $('.someinfo').eq(emptyIdx).focus();
                return;
            }
        }

        var form = $("#regist-frm")[0];
        var formData = new FormData(form);
        if ($('.someinfo').length > 0) {
            let FileList = [];
            for (var i = 0; i < $('.someinfo').length; i++) {
                var item = $('.someinfo').eq(i);
                formData.append('FileList[' + i + ']', item.find('.ChooseResult').text() + ',' + item.find('.filename').text());
            }
        }

        $.ajax({
            type: "POST",
            url: "/TextUser/RegistData",
            processData: false,
            contentType: false,
            data: formData,
            success: function (result) {
                if (result.rc == 0) {
                    location.href = "/Offer/Completed";
                } else {
                    alert(result.msg.replace("\\n", "\n"));
                }
            }
        });
    });
});

function getfileSize(x) {
    var s = ['bytes', 'kB', 'MB', 'GB', 'TB', 'PB'];
    var e = Math.floor(Math.log(x) / Math.log(1024));
    return (x / Math.pow(1024, e)).toFixed(2) + s[e];
};

function CheckValidURL() {
    if ($('.devText').val() != '') {
        if (!isValidURL($('.devText').val())) {
            $('.errorBx').addClass('on');
            $('.devText').focus();
            return false;
        } else {
            $('.errorBx').removeClass('on');
            return true;
        }
    } else {
        $(this).parents('.elWrap').removeClass('ok');
        return false;
    }
}

function isValidURL(url) {
    var RegExp = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/;
    if (RegExp.test(url)) {
        return true;
    } else {
        return false;
    }
}