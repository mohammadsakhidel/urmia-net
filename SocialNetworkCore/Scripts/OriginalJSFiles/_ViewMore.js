$(function () {
    $('a.view_more').live('click', function (e) {
        $(this).text('').attr('class', 'view_more_progress');
        $(this).parent().submit();
        e.stopPropagation();
        return false;
    });
});

function ViewMoreSuccess(data) {
    var frm = $('#frm_view_more_' + data.RandomId);
    var update_target_id = frm.find('input[name="update_target_id"]').eq(0).val();
    if (data.Done) {
        //** append more items:
        $('#' + update_target_id).append(data.PartialView);
        //** update view more:
        frm.find('input[name="CurrentPageIndex"]').eq(0).val(data.NextPageIndex);
        if (data.IsThereMore) {
            frm.find('a.view_more_progress').attr('class', 'view_more').text(frm.find('input[name="w_view_more"]').eq(0).val());
        }
        else {
            frm.find('a.view_more_progress').attr('class', 'view_more').hide();
        }
        _SharedObjectActionsEnd();
    }
    else {
        $('#' + update_target_id).find('div.error_message').remove();
        $('#' + update_target_id).append('<div class="error_message">' + data.Errors[0] + '</div>');
        frm.find('a.view_more_progress').attr('class', 'view_more').text(frm.find('input[name="w_view_more"]').eq(0).val());
    }
}

function ViewMoreFailure() {
    alert($('#w_ajax_fail').val());
}