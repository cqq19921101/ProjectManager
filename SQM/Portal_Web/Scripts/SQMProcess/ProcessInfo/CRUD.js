$(function () {
    var gridSQMProcessDataList = $("#gridSQMProcessDataList");
    var dialog = $("#dialogSQMProcessData");

    jQuery("#btnSQMProcessCreate").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridSQMProcessDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            dialog.attr('Mode', "c");
            DialogSetUIByModeSQMProcess(dialog.attr('Mode'));
            dialog.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnSQMProcessViewEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridSQMProcessDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            SetToViewModeSQMProcess();
        }
    });

    jQuery("#btndialogSQMProcessEditData").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridSQMProcessDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            var r = "ok";
            if (r == "ok") {
                dialog.attr('Mode', "e");
                DialogSetUIByModeSQMProcess(dialog.attr('Mode'));
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

    jQuery("#btndialogSQMProcessCancelEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = "ok";
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewModeSQMProcess();
    });

    function SetToViewModeSQMProcess()
    {
        var RowId = gridSQMProcessDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            dialog.attr('Mode', "v");
            DialogSetUIByModeSQMProcess(dialog.attr('Mode'));
            dialog.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnSQMProcessDelete").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridSQMProcessDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            var RowId = gridSQMProcessDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridSQMProcessDataList.jqGrid('getRowData', RowId)).ProcessType;
                var r = "ok";
                if (r == "ok") {
                    if (confirm("Confirm to delete selected member (" + gridSQMProcessDataList.jqGrid('getRowData', RowId).ProcessType + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMProcess/DeleteMember",
                            data: {
                                "BasicInfoGUID": gridSQMProcessDataList.jqGrid('getRowData', RowId).BasicInfoGUID,
                                "ProcessType": gridSQMProcessDataList.jqGrid('getRowData', RowId).ProcessType,
                                'CName': gridSQMProcessDataList.jqGrid('getRowData', RowId).CName
                            },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSQMProcessSearch").click();
                                    alert("ProcessType delete successfully.");
                                }
                                else {
                                    alert("ProcessType delete fail due to:\n\n" + data);
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