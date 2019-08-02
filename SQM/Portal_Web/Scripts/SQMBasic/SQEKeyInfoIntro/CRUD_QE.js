$(function () {
    

    var gridDataList = $("#gridDataList" + "QE");
    var dialog = $("#dialogData" + "QE");

    $("#btnSearch" + "QE").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataList = $("#gridDataList" + "QE");
        gridDataList.jqGrid('clearGridData');

        var sMemberType = "";
        if ($('#radFilterMemberTypeInternalOnly').is(':checked'))
            sMemberType = "2";
        else if ($('#radFilterMemberTypeExternalOnly').is(':checked'))
            sMemberType = "1";

        gridDataList.jqGrid('setGridParam', { postData: { vendorCode: escape($("#ihBasicInfoGUID").val()) } })
        gridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });

    $("#btnCreate" + "QE").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            dialog.attr('Mode', "c");
            DialogSetUIByModeQE(dialog.attr('Mode'));
            dialog.dialog("option", "title", "Create").dialog("open");
        }
    });

    $("#btnViewEdit" + "QE").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            SetToViewMode();
        }
    });

    $("#btndialogEditData" + "QE").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            var r = "ok";//AcquireDataLock(dialog.attr('QEGUID'))
            if (r == "ok") {
                dialog.attr('Mode', "e");
                DialogSetUIByModeQE(dialog.attr('Mode'));
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

    $("#btndialogCancelEdit" + "QE").click(function () {
        $(this).removeClass('ui-state-focus');
        SetToViewMode();
    });

    function SetToViewMode()
    {
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            dialog.attr('Mode', "v");
            DialogSetUIByModeQE(dialog.attr('Mode'));
            dialog.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    $("#btnDelete" + "QE").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).QEGUID;
                var r = "ok";
                if (r == "ok") {
                    if (confirm("Confirm to delete selected Quality Event (" + gridDataList.jqGrid('getRowData', RowId).AccountID + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMBasic/DeleteQualityEvent",
                            data: { "QEGUID": gridDataList.jqGrid('getRowData', RowId).QEGUID },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSearch" + "QE").click();
                                    alert("QualityEvent delete successfully.");
                                }
                                else {
                                    alert("QualityEvent delete fail due to:\n\n" + data);
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
                        //var r = ReleaseDataLock(DataKey);
                        //if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
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