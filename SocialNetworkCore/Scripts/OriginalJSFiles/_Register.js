$(function () {

    $('#register input[name="Password"]').live('focus', function () {
        show_password_hint($(this));
    });

    $('#register input[name="Password"]').live('blur', function () {
        hide_hint();
    });

});

function show_password_hint(sender) {
    var hint = $('#register #password-hint');
    hint.css('top', sender.position().top + 'px').css('left', (sender.position().left - hint.width() - 20) + 'px').fadeIn("fast");

}

function hide_hint() {
    $('#register #password-hint').fadeOut("fast");
}

//********************************************************************
function BeginRegister() {
    $('#register input[type="submit"]').attr('disabled', 'disabled');
}

function FailureRegister() {
    $('#register .errors').html('<li>' + $('#register #ajax_fail').val() + '</li>');
}

function SuccessRegister(data) {
    if (data.Done) {
        window.location.href = "/account/activation?email=" + $('#register input[name="Email"]').val();
    }
    else {
        var list_items = '';
        for (var i = 0; i < data.Errors.length; i++) {
            list_items += '<li>' + data.Errors[i] + '</li>';
        }
        $('#register .errors').html(list_items);
    }
}

function CompleteRegister() {
    $('#register input[type="submit"]').removeAttr('disabled');
}