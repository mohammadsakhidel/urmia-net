﻿add_css('_Activity');
// load
$(function () {
    _ActivityLoad();
});
function _ActivityLoad() {
    plugin_expand($('div.act_post_text > div, div.act_photo_desc > div, div.quote > div'), $('input#w_read_more').val());
}