add_css('_PostEditor');
add_js('_PostEditor');
//load:
$(function () {
    _PostEditorLoad();
});
function _PostEditorLoad() {
    //********** photo upload related:
    $('#post_editor a#btn_add_photo').live('click', function () {
        document.getElementById('posted_photo').click();
    });
    $('div#post_editor input#posted_photo').live('change', function () {
        $('form#frm_add_photo').submit();
    });
    //**********
    plugin_autosize($('#post_txt'));
    plugin_watermark($('#post_txt'), $('input#w_what_in_mind').val());
    /* ************************ Upload Post Photos ************************ */
    var bar = $('#post_editor .bar');
    var percent = $('#post_editor .percent');
    var status = $('#post_editor .status');
    setTimeout(function () {

        $('#frm_add_photo').ajaxForm({
            dataType: 'json',
            type: 'POST',
            contentType: 'application/json',
            beforeSend: function () {
                $('#post_editor #post_message').hide();
                status.hide();
                var percentVal = '0%';
                bar.width(percentVal)
                percent.html(percentVal);
                $('#post_editor #upload_progress').fadeIn('fast');
            },
            uploadProgress: function (event, position, total, percentComplete) {
                var percentVal = percentComplete + '%';
                bar.width(percentVal)
                percent.html(percentVal);
            },
            success: function () {
                var percentVal = '100%';
                bar.width(percentVal)
                percent.html(percentVal);
            },
            complete: function (xhr) {
                var d = $.parseJSON(xhr.responseText);
                if (d.Done) {
                    var added_temp_files = $('input[id="hid_temp_file_names"]').val();
                    $('input[name="TempFileNames"]').val(added_temp_files + d.TempFileName + ';');
                    $('#post_editor #uploaded_photos').append('<img class="thumb" src="' + d.Path + '" />');
                }
                else {
                    $('#post_editor #post_message').removeClass('success_message').addClass('error_message').html(d.Errors[0]).fadeIn('fast');
                }
            }
        });

    }, 500);

}