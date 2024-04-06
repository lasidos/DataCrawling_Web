$(window).ready(function () {
    // 초기화
    $('.inp_choice').prop("checked", false);

    // 전체선택
    $('#checkboox-check_all').change(function () {
        $('.list_agree .inp_choice').prop("checked",
            $("input:checkbox[id='checkboox-check_all']").is(":checked"));

        if ($("input:checkbox[id='checkboox-terms_age_over_14']").is(":checked") &&
            $("input:checkbox[id='terms_TERM_ACCOUNT']").is(":checked") &&
            $("input:checkbox[id='terms_PR_PEC_1904']").is(":checked")) {
            $('.btn_g.highlight').prop("disabled", false);
        } else {
            $('.btn_g.highlight').prop("disabled", true);
        }
    });

    // 약관동의
    $('.list_agree .inp_choice').change(function () {
        if ($("input:checkbox[id='checkboox-check_all']").is(":checked") &&
            !$(this).is(":checked")) {
            $('#checkboox-check_all').prop("checked", false);
        }

        var checkall = true;
        for (var i = 0; i < $('.list_agree .inp_choice').length; i++) {
            if (!$('.list_agree .inp_choice').eq(i).is(":checked")) {
                checkall = false;
                break;
            }
        }
        if (checkall && !$("input:checkbox[id='checkboox-check_all']").is(":checked")) {
            $('#checkboox-check_all').prop("checked", true);
        }

        if ($("input:checkbox[id='checkboox-terms_age_over_14']").is(":checked") &&
            $("input:checkbox[id='terms_TERM_ACCOUNT']").is(":checked") &&
            $("input:checkbox[id='terms_PR_PEC_1904']").is(":checked")) {
            $('.btn_g.highlight').prop("disabled", false);
        } else {
            $('.btn_g.highlight').prop("disabled", true);
        }
    });

    $('.btn_g.highlight').click(function () {
        var agree = $("input:checkbox[id='terms_AP_GB_1904']").is(":checked");
        $.ajax({
            async: false,
            url: '/Account/AcceptConditions',
            data: {
                accept: agree,
            },
            success: function (data) {
                if (data == "ok") {
                    stepView("requestVerifyEmail");
                }
            },
            error: function (response) {
                alert(response);
            }
        });
        
    });

    // 이메일 인증
    $('.util_tf .btn_round').click(function () {
        var email = $('#input-email').val();
        if (email == '' || !isValidEmailAddress(email)) {
            $('.box_tf .txt_info').text('이메일 형식이 올바르지 않습니다.');
            return;
        }

        $.ajax({
            async: false,
            url: '/Account/CheckExist',
            data: {
                email: email
            },
            success: function (data) {
                if (data.split('|')[0] == "0") {
                    if (Number(JSON.parse(data.split('|')[1])[0].cnt) > 0) {
                        alert('이미 가입된 회원입니다.');
                    } else {
                        $.ajax({
                            async: false,
                            url: '/Account/PushPasscode',
                            data: {
                                email: email
                            },
                            success: function (data) {
                                if (data.split('|')[0] == "0") {
                                    alert('인증번호가 발송되었습니다.');
                                    $('.box_tf:nth-child(2)').css('display', 'block');
                                } else {
                                    alert(data.split('|')[1]);
                                }
                            },
                            error: function (response) {
                                alert(response);
                            }
                        });
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

    $('#input-email').on("propertychange change keyup paste input", function () {
        if (isValidEmailAddress($('#input-email').val())) {
            $('.box_tf').removeClass('error');
        } else {
            $('.box_tf').addClass('error');
        }
    });

    $('#input-email_passcode').on("propertychange change keyup paste input", function () {
        if ($('#input-email_passcode').val().length == 8) {
            $('.confirm_btn .btn_g').prop("disabled", false);
        } else {
            $('.confirm_btn .btn_g').prop("disabled", true);
        }
    });

    $('.confirm_btn.requestVerifyEmail .btn_g').click(function () {
        var email = $('#input-email').val();
        $.ajax({
            async: false,
            url: '/Account/ConfirmPasscode',
            data: {
                email: email
            },
            success: function (data) {
                if (data.split('|')[0] == "0") {
                    if (JSON.parse(data.split('|')[1])[0].passCode == $('#input-email_passcode').val()) {
                        stepView("userPw");
                    } else {
                        alert('발송된 인증코드와 다릅니다.');
                    }
                } else {
                    alert(data.split('|')[0] == "0");
                }
            },
            error: function (response) {
                alert(response);
            }
        });
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
            url: '/Account/UserPassword',
            data: {
                pw: password
            },
            success: function (data) {
                if (data == "ok") {
                    stepView("userProfile");
                }
            },
            error: function (response) {
                alert(response);
            }
        });
    });

    /*사용자 프로필*/
    $('#input-name').on("propertychange change keyup paste input", function () {
        CheckProfile();
    });
    $('#input-tel').on("propertychange change keyup paste input", function () {
        this.value = phoneNumber($('#input-tel').val());
        CheckProfile();
    });
    $('#terms_NoneGender').prop("checked", true);
    $('.gender .inp_choice').change(function () {
        $('.gender .inp_choice').prop("checked", false);
        $(this).prop("checked", true);
    });
    $('.confirm_btn.userProfile .btn_g').click(function () {
        var name = $('#input-name').val();
        var tel = $('#input-tel').val();
        var gender = "";
        for (var i = 0; i < $('.gender .inp_choice').length; i++) {
            if ($('.gender .inp_choice').eq(i).is(":checked")) {
                gender = $('.gender .inp_choice').eq(i).val();
                break;
            }
        }
        $.ajax({
            async: false,
            url: '/Account/UserProfile',
            data: {
                name: name,
                tel: tel,
                gender: gender
            },
            success: function (data) {
                if (data == "ok") {
                    stepView("completed");
                } else {
                    alert('회원가입에 실패하였습니다.\r\n문제가 지속적으로 발생시 고객센터에 문의하시기 바랍니다.\r\n이용상에 불편을 드려 죄송합니다.')
                }
            },
            error: function (response) {
                alert(response);
            }
        });
    });

    $('.confirm_btn.completed .btn_g').click(function () {
        location.href = "/Account/login";
    });
});

function stepView(step) {
    location.href = "/Account/create_account?step=" + step;
}

function isValidEmailAddress(emailAddress) {
    var pattern = new RegExp(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/);
    return pattern.test(emailAddress);
};

function checkPassword(pw) {
    var regexPw = /^(?=.*[a-zA-Z])(?=.*[!@#$%^*+=-])(?=.*[0-9]).{8,25}$/;
    if (!regexPw.test(pw)) {
        return false;
    }
    return true;
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

function CheckProfile() {
    var verification = true;
    var reg_name = /^[가-힣]{2,4}$/;
    if (reg_name.test($('#input-name').val())) {
        $('.box_tf.userName').removeClass('error');
    } else {
        $('.box_tf.userName').addClass('error');
        verification = false;
    }
    var regPhone = /^01([0|1|6|7|8|9])-?([0-9]{3,4})-?([0-9]{4})$/;
    if (regPhone.test($('#input-tel').val())) {
        $('.box_tf.contact').removeClass('error');
    } else {
        $('.box_tf.contact').addClass('error');
        verification = false;
    }

    if (verification) {
        $('.confirm_btn.userProfile .btn_g').prop("disabled", false);
    } else {
        $('.confirm_btn.userProfile .btn_g').prop("disabled", true);
    }
}