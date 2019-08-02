var SessionTimer;
//var SessionTimeValue = 10000;
//var SessionTimeValue = 30000;
var SessionTimeValue = 660000; // check session timeout per 11min, if exceed 30 min then redirect to login page

function StartPortalSessionStopWatch() {
    SessionTimer = setTimeout(CheckPortalSessionExpire, SessionTimeValue);
}

function CheckPortalSessionExpire() {
    $.ajax({
        url: __WebAppPathPrefix + "/Home/CheckSessionExpired",
        type: "post",
        dataType: "text",
        async: false,
        success: function (data) {
            if (data == "X") {
                window.location = __WebAppPathPrefix + "/Account/Login?ReturnUrl=" + encodeURIComponent(__WebAppPathPrefix);
                clearTimeout(SessionTimer);
            }
            else if (data == "") {
                SessionTimer = setTimeout(CheckPortalSessionExpire, SessionTimeValue);
            }
            else {
                alert(data);
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, testStatus) {
        }
    });
}