function SaveBegin() {
    $('#mail_nots #msg_save').hide();
}

function SaveFailure() {
    $('#mail_nots #msg_save').removeClass('success_message').addClass('error_message').html($('#mail_nots #ajax_fail').val()).fadeIn('fast');
}

function SaveSuccess(data) {
    if (data.Done) {
        $('#mail_nots #msg_save').removeClass('error_message').addClass('success_message').html(data.Message).fadeIn('fast');
    }
    else {
        var errorMessage = $('#msg_errors_title').val();
        errorMessage += "<ul>";
        for (var i = 0; i < data.Errors.length; i++) {
            errorMessage += "<li>" + data.Errors[i] + "</li>";
        }
        errorMessage += "</ul>";
        $('#mail_nots #msg_save').removeClass('success_message').addClass('error_message').html(errorMessage).fadeIn('fast');
    }
}