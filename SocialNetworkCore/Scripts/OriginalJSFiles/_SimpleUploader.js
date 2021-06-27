$(function () {
    $('input[type=file]').live('change', function (e) {
        var form = $(this).parents().find('form').eq(0);
        form.find('.uploader_filename input[type="text"]').val($(this).val());
        form.submit();
    });
});