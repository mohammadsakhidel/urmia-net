function submit_mem_considerations() {
    $('div#mem_considerations_dialog form#frm_mem_cons').submit();
}

function SaveMemberConsiderationsSuccess(data) {
    if (data.Done) {
        $('div#mem_considerations_dialog #dialog_msg').removeClass('error_message').addClass('success_message').html(data.Message).show();
    }
    else {
        $('div#mem_considerations_dialog #dialog_msg').removeClass('success_message').addClass('error_message').html(data.Errors[0]).show();
    }
}