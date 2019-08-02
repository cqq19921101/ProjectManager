var __LockIsNotValid = "__lock_is_not_valid__";

function AcquireDataLock(DataKey) {
    var r = "";
    $.ajax({
        url: __WebAppPathPrefix + "/PortalService/AcquireLock",
        data: { "DataKey": DataKey },
        type: "post",
        dataType: 'text',
        async: false,
        success: function (data) {
            r = data;
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
            //$("#ajaxLoading").hide();
        }
    });
    return r;
}

function ReleaseDataLock(DataKey) {
    var r = "";
    $.ajax({
        url: __WebAppPathPrefix + "/PortalService/ReleaseLock",
        data: { "DataKey": DataKey },
        type: "post",
        dataType: 'text',
        async: false,
        success: function (data) {
            r = data;
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
            //$("#ajaxLoading").hide();
        }
    });
    return r;
}