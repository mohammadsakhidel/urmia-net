$(function () {
    $('#post_editor a#btn_add_photo').live('click', function () {
        document.getElementById('posted_photo').click();
    });

    $('div#post_editor input#posted_photo').live('change', function () {
        $('form#frm_add_photo').submit();
    });
});

function AddPostBegin() {
    $('#post_editor #post_message').hide();
}

function AddPostFailure() {
    $('#post_editor #post_message').removeClass('success_message').addClass('error_message').html($('#w_ajax_fail').val()).fadeIn('fast');
}

function AddPostSuccess(data) {
    if (data.Done) {
        clear_editor();
        $('#post_editor #post_message').removeClass('error_message').addClass('success_message').html(data.Message).fadeIn('fast');
        $('#' + $('#hid_pe_update_panel_id').val()).prepend(data.PartialView);
        //scroll_top();
    }
    else {
        $('#post_editor #post_message').removeClass('success_message').addClass('error_message').html(data.Errors[0]).fadeIn('fast');
    }
}

function clear_editor() {
    $('#post_editor #upload_progress').hide();
    $('#post_editor #uploaded_photos').empty();
    $('#post_editor #post_message').hide();
    $('#post_editor #post_txt').val("");
    $('#post_editor #post_txt').height(60);
    $('#post_editor select#VisibleTo').val(3);
    $('#post_editor input[name="PostId"]').val("");
}
