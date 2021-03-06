function SearchMemberBegin() {
    $('#btn_srch_submit').attr('disabled', 'disabled');
    $('div#search_members #search_result').html('<div id="prg_search_members" class="loading_large_circle"></div>');
}

function SearchMemberFailure() {
    $('div#search_members #search_result').html('<div class="error_message">' + $('input#w_ajax_fail').val() + '</div>');
}

function SearchMemberSuccess(data) {
    if (data.Done) {
        $('div#search_members #search_result').html(data.PartialView);
    }
    else {
        $('div#search_members #search_result').html('<div class="error_message">' + data.Errors[0] + '</div>');
    }
}

function SearchMemberComplete() {
    $('#btn_srch_submit').removeAttr('disabled');
}