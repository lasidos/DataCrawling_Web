$(window).ready(function () {
    $('.password').on("keyup", function (key) {
        if (key.keyCode == 13) {
            if ($('.password').val() != "") {
                AccountLogin();
            }
        }
    });

    $('.wrap_btn .btn_g').click(function () {
        AccountLogin();
    });
});

function AccountLogin() {
    var email = $('.account').val();
    var password = $('.password').val();

    if (email == "") {
        alert('계정 (이메일)이 입력되지 않았습니다.');
        return;
    }
    if (password == "") {
        alert('비밀번호가 입력되지 않았습니다.');
        return;
    }

    $.ajax({
        async: false,
        type: 'post',
        url: '/Login/UserLogin',
        data: {
            email: email,
            pw: password,
        },
        success: function (data) {
            if (data.msg == "ok") {
                location.href = data.redirect;
            } else {
                alert(data.msg);
            }
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}