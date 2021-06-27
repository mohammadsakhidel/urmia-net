$(function () {

    $('#acc_set input[name="NewPassword"]').live('focus', function () {
        show_password_hint($(this));
    });

    $('#acc_set input[name="NewPassword"]').live('blur', function () {
        hide_hint();
    });

});

function show_password_hint(sender) {
    var hint = $('#acc_set #password-hint');
    hint.css('top', sender.position().top + 'px').css('left', (sender.position().left - hint.width() - 20) + 'px').fadeIn("fast");

}

function hide_hint() {
    $('#acc_set #password-hint').fadeOut("fast");
}

function SaveBegin() {
    $('#acc_set #msg_save').hide();
}

function SaveFailure() {
    $('#acc_set #msg_save').removeClass('success_message').addClass('error_message').html($('#acc_set #ajax_fail').val()).fadeIn('fast');
}

function SaveSuccess(data) {
    if (data.Done) {
        $('#acc_set #msg_save').removeClass('error_message').addClass('success_message').html(data.Message).fadeIn('fast');
    }
    else {
        var errorMessage = $('#msg_errors_title').val();
        errorMessage += "<ul>";
        for (var i = 0; i < data.Errors.length; i++) {
            errorMessage += "<li>" + data.Errors[i] + "</li>";
        }
        errorMessage += "</ul>";
        $('#acc_set #msg_save').removeClass('success_message').addClass('error_message').html(errorMessage).fadeIn('fast');
    }
}