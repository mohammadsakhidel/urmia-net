$(function () {
    $("input[type='text'], textarea").attr('spellcheck', false);
});

function start_sessions_updater(memberId, url, interval) {
    setInterval(function () {
        var active_session_id = ($('#ActiveChatSessionId').length > 0 ? $('#ActiveChatSessionId').val() : "");
        $.ajax({
            type: "Post",
            url: url,
            dataType: "json",
            data: "{ 'memberId' : '" + memberId + "', 'activeSessionId' : '" + active_session_id + "' }",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.Done && data.HasNewMessage) {
                    $('#chat_sessions_container').html(data.PartialView);
                    _ChatSessionsLoad();
                }
            }
        });
    }, interval);
}