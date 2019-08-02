$(function () {
    var dialogInspVar = $("#dialogDataInspVar");
    var gridDataListInspVar = $("#gridDataListInspVar");

    jQuery("#btnViewEditInspVar").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListInspVar.jqGrid('getGridParam', 'multiselect')) {   //single select
            SetToViewModeInspVar();
        }
    });

    jQuery("#btndialogEditDataInspVar").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataListInspVar.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('VendorInsp'))
            if (true) {
                dialogInspVar.attr('Mode', "e");
                DialogSetUIByModeInspVar(dialogInspVar.attr('Mode'));
                dialogInspVar.dialog("option", "title", "Edit").dialog("open");
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

    jQuery("#btndialogCancelEditInspVar").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialogInspVar.attr('BasicInfoGUID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewModeInspVar();
    });

    function SetToViewModeInspVar() {
        var RowIdInsp = gridDataListInspVar.jqGrid('getGridParam', 'selrow');
        if (RowIdInsp) {
            dialogInspVar.attr('Mode', "v");
            DialogSetUIByModeInspVar(dialogInspVar.attr('Mode'));
            dialogInspVar.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }
    jQuery("#btnDeleteInspVar").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListInspVar.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowIdInsp = gridDataListInspVar.jqGrid('getGridParam', 'selrow');
            if (RowIdInsp) {
                var DataKey = gridDataListInspVar.jqGrid('getRowData', RowIdInsp).ReportSID;
                //var r = AcquireDataLock(DataKey)
                if (true) {
                    if (confirm("Confirm to delete selected member (" + gridDataListInspVar.jqGrid('getRowData', RowIdInsp).InspCode + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMReliability/DeleteQualityInsp",
                            data: {
                                "ReportSID": gridDataListInspVar.jqGrid('getRowData', RowIdInsp).ReportSID
                                , "InspCodeID": gridDataListInspVar.jqGrid('getRowData', RowIdInsp).InspCodeID
                                 , "InspDescID": gridDataListInspVar.jqGrid('getRowData', RowIdInsp).InspDescID
                            },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSearchInspVar").click();
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