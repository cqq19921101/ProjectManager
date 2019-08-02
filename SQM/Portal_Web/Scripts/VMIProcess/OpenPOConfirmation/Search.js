$(function () {
    var diaPlant = $('#dialog_VMI_QueryPlantInfo');
    var diaSBUVendor = $('#dialog_VMI_QuerySBUVendor');

    $('#btnOpenQueryPlantDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtPlant';

            InitdialogPlant();
            ReloadDiaPlantCodegridDataList();

            diaPlant.show();
            diaPlant.dialog("open");
        }
    });

    $('#btnOpenQueryVendorCodeDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtVendorCode';

            InitdialogSBUVendor();
            ReloadDiaSBUVendorCodegridDataList();

            diaSBUVendor.show();
            diaSBUVendor.dialog("open");
        }
    });

    $('#btnQueryOpenPOConfirmation').click(function () {
        $(this).removeClass('ui-state-focus');
        reloadOpenPOConfirmationGridList();
    });

    $('#btnExportOpenPOConfirmation').click(function () {
        $(this).removeClass('ui-state-focus');

        var Plant = $.trim($('#txtPlant').val());
        var VendorCode = $.trim($('#txtVendorCode').val());
        var PONumberFrom = $.trim($('#txtPONumberFrom').val());
        var PONumberTo = $.trim($('#txtPONumberTo').val());
        var DeliveryDateFrom = $.trim($('#txtDeliveryDateFrom').val());
        var DeliveryDateTo = $.trim($('#txtDeliveryDateTo').val());
        var ConfirmedDateFrom = $.trim($('#txtConfirmedDateFrom').val());
        var ConfirmedDateTo = $.trim($('#txtConfirmedDateTo').val());
        var MaterialFrom = $.trim($('#txtMaterialFrom').val());
        var MaterialTo = $.trim($('#txtMaterialTo').val());
        var MaterialDescription = $.trim($('#txtDesc').val());
        var ConfirmStatus = $.trim($('#ddlConfirmStatus').val());

        if (DeliveryDateFrom == '' || DeliveryDateTo == '') {
            alert('Please select Delivery Date(FM) and Delivery Date(TO) first.');
        }
        else if (dateDiff(DeliveryDateFrom, DeliveryDateTo) > 184) {
            alert('The interval of the Delivery Date is exceed 6 month, please reselect it under 6 month.');
        }
        else {
            $.ajax({
                url: __WebAppPathPrefix + '/VMIProcess/OpenPOConfirmationExportExcel',
                data: {
                    PO_CFM: escape(ConfirmStatus),
                    PO_NUM_FM: escape(PONumberFrom),
                    PO_NUM_TO: escape(PONumberTo),
                    DELIV_DATE_FM: escape(DeliveryDateFrom),
                    DELIV_DATE_TO: escape(DeliveryDateTo),
                    PO_CFMDATE_FM: escape(ConfirmedDateFrom),
                    PO_CFMDATE_TO: escape(ConfirmedDateTo),
                    MATERIAL_FM: escape(MaterialFrom),
                    MATERIAL_TO: escape(MaterialTo),
                    MTLDESC: escape(MaterialDescription),
                    PLANT: escape(Plant),
                    VENDOR: escape(VendorCode)
                },
                type: "post",
                dataType: 'json',
                beforeSend: function () {
                    $("#dialogDownloadSplash").dialog({
                        title: 'Download Notify',
                        width: 'auto',
                        height: 'auto',
                        open: function (event, ui) {
                            $(this).parent().find('.ui-dialog-titlebar-close').hide();
                            $(this).parent().find('.ui-dialog-buttonpane').hide();
                            $("#lbDiaDownloadMsg").html('Downloading...</br></br>Please wait for the operation a moment...');
                        }
                    }).dialog("open");
                },
                success: function (data) {
                    if (data.Result) {
                        if (data.FileKey != "") {
                            $("#dialogDownloadSplash_FileKey").val(data.FileKey);
                            $("#dialogDownloadSplash_FileName").val(data.FileName);
                            setTimeout(function () {
                                $("#dialogDownloadSplash_Form").attr('action', __WebAppPathPrefix + '/VMIProcess/RetrieveFileByFileKey').submit();
                                $("#dialogDownloadSplash").dialog("close");
                            }, 10);
                        }
                    }
                    else {
                        $("#dialogDownloadSplash").dialog("close");
                        if (data.FileName != "" && data.FileName != null) {
                            alert(data.FileName);
                        }
                        else {
                            alert("There is no data or no authority of this vendor.");
                        }
                    }

                },
                error: function (xhr, textStatus, thrownError) {
                    $("#dialogDownloadSplash").dialog("close");
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                }
            });
        }
    });
});

function reloadOpenPOConfirmationGridList() {
    var Plant = $.trim($('#txtPlant').val());
    var VendorCode = $.trim($('#txtVendorCode').val());
    var PONumberFrom = $.trim($('#txtPONumberFrom').val());
    var PONumberTo = $.trim($('#txtPONumberTo').val());
    var DeliveryDateFrom = $.trim($('#txtDeliveryDateFrom').val());
    var DeliveryDateTo = $.trim($('#txtDeliveryDateTo').val());
    var ConfirmedDateFrom = $.trim($('#txtConfirmedDateFrom').val());
    var ConfirmedDateTo = $.trim($('#txtConfirmedDateTo').val());
    var MaterialFrom = $.trim($('#txtMaterialFrom').val());
    var MaterialTo = $.trim($('#txtMaterialTo').val());
    var MaterialDescription = $.trim($('#txtDesc').val());
    var ConfirmStatus = $.trim($('#ddlConfirmStatus').val());

    if (DeliveryDateFrom == '' || DeliveryDateTo == '') {
        alert('Please select Delivery Date(FM) and Delivery Date(TO) first.');
    }
    else if (dateDiff(DeliveryDateFrom, DeliveryDateTo) > 184) {
        alert('The interval of the Delivery Date is exceed 6 month, please reselect it under 6 month.');
    }
    else {
        var gridOpenPOConfirmation = $('#gridOpenPOConfirmation');

        gridOpenPOConfirmation.jqGrid('clearGridData');
        gridOpenPOConfirmation.jqGrid('setGridParam', {
            postData: {
                PO_CFM: escape(ConfirmStatus),
                PO_NUM_FM: escape(PONumberFrom),
                PO_NUM_TO: escape(PONumberTo),
                DELIV_DATE_FM: escape(DeliveryDateFrom),
                DELIV_DATE_TO: escape(DeliveryDateTo),
                PO_CFMDATE_FM: escape(ConfirmedDateFrom),
                PO_CFMDATE_TO: escape(ConfirmedDateTo),
                MATERIAL_FM: escape(MaterialFrom),
                MATERIAL_TO: escape(MaterialTo),
                MTLDESC: escape(MaterialDescription),
                PLANT: escape(Plant),
                VENDOR: escape(VendorCode)
            }
        });

        gridOpenPOConfirmation.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    }
}

function dateDiff(fromDate, toDate) {
    var date1 = new Date(fromDate);
    var date2 = new Date(toDate);
    var timeDiff = Math.abs(date2.getTime() - date1.getTime());
    var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
    return diffDays;
}