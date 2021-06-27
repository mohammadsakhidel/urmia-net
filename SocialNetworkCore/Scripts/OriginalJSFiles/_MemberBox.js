var field_seperator = '#';
var member_seperator = ';';
$(function () {
    // textbox change:
    $('div.member_box input.fullname_input').live("keyup", function (e) {
        var source = $(this);
        if ($.trim(source.val()).length > 0) {
            if (e.keyCode == 13) { //enter:
                var selected_index = get_index(source);
                var member = string_to_member(source.parent().find('div.members_pop .member_li').eq(selected_index).find('input[name="member_info"]').val());
                add_member_to_selecteds(source, member);
                //
                source.val('');
                hide_pop(source);
            }
            else if (e.keyCode >= 37 && e.keyCode <= 40) { // arrows:
                if (e.keyCode == 39 || e.keyCode == 40) {
                    go_down(source);
                }
                else if (e.keyCode == 37 || e.keyCode == 38) {
                    go_up(source);
                }
            }
            else {
                show_preloader(source);
                get_members(source);
            }
        }
        else {
            hide_pop(source);
        }
    });

    // remove item:
    $('a.remove_item').live("click", function () {
        var source = $(this).parents().eq(2).find('input.fullname_input');
        var email = $(this).parent().find('input[name="selected_member_email"]').val();
        remove_member_from_selecteds(source, email);
    });

    // add member by mouse click:
    $('a.member_li').live("click", function () {
        var source = $(this).parents().eq(2).find('input.fullname_input');
        var member = string_to_member($(this).find('input[name="member_info"]').val());
        add_member_to_selecteds(source, member);
        //
        source.val('');
        hide_pop(source);
    });

});

function hide_pop(source) {
    var pop = source.parent().find('div.members_pop');
    set_index(source, -1);
    pop.html('').hide();
}

function set_index(source, index) {
    source.parent().find('input[name="selected_index"]').val(index);
}

function get_index(source) {
    return Number(source.parent().find('input[name="selected_index"]').val());
}

function show_preloader(source) {
    var members_pop = source.parent().find('div.members_pop');
    var html =
        '<div class="preloader">' +
            '<div class="loading_small_circle"></div>' +
        '</div>';
    members_pop.html(html)
            .css('top', source.position().top + source.outerHeight())
            .css('left', source.position().left)
            .width(source.outerWidth())
            .show();
}

function get_members(source) {
    $.ajax({
        type: "Post",
        url: source.parent().find('input[name="action_url"]').val(),
        dataType: "json",
        data: "{ 'fullName' : '" + source.val() + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Done) {
                show_members(source, JSON.parse(data.Members));
            }
            else {
                alert(data.Errors[0]);
            }
        }
    });
}

function show_members(source, members) {
    var list_html = '<div class="members_list">';
    for (var i = 0; i < members.length; i++) {
        var member = members[i];
        list_html +=
            '<a class="member_li">' +
                '<img class="member_li_thumb" src="' + member.PathOfThumb + '" />' +
                '<span class="member_li_fullname">' + member.FullName + '</span>' +
                '<input type="hidden" name="member_info" disabled="disabled" value="' + member_to_string(member) + '" />' +
                '<div class="clear"></div>' +
            '</a>';
    }
    list_html += '</div>';
    var members_pop = source.parent().find('div.members_pop');
    members_pop.html(list_html);
}

function go_down(source) {
    var index = get_index(source);
    var li_count = get_list_count(source);
    index = (index < li_count - 1 ? index + 1 : index);
    set_index(source, index);
    refresh_selected_li(source);
}

function go_up(source) {
    var index = get_index(source);
    index = (index > 0 ? index - 1 : 0);
    set_index(source, index);
    refresh_selected_li(source);
}

function get_list_count(source) {
    return source.parent().find('div.members_pop .member_li').size();
}

function refresh_selected_li(source) {
    var index = get_index(source);
    source.parent().find('div.members_pop .member_li').removeClass('member_li_hover');
    source.parent().find('div.members_pop .member_li').eq(index).addClass('member_li_hover');
}

function member_to_string(member) {
    var st = member.Email + field_seperator + member.FullName + field_seperator + member.PathOfThumb + field_seperator + member.PathOfProfilePage;
    return st;
}

function add_member_to_selecteds(source, member) {
    var member_st = member_to_string(member);
    var hid_selecteds = source.parent().find('input[name="selected_members"]');
    var hid_submiting_members = source.parent().find('input[name="' + source.parent().find('input[name="selected_members_hidden_name"]').val() + '"]');
    if ($.trim(hid_selecteds.val()).length > 0) {
        if (!is_member_added(source, member)) {
            hid_selecteds.val(hid_selecteds.val() + member_seperator + member_st);
            hid_submiting_members.val(hid_submiting_members.val() + member_seperator + member.Email);
        }
    }
    else {
        hid_selecteds.val(member_st);
        hid_submiting_members.val(member.Email);
    }
    refresh_selected_members(source);
}

function remove_member_from_selecteds(source, email) {
    // remove from list:
    var list_items = source.parent().find('div.selected_members_list div.selected_member');
    for (var i = 0; i < list_items.size(); i++) {
        var li = list_items.eq(i);
        if (li.find('input[name="selected_member_email"]').val() == email) {
            li.remove();
        }
    }
    // remove from submiting form hidden:
    var new_added_members = '';
    var hid_submiting_members = source.parent().find('input[name="' + source.parent().find('input[name="selected_members_hidden_name"]').val() + '"]');
    if ($.trim(hid_submiting_members.val()).length > 0) {
        var added_members = hid_submiting_members.val().split(member_seperator);
        for (var i = 0; i < added_members.length; i++) {
            if (added_members[i] != email) {
                new_added_members += ($.trim(new_added_members.length) > 0 ? member_seperator + added_members[i] : added_members[i]);
            }
        }
    }
    hid_submiting_members.val(new_added_members);
    // remove from selected member infos:
    new_added_members = '';
    added_members = get_added_members(source);
    for (var i = 0; i < added_members.length; i++) {
        if (added_members[i].Email != email) {
            new_added_members += ($.trim(new_added_members.length) > 0 ? member_seperator + member_to_string(added_members[i]) : member_to_string(added_members[i]));
        }
    }
    source.parent().find('input[name="selected_members"]').val(new_added_members);
}

function is_member_added(source, member) {
    var res = false;
    var members = get_added_members(source);
    for (var i = 0; i < members.length; i++) {
        if (members[i].Email == member.Email) {
            return true;
        }
    }
    return res;
}