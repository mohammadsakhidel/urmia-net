$(function () {
    // conversation click:
    $('div.conversation_item').live('click', function () {
        change_address_bar($(this).find('input[name="conv_url"]').val());
        // hide msg bubble:
        $(this).find('input[name="unreads_count"]').val(0);
        $(this).find('div.unreads_bubble').hide();
        //
        $(this).find('form').eq(0).submit();
    });

    // show delete conv dialog:
    $('a.remove_conv').live("click", function () {
        var dialog = new Dialog(500, 350, '<div class="loading_dialog"></div>');
        dialog.Show();
        e.stopPropagation();
    });

    // show new msg dialog:
    $('div#messages a.new_msg').live("click", function () {
        var dialog = new Dialog(450, 450, '<div class="loading_dialog"></div>');
        dialog.Show();
    });
});
/* ************************* AJAX Functions *************************** */
function LoadConversationFailure() {
    $('div#conv_messages_in').html('<div class="error_message">' + $('input#w_ajax_fail').val() + '</div>');
}

function LoadConversationSuccess(data) {
    if (data.Done) {
        $('div#conv_messages_in').html(data.PartialView);
        scroll_down_msg_list();
        // select conv item:
        $('[id^=conversation_item]').attr('class', 'conversation_item');
        $('#conversation_item_' + data.ConversationId).removeClass('conversation_item').addClass('conversation_item_selected');
        $('div.conversations input#selected_conv_id').val(data.ConversationId);
        // enable send new message:
        $('div#conv_reply .msg_sender').find('input[name="ConversationId"]').eq(0).val(data.ConversationId);
        toggle_msg_sender($('div#conv_reply .msg_sender'));
    }
}

function ShowDialogFailure() {
    ShowDialogError($('input#w_ajax_fail').val());
}