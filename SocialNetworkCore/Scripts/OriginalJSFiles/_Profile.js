$(function () {
    $('div.top_bar_links > a').live('click', function () {
        // set as selected:
        $('div.top_bar_links > a').attr('class', 'top_bar_link');
        $(this).attr('class', 'top_bar_link_selected');
        // change address bar:
        change_address_bar($(this).attr('href'));
        // show loading:
        $('#pp_in_update_panel').html('<div class="loading_medium_circle prg_loading_pp_content"></div>');
    });

    // show send message dlg:
    $('a#ppbtn_send_message').live("click", function () {
        var dialog = new Dialog(450, 450, '<div class="loading_dialog"></div>');
        dialog.Show();
    });
});

function LoadPPContentFailure(data) {
    //$('#pp_in_update_panel').html('<div class="error_message">' + $('#member_profile input#pp_w_ajax_fail').val() + '</div>');
    $('#pp_in_update_panel').html(data.responseText);
}

function ShowDialogFailure() {
    ShowDialogError($('input#pp_w_ajax_fail').val());
}

function CacelFriendshipRequestBegin() {
    $('a#ppbtn_cnl_fnd_req').hide();
}

function CacelFriendshipRequestSuccess(data) {
    if (data.Done) {
        $('a#ppbtn_snd_fnd_req').show();
    }
    else {
        $('a#ppbtn_cnl_fnd_req').show();
    }
}

function SendFriendshipRequestBegin() {
    $('a#ppbtn_snd_fnd_req').hide();
}

function SendFriendshipRequestSuccess(data) {
    if (data.Done) {
        $('a#ppbtn_cnl_fnd_req').attr('href', data.CancelUrl).show();
    }
    else {
        alert(data.Errors[0]);
        $('a#ppbtn_snd_fnd_req').show();
    }
}

function ShowChatDialogBegin() {
    var dialog = new Dialog(550, 550, '<div class="loading_dialog"></div>');
    dialog.Show();
}

function ToggleMemberBlockSuccess(data) {
    if (data.Done) {
        $('#ppbtn_tgl_mem_block').text(data.ToggleText).css('visibility', 'visible');
    }
}

function ToggleMemberBlockBegin() {
    $('#ppbtn_tgl_mem_block').css('visibility', 'hidden');
}