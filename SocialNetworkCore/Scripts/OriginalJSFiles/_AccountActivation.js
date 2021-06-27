//******************************* ACTIVATE ACCOUNT:

function ActivateBegin() {
    $('#acc_activation input#activate_submit').attr('disabled', 'disabled');
    $('#acc_activation #msg_activate').hide();
}

function ActivateFailure() {
    $('#acc_activation #msg_activate').removeClass('success_message').addClass('error_message').html($('#acc_activation #ajax_fail').val()).fadeIn('fast');
}

function ActivateSuccess(data) {
    if (data.Done) {
        $('#acc_activation #msg_activate').removeClass('error_message').addClass('success_message').html(data.Message).fadeIn('fast');
    }
    else {
        $('#acc_activation #msg_activate').removeClass('success_message').addClass('error_message').html(data.Errors[0]).fadeIn('fast');
    }
}

function ActivateComplete() {
    $('#acc_activation input#activate_submit').removeAttr('disabled');
}
//******************************* SEND ACTIVATION CODE:

function SendCodeBegin() {
    $('#acc_activation input#sendcode_submit').attr('disabled', 'disabled');
    $('#acc_activation #msg_sendcode').hide();
}

function SendCodeFailure() {
    $('#acc_activation #msg_sendcode').removeClass('success_message').addClass('error_message').html($('#acc_activation #ajax_fail').val()).fadeIn('fast');
}

function SendCodeSuccess(data) {
    if (data.Done) {
        $('#acc_activation #msg_sendcode').removeClass('error_message').addClass('success_message').html(data.Message).fadeIn('fast');
    }
    else {
        $('#acc_activation #msg_sendcode').removeClass('success_message').addClass('error_message').html(data.Errors[0]).fadeIn('fast');
    }
}

function SendCodeComplete() {
    $('#acc_activation input#sendcode_submit').removeAttr('disabled');
}