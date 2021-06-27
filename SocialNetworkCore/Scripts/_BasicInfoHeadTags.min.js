add_css('_BasicInfo');
add_js('_BasicInfo');

$(function () {
    _BasicInfoLoad();
});

function _BasicInfoLoad() {
    // auto complete:
    $('input[name="LivingCity"]').each(function () {
        var url = $(this).next('input[type="hidden"]').val();
        plugin_autocomplete($(this), url);
    });
}