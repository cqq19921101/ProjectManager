$(function () {
    var gridDataList = $("#SQMContactgridDataList");
    var dialog = $("#SQMContactdialogData");

    jQuery("#btnSQMContactCreate").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            dialog.attr('Mode', "c");
            ContactDialogSetUIByMode(dialog.attr('Mode'));
            dialog.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnSQMContactViewEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            SetToContactViewMode();
        }
    });

    jQuery("#btnSQMContactdialogEditData").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('SID'))
            
            //if (r == "ok") {
            if (true) {                
                dialog.attr('Mode', "e");
                ContactDialogSetUIByMode(dialog.attr('Mode'));
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

    jQuery("#btnSQMContactdialogCancelEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialog.attr('SID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToContactViewMode();
    });

    function SetToContactViewMode()
    {
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            dialog.attr('Mode', "v");
            ContactDialogSetUIByMode(dialog.attr('Mode'));
            dialog.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnSQMContactDelete").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).Vendor;
                //var r = AcquireDataLock(DataKey)
                
                //if (r == "ok") {
                if (true) {
                    if (confirm("Confirm to delete selected member (" + gridDataList.jqGrid('getRowData', RowId).SID + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMBasic/DeleteContact",
                            data: { "SID": gridDataList.jqGrid('getRowData', RowId).SID },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSQMContactSearch").click();
                                    alert("Contact delete successfully.");
                                }
                                else {
                                    alert("Contact delete fail due to:\n\n" + data);
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