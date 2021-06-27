add_css('_EducationsAndSkills');
add_js('_EducationsAndSkills');
//load:
$(function () {
    _EducationsAndSkillsLoad();
});
function _EducationsAndSkillsLoad() {
    // auto complete:
    $('input[name="EducationBranch"], input[name="EducationLocation"], input#txt_add_skill').each(function () {
        var url = $(this).next('input[type="hidden"]').val();
        plugin_autocomplete($(this), url);
    });
}