function CheckIsVMIAdmin() {
    var CorpVendorCode = "";

    $.ajax({
        url: __WebAppPathPrefix + "/Home/CheckIsVMIAdminRole",
        //data: { "DataKey": DataKey },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.CheckResult) {
                CorpVendorCode = "";
            }
            else {
                CorpVendorCode = data.VMICorpVendorCode;
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
            //$("#ajaxLoading").hide();
        }
    });

    return CorpVendorCode;
}