var seted_Width = 0;
var seted_Height = 0;
$(function () {
    var dialog = $('div#dialog');
    $(window).resize(function () {
        if (dialog.find('input#dialog_status').val() == 'vis' && seted_Height > 0 && seted_Width > 0) {
            dialog.find('div.dialog_view').css('left', ($(window).width() / 2) - (seted_Width / 2)).css('top', ($(window).height() / 2) - (seted_Height / 2));
        }
    });
});

Dialog = function () {
    this.Width = 400;
    this.Height = 250;
    this.HideOnBodyClick = false;
    this.Content = '';
    hid_on_body_click = this.HideOnBodyClick;
}

Dialog = function (width, height, content) {
    this.Width = width;
    this.Height = height;
    this.HideOnBodyClick = false;
    this.Content = content;
    hid_on_body_click = this.HideOnBodyClick;
}

Dialog.prototype.Show = function () {
    DisableScroll();
    var dialog = $('div#dialog');
    dialog.find('div.dialog_view').css('min-width', this.Width).css('max-width', this.Width).css('max-height', this.Height).css('left', ($(window).width() / 2) - (this.Width / 2)).css('top', ($(window).height() / 2) - (this.Height / 2)).html(this.Content);
    dialog.show();
    dialog.find('input#dialog_status').val('vis');
    seted_Width = this.Width;
    seted_Height = this.Height;
}

Dialog.prototype.Hide = function () {
    HideDialog();
}

function DisableScroll() {
    $('input#dialog_pg_scroll').val(get_scroll());
    $('body').css('overflow', 'hidden');
}

function EnableScroll() {
    $('body').css('overflow', 'visible');
    set_scroll(Number($('input#dialog_pg_scroll').val()));
}

function HideDialog() {
    var dialog = $('div#dialog');
    dialog.find('#dialog_view').html('');
    dialog.hide();
    dialog.find('input#dialog_status').val('hid');
    EnableScroll();
    seted_Height = 0;
    seted_Width = 0;
}

function FadeOutDialog(speed) {
    var dialog = $('div#dialog');
    dialog.fadeOut(speed);
    dialog.find('input#dialog_status').val('hid');
    EnableScroll();
    seted_Height = 0;
    seted_Width = 0;
}

function ShowDialogError(msg) {
    var dialog = $('div#dialog');
    dialog.find('div.dialog_view')
    .html(
        '<div class="dialog_title">' +
            '<a class="dialog_close" onclick="HideDialog();"></a>' +
            '<h2>' + $('div#dialog').find('div#dialog_words input#dw_DialogErrorTitle').val() + '</h2>' +
            '<div class="clear"></div>' +
        '</div>' +
        '<div class="dialog_view_in">' +
            '<div class="error_message">' + msg + '</div>' +
            '<div class="dialog_actions">' +
                '<a class="cancel" onclick="HideDialog();">' + $('div#dialog').find('div#dialog_words input#dw_Close').val() + '</a>' +
            '</div>' +
            '<div class="clear"></div>' +
         '</div>'
    );
}