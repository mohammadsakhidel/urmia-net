$(function () {
    $('div.album_tools a.album_edit').live('click', function () {
        pageurl = $(this).attr('href');
        change_address_bar(pageurl);
    });

    $('div.album_tools a.album_photos').live('click', function () {
        pageurl = $(this).attr('href');
        change_address_bar(pageurl);
    });
});

function EditAlbumComplete() {
    scroll_top();
}

function AlbumPhotosComplete() {
    scroll_top();
}

function DeleteAlbumBegin() {
    var scroll_top = get_scroll();
    $('input#scroll_top').val(scroll_top);
}

function DeleteAlbumComplete() {
    var scroll_top = Number($('input#scroll_top').val());
    set_scroll(scroll_top);
}