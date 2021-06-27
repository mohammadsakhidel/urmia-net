add_css('_SearchMembers');
add_js('_SearchMembers');
add_css('_MemberBrief');
//load:
$(function () {
    _SearchMembersLoad();
});

function _SearchMembersLoad() {
    //set frame paddings:
    $('div#search_members').parents().find('div.frame_content')
    .css('padding-right', '0')
    .css('padding-top', '10px')
    .css('padding-bottom', '0')
    .css('padding-left', '0');
    $('div#search_members').parents().find('div.frame_parent').css('padding', '20px');
    // watermarks:
    plugin_watermark($('div#search_members .search_info_item input[name="FullName"]'), $('input#w_fullname').val());
}
