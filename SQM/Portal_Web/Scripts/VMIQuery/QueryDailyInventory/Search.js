$(function () {
    var diaPlant = $('#dialog_VMI_QueryPlantInfo');
    var diaSBUVendor = $('#dialog_VMI_QuerySBUVendor');

    $('#btnQueryDailyInventory').click(function () {
        $(this).removeClass('ui-state-focus');
        ReloadQueryDailyInventoryListgridDataList();
    });

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

    $('#btnExportExcel').click(function () {
        $(this).removeClass('ui-state-focus');
        $.ajax({
            url: __WebAppPathPrefix + '/VMIQuery/QueryDailyInventoryExportExcel',
            data: {
                plant: escape($.trim($("#tdPlant").text())),
                vendor: escape($.trim($("#tdVendorCode").text())),
                vendorName: $.trim($("#tdVendorName").text()),
                fromMaterial: escape($.trim($("#txtMaterialFrom").val())),
                toMaterial: escape($.trim($("#txtMaterialTo").val()))
            },
            type: "post",
            dataType: 'json',
            //async: false,
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
                            $("#dialogDownloadSplash_Form").attr('action', __WebAppPathPrefix + '/VMIQuery/RetrieveFileByFileKey').submit();
                            $("#dialogDownloadSplash").dialog("close");
                        }, 10);
                    }
                }
                else
                    alert("Export failure. Please contact administrator manager.");
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            }
        });
    });
});

function ReloadQueryDailyInventoryListgridDataList() {
    var gridQueryDailyInventory = $('#gridQueryDailyInventory');

    gridQueryDailyInventory.jqGrid('clearGridData');
    gridQueryDailyInventory.jqGrid('setGridParam', {
        postData: {
            plant: escape($.trim($("#txtPlant").val())),
            vendor: escape($.trim($("#txtVendorCode").val())),
            fromMaterial: escape($.trim($("#txtMaterialFrom").val())),
            toMaterial: escape($.trim($("#txtMaterialTo").val()))
        }
    });

    gridQueryDailyInventory.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}