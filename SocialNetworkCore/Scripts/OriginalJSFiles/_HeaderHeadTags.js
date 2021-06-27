add_css('_Header');
add_js('_Header');

function show_hbubbles(new_reqs, new_msgs, new_nots) {
    $('#hbubble_reqs').html(new_reqs);
    $('#hbubble_msgs').html(new_msgs);
    $('#hbubble_nots').html(new_nots);
    if (new_reqs > 0) {
        $('#hbubble_reqs').show();
    }
    else {
        $('#hbubble_reqs').hide();
    }
    if (new_msgs > 0) {
        $('#hbubble_msgs').show();
    }
    else {
        $('#hbubble_msgs').hide();
    }
    if (new_nots > 0) {
        $('#hbubble_nots').show();
    }
    else {
        $('#hbubble_nots').hide();
    }
}