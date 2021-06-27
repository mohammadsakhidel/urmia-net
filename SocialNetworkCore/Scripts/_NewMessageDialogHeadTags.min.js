add_css('_NewMessageDialog');
add_js('_NewMessageDialog');
//load:
$(function () {
    _NewMessageDialogLoad();
});
function _NewMessageDialogLoad() {
    plugin_watermark($('div#new_msg_dlg textarea[name="Text"]'), $('#dlg_new_msg_w_write_new_message').val());
}