function SaveNewsFeedSettingsBegin() {
    $('#newsfeed_settings #msg_save').removeClass("error_message").removeClass("success_message").hide();
}

function SaveNewsFeedSettingsSuccess(data) {
    if (data.Done) {
        $('#newsfeed_settings #msg_save').removeClass("error_message").addClass("success_message").html(data.Message).show();
    }
    else {
        $('#newsfeed_settings #msg_save').removeClass("success_message").addClass("error_message").html(data.Errors[0]).show();
    }
}

function SaveNewsFeedSettingsFailure() {
    $('#newsfeed_settings #msg_save').removeClass("success_message").addClass("error_message").html($("#newsfeed_settings #w_ajax_fail").val()).show();
}