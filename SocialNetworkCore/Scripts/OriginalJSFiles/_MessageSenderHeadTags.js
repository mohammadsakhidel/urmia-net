add_css('_MessageSender');
add_js('_MessageSender');
//load:
$(function () {
    _MessageSenderLoad();
});
function _MessageSenderLoad() {
    plugin_watermark($('textarea.msg_text'), $('#msg_w_write_new_message').val());
}