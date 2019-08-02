$(function () {
    var gridDataList = $("#SQMSQEPURgridDataList");
    var dialog = $("#SQMSQEPURdialogData");

    jQuery("#btnSQMSQEPURCreate").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            dialog.attr('Mode', "c");
            SQMSQEDialogSetUIByMode(dialog.attr('Mode'));
            dialog.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnSQMSQEPUREdit").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            dialog.attr('Mode', "e");
            SQMSQEDialogSetUIByMode(dialog.attr('Mode'));
            dialog.dialog("option", "title", "Edit").dialog("open");
        }
    });





    jQuery("#btnSQMSQEPURdialogCancelEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialog.attr('SID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToPlantViewMode();
    });



    jQuery("#btnSQMSQEPURDelete").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                //var DataKey = (gridDataList.jqGrid('getRowData', RowId)).Vendor;
                //var r = AcquireDataLock(DataKey)
                
                //if (r == "ok") {
                if (true) {
                    if (confirm("Confirm to delete selected member (" + gridDataList.jqGrid('getRowData', RowId).VendorCode + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMSQEPUR/DeleteSQMSQEPUR",
                            data: { "VendorCode": gridDataList.jqGrid('getRowData', RowId).VendorCode },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSQMSQEPURSearch").click();
                                    alert("VendorCode delete successfully.");
                                }
                                else {
                                    alert("VendorCode delete fail due to:\n\n" + data);
                                }
                            },
                            error: function (xhr, textStatus, thrownError) {
                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                            },
                            complete: function (jqXHR, textStatus) {
                                //$("#ajaxLoading").hide();
                            }
                        });
                    }
                    else {
                        var r = ReleaseDataLock(DataKey);
                        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
                    }
                }
                else {
                    switch (r) {
                        case "timeout": $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); break;
                        case "l": alert("Data already lock by other user."); break;
                        default: alert("Data lock fail or application error."); break;
                    }
                }
            } else { alert("Please select a row data to delete."); }
        }
    });

});