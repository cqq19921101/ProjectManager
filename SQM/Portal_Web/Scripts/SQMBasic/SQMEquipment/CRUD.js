$(function () {
    var gridDataList = $("#SQMEquipmentgridDataList");
    var dialog = $("#SQMEquipmentdialogData");
    //function Load() {
    //    var colname;
    //    var colmodel;


    //    $.ajax({
    //        url: __WebAppPathPrefix + '/SQMBasic/getModel',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
    //        data: { EquipmentType: $('#ddlEquipmentType').val(), EquipmentSpecialType: $('#ddlEquipmentSpecialType').val() },
    //        type: "post",
    //        dataType: 'json',
    //        async: false,
    //        success: function (data) {
    //            for (var idx in data.model) {
    //                colmodel = data.model[idx]["ColModel"];
    //                colname = data.model[idx]["Colname"];
    //            }
    //            $("#SQMEquipmentgridDataList");
    //        },
    //        error: function (xhr, textStatus, thrownError) {
    //            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
    //        },
    //        complete: function (jqXHR, textStatus) {
    //        }
    //    });
    //    //Data List
    //    try {
    //        $("#SQMEquipmentgridDataList").jqGrid("GridUnload");
    //    } catch (e) {

    //    }
    //    setTimeout(function () {

    //    }, 200);
    //    var gridDataList = $("#SQMEquipmentgridDataList");

    //    gridDataList.jqGrid({
    //        url: __WebAppPathPrefix + '/SQMEquipment/LoadEquipmentJSonWithFilter',
    //        postData: { EquipmentType: $('#ddlEquipmentType').val(), EquipmentSpecialType: $('#ddlEquipmentSpecialType').val() },
    //        type: "post",
    //        datatype: "json",
    //        jsonReader: {
    //            root: "Rows",
    //            page: "Page",
    //            total: "Total",
    //            records: "Records",
    //            repeatitems: false
    //        },
    //        width: 900,
    //        height: "auto",
    //        colNames: colname,
    //        colModel: colmodel,
    //        rowNum: 10,
    //        //rowList: [10, 20, 30],
    //        sortname: 'DeviceName',
    //        viewrecords: true,
    //        loadonce: true,
    //        mtype: 'POST',
    //        pager: '#SQMEquipmentgridListPager',
    //        //sort by reload
    //        loadComplete: function () {
    //            var $this = $(this);
    //            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
    //                if ($this.jqGrid('getGridParam', 'sortname') !== '')
    //                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
    //        }
    //    });
    //    gridDataList.jqGrid('navGrid', '#SQMEquipmentgridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    //}

    jQuery("#btnSQMEquipmentCreate").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            dialog.attr('Mode', "c");
            EquipmentDialogSetUIByMode(dialog.attr('Mode'));
            dialog.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnSQMEquipmentViewEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            SetToEquipmentViewMode();
        }
    });

    jQuery("#btnSQMEquipmentdialogEditData").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('SID'))
            
            //if (r == "ok") {
            if (true) {                
                dialog.attr('Mode', "e");
                EquipmentDialogSetUIByMode(dialog.attr('Mode'));
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

    jQuery("#btnSQMEquipmentdialogCancelEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialog.attr('SID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToEquipmentViewMode();
    });

    function SetToEquipmentViewMode()
    {
        gridDataList = $("#SQMEquipmentgridDataList");
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            dialog.attr('Mode', "v");
            EquipmentDialogSetUIByMode(dialog.attr('Mode'));
            dialog.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnSQMEquipmentDelete").click(function () {
        gridDataList = $("#SQMEquipmentgridDataList");
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).SID;
                //var r = AcquireDataLock(DataKey)
                
                //if (r == "ok") {
                if (true) {
                    if (confirm("Confirm to delete selected member (" + gridDataList.jqGrid('getRowData', RowId).SID + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMBasic/DeleteEquipment",
                            data: { "SID": gridDataList.jqGrid('getRowData', RowId).SID },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    //$("#btnSearch").click();
                                    Load();
                                    alert("Equipment delete successfully.");
                                }
                                else {
                                    alert("Equipment delete fail due to:\n\n" + data);
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