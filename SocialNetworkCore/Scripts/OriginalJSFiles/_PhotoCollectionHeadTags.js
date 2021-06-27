add_css('_PhotoCollection');
add_js('_PhotoCollection');
//load:
$(function () {
    _PhotoCollectionLoad();
});
function _PhotoCollectionLoad() {
    // make next and prev vertically center:
    var el = $('img.def_photo').size() > 0 ? $('img.def_photo') : $('div.def_photo_access_denied');
    $('a.next').css('top', ((el.height() / 2) - 16) + 'px').show();
    $('a.prev').css('top', ((el.height() / 2) - 16) + 'px').show();
    $('img.def_photo').load(function () {
        $('a.next').css('top', (($(this).height() / 2) - 16) + 'px').show();
        $('a.prev').css('top', (($(this).height() / 2) - 16) + 'px').show();
    });
}