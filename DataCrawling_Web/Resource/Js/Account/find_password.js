$(window).ready(function () {
    $('#input-user_account').on("propertychange change keyup paste input", function () {
        CheckVerification();
    });

    $('#input-user_account').on("keyup", function (key) {
        if (key.keyCode == 13) {
            if ($('#input-user_account').val() != "") {
                findPassword();
            }
        }
    });

    $('#input-email_passcode').on("propertychange change keyup paste input", function () {
        if ($('#input-email_passcode').val().length == 8) {
            $('.confirm_btn .btn_g').prop("disabled", false);
        } else {
            $('.confirm_btn .btn_g').prop("disabled", true);
        }
    });

    /*비밀번호*/
    $('#input-password').on("propertychange change keyup paste input", function () {
        CheckPasswords();
    });
    $('#input-repassword').on("propertychange change keyup paste input", function () {
        CheckPasswords();
    });
    $('.confirm_btn.userPw .btn_g').click(function () {
        var password = $('#input-password').val();
        $.ajax({
            async: false,
            url: '/Account/ChangePassword',
            data: {
                pw: password
            },
            success: function (data) {
                if (data == "ok") {
                    stepView("completed");
                } else {
                    alert('비밀번호 변경에 실패하였습니다.\r\n문제가 지속적으로 발생시 고객센터에 문의하시기 바랍니다.\r\n이용상에 불편을 드려 죄송합니다.')
                }
            },
            error: function (response) {
                alert(response);
            }
        });
    });

    $('.confirm_btn.find-password').click(function () {
        findPassword();
    });

    $('.confirm_btn.certEmail .btn_g').click(function () {
        var email = $('.userEmail').text();
        $.ajax({
            async: false,
            url: '/Account/ConfirmPasscode',
            data: {
                email: email
            },
            success: function (data) {
                if (data.split('|')[0] == "0") {
                    if (JSON.parse(data.split('|')[1])[0].passCode == $('#input-email_passcode').val()) {
                        stepView("changePw");
                    } else {
                        alert('발송된 인증코드와 다릅니다.');
                    }
                } else {
                    alert(data.split('|')[1]);
                }
            },
            error: function (response) {
                alert(response);
            }
        });
    });

    $('.confirm_btn.find-compl').click(function () {
        location.href = "/Account/login";
    });
});

function stepView(step) {
    location.href = "/Account/find_password?step=" + step;
}

function CheckVerification() {
    if ($('#input-user_account').val() != "") {
        $('.confirm_btn.find-password .btn_g').prop("disabled", false);
    } else {
        $('.confirm_btn.find-password .btn_g').prop("disabled", true);
    }
}

function findPassword() {
    var email = $('#input-user_account').val();

    if (email == "") {
        alert('이메일 또는 전화번호가 입력되지 않았습니다.');
    }

    $.ajax({
        async: false,
        url: '/Account/findPassword',
        data: {
            email: email
        },
        success: function (data) {
            if (data == "ok") {
                stepView('cert-email');
            } else {
                alert('입력하신 정보와 일치하는 계정이 없습니다.');
            }
        },
        error: function (response) {
            alert(response);
        }
    });
}

function CheckPasswords() {
    var verification = true;
    if ($('#input-password').val().length > 7 && checkPassword($('#input-password').val())) {
        $('.box_tf.password').removeClass('error');
    } else {
        $('.box_tf.password').addClass('error');
        verification = false;
    }
    if ($('#input-password').val() == $('#input-repassword').val()) {
        $('.box_tf.repassword').removeClass('error');
    } else {
        $('.box_tf.repassword').addClass('error');
        verification = false;
    }

    if (verification) {
        $('.confirm_btn.userPw .btn_g').prop("disabled", false);
    } else {
        $('.confirm_btn.userPw .btn_g').prop("disabled", true);
    }
}

function checkPassword(pw) {
    var regexPw = /^(?=.*[a-zA-Z])(?=.*[!@#$%^*+=-])(?=.*[0-9]).{8,25}$/;
    if (!regexPw.test(pw)) {
        return false;
    }
    return true;
}