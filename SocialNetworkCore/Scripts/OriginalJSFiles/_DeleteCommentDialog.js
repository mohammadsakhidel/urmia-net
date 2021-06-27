function hide_dialog() {
    HideDialog();
}

function DeleteCommentBegin() {
    $('div#dialog div#dialog_msg').hide();
}

function DeleteCommentFailure() {
    $('div#dialog div#dialog_msg').removeClass('success_message').addClass('error_message').html($('input#w_ajax_fail').val()).fadeIn('fast');
}

function DeleteCommentSuccess(data) {
    if (data.Done) {
        var cId = data.CommentId;
        $('div#comment_' + cId).remove();
        FadeOutDialog(300);
    }
    else {
        $('div#dialog div#dialog_msg').removeClass('success_message').addClass('error_message').html(data.Errors[0]).fadeIn('fast');
    }
}