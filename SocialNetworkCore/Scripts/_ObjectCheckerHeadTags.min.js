add_css('_ObjectChecker');
add_js('_ObjectChecker');
// load:
$(function () {
    _ObjectCheckerLoad();
});
function _ObjectCheckerLoad() {
    plugin_mask($('#tb_date_of_check'), '2099-99-99');
}