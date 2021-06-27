function show_objects(raw_url) {
    var dt = $('#tb_date_of_check').val();
    if ($.trim(dt).length > 0) {
        window.location = raw_url + '?dt=' + dt;
    }
}