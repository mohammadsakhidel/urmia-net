//*********************************** login on header:
function FailureLogin() {
    alert($('#login #ajax_fail').val());
}

function SuccessLogin(data) {
    if (data.StatusCode != null && data.StatusCode == 2) {
        window.location = "/homepage";
    }
    else {
        var user = $('#login input#login_Email').val();
        window.location = "/account/login?sc=" + data.StatusCode + "&u=" + user;
    }
}
//*********************************** login on content page:
function BeginLogin2() {
    $('.login_on_content #status_message').hide();
}

function FailureLogin2() {
    $('.login_on_content #status_message').html($('.login_on_content #ajax_fail').val()).fadeIn('fast');
}

function SuccessLogin2(data) {
    if (data.StatusCode != null) {
        if (data.StatusCode != 2) { //login faild
            $('.login_on_content #status_message').html(data.Errors[0]).fadeIn('fast');
        }
        else {
            window.location = "/homepage";
        }
    }
    else {
        // exception occured:
        $('.login_on_content #status_message').html(data.Errors[0]).fadeIn('fast');
    }
}