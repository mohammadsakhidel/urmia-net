function SaveBegin() {
    $('#del_account input[type="submit"]').attr('disabled', 'disabled');
    $('#del_account #msg_save').hide();
}

function SaveFailure() {
    $('#del_account #msg_save').removeClass('success_message').addClass('error_message').html($('#del_account #ajax_fail').val()).fadeIn('fast');
}

function SaveSuccess(data) {
    if (data.Done) {
        $('#del_account #msg_save').removeClass('error_message').addClass('success_message').html(data.Message).fadeIn('fast');
    }
    else {
        var errorMessage = $('#msg_errors_title').val();
        errorMessage += "<ul>";
        for (var i = 0; i < data.Errors.length; i++) {
            errorMessage += "<li>" + data.Errors[i] + "</li>";
        }
        errorMessage += "</ul>";
        $('#del_account #msg_save').removeClass('success_message').addClass('error_message').html(errorMessage).fadeIn('fast');
    }
}

function SaveComplete() {
    $('#del_account input[type="submit"]').removeAttr('disabled');
}