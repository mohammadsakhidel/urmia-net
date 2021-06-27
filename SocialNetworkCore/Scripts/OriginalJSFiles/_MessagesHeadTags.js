add_css('_Messages');
add_js('_Messages');
//load:
$(function () {
    _MessagesLoad();
});
function _MessagesLoad() {
    set_height();

    $(window).on('resize', function () {
        set_height();
    });

    // show selected conv:
    if (Number($('#selected_conv_id').val()) > 0) {
        $('#conversation_item_' + $('#selected_conv_id').val()).removeClass('conversation_item').addClass('conversation_item_selected');
        scroll_down_msg_list();
    }
}
function set_height() {
    var window_height = $(window).height();
    min_height = 400;
    var h = window_height - 180;
    var seted_h = (h < min_height ? min_height : h);
    $('div.conversation_details').height(seted_h);
    $('div.conversations').height(seted_h);
}
function scroll_down_msg_list() {
    $('div.conv_messages').scrollTop($('div#conv_messages_in').height());
}