$(function () {
    var gridSQMHRDataList = $("#gridSQMHRDataList");
    var dialog = $("#dialogSQMHRData");

    jQuery("#btnSQMHRCreate").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridSQMHRDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            dialog.attr('Mode', "c");
            DialogSetUIByModeSQMHR(dialog.attr('Mode'));
            dialog.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnSQMHRViewEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridSQMHRDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            SetToViewModeSQMHR();
        }
    });

    jQuery("#btndialogSQMHREditData").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridSQMHRDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            var r = "ok";
            if (r == "ok") {
                dialog.attr('Mode', "e");
                DialogSetUIByModeSQMHR(dialog.attr('Mode'));
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

    jQuery("#btndialogSQMHRCancelEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = "ok";
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewModeSQMHR();
    });

    function SetToViewModeSQMHR()
    {
        var RowId = gridSQMHRDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            dialog.attr('Mode', "v");
            DialogSetUIByModeSQMHR(dialog.attr('Mode'));
            dialog.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnSQMHRDelete").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridSQMHRDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            var RowId = gridSQMHRDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridSQMHRDataList.jqGrid('getRowData', RowId)).HRType;
                var r = "ok";
                if (r == "ok") {
                    if (confirm("Confirm to delete selected member (" + gridSQMHRDataList.jqGrid('getRowData', RowId).HRType + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMHR/DeleteMember",
                            data: {
                                "BasicInfoGUID": gridSQMHRDataList.jqGrid('getRowData', RowId).BasicInfoGUID,
                                "HRType": gridSQMHRDataList.jqGrid('getRowData', RowId).HRType,
                                'CName': gridSQMHRDataList.jqGrid('getRowData', RowId).CName
                            },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSQMHRSearch").click();
                                    alert("HRType delete successfully.");
                                    LoadTotal();
                                }
                                else {
                                    alert("HRType delete fail due to:\n\n" + data);
                                }
                            },
                            error: function (xhr, textStatus, thrownError) {
                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                            },
                            complete: function (jqXHR, textStatus) {
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