$(function () {
    var dialogMap = $("#dialogDataMap");
    var gridDataListMap = $("#gridDataListMap");

    jQuery("#btnBack").click(function () {
        $(this).removeClass('ui-state-focus');
        gridDataListMap.jqGrid('clearGridData');

        $('#div').show();
        $('#divMap').hide();
        $('#tbMain1Map').hide();

    });

    jQuery("#btnCreateMap").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListMap.jqGrid('getGridParam', 'multiselect')) {   //single select
            dialogMap.attr('Mode', "c");
            DialogSetUIByModeMap(dialogMap.attr('Mode'));
            dialogMap.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnViewEditMap").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListMap.jqGrid('getGridParam', 'multiselect')) {   //single select
            SetToViewModeMap();
        }
    });

    jQuery("#btndialogEditDataMap").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataListMap.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('VendorCode'))
            if (true) {
                dialogMap.attr('Mode', "e");
                DialogSetUIByModeMap(dialogMap.attr('Mode'));
                dialogMap.dialog("option", "title", "Edit").dialog("open");
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

    jQuery("#btndialogCancelEditMap").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialogMap.attr('SID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewModeMap();
    });

    function SetToViewModeMap() {
        var RowIdCode = gridDataListMap.jqGrid('getGridParam', 'selrow');
        if (RowIdCode) {
            dialogMap.attr('Mode', "v");
            DialogSetUIByModeMap(dialogMap.attr('Mode'));
            dialogMap.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnDeleteMap").click(function () {
        $(this).removeClass('ui-state-focus');
        DelItem($("#gridDataListMap"));
    });
    jQuery("#btnDeleteMapVar").click(function () {
        $(this).removeClass('ui-state-focus');
        DelItem($("#gridDataListMapVariables"));
    });
})
function DelItem(gridDataListMap) {
    $(this).removeClass('ui-state-focus');
    if (!gridDataListMap.jqGrid('getGridParam', 'multiselect')) {   //single select
        var RowIdCode = gridDataListMap.jqGrid('getGridParam', 'selrow');
        if (RowIdCode) {
            var DataKey = gridDataListMap.jqGrid('getRowData', RowIdCode).SID
            //var r = AcquireDataLock(DataKey)
            if (true) {
                if (confirm("Confirm to delete selected member (" + gridDataListMap.jqGrid('getRowData', RowIdCode).SID + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                    $.ajax({
                        url: __WebAppPathPrefix + "/SQMBasic/DeleteAQLPlantMap",
                        data: {
                            "SID": gridDataListMap.jqGrid('getRowData', RowIdCode).SID
                            , "SSID": gridDataListMap.jqGrid('getRowData', RowIdCode).SSID
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                $("#btnSearchMap").click();
                                $("#btnSearchMapVar").click();
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
                            ReflashMap();
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
}