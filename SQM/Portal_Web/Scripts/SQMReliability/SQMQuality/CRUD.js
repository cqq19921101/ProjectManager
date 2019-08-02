$(function () {
    var gridDataList = $("#gridDataList");
    var dialog = $("#dialogData");

    jQuery("#btnCreate").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            dialog.attr('Mode', "c");
            DialogSetUIByMode(dialog.attr('Mode'));
            dialog.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnViewEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            SetToViewMode();
        }
    });

    jQuery("#btndialogEditData").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            var r = AcquireDataLock(dialog.attr('ReportSID'))
            if (r == "ok") {
                dialog.attr('Mode', "e");
                DialogSetUIByMode(dialog.attr('Mode'));
                dialog.dialog("option", "title", "Edit").dialog("open");
            }
            else {
                switch (r) {
                    case "timeout": $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); break;
                    case "l": alert("Data already lock by other user."); break;
                    default: alert("Data lock fail or application error."); break;
                }
            }
        } else { alert("Please select a row data to edit."); }
    });

    jQuery("#btndialogCancelEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialog.attr('ReportSID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewMode();
    });
     
    jQuery("#txtisChange").change(function () {
        if ($(this).val() == "1") {
            $("#trChangeNote").show();
        } else {
            $("#trChangeNote").hide();
        }
    });

    function SetToViewMode()
    {
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            dialog.attr('Mode', "v");
            DialogSetUIByMode(dialog.attr('Mode'));
            dialog.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnDelete").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).ReportSID;
                //var r = AcquireDataLock(DataKey)
                //if (r == "ok") {
                    if (confirm("Confirm to delete selected member (" + gridDataList.jqGrid('getRowData', RowId).ReportSID + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMReliability/DeleteQuality",
                            data: { "ReportSID": gridDataList.jqGrid('getRowData', RowId).ReportSID },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSearch").click();
                                    alert("Quality delete successfully.");
                                }
                                else {
                                    alert("Quality delete fail due to:\n\n" + data);
                                }
                            },
                            error: function (xhr, textStatus, thrownError) {
                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                            },
                            complete: function (jqXHR, textStatus) {
                                //$("#ajaxLoading").hide();
                            }
                        });
                    //}
                    //else {
                    //    var r = ReleaseDataLock(DataKey);
                    //    if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
                    //}
                //}
                //else {
                //    switch (r) {
                //        case "timeout": $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); break;
                //        case "l": alert("Data already lock by other user."); break;
                //        default: alert("Data lock fail or application error."); break;
                //    }
                }
            } else { alert("Please select a row data to delete."); }
        }
    });
});