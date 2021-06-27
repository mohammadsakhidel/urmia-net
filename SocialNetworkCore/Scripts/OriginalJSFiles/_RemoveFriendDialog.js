function hide_dialog() {
    HideDialog();
}

function RemoveFriendBegin() {
    $('div#dialog div#dialog_msg').hide();
}

function RemoveFriendFailure() {
    $('div#dialog div#dialog_msg').removeClass('success_message').addClass('error_message').html($('input#w_ajax_fail').val()).fadeIn('fast');
}

function RemoveFriendSuccess(data) {
    if (data.Done) {
        FadeOutDialog(300);
        if ($.trim($('input#hid_removing_item').val()).length > 0) {
            $('div#friend_item_' + data.FriendId).remove();
            $('input#hid_removing_item').val('');
        }
    }
    else {
        $('div#dialog div#dialog_msg').removeClass('success_message').addClass('error_message').html(data.Errors[0]).fadeIn('fast');
    }
}