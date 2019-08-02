$(function () {
    $('#btn_VMIProcess_ASNSpecialCancel_Cancel').click(function () {
        $(this).removeClass('ui-state-focus');
        var VMI_ASNSpecialCancelDetailgridDataList = $('#VMI_Process_ASNSpecialCancel_gridDataList');
        var arrSelRowID = VMI_ASNSpecialCancelDetailgridDataList.jqGrid("getGridParam", "selarrrow");
        var CancelASNNUM = "";

        if (arrSelRowID.length > 0) {
            for (var i = 0 ; i < arrSelRowID.length; i++) {
                CancelASNNUM += VMI_ASNSpecialCancelDetailgridDataList.jqGrid('getRowData', arrSelRowID[i]).ASN_NUM + ",";
            }

            CancelByMultiASNNUM(CancelASNNUM);
        }
        else {
            alert("請選擇要Cancel的ASN單.");
        }
    });

    $('#dia_btn_VMIProcess_ASNSpecialCancelManage_Cancel').click(function () {
        $(this).removeClass('ui-state-focus');
        var diaASNSpecialCancel = $('#dialog_VMIProcess_ASNSpecialCancelManage');
        var VMI_ASNSpecialCancelManagegridDataList = $('#VMI_Process_ASNSpecialCancelManage_gridDataList');
        var arrSelRowID = VMI_ASNSpecialCancelManagegridDataList.jqGrid("getGridParam", "selarrrow");
        var CancelASNLINE = "";

        if (arrSelRowID.length > 0) {
            for (var i = 0 ; i < arrSelRowID.length; i++) {
                CancelASNLINE += VMI_ASNSpecialCancelManagegridDataList.jqGrid('getRowData', arrSelRowID[i]).ASNLINE + ",";
            }

            CancelByASNLINE(diaASNSpecialCancel.prop("ASN_NUM"), CancelASNLINE);
        }
        else {
            alert("請選擇.");
        }
    });

    $('#dia_btn_VMIProcess_ASNSpecialCancelManage_CancelAll').click(function () {
        $(this).removeClass('ui-state-focus');
        var diaASNSpecialCancel = $('#dialog_VMIProcess_ASNSpecialCancelManage');
        CancelByASNNUM(diaASNSpecialCancel.prop("ASN_NUM"));
    });
})

function CancelByMultiASNNUM(AsnNums) {
    var diaErrorResult = $('#dialog_VMIProcess_ASNSpecialCancelErrorResult');
    var VMI_ASNSpecialCancelErrorResultgridDataList = $('#VMI_Process_ASNSpecialCancelErrorResult_gridDataList');
    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/ASNSpecialCancelByMultiASNNUM',
        data: {
            ASNNUMs: escape($.trim(AsnNums))
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.Rows.length > 0) {
                VMI_ASNSpecialCancelErrorResultgridDataList.jqGrid('clearGridData');
                for (var iCnt = 0; iCnt < data.Rows.length; iCnt++) {
                    VMI_ASNSpecialCancelErrorResultgridDataList.jqGrid('addRowData', data.Rows[iCnt].ASNNO,
                                {
                                    "ASNNO": data.Rows[iCnt].ASNNO,
                                    "IDNNUM": data.Rows[iCnt].IDNNUM,
                                    "ERRORCODE": data.Rows[iCnt].ERRORCODE
                                });
                }
                VMI_ASNSpecialCancelErrorResultgridDataList.trigger("reloadGrid");
                diaErrorResult.show();
                diaErrorResult.dialog("open");
            }
            else {
                alert("Cancel successfully.");
                ReloadASNSpecialCancelgridDataList();
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
            //HideAjaxLoading();
        }
    });
}

function CancelByASNNUM(AsnNum) {
    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/ASNSpecialCancelByASNNUM',
        data: {
            ASNNUM: escape($.trim(AsnNum))
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.Result == true) {
                alert("Cancel successfully.");
                ReloadASNSpecialCancelManagegridDataList();
                ReloadASNSpecialCancelgridDataList();
            }
            else {
                alert("ASN Cancel Failure, Error Code :" + data.Message);
                ReloadASNSpecialCancelManagegridDataList();
                ReloadASNSpecialCancelgridDataList();
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
            //HideAjaxLoading();
        }
    });
}

function CancelByASNLINE(AsnNum, AsnLines) {
    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/ASNSpecialCancelByASNLINE',
        data: {
            ASNNUM: escape($.trim(AsnNum)),
            ASNLINES: escape($.trim(AsnLines))
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.Result == true) {
                alert("Cancel successfully.");
                ReloadASNSpecialCancelgridDataList();
            }
            else {
                alert("ASN Cancel Failure, Error Code :" + data.Message);
                ReloadASNSpecialCancelgridDataList();
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
            //HideAjaxLoading();
        }
    });
}