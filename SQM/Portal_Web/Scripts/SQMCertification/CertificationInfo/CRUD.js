$(function () {
    var gridSQMCertificationDataList = $("#gridSQMCertificationDataList");
    var dialog = $("#dialogSQMCertificationData");

    jQuery("#btnSQMCertificationCreate").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridSQMCertificationDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            dialog.attr('Mode', "c");
            DialogSetUIByMode(dialog.attr('Mode'));
            dialog.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnSQMCertificationViewEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridSQMCertificationDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            SetToViewMode();
        }
    });

    jQuery("#btndialogSQMCertificationEditData").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridSQMCertificationDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            var r = "ok";
            if (r == "ok") {
                dialog.attr('Mode', "e");
                DialogSetUIByMode(dialog.attr('Mode'));
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

    jQuery("#btndialogSQMCertificationCancelEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = "ok";
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewMode();
    });

    function SetToViewMode()
    {
        var RowId = gridSQMCertificationDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            dialog.attr('Mode', "v");
            DialogSetUIByMode(dialog.attr('Mode'));
            dialog.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnSQMCertificationDelete").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridSQMCertificationDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            var RowId = gridSQMCertificationDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridSQMCertificationDataList.jqGrid('getRowData', RowId)).CertificationType;
                var r = "ok";
                if (r == "ok") {
                    if (confirm("Confirm to delete selected member (" + gridSQMCertificationDataList.jqGrid('getRowData', RowId).CertificationType + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMCertification/DeleteMember",
                            data: {
                                "BasicInfoGUID": gridSQMCertificationDataList.jqGrid('getRowData', RowId).BasicInfoGUID,
                                "CertificationType": gridSQMCertificationDataList.jqGrid('getRowData', RowId).CertificationType,
                                'CName': gridSQMCertificationDataList.jqGrid('getRowData', RowId).CName
                            },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSQMCertificationSearch").click();
                                    alert("CertificationType delete successfully.");
                                    loadTypeList();
                                }
                                else {
                                    alert("CertificationType delete fail due to:\n\n" + data);
                                }
                            },
                            error: function (xhr, textStatus, thrownError) {
                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                            },
                            complete: function (jqXHR, textStatus) {
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
            } else { alert("Please select a row data to delete."); }
        }
    });
});