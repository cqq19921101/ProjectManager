$(function () {
    var gridDataList = $("#CriticismgridDataList");
    var Mapdialog = $("#MapdialogData");
    var dialog = $("#CriticismdialogData");
    var infodialog = $("#InfodialogData");
    jQuery("#btnCriticismCreate").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            Mapdialog.attr('Mode', "c");
            MapDialogSetUIByMode(Mapdialog.attr('Mode'));
            Mapdialog.dialog("option", "title", "Create").dialog("open");
        }
    });
    jQuery("#btnCreatCriticism").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                dialog.attr('Mode', "c");
                CriticismDialogSetUIByMode(dialog.attr('Mode'));
               
                dialog.dialog("option", "title", "Create").dialog("open");
            } else { alert("Please select a row data to edit."); }
 
        }
    });

    jQuery("#btnCriticismViewEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            SetToMapViewMode();
        }
    });

    $("#btnCommit").click(function () {
        var gridDataList = $("#CriticismgridDataList");

        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        var dataRow = gridDataList.jqGrid('getRowData', RowId);
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQECriticism/ExcelCriticism",
                            data: { SearchText: dataRow.CriticismID },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    alert("Commit successfully.");
                                    var gridDataList = $("#gridDataListBasicInfoType");
                            
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
                    
                  
    });
    jQuery("#btnMapdialogEditData").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('SID'))
            
            //if (r == "ok") {
            if (true) {                
                Mapdialog.attr('Mode', "e");
                MapDialogSetUIByMode(Mapdialog.attr('Mode'));
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

    jQuery("#btnMapdialogCancelEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(Mapdialog.attr('CriticismID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewMode();
    });

    jQuery("#btnShowCriticism").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!infodialog.jqGrid('getGridParam', 'multiselect')) {   //single select
            SetToInfoViewMode();
        }
    });
    function SetToMapViewMode()
    {
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            Mapdialog.attr('Mode', "v");
            MapDialogSetUIByMode(Mapdialog.attr('Mode'));
            Mapdialog.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }
    function SetToInfoViewMode() {
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            InfoDialogSetUIByMode();
            infodialog.dialog("option", "title", "show").dialog("open");
        } else { alert("Please select a row data to show."); }
    }
  
    jQuery("#btnCriticismDelete").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).CriticismID;
                //var r = AcquireDataLock(DataKey)
                
                //if (r == "ok") {
                if (true) {
                    if (confirm("Confirm to delete selected ?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQECriticism/DeleteCriticismMap",
                            data: { "CriticismID": gridDataList.jqGrid('getRowData', RowId).CriticismID },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnCriticismSearch").click();
                                    alert("Criticism delete successfully.");
                                }
                                else {
                                    alert("Criticism delete fail due to:\n\n" + data);
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
});