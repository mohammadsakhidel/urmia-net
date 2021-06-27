$(function () {
    $('#hmi_requests a.okey, #hmi_requests a.cancel').live('click', function () {
        $(this).parent().parent().find('div.hmi_req_actions').hide();
        $(this).parent().parent().find('div.loading_small_circle').show();
    });
});

function AcceptFriendshipRequestSuccess(data) {
    if (data.Done) {
        $('#hmi_req_' + data.RequestId).remove();
    }
    else {
        alert(data.Errors[0]);
    }
}

function RejectFriendshipRequestSuccess(data) {
    if (data.Done) {
        $('#hmi_req_' + data.RequestId).remove();
    }
    else {
        alert(data.Errors[0]);
    }
}