$(function () {
    var gridDataList = $("#gridDataListBasicInfoType");
    var dialog = $("#dialogDataBasicInfoType");

    $("#btnCreateBasicInfoType").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            dialog.attr('Mode', "c");
            DialogSetUIByMode_BasicInfoType(dialog.attr('Mode'));
            dialog.dialog("option", "title", "Create").dialog("open");
        }
    });

    $("#btnViewEditBasicInfoType").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            SetToViewMode_BasicInfoType();
        }
    });

    $("#btndialogEditDataBasicInfoType").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('BasicInfoTypeGUID'));
            var r = "ok";
            if (r == "ok") {
                dialog.attr('Mode', "e");
                DialogSetUIByMode_BasicInfoType(dialog.attr('Mode'));
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

    $("#btndialogCancelEditBasicInfoType").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialog.attr('BasicInfoTypeGUID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewMode_BasicInfoType();
    });

    function SetToViewMode_BasicInfoType()
    {
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            dialog.attr('Mode', "v");
            DialogSetUIByMode_BasicInfoType(dialog.attr('Mode'));
            dialog.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    $("#btnDeleteBasicInfoType").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).BasicInfoGUID;
                var r = AcquireDataLock(DataKey)
                if (r == "ok") {
                    if (confirm("Confirm to delete selected BasicInfoType (" + gridDataList.jqGrid('getRowData', RowId).BasicInfoTypeGUID + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMBasic/DeleteBasicInfoType",
                            data: { "BasicInfoGUID": gridDataList.jqGrid('getRowData', RowId).BasicInfoGUID },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    //$("#btnSearch").click();
                                    alert("BasicInfoType delete successfully.");
                                    var gridDataList = $("#gridDataListBasicInfoType");
                                    gridDataList.jqGrid('clearGridData');
                                    gridDataList.jqGrid('setGridParam', { postData: { SearchText: "" } })
                                    gridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                                }
                                else {
                                    alert("BasicInfoType delete fail due to:\n\n" + data);
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
            } else { alert("Please select a row data to delete."); }
        }
    });

    $("#btnCommit").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).BasicInfoGUID;
                //var r = AcquireDataLock(DataKey)

                //if (r == "ok") {
                if (true) {
                    if (confirm("Confirm to submit selected?")) {

                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMBasic/ExcelContact",
                            data: { "BasicInfoGUID": gridDataList.jqGrid('getRowData', RowId).BasicInfoGUID, "TB_SQM_Vendor_TypeCID": gridDataList.jqGrid('getRowData', RowId).TB_SQM_Vendor_TypeCID },
                            type: "post",
                            beforeSend: function () {
                                $("#dialogDownloadSplash").dialog({
                                    title: 'Uploading Notify',
                                    width: 'auto',
                                    height: 'auto',
                                    modal: true,
                                    open: function (event, ui) {
                                        $(this).parent().find('.ui-dialog-titlebar-close').hide();
                                        $(this).parent().find('.ui-dialog-buttonpane').hide();
                                        $("#lbDiaDownloadMsg").html('Uploading...</br></br>Please wait for the operation a moment...');
                                    }
                                }).dialog("open");
                            },
                            dataType: 'text',
                            //async: false,
                         
                            success: function (data) {
                                if (data == "") {
                                    $("#dialogDownloadSplash").dialog('close');
                                    alert("Commit successfully.");
                                    var gridDataList = $("#gridDataListBasicInfoType");
                                    gridDataList.jqGrid('clearGridData');
                                    gridDataList.jqGrid('setGridParam', { postData: { SearchText: "" } })
                                    gridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                                    $("#btnSuspend").hide();
                                    $("#btnCommit").hide();
                                }
                                else
                                {
                                    alert(data);
                                    $("#dialogDownloadSplash").dialog('close');
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
    $("#btnSuspend").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).BasicInfoGUID;
                //var r = AcquireDataLock(DataKey)

                //if (r == "ok") {
                if (true) {
                    $.ajax({
                        url: __WebAppPathPrefix + "/SQMBasic/SuspendApproveCase",
                        data: {
                            "BasicInfoGUID": gridDataList.jqGrid('getRowData', RowId).BasicInfoGUID
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                //DoSuccessfully = true;
                                //if (dialog.attr('Mode') == "c")
                                //    alert("create successfully.");
                                //else
                                //    alert("edit successfully.");
                                alert("Suspend successfully.");
                                var gridDataList = $("#gridDataListBasicInfoType");
                                gridDataList.jqGrid('clearGridData');
                                gridDataList.jqGrid('setGridParam', { postData: { SearchText: "" } })
                                gridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                                $("#btnSuspend").hide();
                                $("#btnCommit").hide();
                            }
                            else {
                                //if ((dialog.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                //    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                //    DoSuccessfully = true;
                                //}
                                //else
                                //    $("#lblDiaErrMsg").html(data);
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
                    //switch (r) {
                    //    case "timeout": $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); break;
                    //    case "l": alert("Data already lock by other user."); break;
                    //    default: alert("Data lock fail or application error."); break;
                    //}
                }
            } else { alert("Please select a row data to export."); }
        }
        
    });
});