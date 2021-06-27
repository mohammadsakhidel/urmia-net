$(function () {
    $('a.next, a.prev').live('click', function () {
        change_address_bar($(this).next('input').val());
    });
});

function LoadDetailsBegin() {
    // show loading:
    var prg = $('div#prg_photo_collection');
    var p_width = prg.parent().width();
    var p_height = prg.parent().height();
    var left = (p_width / 2) - (prg.outerWidth() / 2);
    var top = (p_height / 2) - (prg.outerHeight() / 2);
    prg.css('left', left + 'px').css('top', top + 'px').show();
}