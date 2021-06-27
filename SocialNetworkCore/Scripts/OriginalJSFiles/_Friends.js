$(function () {

    // remove frisnd dialog:
    $('a[id^=btn_remove_friend_]').live("click", function () {
        $('input#hid_removing_item').val($(this).parent().attr('id'));
        var dialog = new Dialog(550, 400, '<div class="loading_dialog"></div>');
        dialog.Show();
    });

});

function ShowDialogFailure() {
    ShowDialogError($('input#w_ajax_fail').val());
}