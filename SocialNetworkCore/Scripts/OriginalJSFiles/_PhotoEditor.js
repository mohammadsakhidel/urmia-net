var photoId = 0;
var type = '';

$(function () {

    $('a[id^=btn_edit_thumb_]').live("click", function () {
        show_img_editor($(this), 'thumb');
    });

    $('a[id^=btn_edit_cover_]').live("click", function () {
        show_img_editor($(this), 'cover');
    });

    $('a[id^=close_editor_]').live("click", function () {
        set_thumb_defaults();
        $('#div_thumb_editor_' + photoId).slideUp('fast');
    });

    $('input[name="SavePhoto"]').live("click", function () {
        $(this).next('div').next('div').hide();
    });

});

function updateCoords(c) {
    if (type == 'thumb') {
        $('#thumb_x_' + photoId).val(c.x);
        $('#thumb_y_' + photoId).val(c.y);
        $('#thumb_w_' + photoId).val(c.w);
        $('#thumb_h_' + photoId).val(c.h);
    }
    else if (type == 'cover') {
        $('#cover_x_' + photoId).val(c.x);
        $('#cover_y_' + photoId).val(c.y);
        $('#cover_w_' + photoId).val(c.w);
        $('#cover_h_' + photoId).val(c.h);
    }
}

function set_thumb_defaults() {
    $('#thumb_x_' + photoId).val(0);
    $('#thumb_y_' + photoId).val(0);
    $('#thumb_w_' + photoId).val(0);
    $('#thumb_h_' + photoId).val(0);
    $('#scale_' + photoId).val(1);
}

function show_img_editor(sender, t) {
    var root = sender.parents().eq(2);
    photoId = root.find('input[name="Id"]').val();
    type = t;
    var photoUrl = root.find('input[name="Url"]').val();
    var width = Number(root.find('input[name="Width"]').val());
    var height = Number(root.find('input[name="Height"]').val());
    var newWidth = (width > 600 ? 600 : width);
    var newHeight = (height * newWidth) / width;
    var scale = newWidth / width;
    var coverHeight = Number($('#cover_height').val());
    var thumb_sides = Number($('#thumb_sides').val());
    $('#scale_' + photoId).val(scale);
    var prg = sender.next('div.loading_small_circle');
    prg.show();
    $('#img_photo_' + photoId).removeAttr('src').attr('src', photoUrl).load(function () {
        $('#img_photo_' + photoId).attr('width', newWidth).attr('height', newHeight).Jcrop({
            aspectRatio: (type == 'cover' ? 3.39 : 1),
            minSize: (type == 'cover' ? [newWidth, scale * coverHeight] : [180, 180]),
            setSelect: (type == 'cover' ? [0, 0, newWidth, scale * coverHeight] : [0, 0, 180, 180]),
            onSelect: updateCoords
        });
        $('#div_thumb_editor_' + photoId).slideDown('fast');
        prg.hide();
    });
}
//-------------------------------------------------------------
function EditPhotoSuccess(data) {
    var photoId = data.Id;
    if (data.Done) {
        $('#msg_edit_photo_' + photoId).removeClass('fail').addClass('done').show();
    }
    else {
        alert(data.Errors[0]);
        $('#msg_edit_photo_' + photoId).removeClass('done').addClass('fail').show();
    }
}