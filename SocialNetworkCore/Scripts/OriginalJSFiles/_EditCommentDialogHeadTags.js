add_css('_EditCommentDialog');
add_js('_EditCommentDialog');
//load:
$(function () {
    _EditCommentDialogLoad();
});
function _EditCommentDialogLoad() {
    plugin_autosize($('#dialog_txt_comment')).css('max-height', 200).css('overflow-y', 'auto');
}