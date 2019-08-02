$(function () {
    var diaPlant = $('#dialog_VMI_QueryPlantInfo');
    var diaSBUVendor = $('#dialog_VMI_QuerySBUVendor');

    $('#btnOpenQueryFromPlantDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtFromPlant';

            InitdialogPlant();
            ReloadDiaPlantCodegridDataList();

            diaPlant.show();
            diaPlant.dialog("open");
        }
    });

    $('#btnOpenQueryToPlantDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtToPlant';

            InitdialogPlant();
            ReloadDiaPlantCodegridDataList();

            diaPlant.show();
            diaPlant.dialog("open");
        }
    });

    $('#btnOpenQueryFromVendorCodeDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtFromVendorCode';

            InitdialogSBUVendor();
            ReloadDiaSBUVendorCodegridDataList();

            diaSBUVendor.show();
            diaSBUVendor.dialog("open");
        }
    });

    $('#btnOpenQueryToVendorCodeDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtToVendorCode';

            InitdialogSBUVendor();
            ReloadDiaSBUVendorCodegridDataList();

            diaSBUVendor.show();
            diaSBUVendor.dialog("open");
        }
    });

    $('#btnExportToExcel').click(function () {
        $(this).removeClass('ui-state-focus');
        var fromDate = $('#fromGRDate').val();
        var toDate = $('#toGRDate').val();
        if (dateDiff(fromDate, toDate) > 92) {
            alert('The interval of the GRDate is exceed 92 days, please reselect it under 92 days.');
        }
        else if (fromDate == '' || toDate == '') {
            alert('Please select GRDate(FM) and GRDate(TO) first.');
        }
        else {
            $.ajax({
                url: __WebAppPathPrefix + '/VMIQuery/GRReportExportExcel',
                data: {
                    fromPlant: escape($.trim($("#txtFromPlant").val())),
                    toPlant: escape($.trim($("#txtToPlant").val())),
                    fromVendorCode: escape($.trim($("#txtFromVendorCode").val())),
                    toVendorCode: escape($.trim($("#txtToVendorCode").val())),
                    fromMaterial: escape($.trim($("#txtFromMaterial").val())),
                    toMaterial: escape($.trim($("#txtToMaterial").val())),
                    fromGRDate: escape($.trim($("#fromGRDate").val())),
                    toGRDate: escape($.trim($("#toGRDate").val()))
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
                                $("#dialogDownloadSplash_Form").attr('action', __WebAppPathPrefix + '/VMIQuery/RetrieveFileByFileKey').submit();
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

function dateDiff(fromDate, toDate) {
    var date1 = new Date(fromDate);
    var date2 = new Date(toDate);
    var timeDiff = Math.abs(date2.getTime() - date1.getTime());
    var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
    return diffDays;
}