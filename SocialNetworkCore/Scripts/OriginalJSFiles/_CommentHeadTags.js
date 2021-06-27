add_css('_Comment');
add_js('_Comment');
//load:
$(function () {
    _CommentLoad();
});
function _CommentLoad() {
    // read more comment:
    plugin_expand($('div.comment_text'), $('input#w_read_more').val());
}