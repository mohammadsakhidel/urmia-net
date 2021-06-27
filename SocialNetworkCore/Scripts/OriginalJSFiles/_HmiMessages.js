$(function () {
    // show new msg dialog:
    $('a.hmi_new_message').live("click", function () {
        var dialog = new Dialog(450, 450, '<div class="loading_dialog"></div>');
        dialog.Show();
    });
});

function ShowDialogFailure() {
    ShowDialogError($('input#w_ajax_fail').val());
}