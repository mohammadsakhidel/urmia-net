$(function () {
    //SignalR:
    var hub = $.connection.socialNetHub;
    // notify:
    hub.client.notify = function (type) {
        increase_bubbles(type);
    }
    // updateChat:
    hub.client.updateChat = function (signal) {
        update_chat(signal);
    }
    $.connection.hub.start();
});

function increase_bubbles(type) {
    var new_reqs = Number($('#hbubble_reqs').html());
    var new_msgs = Number($('#hbubble_msgs').html());
    var new_nots = Number($('#hbubble_nots').html());
    if (type == 1) { //new message
        if ($('#hbubble_msgs').is(':visible')) {
            var new_msgs = Number($('#hbubble_msgs').html());
            new_msgs = new_msgs + 1;
            show_msg_bubble(new_msgs);
        }
        else {
            show_msg_bubble(1);
        }
    }
    else if (type == 2) { // new request
        if ($('#hbubble_reqs').is(':visible')) {
            var new_reqs = Number($('#hbubble_reqs').html());
            new_reqs = new_reqs + 1;
            show_req_bubble(new_reqs);
        }
        else {
            show_req_bubble(1);
        }
    }
    else if (type == 3) { // new notification
        if ($('#hbubble_nots').is(':visible')) {
            var new_nots = Number($('#hbubble_nots').html());
            new_nots = new_nots + 1;
            show_not_bubble(new_nots);
        }
        else {
            show_not_bubble(1);
        }
    }
    ////////
    play_sound('/Content/Sounds/notify.mp3');
}

function update_chat(signal) {
    var msg = signal.Message;
    var is_dlg_open = is_session_open(msg.ChatSessionId);
    if (is_dlg_open) {
        $('div.chat_dlg_msgs_in').append(msg.PartialView);
        scroll_down_chat_msgs();
        $.connection.socialNetHub.server.setMessageAsRead(msg);
        // sound if window not active:
    }
    else {
        play_sound('/Content/Sounds/new_chat_msg.mp3');
    }
    // update sessions:
    $('#chat_sessions_container').html(signal.SessionsView).hide();
    var open_session_id = get_open_session_id();
    if ($.trim(open_session_id).length > 0) {
        $('#chat_session_thumb_buble_' + open_session_id).hide();
    }
    $('#chat_sessions_container').show();
}

function is_session_open(sessionId) {
    if ($('div#dialog').length > 0 && $('div#dialog').is(':visible')) {
        var dialog = $('div#dialog');
        if (dialog.find('input[name="ChatSessionId"]').length > 0 && dialog.find('input[name="ChatSessionId"]').eq(0).val() == sessionId) {
            return true;
        }
        else {
            return false;
        }
    }
    else {
        return false;
    }
}

function get_open_session_id() {
    if ($('div#dialog').length > 0 && $('div#dialog').is(':visible')) {
        var dialog = $('div#dialog');
        return dialog.find('input[name="ChatSessionId"]').eq(0).val();
    }
    else {
        return "";
    }
}