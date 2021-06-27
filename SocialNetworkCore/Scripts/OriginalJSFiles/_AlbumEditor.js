function SaveBegin() {
    $('#album_editor #msg_save').hide();
    $('#album_editor input[type="submit"]').attr('disabled', 'disabled');
}

function SaveFailure() {
    $('#album_editor #msg_save').removeClass('success_message').addClass('error_message').html($('#album_editor #ajax_fail').val()).fadeIn('fast');
}

function SaveSuccess(data) {
    if (data.Done) {
        $('#album_editor #msg_save').removeClass('error_message').addClass('success_message').html(data.Message).fadeIn('fast');
    }
    else {
        var errorMessage = $('#msg_errors_title').val();
        errorMessage += "<ul>";
        for (var i = 0; i < data.Errors.length; i++) {
            errorMessage += "<li>" + data.Errors[i] + "</li>";
        }
        errorMessage += "</ul>";
        $('#album_editor #msg_save').removeClass('success_message').addClass('error_message').html(errorMessage).fadeIn('fast');
    }
}

function SaveComplete() {
    $('#album_editor input[type="submit"]').removeAttr('disabled');
}