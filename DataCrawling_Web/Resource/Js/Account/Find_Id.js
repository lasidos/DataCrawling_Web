$(window).ready(function () {
    $('#find-account-nick-phone-number').click(function () {
        stepView('name-phone');
    });

    $('#input-user_name').on("propertychange change keyup paste input", function () {
        checkVerification();
    });
    $('#input-user_contact').on("propertychange change keyup paste input", function () {
        this.value = phoneNumber($('#input-user_contact').val());
        checkVerification();
    });

    $('#input-user_contact').on("keyup", function (key) {
        if (key.keyCode == 13) {
            if ($('#input-user_contact').val() != "") {
                findAccount();
            }
        }
    });

    $('.confirm_btn.find-info').click(function () {
        findAccount();
    });

    $('.confirm_btn.find-btn').click(function () {
        location.href = "/Account/login";
    });
});

function stepView(step) {
    location.href = "/Account/find_account?step=" + step;
}

function findAccount() {
    var name = $('#input-user_name').val();
    var contact = $('#input-user_contact').val();

    if (name == "") {
        alert('닉네임 또는 이름이 입력되지 않았습니다.');
    }
    if (contact == "") {
        alert('연락처가 입력되지 않았습니다.');
    }

    $.ajax({
        async: false,
        url: '/Account/find_userId',
        data: {
            name: name,
            contact: contact,
        },
        success: function (data) {
            if (data.split('|')[0] == "0") {
                stepView('find-completed');
            } else {
                alert('입력하신 정보와 일치하는 계정이 없습니다.');
            }
        },
        error: function (response) {
            alert(response);
        }
    });
}

