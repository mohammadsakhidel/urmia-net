$(function () {
    $('div#chat_sessions a').live('click', function () {
        var dialog = new Dialog(550, 550, '<div class="loading_dialog"></div>');
        dialog.Show();
        var action_url = $(this).parents().eq(1).find('input[name="action_url"]').eq(0).val();
        var sessionId = $(this).parents().eq(1).find('input[name="sessionId"]').eq(0).val();
        $('input#ActiveChatSessionId').val(sessionId);
        $(this).parents().eq(1).find('.chat_session_thumb_buble').remove();
        show_chat_dialog(action_url, sessionId);
        return false;
    });
});

function show_chat_dialog(action_url, sessionId) {
    $.ajax({
        type: "Post",
        url: action_url,
        dataType: "json",
        data: "{ 'sessionId' : '" + sessionId + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Done) {
                $('#dialog_view').html(data.PartialView).show();
            }
            else {
                $('#dialog_view').html('<div class="error_message">' + data.Errors[0] + '</div>').show();
            }
        }
    });
}