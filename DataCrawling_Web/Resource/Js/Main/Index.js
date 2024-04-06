var scroll2 = 0;
var initYn2 = false;

var defaultPage2 = {
    M27_B: 0,
    M03_A: 0,
}

$(document).ready(function () {
    var COUNT = 2;
    var morePage = defaultPage2['M03_A'];
    var $caseList = division($('.example-section .module_ty .li'), COUNT);
    var btnEl = $('.example-section .btn_m.btn_ty');
        
    $(".inner .md_link").on("focus mouseover", function () {
        var mask_over = $(this).find(".mask_over");

        if (!$(mask_over).length) { return; }
        $(this).addClass("on");
    });

    $(".inner .md_link").on("focusout mouseleave", function () {
        var mask_over = $(this).find(".mask_over");

        if (!$(mask_over).length) { return; }
        $(this).removeClass("on");
    });

    $(document).on('click', '.example-section .btn_m.btn_ty', function (e) {
        e.preventDefault();
        morePage++;
        defaultPage2['M03_A'] = morePage;
        listView(btnEl, $caseList.length, $caseList, morePage);

        viewLiTag = $caseList[morePage].length;
        viewLiScroll = $caseList[morePage][0];

        $('html, body').stop().animate({
            scrollTop: $(viewLiScroll).offset().top
        }, 600, function () {
            //$(viewLiScroll).find('.img .md_link').focus();
        });
    })

    $(".common-modal .close-button").on("click", function (e) {
        e.preventDefault();
        $('.common-modal.service-modal').removeClass('active');
    });

    $(".service-section .service-list li button").on("click", function (e) {
        e.preventDefault();
        $('.common-modal.service-modal').addClass('active');
    });
});

function division(arr, n) {
    var arr = arr;
    var len = arr.length;
    var cnt = Math.floor(len / n) + (Math.floor(len % n) > 0 ? 1 : 0);
    var tmp = [];

    for (var i = 0; i < cnt; i++) {
        tmp.push(arr.splice(0, n));
    }

    return tmp;
}

function listView(btnEl, length, arr, cpage) {
    if (btnEl.parents().hasClass('M03_A_cont')) {
        for (var i = 0; i < arr[cpage].length; i++) {
            $(arr[cpage][i]).addClass('on');
        }
    } else {
        for (var i = 0; i < arr[cpage].length; i++) {
            $(arr[cpage][i]).show();
        }
    }

    if (length === cpage + 1) {
        btnEl.hide();
    } else {
        btnEl.show();
    }
}
