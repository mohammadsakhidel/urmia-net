add_css('_Pages');
add_js('tinymce/tinymce');
add_js('_Pages');
$(function () {
    _PagesLoad();
});
function _PagesLoad() {
    tinymce.init({
        selector: "#pages textarea#page_editor",
        theme: "modern",
        height: 200,
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