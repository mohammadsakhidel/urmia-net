add_css('_DeleteAccount');
add_js('_DeleteAccount');
//load:
$(function () {
    _DeleteAccountLoad();
});
function _DeleteAccountLoad() {
    plugin_autosize($('div#del_account textarea[name="Reason"]'));
}