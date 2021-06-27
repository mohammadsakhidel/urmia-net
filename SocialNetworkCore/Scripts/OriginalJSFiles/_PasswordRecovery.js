function SendPasswordBegin() {
    $('#pwd_recovery #msg').hide();
    $('#pwd_recovery input[type="submit"]').attr('disabled', 'disabled');
}

function SendPasswordFailure() {
}

function SendPasswordSuccess(data) {
    if (data.Done) {
        $('#pwd_recovery #msg').removeClass('error_message').addClass('success_message').html(data.Message).fadeIn('fast');
    }
    else {
        $('#pwd_recovery #msg').removeClass('success_message').addClass('error_message').html(data.Errors[0]).fadeIn('fast');
    }
}

function SendPasswordComplete() {
    $('#pwd_recovery input[type="submit"]').removeAttr('disabled');
}