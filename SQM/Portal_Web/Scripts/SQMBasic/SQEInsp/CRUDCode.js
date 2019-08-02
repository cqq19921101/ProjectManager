$(function () {
    var dialogCode = $("#dialogDataCode");
    var gridDataListCode = $("#gridDataListCode");
    var gridDataListDesc = $("#gridDataListDesc");
    var gridDataListDescVariables = $("#gridDataListDescVariables");
    jQuery("#btnShowDesc").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataListCode.jqGrid('getGridParam', 'selrow');
        if (RowId) {   //single select
            var dataRow = gridDataListCode.jqGrid('getRowData', RowId);
            if (dataRow.Insptype == 'Attributes') {
                gridDataListDesc.jqGrid('clearGridData');
                gridDataListDesc.jqGrid('setGridParam', { postData: { SID: dataRow.SID } })
                gridDataListDesc.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                $('#inspCode').hide();
                $('#inspDesc').show();
                $('#tbMain1Desc').show();
            } else if (dataRow.Insptype == 'Variables') {
                gridDataListDescVariables.jqGrid('clearGridData');
                gridDataListDescVariables.jqGrid('setGridParam', { postData: { SID: dataRow.SID } })
                gridDataListDescVariables.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                $('#inspCode').hide();
                $('#inspDescVar').show();
                $('#tbMain1DescVar').show();
            }
            $("#dialogDataCode").attr('Insptype', dataRow.Insptype);
            $("#dialogDataCode").attr('SID', dataRow.SID);
            $("#spInspCode").html(dataRow.Name)

        } else { alert("Please select a row data to show."); }

    });

    jQuery("#btnCreateCode").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListCode.jqGrid('getGridParam', 'multiselect')) {   //single select
            dialogCode.attr('Mode', "c");
            DialogSetUIByModeCode(dialogCode.attr('Mode'));
            dialogCode.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnViewEditCode").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListCode.jqGrid('getGridParam', 'multiselect')) {   //single select
            SetToViewModeCode();
        }
    });

    jQuery("#btndialogEditDataCode").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataListCode.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('VendorCode'))
            if (true) {
                dialogCode.attr('Mode', "e");
                DialogSetUIByModeCode(dialogCode.attr('Mode'));
                dialogCode.dialog("option", "title", "Edit").dialog("open");
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

    jQuery("#btndialogCancelEditCode").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialogCode.attr('BasicInfoGUID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewModeCode();
    });

    function SetToViewModeCode() {
        var RowIdCode = gridDataListCode.jqGrid('getGridParam', 'selrow');
        if (RowIdCode) {
            dialogCode.attr('Mode', "v");
            DialogSetUIByModeCode(dialogCode.attr('Mode'));
            dialogCode.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnDeleteCode").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListCode.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowIdCode = gridDataListCode.jqGrid('getGridParam', 'selrow');
            if (RowIdCode) {
                //var DataKey = (gridDataListCode.jqGrid('getRowData', RowId)).SubFuncGUID;
                //var r = AcquireDataLock(DataKey)
                if (true) {
                    if (confirm("Confirm to delete selected member (" + gridDataListCode.jqGrid('getRowData', RowIdCode).Name + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMBasic/DeleteInsp",
                            data: { "SID": gridDataListCode.jqGrid('getRowData', RowIdCode).SID },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSearchCode").click();
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