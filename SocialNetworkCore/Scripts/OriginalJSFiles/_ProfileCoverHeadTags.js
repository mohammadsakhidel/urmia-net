add_css('_ProfileCover');
add_js('_ProfileCover');
//load:
$(function () {
    _ProfileCoverLoad();
});
function _ProfileCoverLoad() {
    var cover_w = Number($('input#cover_w').val());
    var cover_h = Number($('input#cover_h').val());
    var display_width = $('#img_cover').width();
    var scale = display_width / cover_w;
    $('input#scale').val(scale);
    var crop_height = cover_h * scale;
    $('#img_cover').Jcrop({
        aspectRatio: (cover_w / cover_h),
        minSize: [display_width, crop_height],
        setSelect: [0, 0, display_width, crop_height],
        onSelect: updateCoords
    });
}
function updateCoords(c) {
    $('input#x').val(c.x);
    $('input#y').val(c.y);
    $('input#w').val(c.w);
    $('input#h').val(c.h);
}