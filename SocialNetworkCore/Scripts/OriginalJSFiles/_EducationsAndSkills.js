$(function () {

    //delete skill begin:
    $('a.remove_skill').live('click', function () {
        $('#prg_delete_skill').css('top', $(this).position().top).css('left', 80);
        var skillId = $(this).next('input').val();
        $('#skill_to_delete').val(skillId);
        $(this).parents().eq(2).submit();
    });

    // add edu begin:
    $('a#btn_add_edu').live('click', function () {
        $('form#frm_add_edu').submit();
    });

    //delete edu begin:
    $('a.remove_edu').live('click', function () {
        $('#prg_delete_edu').css('top', $(this).position().top).css('left', 80);
        var eduId = $(this).next('input').val();
        $('#edu_to_delete').val(eduId);
        $(this).parents().eq(2).submit();
    });

});

function txt_skills_key_down(e) {
    if (e.keyCode == 13) {
        var txt = $('input#txt_add_skill').val();
        if ($.trim(txt).length > 0) {
            $('form#frm_add_skill').submit();
            return false;
        }
        else {
            return false;
        }
    }
}

function submit_skills_form() {
    $('form#frm_add_skill').submit();
}

/* ******************************* Ajax **************************** */
function AddSkillBegin() {
    $('#msg_skills').html('').hide();
}

function AddSkillFailure() {
    $('#msg_skills').html($('#ajax_fail').val()).show();
}

function AddSkillSuccess(data) {
    if (data.Done) {
        var skills = data.Skills;
        show_list_of_skills(skills);
        $('#txt_add_skill').val('');
    }
    else {
        var errorMessage = $('#msg_errors_title').val();
        errorMessage += "<ul>";
        for (var i = 0; i < data.Errors.length; i++) {
            errorMessage += "<li>" + data.Errors[i] + "</li>";
        }
        errorMessage += "</ul>";
        $('#msg_skills').html(errorMessage).fadeIn('fast');
    }
}

function DeleteSkillSuccess(data) {
    if (data.Done) {
        $('#skill_row_' + data.SkillId).remove();
    }
}

function AddEducationBegin() {
    $('#msg_add_edu').html('').hide();
}

function AddEducationFailure() {
    $('#msg_add_edu').html($('#ajax_fail').val()).show();
}

function AddEducationSuccess(data) {
    if (data.Done) {
        var edus = data.Educations;
        show_list_of_educations(edus);
        clear_add_edu_form();
    }
    else {
        var errorMessage = $('#msg_errors_title').val();
        errorMessage += "<ul>";
        for (var i = 0; i < data.Errors.length; i++) {
            errorMessage += "<li>" + data.Errors[i] + "</li>";
        }
        errorMessage += "</ul>";
        $('#msg_add_edu').html(errorMessage).fadeIn('fast');
    }
}

function DeleteEducationSuccess(data) {
    if (data.Done) {
        $('#edu_row_' + data.EducationId).remove();
    }
}
/* ****************************************************************** */
function show_list_of_skills(skills) {
    var html = '';
    for (var i = 0; i < skills.length; i++) {
        html +=
            '<div class="skill_row" id="skill_row_' + skills[i].Id + '">' +
                '<a class="remove_skill">' + $('input#w_delete').val() + '</a>' +
                '<input type="hidden" disabled="disabled" value="' + skills[i].Id + '" />' + 
                skills[i].SkillTitle +
            '</div>';
    }
    $('div#skills_list').html(html);
}

function show_list_of_educations(educations) {
    var html = '';
    for (var i = 0; i < educations.length; i++) {
        html +=
            '<div class="edu_row" id="edu_row_' + educations[i].Id + '">' +
                '<a class="remove_edu">' + $('input#w_delete').val() + '</a>' +
                '<input type="hidden" disabled="disabled" value="' + educations[i].Id + '" />' +
                educations[i].EducationBranch + ' ' +
                '<span class="edu_row_explan">' +
                    educations[i].EducationLevelText + ' ' +
                    ($.trim(educations[i].EducationLocation).length > 0 ?
                    $('input#w_in').val() + ' ' +
                    educations[i].EducationLocation
                    : '') +
                '</span>' +
            '</div>';
    }
    $('div#educations_list').html(html);
}

function clear_add_edu_form() {
    $('select#EducationLevel').val('');
    $('input#EducationBranch').val('');
    $('input#EducationLocation').val('');
    $('select[name="FromYear"]').val('');
    $('select[name="ToYear"]').val('');
}