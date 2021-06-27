add_css('_People');
add_css('_MemberBrief');
//load:
$(function () {
    _PeopleLoad();
});

function _PeopleLoad() {
    //set frame paddings:
    $('div#people').parents().find('div.frame_content')
    .css('padding', '0');
    $('div#people').parents().find('div.frame_parent').css('padding', '25px');
}