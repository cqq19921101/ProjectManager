$(function () {
    var gridDataList = $("#ReliabilitygridDataList");
    var gridDataListInfo = $("#ReliInfogridDataList");
    var Mapdialog = $("#MapdialogData");
    var Uploaddialog = $("#UploaddialogData");
    var dialog = $("#ReliabilitydialogData");
    //var infodialog = $("#InfodialogData");

    //Date Select
    $("#txtPlannedTestTime").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtPlannedTestTime").datepicker("setDate", '-31d');

    $("#txtActualTestTime").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            var planTestTime = dataRow.PlannedTestTime;
            var actualTime = $(this).val();
            if (actualTime<planTestTime) {
                alert("實際試驗時間必須晚於計畫實驗時間");
                $(this).val(planTestTime);
            }
        }
    });
    $("#txtActualTestTime").datepicker("setDate", '-31d');


    jQuery("#btnReliInfoSearch").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataList = $("#ReliInfogridDataList");
        gridDataList.jqGrid('clearGridData');

        gridDataList.jqGrid('setGridParam', { postData: { SearchText: escape($.trim($("#txtFilterText").val())) } })
        gridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });

    //jQuery("#btnReliInfoSearch").click(function () {
    //    $(this).removeClass('ui-state-focus');
    //    var gridDataList = $("#ReliInfogridDataList");
    //    gridDataList.jqGrid('clearGridData');

    //    gridDataList.jqGrid('setGridParam', { postData: { SearchText: escape($.trim($("#txtFilterTextReliInfo").val())), ReliabititySID: $("#ReliabilitygridDataList").jqGrid('getRowData', $("#ReliabilitygridDataList").jqGrid('getGridParam', 'selrow')).SID } })
    //    gridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    //});

    jQuery("#btnReliabilityCreate").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            Mapdialog.attr('Mode', "c");
            RMapDialogSetUIByMode(Mapdialog.attr('Mode'));
            Mapdialog.dialog("option", "title", "Create").dialog("open");
        }
    });
    jQuery("#btnReliInfoCreate").click(function () {
        //var rowcount = parseInt(gridDataListInfo.getGridParam("records"));
        //if (rowcount > 0) {
        //    alert("unable to create because it already exist a project");
        //} else {
            $(this).removeClass('ui-state-focus');
            if (!gridDataListInfo.jqGrid('getGridParam', 'multiselect')) {   //single select
                dialog.attr('Mode', "c");
                ReliabilityDialogSetUIByMode(dialog.attr('Mode'));
                dialog.dialog("option", "title", "Create").dialog("open");
            //}
        }
    });

    jQuery("#btnReliabilityViewEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListInfo.jqGrid('getGridParam', 'multiselect')) {   //single select
            SetToMapViewMode();
        }
    });
    jQuery("#btnReliInfoViewEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListInfo.jqGrid('getGridParam', 'multiselect')) {   //single select
            SetToReliInfoViewMode();
        }
    });


    jQuery("#btnMapdialogEditData").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            if (true) {
                Mapdialog.attr('Mode', "e");
                RMapDialogSetUIByMode(Mapdialog.attr('Mode'));
                Mapdialog.dialog("option", "title", "Edit").dialog("open");
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
    jQuery("#btnReliabilitydialogEditData").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataListInfo.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            if (true) {
                dialog.attr('Mode', "e");
                ReliabilityDialogSetUIByMode(dialog.attr('Mode'));
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

    jQuery("#btnMapdialogCancelEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(Mapdialog.attr('SID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToMapViewMode();
    });
    jQuery("#btnReliabilitydialogCancelEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialog.attr('SID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToReliInfoViewMode();
    });

    jQuery("#btnShowReliability").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {   //single select
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            gridDataListInfo.jqGrid('clearGridData');
            gridDataListInfo.jqGrid('setGridParam', { postData: { MemberGUID: dataRow.MemberGUID } })
            gridDataListInfo.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');

            InitReli();

        } else { alert("Please select a row data to show."); }

        //if (RowId) {
        //    loadBasicDataDetail(gridDataList.jqGrid('getRowData', RowId).SID);
        //    SetToInfoViewMode(gridDataList.jqGrid('getRowData', RowId).SID);
        //} else { alert("Please select a row data to edit."); }
    });

    jQuery("#btnUploadInfo").click(function () {
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            Mapdialog.attr('Mode', "u");
            RMapDialogSetUIByMode(Mapdialog.attr('Mode'));
            Mapdialog.dialog("option", "title", "Uplaod").dialog("open");

            //Uploaddialog.attr('Mode', "c");
            //UploadDialogSetUIByMode(Uploaddialog.attr('Mode'));
            //dialog.dialog("option", "title", "Edit").dialog("open");
            //Uploaddialog.dialog("option", "title", "Edit").dialog("open");
        } else { alert("Please select a row data to edit."); }
        //$("UploaddialogData").dialog("option", "title", "Edit").dialog("open");
    });
    function SetToMapViewMode() {
        var RowId = gridDataListInfo.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            Mapdialog.attr('Mode', "v");
            RMapDialogSetUIByMode(Mapdialog.attr('Mode'));
            Mapdialog.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }
    //function SetToInfoViewMode(MemberGUID) {
    //    //var RowId = gridDataListInfo.jqGrid('getGridParam', 'selrow');
    //    //if (RowId) {
    //    //    //InfoDialogSetUIByMode();
    //    //    //infodialog.dialog("option", "title", "show").dialog("open");
    //    //}  
    //    InitReli(MemberGUID);
    //}
    function SetToReliInfoViewMode() {
        var RowId = gridDataListInfo.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            dialog.attr('Mode', "v");
            ReliabilityDialogSetUIByMode(dialog.attr('Mode'));
            dialog.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnReliabilityDelete").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).SID;
                //var r = AcquireDataLock(DataKey)

                //if (r == "ok") {
                if (true) {
                    if (confirm("Confirm to delete selected ?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQEReliability/DeleteReliability",
                            data: { "SID": DataKey },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnReliabilitySearch").click();
                                    alert("Reliability delete successfully.");
                                }
                                else {
                                    alert("Reliability delete fail due to:\n\n" + data);
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
    jQuery("#btnReliInfoDelete").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListInfo.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataListInfo.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataListInfo.jqGrid('getRowData', RowId)).ReliabilitySID;
                //var r = AcquireDataLock(DataKey)

                //if (r == "ok") {
                if (true) {
                    if (confirm("Confirm to delete selected ?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQEReliability/DeleteReliInfo",
                            data: { "ReliabilitySID": DataKey, "TestProjet": (gridDataListInfo.jqGrid('getRowData', RowId)).TestProjet },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnReliInfoSearch").click();
                                    alert("ReliabilityItem delete successfully.");
                                }
                                else {
                                    alert("ReliabilityItem delete fail due to:\n\n" + data);
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


    jQuery("#btnCommitReliability").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = gridDataList.jqGrid('getRowData', RowId).SID;
                //var r = AcquireDataLock(DataKey)

                //if (r == "ok") {
                if (true) {
                    if (confirm("Confirm to submit selected?")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQEReliability/ExcelContact",
                            data: {
                                "SID": DataKey,
                                //"TB_SQM_Vendor_TypeCID": gridDataList.jqGrid('getRowData', RowId).TB_SQM_Vendor_TypeCID
                            },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    alert("Commit successfully.");
                                    $("#btnReliabilitySearch").click();
                                    $("#btnCommitReliability").hide();
                                }
                                else {
                                    alert(data);
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
});