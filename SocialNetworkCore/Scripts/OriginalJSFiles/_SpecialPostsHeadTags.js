add_css('_SpecialPosts');
add_js('tinymce/tinymce');
add_js('_SpecialPosts');
//load:
$(function () {
    _SpecialPostsLoad();
});
function _SpecialPostsLoad() {
    tinymce.init({
        selector: "#special_posts textarea#special_post_editor",
        theme: "modern",
        height: 120,
        directionality: 'rtl',
        menubar: true,
        relative_urls: false,
        remove_script_host: false,
        convert_urls: true,
        plugins: ["advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
         "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
         "save table contextmenu directionality emoticons template paste textcolor"]
    });
}