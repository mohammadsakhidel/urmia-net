var randomId = 0;
var msg_delay = 3000;
/* ******************************** */
function AddCommentBegin() {
    $('div#msg_add_comment_' + randomId).hide();
}
function AddCommentSuccess(data) {
    var randomId = data.RandomId;
    if (data.Done) {
        //$('#msg_add_comment_' + objId).removeClass('error_message').addClass('success_message').html(data.Message).fadeIn('fast').delay(msg_delay).fadeOut('slow');
        $('#txt_comment_' + randomId).val('');
        $('#txt_comment_' + randomId).height(20);
        $('#comments_' + randomId).append(data.PartialView);
        $('#btn_add_comment_' + randomId).text($('#w_comment').val() + '(' + data.CommentsCount + ')');
    }
    else {
        var msgText = "";
        for (var i = 0; i < data.Errors.length; i++) {
            msgText += "- " + data.Errors[i] + "<br />";
        }
        $('#msg_add_comment_' + randomId).removeClass('success_message').addClass('error_message').html(msgText).fadeIn('fast').delay(msg_delay).fadeOut('slow');
    }
}
function GetMoreCommentsSuccess(data) {
    var randomId = data.RandomId;
    $('#comments_' + randomId).prepend(data.PartialView);
    $('#date_of_first_comment_' + randomId).val(data.DateOfFirstComment);
    var all_comments_count = Number($('#comments_count_' + randomId).val());
    var current_comments_count = Number($('#comments_' + randomId).find('div.comment').size());
    if (all_comments_count <= current_comments_count)
        $('#more_comments_' + randomId).hide();
    else
        $('#more_comments_' + randomId).show();
}

/* ******************** event handlers ******************* */
function btn_add_comment_click(source) {
    var sender = $('#' + source.id);
    sender.parent().parent().find('div.new_comment').find('textarea').focus();
}

function txt_comment_keypress(event, source) {
    var sender = $('#' + source.id);
    if (event.keyCode == 13 && !event.shiftKey) {
        randomId = Number(sender.prev('input').prev('input').val());
        sender.parent().submit();
        return false;
    }
}

function btn_like_obj_click(source) {
    var sender = $('#' + source.id);
    var has_liked_shared_obj = (sender.parent().parent().find('div.actions_summery').find('input[name="has_liked_shared_obj"]').val() == 'true' ? true : false);
    if (!has_liked_shared_obj) {
        sender.text($('input#w_unlike').val());
        var summ_a = sender.parent().parent().find('div.actions_summery').find('a');
        var likes_count = Number(sender.parent().parent().find('div.actions_summery').find('input[name="likes_count"]').val());
        likes_count++;
        sender.parent().parent().find('div.actions_summery').find('input[name="likes_count"]').val(likes_count);
        sender.parent().parent().find('div.actions_summery').find('input[name="has_liked_shared_obj"]').val(!has_liked_shared_obj);
        summ_a.text(get_likes_text(likes_count, !has_liked_shared_obj));
    }
    else {
        sender.text($('input#w_like').val());
        var summ_a = sender.parent().parent().find('div.actions_summery').find('a');
        var likes_count = Number(sender.parent().parent().find('div.actions_summery').find('input[name="likes_count"]').val());
        likes_count--;
        sender.parent().parent().find('div.actions_summery').find('input[name="likes_count"]').val(likes_count);
        sender.parent().parent().find('div.actions_summery').find('input[name="has_liked_shared_obj"]').val(!has_liked_shared_obj);
        summ_a.text(get_likes_text(likes_count, !has_liked_shared_obj));
    }
}

function btn_more_comments_click(source) {
    var sender = $('#' + source.id);
    var form = sender.parent();
    sender.next('div.loading_small_circle').css('left', sender.position().left - 20).css('top', sender.position().top);
    form.submit();
}

function btn_obj_share_click() {
    var dialog = new Dialog(600, 500, '<div class="loading_dialog"></div>');
    dialog.Show();
}

function btn_obj_likes_click() {
    var dialog = new Dialog(400, 500, '<div class="loading_dialog"></div>');
    dialog.Show();
}