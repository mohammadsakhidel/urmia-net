$(function () {
    $('div#edit_comment_dialog a.okey').live('click', function () {
        $('form#frm_edit_comment').submit();
    });
});

function EditCommentBegin() {
    $('div#dialog div#dialog_msg').hide();
}

function EditCommentFailure() {
    $('div#dialog div#dialog_msg').removeClass('success_message').addClass('error_message').html($('input#w_ajax_fail').val()).fadeIn('fast');
}

function EditCommentSuccess(data) {
    if (data.Done) {
        var cId = data.CommentId;
        $('div#comment_' + cId).find('div.comment_text').html(data.CommentText);
        FadeOutDialog(300);
    }
    else {
        $('div#dialog div#dialog_msg').removeClass('success_message').addClass('error_message').html(data.Errors[0]).fadeIn('fast');
    }
}