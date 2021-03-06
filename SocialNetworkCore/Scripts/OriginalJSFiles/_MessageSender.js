var source = null;

$(function () {
    // send message begin:
    $('div.msg_sender input[type="submit"]').live('click', function () {
        source = $(this).parents().eq(2);
    });

    // send message begin by press enter:
    $('div.msg_sender textarea').live('keypress', function (event) {
        source = $(this).parents().eq(2);
        var send_on_enter = source.find('input[id^="ch_send_on_enter_"]').attr('checked') == 'checked' ? true : false;
        if (event.keyCode == 13 && !event.shiftKey && send_on_enter) {
            source.find('form').submit();
            return false;
        }
    });
});

function set_conversation(source, convId) {
    source.find('input[name="ConversationId"]').eq(0).val(convId);
}

function toggle_msg_sender(source) {
    var convId = source.find('input[name="ConversationId"]').eq(0).val();
    if (convId != null && convId > 0) {
        enable_msg_sender(source);
    }
    else {
        disable_msg_sender(source);
    }
}

function enable_msg_sender(source) {
    source.find('input[type="submit"]').removeAttr('disabled');
    source.find('textarea').removeAttr('disabled');
}

function disable_msg_sender(source) {
    source.find('input[type="submit"]').attr('disabled', 'disabled');
    source.find('textarea').attr('disabled', 'disabled');
}
/********************************** AJAX FUNCTIONS ************************************/
function SendMessageBegin() {
    source.find('div.snd_result').hide();
}

function SendMessageFailure() {
    source.find('div.snd_result').html($('#msg_w_ajax_fail').val()).fadeIn('fast');
}

function SendMessageSuccess(data) {
    if (data.Done) {
        source.find('textarea').val('');
        var messages_div = source.parent().prev('div').find('div#conv_messages_in');
        messages_div.append(data.PartialView);
        // from parent functions:
        scroll_down_msg_list();
    }
    else {
        source.find('div.snd_result').html(data.Errors[0]).fadeIn('fast');
    }
}