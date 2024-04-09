$(document).ready(function () {
    $('.navigation .left-menu li').mouseover(function () {
        if ($(this).index() > 0) {
            $('.navigation .left-menu-list li').removeClass('active');
            $(this).addClass('active');
        }
    });

    $('.navigation .left-menu li').mouseleave(function () {
        $('.navigation .left-menu-list li').removeClass('active');
    });
});

// 이메일 유효성 검사
function isValidEmailAddress(emailAddress) {
    var pattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i;
    return pattern.test(emailAddress);
}

// 비밀번호 정규식
function checkPassword(pw) {
    var regexPw = /^(?=.*[a-zA-Z])(?=.*[!@@#$%^*+=-])(?=.*[0-9]).{8,25}$/;
    if (!regexPw.test(pw)) {
        return false;
    }
    return true;
}

function phoneNumber(value) {
    if (!value) {
        return "";
    }

    value = value.replace(/[^0-9]/g, "");

    let result = [];
    let restNumber = "";

    // 지역번호와 나머지 번호로 나누기
    if (value.startsWith("02")) {
        // 서울 02 지역번호
        result.push(value.substr(0, 2));
        restNumber = value.substring(2);
    } else if (value.startsWith("1")) {
        // 지역 번호가 없는 경우
        // 1xxx-yyyy
        restNumber = value;
    } else {
        // 나머지 3자리 지역번호
        // 0xx-yyyy-zzzz
        result.push(value.substr(0, 3));
        restNumber = value.substring(3);
    }

    if (restNumber.length === 7) {
        // 7자리만 남았을 때는 xxx-yyyy
        result.push(restNumber.substring(0, 3));
        result.push(restNumber.substring(3));
    } else {
        result.push(restNumber.substring(0, 4));
        result.push(restNumber.substring(4));
    }

    return result.filter((val) => val).join("-");
}