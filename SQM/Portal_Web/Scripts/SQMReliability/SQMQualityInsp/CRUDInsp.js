$(function () {
    var dialogInsp = $("#dialogDataInsp");
    var gridDataListInsp = $("#gridDataListInsp");
    var gridDataListInspVar = $("#gridDataListInspVar");

    jQuery("#btnBackInsp").click(function () {
        $(this).removeClass('ui-state-focus');
        gridDataListInsp.jqGrid('clearGridData');

        $("#quality").show();
        $("#inspInsp").hide();
        $("#tbMain1Insp").hide();
        $("#inspInspVar").hide();
        $("#tbMain1InspVar").hide();

        $("#ddlSInspCode  option:first").prop("selected", 'selected').change();
        $("#ddlSInspCodeVar  option:first").prop("selected", 'selected').change();
    });

    jQuery("#btnCreateInsp").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListInsp.jqGrid('getGridParam', 'multiselect')) {   //single select
            dialogInsp.attr('Mode', "c");
            DialogSetUIByModeInsp(dialogInsp.attr('Mode'));
            dialogInsp.dialog("option", "title", "Create").dialog("open");
            InspDescChange();
        }
    });

    jQuery("#btnViewEditInsp").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListInsp.jqGrid('getGridParam', 'multiselect')) {   //single select
            SetToViewModeInsp();
        }
    });

    jQuery("#btndialogEditDataInsp").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataListInsp.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('VendorInsp'))
            if (true) {
                dialogInsp.attr('Mode', "e");
                DialogSetUIByModeInsp(dialogInsp.attr('Mode'));
                dialogInsp.dialog("option", "title", "Edit").dialog("open");
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

    jQuery("#btndialogCancelEditInsp").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialogInsp.attr('BasicInfoGUID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewModeInsp();
    });

    function SetToViewModeInsp() {
        var RowIdInsp = gridDataListInsp.jqGrid('getGridParam', 'selrow');
        if (RowIdInsp) {
            dialogInsp.attr('Mode', "v");
            DialogSetUIByModeInsp(dialogInsp.attr('Mode'));
            dialogInsp.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnDeleteInsp").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListInsp.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowIdInsp = gridDataListInsp.jqGrid('getGridParam', 'selrow');
            if (RowIdInsp) {
                var DataKey =  gridDataListInsp.jqGrid('getRowData', RowIdInsp).ReportSID;
                var r = AcquireDataLock(DataKey)
                if (true) {
                    if (confirm("Confirm to delete selected member (" + gridDataListInsp.jqGrid('getRowData', RowIdInsp).InspCode + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMReliability/DeleteQualityInsp",
                            data: {
                                "ReportSID": gridDataListInsp.jqGrid('getRowData', RowIdInsp).ReportSID
                                , "InspCodeID": gridDataListInsp.jqGrid('getRowData', RowIdInsp).CodeSID
                                , "InspDescID": gridDataListInsp.jqGrid('getRowData', RowIdInsp).DescSID
                                , "Judge": gridDataListInsp.jqGrid('getRowData', RowIdInsp).Judge
                                , "InspResult": gridDataListInsp.jqGrid('getRowData', RowIdInsp).InspResult
                            },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSearchInsp").click();
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