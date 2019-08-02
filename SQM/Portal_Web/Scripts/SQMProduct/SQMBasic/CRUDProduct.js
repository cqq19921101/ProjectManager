$(function () {
    var gridDataListP = $("#gridDataListP");
    var dialogP = $("#dialogDataP");

    jQuery("#btnCreateP").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListP.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            dialogP.attr('Mode', "c");
            DialogSetUIByModeP(dialogP.attr('Mode'));
            dialogP.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnViewEditP").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListP.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            SetToViewModeP();
        }
    });

    jQuery("#btndialogEditDataP").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataListP.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('VendorCode'))
            if (true) {
                dialogP.attr('Mode', "e");
                DialogSetUIByModeP(dialogP.attr('Mode'));
                dialogP.dialog("option", "title", "Edit").dialog("open");
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

    jQuery("#btndialogCancelEditP").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialogP.attr('BasicInfoGUID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewModeP();
    });

    function SetToViewModeP()
    {
        var RowIdP = gridDataListP.jqGrid('getGridParam', 'selrow');
        if (RowIdP) {
            dialogP.attr('Mode', "v");
            DialogSetUIByModeP(dialogP.attr('Mode'));
            dialogP.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnDeleteP").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListP.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            var RowIdP = gridDataListP.jqGrid('getGridParam', 'selrow');
            if (RowIdP) {
                //var DataKey = (gridDataListP.jqGrid('getRowData', RowId)).SubFuncGUID;
                //var r = AcquireDataLock(DataKey)
                if (true) {
                    if (confirm("Confirm to delete selected member (" + gridDataListP.jqGrid('getRowData', RowIdP).PrincipalProducts + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMProduct/DeletePro",
                            data: { "PrincipalProducts": gridDataListP.jqGrid('getRowData', RowIdP).PrincipalProducts },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSearchP").click();
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
                        var rP = ReleaseDataLock(DataKey);
                        if (rP == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
                    }
                }
                else {
                    switch (rP) {
                        case "timeout": $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); break;
                        case "l": alert("Data already lock by other user."); break;
                        default: alert("Data lock fail or application error."); break;
                    }
                }
            } else { alert("Please select a row data to delete."); }
        }
    });
});