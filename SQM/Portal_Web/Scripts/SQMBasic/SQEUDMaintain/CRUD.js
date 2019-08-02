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
            //var r = AcquireDataLock(dialog.attr('SID'))
            //if (r == "ok") {
                dialog.attr('Mode', "e");
                DialogSetUIByMode(dialog.attr('Mode'));
                dialog.dialog("option", "title", "Edit").dialog("open");
            //}
            //else {
            //    switch (r) {
            //        case "timeout": $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); break;
            //        case "l": alert("Data already lock by other user."); break;
            //        default: alert("Data lock fail or application error."); break;
            //    }
            //}
        } else { alert("Please select a row data to edit."); }
    });

    jQuery("#btndialogCancelEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialog.attr('SID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewMode();
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
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).SID;
                //var r = AcquireDataLock(DataKey)
                //if (r == "ok") {
                if (confirm("Confirm to delete selected member (" + gridDataList.jqGrid('getRowData', RowId).UD + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMBasic/DeleteUD",
                            data: { "SID": gridDataList.jqGrid('getRowData', RowId).SID },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSearch").click();
                                    alert(" delete successfully.");
                                }
                                else {
                                    alert(" delete fail due to:\n\n" + data);
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
                //}
                //else {
                //    switch (r) {
                //        case "timeout": $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); break;
                //        case "l": alert("Data already lock by other user."); break;
                //        default: alert("Data lock fail or application error."); break;
                //    }
                //}
            } else { alert("Please select a row data to delete."); }
        }
    });
    jQuery("#btnCommit").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = gridDataList.jqGrid('getRowData', RowId).MemberGUID;
                //var r = AcquireDataLock(DataKey)

                //if (r == "ok") {
                if (true) {
                    if (confirm("Confirm to submit selected?")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMReliability/ECNCommit",
                            data: {
                                "SID": gridDataList.jqGrid('getRowData', RowId).SID,
                                "MemberGUID": DataKey,
                                "ChangeItemTypeSID": gridDataList.jqGrid('getRowData', RowId).ChangeItemTypeSID
                            },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    alert("Commit successfully.");
                                    $("#btnReliabilitySearch").click();
                                    $("#btnCommitReliability").hide();
                                }
                                else {
                                    alert(data);
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
            } else { alert("Please select a row data to export."); }
        }
    });
});