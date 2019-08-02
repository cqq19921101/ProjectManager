$(function () {

    var diaSBUVendor = $('#dialog_VMI_QuerySBUVendor');
    var diaPlant = $('#dialog_VMI_QueryPlantInfo');
    var diagridview = $('#VMI_Query_ASNReport_gridDataList');

    $('#btn_VMIQuery_ASNReport_Query').click(function () {
        $(this).removeClass('ui-state-focus');
        checkDateTimeIsEmpty();        
    });
    $('#btn_VMIQuery_ASNReport_Print').click(function () {
        $(this).removeClass('ui-state-focus');

        var diagridview = $('#VMI_Query_ASNReport_gridDataList');
        var arrSelRowID = diagridview.jqGrid("getGridParam", "selarrrow");
        var ASNNOLines = "";

        if (arrSelRowID.length > 0) {
            for (var i = 0 ; i < arrSelRowID.length; i++) {
                ASNNOLines += diagridview.jqGrid('getRowData', arrSelRowID[i]).ASN_NUM + ",";
            }

            //check print type is diff or not
            $.ajax({
                url: __WebAppPathPrefix + '/VMIQuery/CheckASNReportType',
                data: {
                    ASNNUMs: escape($.trim(ASNNOLines))
                },
                type: "post",
                dataType: 'json',
                async: false,
                success: function (data) {
                    if (data.Result == true) {
                        window.open(__WebAppPathPrefix + '/VMIReport/VMIASNReportForm?ASN_NUMs=' + escape($.trim(ASNNOLines)), '_blank');
                    }
                    else {
                        alert(data.Message);
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
        else {
            alert("Please select a data to print.");
        }
    });
    $('#btn_VMIQuery_ASNReport_PrintBarCode').click(function () {
        $(this).removeClass('ui-state-focus');
        
        var diagridview = $('#VMI_Query_ASNReport_gridDataList');
        var arrSelRowID = diagridview.jqGrid("getGridParam", "selarrrow");
        var ASNNOLines = "";

        if (arrSelRowID.length > 0) {
            for (var i = 0 ; i < arrSelRowID.length; i++) {
                ASNNOLines += diagridview.jqGrid('getRowData', arrSelRowID[i]).ASN_NUM + ",";
            }

            window.open(__WebAppPathPrefix + '/VMIReport/VMIASNReportFormBarcode?ASN_NUMs=' + escape($.trim(ASNNOLines)), '_blank');
        }
        else {
            alert("Please select a data to print.");
        }
    });

    $('#btn_VMIQuery_ASNReport_SBU_VDN_Query').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txt_VMIQuery_ASNReport_SBU_VDN';

            InitdialogSBUVendor();
            ReloadDiaSBUVendorCodegridDataList();

            diaSBUVendor.show();
            diaSBUVendor.dialog("open");
            // div with class ui-dialog
            $('.ui-dialog :button').blur();
        }
    });

    $('#btn_VMIQuery_ASNReport_Plant_Query').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txt_VMIQuery_ASNReport_Plant';

            InitdialogPlant();
            ReloadDiaPlantCodegridDataList();

            diaPlant.show();
            diaPlant.dialog("open");
            // div with class ui-dialog
            $('.ui-dialog :button').blur();
        }
    });
});

function checkDateTimeIsEmpty() {
    if ($.trim($("#dpASNDate_FM").val()) == "") {
        alert("Please input Date(FM)");
    }
    else if ($.trim($("#dpASNDate_TO").val()) == "") {
        alert("Please input Date(TO)");
    }
    else {
        ReloadQueryASNReportListgridDataList();
    }
}



//function checkDateTimePickerAndReplace() {
//    var oldASNDateFrom = $("#dpASNDate_FM").val();
//    var oldASNDateTO = $("#dpASNDate_TO").val();

//    if (new Date(oldASNDateFrom) > new Date(oldASNDateTO)) {
//        $("#dpASNDate_FM").val(oldASNDateTO);
//        $("#dpASNDate_TO").val(oldASNDateFrom);
//    }
//}

function ReloadQueryASNReportListgridDataList() {
    var VMI_QueryASNReportgridDataList = $('#VMI_Query_ASNReport_gridDataList');

    VMI_QueryASNReportgridDataList.jqGrid('clearGridData');
    VMI_QueryASNReportgridDataList.jqGrid('setGridParam', {
        postData: {
            ASNNoFM: escape($.trim($("#txt_VMIQuery_ASNReport_ASNNoFM").val()))
            , ASNNoTO: escape($.trim($("#txt_VMIQuery_ASNReport_ASNNoTO").val()))
            , Plant: escape($.trim($("#txt_VMIQuery_ASNReport_Plant").val()))
            , VendorCode: escape($.trim($("#txt_VMIQuery_ASNReport_SBU_VDN").val()))
            , ASNDateFM: escape($.trim($("#dpASNDate_FM").val()))
            , ASNDateTO: escape($.trim($("#dpASNDate_TO").val()))
            , Status: escape($.trim($("#dropbox_VMIQuery_ASNReport_Status").val()))
            , TradeType: escape($.trim($("#dropbox_VMIQuery_ASNReport_TradeType").val()))
        }
    });
    VMI_QueryASNReportgridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}