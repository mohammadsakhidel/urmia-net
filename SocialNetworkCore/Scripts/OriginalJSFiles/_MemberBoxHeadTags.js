add_css('_MemberBox');
add_js('_MemberBox');
// load:
var field_seperator = '#';
var member_seperator = ';';
$(function () {
    _MemberBoxLoad();
});
function _MemberBoxLoad() {
    // show defalult selected members:
    var boxes = $('div.member_box');
    for (var i = 0; i < boxes.size(); i++) {
        var source = boxes.eq(i).find('input.fullname_input');
        refresh_selected_members(source);
    }
}
function refresh_selected_members(source) {
    // create members array:
    var id_type = source.parent().find('input[name="id_type"]').val();
    var members = get_added_members(source);
    var members_html = '';
    for (var i = 0; i < members.length; i++) {
        var member = members[i];
        members_html +=
        (id_type == 'full' ?
        '<div class="selected_member">' +
            '<a class="thumb_parent" href="' + member.PathOfProfilePage + '" title="' + member.FullName + '">' +
                '<img class="userthumb" src="' + member.PathOfThumb + '" />' +
            '</a>' +
            '<label class="selected_member_fullname">' + member.FullName + '</label>' +
            '<a class="remove_item"></a>' +
            '<input type="hidden" name="selected_member_email" disabled="disabled" value="' + member.Email + '">' +
            '<div class="clear"></div>' +
        '</div>'
        :
        '<a class="thumb_parent" href="' + member.PathOfProfilePage + '" title="' + member.FullName + '">' +
            '<img class="userthumb" src="' + member.PathOfThumb + '" />' +
        '</a>');
    }
    members_html += '<div class="clear"></div>';
    source.parent().find('div.selected_members_list').html(members_html);
}
function get_added_members(source) {
    var selected_members_st = source.parent().find('input[name="selected_members"]').val();
    var selected_members_ar = selected_members_st.split(member_seperator);
    var members = [];
    for (var i = 0; i < selected_members_ar.length; i++) {
        if ($.trim(selected_members_ar[i]).length > 0) {
            members.push(string_to_member(selected_members_ar[i]));
        }
    }
    return members;
}
function string_to_member(st) {
    var st_ar = st.split(field_seperator);
    var member = {
        Email: st_ar[0],
        FullName: st_ar[1],
        PathOfThumb: st_ar[2],
        PathOfProfilePage: st_ar[3]
    };
    return member;
}