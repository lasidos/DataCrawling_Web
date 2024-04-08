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
        $('.account').focus();
        return;
    } else {
        if (!ValidateEmail(email)) {
            alert('유효하지 않은 이메일 주소입니다.');
            $('.account').focus();
            return;
        }
    }

    if (password == "") {
        alert('비밀번호가 입력되지 않았습니다.');
        $('.password').focus();
        return;
    }

    $.ajax({
        type: 'POST',
        async: false,
        type: 'post',
        url: '/Login/UserLogin',
        data: {
            email: email,
            pw: password,
            redirect: $('#redirect').val()
        },
        success: function (data) {
            if (data.msg == "OK") {
                location.href = data.redirect;
            } else {
                var msg = data.msg.replace("\\n", "\n");
                alert(msg);
            }
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function ValidateEmail(email_address) {
    email_regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i;
    if (!email_regex.test(email_address)) {
        return false;
    } else {
        return true;
    }
}