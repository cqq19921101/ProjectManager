$(function () {
    $('a:contains("CCC")').click(function (e) {
        e.preventDefault();
        $.ajax({
            url: __WebAppPathPrefix + '/CCC/GetCCCLink',
            type: "post",
            dataType: 'text',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                if (data != "") {
                    window.open(data, "_target");
                }
                else {
                    window.location = __WebAppPathPrefix + '/CCC/Index';
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    });

    $('a:contains("GMS")').click(function (e) {
        e.preventDefault();
        $.ajax({
            url: __WebAppPathPrefix + '/GMS/GetGMSLink',
            type: "post",
            dataType: 'json',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                if (data.isSuccess) {
                    var winW = 800;
                    var winH = 600;
                    screenW = screen.width / 2;
                    screenH = screen.height / 2;
                    winL = screenW - (winW / 2);
                    winT = screenH - (winH / 2);
                    window.open(data.Url, "", "toolbar=yes,resizable=yes,scrollbars=yes,width=" + winW + ",height=" + winH + ",left=screen.width/2,top=" + winT);
                }
                else {
                    window.location = __WebAppPathPrefix + '/GMS/Index?Message=' + encodeURI(data.Message);
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    });
});