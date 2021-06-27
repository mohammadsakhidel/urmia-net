$(function () {
    $('div#share_dialog a.okey').live('click', function () {
        $('form#frm_share_dialog').submit();
    });
});

function ReshareObjectBegin() {
}

function ReshareObjectFailure() {
    //$('div#dialog div#dialog_msg').html(context.responseText).fadeIn('fast');
    $('div#dialog div#dialog_msg').removeClass('success_message').addClass('error_message').html($('input#w_ajax_fail').val()).fadeIn('fast');
}

function ReshareObjectSuccess(data) {
    if (data.Done) {
        var randomId = $('input#share_dlg_randomId').val();
        var shares_count = Number(data.SharesCount);
        $('#btn_object_share_' + randomId).text($('#w_share').val() + '(' + shares_count + ')');
        FadeOutDialog(300);
    }
    else {
        $('div#dialog div#dialog_msg').removeClass('success_message').addClass('error_message').html(data.Errors[0]).fadeIn('fast');
    }
}