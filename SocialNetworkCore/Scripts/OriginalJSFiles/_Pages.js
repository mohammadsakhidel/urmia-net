function DeletePageSuccess(data) {
    if (data.Done) {
        $('#page_' + data.PageId).remove();
    }
    else {
        show_errors(data.Errors);
    }
}

function EditPageSuccess(data) {
    if (data.Done) {
        var frm = $('#frm_page_editor');
        frm.find('input[name="PageId"]').val(data.PageId);
        frm.find('input[name="PageName"]').val(data.Name);
        frm.find('input[name="Title"]').val(data.Title);
        tinyMCE.get('page_editor').setContent(data.Content);
        frm.find('input[name="PageName"]').attr('disabled', 'disabled');
    }
    else {
        show_errors(data.Errors);
    }
}

function SendPageSuccess(data) {
    if (data.Done) {
        $('#msg_page_editor').removeClass('error_message').addClass('success_message').html(data.Message).show();
        clear_page_editor();
    }
    else {
        show_errors(data.Errors);
    }
}

function clear_page_editor() {
    var frm = $('#frm_page_editor');
    frm.find('input[name="PageId"]').val('');
    frm.find('input[name="PageName"]').val('');
    frm.find('input[name="PageName"]').removeAttr('disabled');
    frm.find('input[name="Title"]').val('');
    tinyMCE.get('page_editor').setContent('');
}

function show_errors(errors) {
    var errorMessage = "";
    errorMessage += "<ul>";
    for (var i = 0; i < errors.length; i++) {
        errorMessage += "<li>" + errors[i] + "</li>";
    }
    errorMessage += "</ul>";
    $('#msg_page_editor').removeClass('success_message').addClass('error_message').html(errorMessage).show();
}