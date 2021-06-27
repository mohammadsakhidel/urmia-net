add_css('_SharedObjectActions');
add_js('_SharedObjectActions');
//load:
$(function () {
    _SharedObjectActionsLoad();
});
function _SharedObjectActionsLoad() {
    plugin_autosize($('div.obj_actions div.new_comment textarea'));
    plugin_watermark($('div.obj_actions div.new_comment textarea'), $('input#w_write_your_comment').val());
}