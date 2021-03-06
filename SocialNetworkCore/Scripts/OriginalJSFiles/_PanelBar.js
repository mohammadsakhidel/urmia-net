//****************************************************************************************************** AJAX BEGIN FUNCs
var mTop = 3;
var mLeft = 2;

function BasicInfoBegin() {
    $('#panelbar .loading_small_circle').css('left', $('#panel_link_basicinfo').position().left + mLeft).css('top', $('#panel_link_basicinfo').position().top + mTop);
    pageurl = $('#panel_link_basicinfo').attr('href');
    change_address_bar(pageurl);
}

function BasicInfoSuccess() {
    clear_selections();
    $('#panel_link_basicinfo').attr('class', 'panel_link_selected');
    scroll_top();
}

function FavoritesBegin() {
    $('#panelbar .loading_small_circle').css('left', $('#panel_link_favorites').position().left + mLeft).css('top', $('#panel_link_favorites').position().top + mTop);
    pageurl = $('#panel_link_favorites').attr('href');
    change_address_bar(pageurl);
}

function FavoritesSuccess() {
    clear_selections();
    $('#panel_link_favorites').attr('class', 'panel_link_selected');
    scroll_top();
}

function EducationsAndSkillsBegin() {
    $('#panelbar .loading_small_circle').css('left', $('#panel_link_educations').position().left + mLeft).css('top', $('#panel_link_educations').position().top + mTop);
    pageurl = $('#panel_link_educations').attr('href');
    change_address_bar(pageurl);
}

function EducationsAndSkillsSuccess() {
    clear_selections();
    $('#panel_link_educations').attr('class', 'panel_link_selected');
    scroll_top();
}

function DeleteAccountBegin() {
    $('#panelbar .loading_small_circle').css('left', $('#panel_link_delete_account').position().left + mLeft).css('top', $('#panel_link_delete_account').position().top + mTop);
    pageurl = $('#panel_link_delete_account').attr('href');
    change_address_bar(pageurl);
}

function DeleteAccountSuccess() {
    clear_selections();
    $('#panel_link_delete_account').attr('class', 'panel_link_selected');
    scroll_top();
}

function PhotosBegin() {
    $('#panelbar .loading_small_circle').css('left', $('#panel_link_photos').position().left + mLeft).css('top', $('#panel_link_photos').position().top + mTop);
    pageurl = $('#panel_link_photos').attr('href');
    change_address_bar(pageurl);
}

function PhotosSuccess() {
    clear_selections();
    $('#panel_link_photos').attr('class', 'panel_link_selected');
    scroll_top();
}

function ProfileCoverBegin() {
    $('#panelbar .loading_small_circle').css('left', $('#panel_link_profile_cover').position().left + mLeft).css('top', $('#panel_link_profile_cover').position().top + mTop);
    pageurl = $('#panel_link_profile_cover').attr('href');
    change_address_bar(pageurl);
}

function ProfileCoverSuccess() {
    clear_selections();
    $('#panel_link_profile_cover').attr('class', 'panel_link_selected');
    scroll_top();
}

function FriendsBegin() {
    $('#panelbar .loading_small_circle').css('left', $('#panel_link_friends').position().left + mLeft).css('top', $('#panel_link_friends').position().top + mTop);
    pageurl = $('#panel_link_friends').attr('href');
    change_address_bar(pageurl);
}

function FriendsSuccess() {
    clear_selections();
    $('#panel_link_friends').attr('class', 'panel_link_selected');
    scroll_top();
}

function MessagesBegin() {
    $('#panelbar .loading_small_circle').css('left', $('#panel_link_messages').position().left + mLeft).css('top', $('#panel_link_messages').position().top + mTop);
    pageurl = $('#panel_link_messages').attr('href');
    change_address_bar(pageurl);
}

function MessagesSuccess() {
    clear_selections();
    $('#panel_link_messages').attr('class', 'panel_link_selected');
    scroll_top();
}

function PrivacyBegin() {
    $('#panelbar .loading_small_circle').css('left', $('#panel_link_privacy').position().left + mLeft).css('top', $('#panel_link_privacy').position().top + mTop);
    pageurl = $('#panel_link_privacy').attr('href');
    change_address_bar(pageurl);
}

function PrivacySuccess() {
    clear_selections();
    $('#panel_link_privacy').attr('class', 'panel_link_selected');
    scroll_top();
}

function NewsFeedSettingsBegin() {
    $('#panelbar .loading_small_circle').css('left', $('#panel_link_newsfeed').position().left + mLeft).css('top', $('#panel_link_newsfeed').position().top + mTop);
    pageurl = $('#panel_link_newsfeed').attr('href');
    change_address_bar(pageurl);
}

function NewsFeedSettingsSuccess() {
    clear_selections();
    $('#panel_link_newsfeed').attr('class', 'panel_link_selected');
    scroll_top();
}

function MailInformsBegin() {
    $('#panelbar .loading_small_circle').css('left', $('#panel_link_mail_informs').position().left + mLeft).css('top', $('#panel_link_mail_informs').position().top + mTop);
    pageurl = $('#panel_link_mail_informs').attr('href');
    change_address_bar(pageurl);
}

function MailInformsSuccess() {
    clear_selections();
    $('#panel_link_mail_informs').attr('class', 'panel_link_selected');
    scroll_top();
}

function AccountSettingsBegin() {
    $('#panelbar .loading_small_circle').css('left', $('#panel_link_account_settings').position().left + mLeft).css('top', $('#panel_link_account_settings').position().top + mTop);
    pageurl = $('#panel_link_account_settings').attr('href');
    change_address_bar(pageurl);
}

function AccountSettingsSuccess() {
    clear_selections();
    $('#panel_link_account_settings').attr('class', 'panel_link_selected');
    scroll_top();
}
//******************************************************************************************************
function clear_selections() {
    $('#panelbar .panel_link_selected').attr('class', 'panel_link');
}
