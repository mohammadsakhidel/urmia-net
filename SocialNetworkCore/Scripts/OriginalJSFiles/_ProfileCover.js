function EditCoverBegin() {
    $('div#msg_EditCover').hide();
}

function EditCoverFailure() {
    var msg = $('#profile_cover input#ajax_fail').val();
    $('div#msg_EditCover').removeClass('success_message').addClass('error_message').html(msg).fadeIn('fast');
}

function EditCoverSuccess(data) {
    if (data.Done) {
        $('div#msg_EditCover').removeClass('error_message').addClass('success_message').html(data.Message).fadeIn('fast');
    }
    else {
        $('div#msg_EditCover').removeClass('success_message').addClass('error_message').html(data.Errors[0]).fadeIn('fast');
    }
}

