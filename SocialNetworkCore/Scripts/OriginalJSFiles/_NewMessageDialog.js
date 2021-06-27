$(function () {
    $('div#new_msg_dlg a.okey').live("click", function () {
        $(this).parents().eq(1).submit();
    });
});

function hide_dialog() {
    HideDialog();
}

function SendNewMessageBegin() {
    $('div#dialog div#dialog_msg').hide();
}

function SendNewMessageFailure() {
    $('div#dialog div#dialog_msg').removeClass('success_message').addClass('error_message').html($('input#w_ajax_fail').val()).fadeIn('fast');
}

function SendNewMessageSuccess(data) {
    if (data.Done) {
        $('div#dialog div#dialog_msg').removeClass('error_message').addClass('success_message').html(data.Message).fadeIn('fast');
    }
    else {
        $('div#dialog div#dialog_msg').removeClass('success_message').addClass('error_message').html(data.Errors[0]).fadeIn('fast');
    }
}