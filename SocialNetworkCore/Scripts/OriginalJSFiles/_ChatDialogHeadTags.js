add_css('_ChatDialog');
add_js('_ChatDialog');
// onload: called at the end of module
function _ChatDialogLoad() {
    $('.chat_dlg_newmsg textarea').focus();
    plugin_watermark($('.chat_dlg_newmsg textarea'), $('input#chat_dlg_w_write_your_msg').val());
    plugin_autosize($('.chat_dlg_newmsg textarea'));
    scroll_down_chat_msgs();
}
function scroll_down_chat_msgs() {
    $('div.chat_dlg_msgs').scrollTop($('div.chat_dlg_msgs_in').outerHeight());
}
function append_new_message(data) {
    if (data.Done) {
        var msg = data.PartialView;
        if ($.trim(msg).length > 0) {
            $('#chat_dialog .chat_dlg_msgs_in').append(msg);
            scroll_down_chat_msgs();
        }
    }
}