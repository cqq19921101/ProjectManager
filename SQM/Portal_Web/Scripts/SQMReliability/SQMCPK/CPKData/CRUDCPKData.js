$(function () {
    var dialogCPKData = $("#dialogDataCPKData");
    var gridDataListCPKData = $("#gridDataListCPKData");
    jQuery("#btnBackCPKData").click(function () {
        $(this).removeClass('ui-state-focus');
        gridDataListCPKData.jqGrid('clearGridData');

        $('#inspCPKSub').show();
        $('#inspCPKData').hide();
        $('#tbMain1CPKData').hide();

    });

    jQuery("#btnCreateCPKData").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListCPKData.jqGrid('getGridParam', 'multiselect')) {   //single select
            dialogCPKData.attr('Mode', "c");
            DialogSetUIByModeCPKData(dialogCPKData.attr('Mode'));
            dialogCPKData.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnViewEditCPKData").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListCPKData.jqGrid('getGridParam', 'multiselect')) {   //single select
            SetToViewModeCPKData();
        }
    });

    jQuery("#btndialogEditDataCPKData").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataListCPKData.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('VendorCPK'))
            if (true) {
                dialogCPKData.attr('Mode', "e");
                DialogSetUIByModeCPKData(dialogCPKData.attr('Mode'));
                dialogCPKData.dialog("option", "title", "Edit").dialog("open");
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

    jQuery("#btndialogCancelEditCPKData").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialogCPKData.attr('plantCode'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewModeCPKData();
    });

    function SetToViewModeCPKData() {
        var RowIdCPK = gridDataListCPKData.jqGrid('getGridParam', 'selrow');
        if (RowIdCPK) {
            dialogCPKData.attr('Mode', "v");
            DialogSetUIByModeCPKData(dialogCPKData.attr('Mode'));
            dialogCPKData.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnDeleteCPKData").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListCPKData.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowIdCPK = gridDataListCPKData.jqGrid('getGridParam', 'selrow');
            if (RowIdCPK) {
                //var DataKey = (gridDataListCPKData.jqGrid('getRowData', RowId)).SubFuncGUID;
                //var r = AcquireDataLock(DataKey)
                if (true) {
                    if (confirm("Confirm to delete selected member (" + gridDataListCPKData.jqGrid('getRowData', RowIdCPK).Name + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMReliability/DeleteCPKData",
                            data: {
                                "plantCode": gridDataListCPKData.jqGrid('getRowData', RowIdCPK).plantCode
                                , "Designator": gridDataListCPKData.jqGrid('getRowData', RowIdCPK).Designator
                            },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSearchCPKData").click();
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