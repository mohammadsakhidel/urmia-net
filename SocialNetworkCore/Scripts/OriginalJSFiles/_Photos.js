function CreateAlbumBegin() {
    var link = $('#summary #link_CreateAlbum');
    change_address_bar(link.attr('href'));
}

function CreateAlbumComplete() {
    scroll_top();
}