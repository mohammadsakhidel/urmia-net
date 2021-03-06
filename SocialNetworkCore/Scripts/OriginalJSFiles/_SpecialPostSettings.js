$(function () {
    // ************************************* show and hide menu:
    $('a.spec_pst_set').live('click', function () {
        show_post_settings($(this));
    });

    $(document).live('click', function (e) {
        if (!is_settings_clicked(e.target)) {
            $('div.post_menu').hide();
        }
    });

    $('div.post_menu a').live('click', function () {
        show_post_settings_loading($(this));
    });
});

function show_post_settings(sender) {
    $('div.post_menu').hide();
    var menu = sender.next('div');
    menu.css('left', sender.position().left).css('top', sender.position().top + 16).show();
}

function is_settings_clicked(target) {
    if (target.className == 'spec_pst_set' || target.className.match(/\bpost_menu\b/) || target.className.match(/\bpost_menu_a\b/)) {
        return true;
    }
    else {
        return false;
    }
}

function show_post_settings_loading(sender) {
    var prg = sender.parent().find('div.loading_small_circle');
    prg.css('left', sender.position().left).css('top', sender.position().top + 10).show();
}
/* ********* ajax ********* */
function DeleteSpecialPostSuccess(data) {
    if (data.Done) {
        $('#special_post_' + data.PostId).remove();
    }
    else {
        alert(data.Errors[0]);
    }
}

function ToggleVisibilitySpecialPostSuccess(data) {
    if (data.Done) {
        $('#pma_tgl_pst_' + data.PostId).text(data.NextToggleText);
    }
    else {
        alert(data.Errors[0]);
    }
}

function EditSpecialPostSuccess(data) {
    if (data.Done) {
        tinyMCE.get('special_post_editor').setContent(data.PostText);
        $('#frm_special_post_editor').find('input[name="FormAction"]').val('edit');
        $('#frm_special_post_editor').find('input[name="PostId"]').val(data.PostId);
        $('#frm_special_post_editor').find('input[name="Priority"]').val(data.Priority);
        $('#frm_special_post_editor').find('select[name="ShowMethod"]').val(data.ShowMethod);
        scroll_top();
    }
    else {
        alert(data.Errors[0]);
    }
}