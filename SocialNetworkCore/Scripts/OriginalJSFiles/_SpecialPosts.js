function SendSpecialPostBegin() {
    $('#msg_spec_post').removeClass('error_message').removeClass('success_message').html('').hide();
    $('#btn_send_spec_post').attr('disabled', 'disabled');
}

function SendSpecialPostSuccess(data) {
    if (data.Done) {
        $('#msg_spec_post').removeClass('error_message').addClass('success_message').html(data.Message).fadeIn('fast');
        set_editor_to_defaults();
    }
    else {
        $('#msg_spec_post').removeClass('success_message').addClass('error_message').html(data.Errors[0]).fadeIn('fast');
    }
}

function SendSpecialPostFailure() {
    $('#msg_spec_post').removeClass('success_message').addClass('error_message').html($('input#w_ajax_fail').val()).fadeIn('fast');
}

function SendSpecialPostComplete() {
    $('#btn_send_spec_post').removeAttr('disabled');
}

function set_editor_to_defaults() {
    tinyMCE.get('special_post_editor').setContent('');
    $('#frm_special_post_editor').find('input[name="FormAction"]').val('new');
    $('#frm_special_post_editor').find('input[name="PostId"]').val('0');
    $('#frm_special_post_editor').find('input[name="Priority"]').val('');
    $('#frm_special_post_editor').find('select[name="ShowMethod"]').val('1');
}