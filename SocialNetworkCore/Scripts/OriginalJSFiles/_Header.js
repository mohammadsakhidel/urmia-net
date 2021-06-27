$(function () {
    // hmi tool tips:
    $('#hmenu > a').live('mouseover', function () {
        show_tool_tip($(this));
    });
    $('#hmenu > a').live('mouseleave', function () {
        hide_tool_tip();
    });
    // show hide submenu:
    $(document).live('click', function (e) {
        if (!is_hmi_clicked(e.target) && !is_inside_submenu(e)) {
            hide_submenu();
        }
    });
    $("div#hsubmenu").live('click', function (e) {
        $('div#hsubmenu').show();
    });
});

function hide_submenu() {
    $('div#hsubmenu').hide();
    set_unselected();
}

function is_hmi_clicked(target) {
    if (target.id == 'hmi_settings' || target.id == 'hmi_requests' || target.id == 'hmi_messages' || target.id == 'hmi_notifications' || target.id == 'hmi_home')
        return true;
    else
        return false;
}

function is_inside_submenu(e) {
    var submenu = $('div#hsubmenu');
    var sm_left = submenu.offset().left;
    var sm_top = submenu.offset().top;
    var sm_height = submenu.outerHeight();
    var sm_width = submenu.outerWidth();
    var sm_right = sm_left + sm_width;
    var sm_bottom = sm_top + sm_height;
    var is_inside = (e.pageX >= sm_left && e.pageX <= sm_right && e.pageY >= sm_top && e.pageY <= sm_bottom ? true : false);
    return is_inside;
}

function hmi_NotificationsBegin() {
    show_submenu($('div#hmenu a#hmi_notifications'));
}

function hmi_MessagesBegin() {
    show_submenu($('div#hmenu a#hmi_messages'));
}

function hmi_RequestsBegin() {
    show_submenu($('div#hmenu a#hmi_requests'));
}

function hmi_SettingsBegin() {
    show_submenu($('div#hmenu a#hmi_settings'));
}

function show_submenu(sender) {
    var submenu = $('div#hsubmenu');
    var submenu_left = -10;
    var submenu_top = sender.position().top + 16;
    var pointer_left = sender.position().left + 8;
    set_unselected();
    set_selected(sender);
    submenu.find('div.hsubmenu_pointer').css('left', pointer_left);
    submenu.find('div#hsubmenu_in').html('<div><div class="loading_small_circle"></div></div>');
    submenu.css('left', submenu_left).css('top', submenu_top).show();
}

function set_unselected() {
    $('div#hmenu a#hmi_settings').attr('class', 'hmi_settings');
    $('div#hmenu a#hmi_requests').attr('class', 'hmi_requests');
    $('div#hmenu a#hmi_messages').attr('class', 'hmi_messages');
    $('div#hmenu a#hmi_notifications').attr('class', 'hmi_notifications');
}

function set_selected(sender) {
    var c = sender.attr('id') + "_sel";
    sender.attr('class', c);
}

function show_tool_tip(sender) {
    $('#htooltip').find('.htooltip_bg').html(sender.attr('title'));
    var tooltip_w = $('#htooltip').outerWidth();
    var tooltip_h = $('#htooltip').outerHeight();
    $('#htooltip').css('left', sender.position().left - (tooltip_w / 2) + (sender.width() / 2)).css('top', sender.position().top - tooltip_h).show();
}

function hide_tool_tip() {
    $('#htooltip').hide();
}

function show_not_bubble(count) {
    $('#hbubble_nots').css('left', $('#hmi_notifications').position().left - 10).html(count).show();
}

function show_req_bubble(count) {
    $('#hbubble_reqs').css('left', $('#hmi_requests').position().left - 10).html(count).show();
}

function show_msg_bubble(count) {
    $('#hbubble_msgs').css('left', $('#hmi_messages').position().left - 10).html(count).show();
}