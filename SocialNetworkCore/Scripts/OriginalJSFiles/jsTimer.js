jsTimer = function (interval, url, data, onSuccess) {
    this.Interval = interval;
    this.Url = url;
    this.Data = data;
    this.OnSuccess = onSuccess;
}

jsTimer.prototype.Start = function () {
    var url = this.Url;
    var data = this.Data;
    var onsuccess = this.OnSuccess;
    // interval:
    ServerCall(url, data, onsuccess);
    return setInterval(function () { ServerCall(url, data, onsuccess); }, this.Interval);
}

ServerCall = function (url, data, onsuccess) {
    $.ajax({
        type: "Post",
        url: url,
        dataType: "json",
        data: data,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            onsuccess(data);
        }
    });
}