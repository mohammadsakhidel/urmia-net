function GetEditMemberConsiderationsDialogBegin() {
    var dialog = new Dialog(500, 500, '<div class="loading_dialog"></div>');
    dialog.Show();
}

function ShowDialogFailure() {
    ShowDialogError($('input#w_ajax_fail').val());
}