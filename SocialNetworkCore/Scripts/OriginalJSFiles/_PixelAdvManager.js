function DeletePixelAdvSuccess(data) {
    if (data.Done) {
        $('#adv_item_' + data.Id).remove();
    }
    else {
        alert(data.Errors[0]);
    }
}

function EditPixelAdvSuccess(data) {
    if (data.Done) {
        set_form_data(data);
    }
    else {
        alert(data.Errors[0]);
    }
}

function ChangeVisibilityPixelAdvSuccess(data) {
    if (data.Done) {
        if (data.NewVisibility) {
            $('#adv_item_' + data.Id + ' span').removeClass('adv_item_hidden');
        }
        else {
            $('#adv_item_' + data.Id + ' span').addClass('adv_item_hidden');
        }
        $('#btn_change_adv_vis_' + data.Id).text(data.NewText);
    }
    else {
        alert(data.Errors[0]);
    }
}

function DeletePixelAdvFailure() {
    alert($('#w_ajax_fail').val());
}

function EditPixelAdvFailure() {
    alert($('#w_ajax_fail').val());
}

function ChangeVisibilityPixelAdvFailure() {
    alert($('#w_ajax_fail').val());
}

function set_form_data(data) {
    var form = $('#frm_pix_adv_editor');
    // obj data:
    form.find('input[name="Title"]').val(data.Title);
    form.find('input[name="Target"]').val(data.Target);
    form.find('input[name="WidthBlocks"]').val(data.WidthBlocks);
    form.find('input[name="HeightBlocks"]').val(data.HeightBlocks);
    form.find('select[name="BeginDateDay"]').val(data.BeginDateDay);
    form.find('select[name="BeginDateMonth"]').val(data.BeginDateMonth);
    form.find('select[name="BeginDateYear"]').val(data.BeginDateYear);
    form.find('select[name="EndDateDay"]').val(data.EndDateDay);
    form.find('select[name="EndDateMonth"]').val(data.EndDateMonth);
    form.find('select[name="EndDateYear"]').val(data.EndDateYear);
    form.find('select[name="Type"]').val(data.Type);
    form.find('select[name="PaymentStatus"]').val(data.PaymentStatus);
    form.find('input[name="Cost"]').val(data.Cost);
    form.find('textarea[name="Considerations"]').val(data.Considerations);
    if (data.HomeVis) form.find('input[name="HomeVis"]').attr('checked', 'checked');
    else form.find('input[name="HomeVis"]').removeAttr('checked');
    form.find('input[name="HomeTopIndex"]').val(data.HomeTopIndex);
    form.find('input[name="HomeLeftIndex"]').val(data.HomeLeftIndex);
    if (data.HomePageVis) form.find('input[name="HomePageVis"]').attr('checked', 'checked');
    else form.find('input[name="HomePageVis"]').removeAttr('checked');
    form.find('input[name="HomePageTopIndex"]').val(data.HomePageTopIndex);
    form.find('input[name="HomePageLeftIndex"]').val(data.HomePageLeftIndex);
    if (data.ProfileVis) form.find('input[name="ProfileVis"]').attr('checked', 'checked');
    else form.find('input[name="ProfileVis"]').removeAttr('checked');
    form.find('input[name="ProfileTopIndex"]').val(data.ProfileTopIndex);
    form.find('input[name="ProfileLeftIndex"]').val(data.ProfileLeftIndex);
    // form data for editing:
    form.find('input[name="FormAction"]').val('edit');
    form.find('input[name="Id"]').val(data.Id);
    scroll_top();
}