add_css('_ChatSessions');
add_js('_ChatSessions');
//load:
$(function () {
    $(window).load(function () {
        _ChatSessionsLoad();
    });
});
function _ChatSessionsLoad() {
    var height = $(window).height();
    var sessions = $('div#chat_sessions');
    sessions.css('top', (height / 2) - (sessions.height() / 2));
}