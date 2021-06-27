function change_address_bar(pageurl) {
    window.history.pushState({ path: pageurl }, '', pageurl);
    return false;
}

function scroll_top() {
    $(document).scrollTop(0);
}

function get_scroll() {
    return $(document).scrollTop();
}

function set_scroll(top) {
    $(document).scrollTop(top);
}

function split(st) {
    var list = st.split(/\s+/);
    var res = new Array();
    for (var i = 0; i < list.length; i++) {
        if ($.trim(list[i]).length > 0)
            res.push(list[i]);
    }
    return res;
}

function get_likes_text(likes_count, has_liked_shared_obj) {
    var gw_YouLikeThis = $('input#gw_YouLikeThis').val();
    var gw_YouAnd = $('input#gw_YouAnd').val();
    var gw_PeopleElseLikeThis = $('input#gw_PeopleElseLikeThis').val();
    var gw_PeopleLikeThis = $('input#gw_PeopleLikeThis').val();
    var likes_text = (has_liked_shared_obj ? (likes_count <= 1 ? gw_YouLikeThis : gw_YouAnd + " " + (likes_count - 1).toString() + " " + gw_PeopleElseLikeThis) : likes_count.toString() + " " + gw_PeopleLikeThis)
    return likes_text;
}

function add_js(filename) {
    filename = filename + '.min.js';
    if (!is_js_added(filename)) {
        var head = document.getElementsByTagName('head')[0];
        var script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = '/Scripts/' + filename;
        head.appendChild(script);
    }
}

function add_css(filename) {
    filename = filename + '.min.css';
    if (!is_css_added(filename)) {
        var head = document.getElementsByTagName('head')[0];
        var lnk = document.createElement('link');
        lnk.href = '/App_Themes/' + $('#app_theme').val() + '/' + filename;
        lnk.rel = 'stylesheet';
        lnk.type = 'text/css';
        head.appendChild(lnk);
    }
}

function is_js_added(filename) {
    var res = false;
    var head = document.getElementsByTagName('head')[0];
    var head_tags = head.getElementsByTagName('script')
    for (var i = 0; i < head_tags.length; i++) {
        if (head_tags[i].src.indexOf(filename) != -1) {
            return true;
        }
    }
    return res;
}

function is_css_added(filename) {
    var res = false;
    var head = document.getElementsByTagName('head')[0];
    var head_tags = head.getElementsByTagName('link')
    for (var i = 0; i < head_tags.length; i++) {
        if (head_tags[i].href.indexOf(filename) != -1) {
            return true;
        }
    }
    return res;
}

function play_sound(url) {
    var errSound = document.createElement('audio');
    errSound.setAttribute('src', url);
    errSound.load;
    errSound.play();
}