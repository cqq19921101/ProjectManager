$(function () {
    var dialogDesc = $("#dialogDataDesc");
    var gridDataListDesc = $("#gridDataListDesc");

    jQuery("#btnBack").click(function () {
        $(this).removeClass('ui-state-focus');
        gridDataListDesc.jqGrid('clearGridData');

        $('#inspCode').show();
        $('#inspDesc').hide();
        $('#tbMain1Desc').hide();

    });

    jQuery("#btnCreateDesc").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListDesc.jqGrid('getGridParam', 'multiselect')) {   //single select
            dialogDesc.attr('Mode', "c");
            DialogSetUIByModeDesc(dialogDesc.attr('Mode'));
            dialogDesc.dialog("option", "title", "Create").dialog("open");
            Changedddl();
        }
    });

    jQuery("#btnViewEditDesc").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListDesc.jqGrid('getGridParam', 'multiselect')) {   //single select
            SetToViewModeDesc();
        }
    });

    jQuery("#btndialogEditDataDesc").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataListDesc.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('VendorCode'))
            if (true) {
                dialogDesc.attr('Mode', "e");
                DialogSetUIByModeDesc(dialogDesc.attr('Mode'));
                dialogDesc.dialog("option", "title", "Edit").dialog("open");
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

    jQuery("#btndialogCancelEditDesc").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialogDesc.attr('SID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewModeDesc();
    });

    function SetToViewModeDesc() {
        var RowIdCode = gridDataListDesc.jqGrid('getGridParam', 'selrow');
        if (RowIdCode) {
            dialogDesc.attr('Mode', "v");
            DialogSetUIByModeDesc(dialogDesc.attr('Mode'));
            dialogDesc.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnDeleteDesc").click(function () {
        $(this).removeClass('ui-state-focus');
        DelItem($("#gridDataListDesc"));
    });
    jQuery("#btnDeleteDescVar").click(function () {
        $(this).removeClass('ui-state-focus');
        DelItem($("#gridDataListDescVariables"));
    });
})
function DelItem(gridDataListDesc) {
    $(this).removeClass('ui-state-focus');
    if (!gridDataListDesc.jqGrid('getGridParam', 'multiselect')) {   //single select
        var RowIdCode = gridDataListDesc.jqGrid('getGridParam', 'selrow');
        if (RowIdCode) {
            var DataKey = gridDataListDesc.jqGrid('getRowData', RowIdCode).SID
            //var r = AcquireDataLock(DataKey)
            if (true) {
                if (confirm("Confirm to delete selected member (" + gridDataListDesc.jqGrid('getRowData', RowIdCode).InspDesc + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                    $.ajax({
                        url: __WebAppPathPrefix + "/SQMBasic/DeleteInspMap",
                        data: {
                            "SID": gridDataListDesc.jqGrid('getRowData', RowIdCode).SID
                            , "SSID": gridDataListDesc.jqGrid('getRowData', RowIdCode).SSID
                            , "Insptype":escape($("#dialogDataCode").attr('Insptype'))
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                $("#btnSearchDesc").click();
                                $("#btnSearchDescVar").click();
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
}