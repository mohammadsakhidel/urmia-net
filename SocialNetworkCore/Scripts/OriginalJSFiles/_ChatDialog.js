$(function () {
    // event handlers:
    $('.chat_dlg_newmsg textarea').live('keypress', function (event) {
        if (event.keyCode == 13 && !event.shiftKey) {
            if ($.trim($(this).val()).length > 0) {
                var form = $(this).parent();
                form.submit();
                $(this).val('');
            }
            return false;
        }
    });
});

function SendChatMessageFailure() {
    alert('fail');
}

function SendChatMessageSuccess(data) {
    if (data.Done) {
        $('div.chat_dlg_msgs_in').append(data.PartialView);
        scroll_down_chat_msgs();
    }
    else {
        alert(data.Errors[0]);
    }
}