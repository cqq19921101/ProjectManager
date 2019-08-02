$(function () {
    var diaSBUVendor = $('#dialog_VMI_QuerySBUVendor');
    var diaPlant = $('#dialog_VMI_QueryPlantInfo');

    $('#btn_VMIProcess_ASNSpecialCancel_Query').click(function () {
        $(this).removeClass('ui-state-focus');
        //checkDateTimePickerAndReplace();
        checkDateTimeIsEmpty();
    });


    $('#btn_VMIProcess_ASNSpecialCancel_SBU_VDN_Query').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txt_VMIProcess_ASNSpecialCancel_SBU_VDN';

            InitdialogSBUVendor();
            ReloadDiaSBUVendorCodegridDataList();

            diaSBUVendor.show();
            diaSBUVendor.dialog("open");
            // div with class ui-dialog
            $('.ui-dialog :button').blur();
        }
    });

    $('#btn_VMIProcess_ASNSpecialCancel_Plant_Query').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txt_VMIProcess_ASNSpecialCancel_Plant';

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
        ReloadASNSpecialCancelgridDataList();
    }
}

function ReloadASNSpecialCancelgridDataList() {
    var VMI_ASNSpecialCancelgridDataList = $('#VMI_Process_ASNSpecialCancel_gridDataList');

    VMI_ASNSpecialCancelgridDataList.jqGrid('clearGridData');
    VMI_ASNSpecialCancelgridDataList.jqGrid('setGridParam', {
        postData: {
            ASNNoFM: escape($.trim($("#txt_VMIProcess_ASNSpecialCancel_ASNNoFM").val()))
            , ASNNoTO: escape($.trim($("#txt_VMIProcess_ASNSpecialCancel_ASNNoTO").val()))
            , Plant: escape($.trim($("#txt_VMIProcess_ASNSpecialCancel_Plant").val()))
            , VendorCode: escape($.trim($("#txt_VMIProcess_ASNSpecialCancel_SBU_VDN").val()))
            , ASNDateFM: escape($.trim($("#dpASNDate_FM").val()))
            , ASNDateTO: escape($.trim($("#dpASNDate_TO").val()))
            , TradeType: escape($.trim($("#dropbox_VMIProcess_ASNSpecialCancel_TradeType").val()))
        }
    });

    VMI_ASNSpecialCancelgridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}

function ReloadASNSpecialCancelManagegridDataList() {
    var VMI_ASNSpecialCancelgridDataList = $('#VMI_Process_ASNSpecialCancelManage_gridDataList');
    var diaASNSpecialCancel = $('#dialog_VMIProcess_ASNSpecialCancelManage');

    VMI_ASNSpecialCancelgridDataList.jqGrid('clearGridData');
    VMI_ASNSpecialCancelgridDataList.jqGrid('setGridParam', { postData: { ASN_NUM: escape($.trim(diaASNSpecialCancel.prop("ASN_NUM"))) } });
    VMI_ASNSpecialCancelgridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}