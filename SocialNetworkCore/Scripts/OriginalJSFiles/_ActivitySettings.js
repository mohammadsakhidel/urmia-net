$(function () {
    // ************************************* show and hide menu:
    $('a.act_set').live('click', function () {
        show_act_settings($(this));
    });

    $(document).live('click', function (e) {
        if (!is_settings_clicked(e.target)) {
            $('div.act_menu').hide();
        }
    });
    // ************************************ 
    $('div.act_menu a').live('click', function () {
        show_act_settings_loading($(this));
    });
});

function show_act_settings(sender) {
    $('div.act_menu').hide();
    var menu = sender.next('div');
    menu.css('left', sender.position().left).css('top', sender.position().top + 16).show();
}

function is_settings_clicked(target) {
    if (target.className == 'act_set' || target.className.match(/\bact_menu\b/) || target.className.match(/\bact_menu_a\b/))
        return true;
    else
        return false;
}

function show_act_settings_loading(sender) {
    var prg = sender.parent().find('div.loading_small_circle');
    prg.css('left', sender.position().left).css('top', sender.position().top + 10).show();
}
/* ********************************* AJAX FUNCTIONS: ********************************** */
function HideActivityFailure() {
    alert($('#w_ajax_fail').val());
}

function HideActivitySuccess(data) {
    if (data.Done) {
        $('#activity_' + data.ActivityId).remove();
    }
    else {
        alert(data.Errors[0]);
    }
}

function HideMemberActivitiesSuccess(data) {
    if (data.Done) {
        $('#activity_' + data.ActivityId).remove();
    }
    else {
        alert(data.Errors[0]);
    }
}

function HideMemberActivitiesFailure() {
    alert($('#w_ajax_fail').val());
}

function DeleteActivitySuccess(data) {
    if (data.Done) {
        $('#activity_' + data.ActivityId).remove();
    }
    else {
        alert(data.Errors[0]);
    }
}

function DeleteActivityFailure() {
    alert($('#w_ajax_fail').val());
}
