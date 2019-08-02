$(function () {
    var dialogFile = $("#dialogDataFile");
    var gridDataListFile = $("#gridDataListFile");

    jQuery("#btnBackFile").click(function () {
        gridDataListFile.jqGrid('clearGridData');

        $("#quality").show();
        $("#inspFile").hide();
        $("#tbMain1File").hide();

    });

    jQuery("#btnCreateFile").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListFile.jqGrid('getGridParam', 'multiselect')) {   //single select
            dialogFile.attr('Mode', "c");
            DialogSetUIByModeFile(dialogFile.attr('Mode'));
            dialogFile.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnViewEditFile").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListFile.jqGrid('getGridParam', 'multiselect')) {   //single select
            SetToViewModeFile();
        }
    });

    jQuery("#btndialogEditDataFile").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataListFile.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('VendorFile'))
            if (true) {
                dialogFile.attr('Mode', "e");
                DialogSetUIByModeFile(dialogFile.attr('Mode'));
                dialogFile.dialog("option", "title", "Edit").dialog("open");
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

    jQuery("#btndialogCancelEditFile").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialogFile.attr('BasicInfoGUID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewModeFile();
    });

    function SetToViewModeFile() {
        var RowIdFile = gridDataListFile.jqGrid('getGridParam', 'selrow');
        if (RowIdFile) {
            dialogFile.attr('Mode', "v");
            DialogSetUIByModeFile(dialogFile.attr('Mode'));
            dialogFile.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnDeleteFile").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListFile.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowIdFile = gridDataListFile.jqGrid('getGridParam', 'selrow');
            if (RowIdFile) {
                //var DataKey = (gridDataListFile.jqGrid('getRowData', RowId)).SubFuncGUID;
                //var r = AcquireDataLock(DataKey)
                if (true) {
                    if (confirm("Confirm to delete selected member (" + gridDataListFile.jqGrid('getRowData', RowIdFile).Name + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMReliability/DeleteQualityFile",
                            data: {
                                "ReportSID": gridDataListFile.jqGrid('getRowData', RowIdFile).ReportSID
                                , "DocNo": gridDataListFile.jqGrid('getRowData', RowIdFile).DocNo
                            },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSearchFile").click();
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

})