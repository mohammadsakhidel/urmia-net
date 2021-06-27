$(function () {

    // like comment begin:
    $('a[id^=btn_like_comment_]').live('click', function () {
        if ($(this).text() == $('input#w_like').val()) {
            var a_comment_likes = $(this).parent().find('a.comment_likes');
            var likes_count = Number(a_comment_likes.text());
            likes_count++;
            a_comment_likes.text(likes_count);
            $(this).text($('input#w_unlike').val());
        }
        else {
            var a_comment_likes = $(this).parent().find('a.comment_likes');
            var likes_count = Number(a_comment_likes.text());
            likes_count--;
            a_comment_likes.text(likes_count);
            $(this).text($('input#w_like').val());
        }
    });

    // delete comment dialog:
    $('a[id^=btn_delete_comment_]').live("click", function () {
        var dialog = new Dialog(550, 400, '<div class="loading_dialog"></div>');
        dialog.Show();
    });

    // edit comment dialog:
    $('a[id^=btn_edit_comment_]').live("click", function () {
        var dialog = new Dialog(550, 400, '<div class="loading_dialog"></div>');
        dialog.Show();
    });

    // likes dialoge:
    $('a[id^=btn_comment_likes_]').live("click", function () {
        var dialog = new Dialog(400, 500, '<div class="loading_dialog"></div>');
        dialog.Show();
    });

});