$(function () {
    var gridDataListT = $("#gridDataListT");
    var dialog = $("#dialogDataT");

       jQuery("#btnCreateT").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListT.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            dialog.attr('Mode', "c");
            DialogSetUIByModeT(dialog.attr('Mode'));
            dialog.dialog("option", "title", "Create").dialog("open");
        }

    });

    jQuery("#btnViewEditT").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListT.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            SetToViewMode();
        }
    });

    jQuery("#btndialogEditDataT").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataListT.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('VendorCode'))
            if (true) {
                dialog.attr('Mode', "e");
                DialogSetUIByModeT(dialog.attr('Mode'));
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

    jQuery("#btndialogCancelEditT").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialog.attr('BasicInfoGUID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewMode();
    });

    function SetToViewMode()
    {
        var RowId = gridDataListT.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            dialog.attr('Mode', "v");
            DialogSetUIByModeT(dialog.attr('Mode'));
            dialog.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnDeleteT").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListT.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            var RowId = gridDataListT.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                //var DataKey = (gridDataListT.jqGrid('getRowData', RowId)).SubFuncGUID;
                //var r = AcquireDataLock(DataKey)
                if (true) {
                    if (confirm("Confirm to delete selected member (" + gridDataListT.jqGrid('getRowData', RowId).PrincipalProducts + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMTraders/DeleteTraders",
                            data: { "PrincipalProducts": gridDataListT.jqGrid('getRowData', RowId).PrincipalProducts },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSearchT").click();
                                    alert("PrincipalProducts delete successfully.");
                                }
                                else {
                                    alert("PrincipalProducts delete fail due to:\n\n" + data);
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