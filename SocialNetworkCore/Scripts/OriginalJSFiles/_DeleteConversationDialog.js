function hide_dialog() {
    HideDialog();
}

function DeleteConversationBegin() {
    $('div#dialog div#dialog_msg').hide();
}

function DeleteConversationFailure() {
    $('div#dialog div#dialog_msg').removeClass('success_message').addClass('error_message').html($('input#w_ajax_fail').val()).fadeIn('fast');
}

function DeleteConversationSuccess(data) {
    if (data.Done) {
        var selected_conv_id = Number($('div.conversations input#selected_conv_id').val());
        var deleted_conv_id = data.ConversationId;
        $('div#conversation_item_' + deleted_conv_id).remove();
        if (selected_conv_id == deleted_conv_id) {
            $('div#conv_messages_in').html('');
            change_address_bar($('div.conversations input[name="messages_url"]').val());
        }
        // enable send new message:
        $('div#conv_reply .msg_sender').find('input[name="ConversationId"]').eq(0).val(-1);
        toggle_msg_sender($('div#conv_reply .msg_sender'));
        //
        FadeOutDialog(300);
    }
    else {
        $('div#dialog div#dialog_msg').removeClass('success_message').addClass('error_message').html(data.Errors[0]).fadeIn('fast');
    }
}