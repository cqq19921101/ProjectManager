$(function () {
    var gridDataList = $("#gridDataList");
    var dialog = $("#dialogData");
    var MapgridDataList = $("#MapgridDataList");
    var Mapdialog = $("#MapdialogData");

    jQuery("#btnCreate").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            dialog.attr('Mode', "c");
            DialogSetUIByMode(dialog.attr('Mode'));
            dialog.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnViewEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            SetToViewMode();
        }
    });

    jQuery("#btndialogEditData").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('SID'))
            
            //if (r == "ok") {
            if (true) {                
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
    jQuery("#btndialogCancelEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialog.attr('SID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewMode();
    });
    function SetToViewMode() {
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            dialog.attr('Mode', "v");
            DialogSetUIByMode(dialog.attr('Mode'));
            dialog.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnDelete").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).Vendor;
                //var r = AcquireDataLock(DataKey)

                //if (r == "ok") {
                if (true) {
                    if (confirm("Confirm to delete selected member (" + gridDataList.jqGrid('getRowData', RowId).SID + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/MRBAppove/Delete",
                            data: { "SID": gridDataList.jqGrid('getRowData', RowId).SID },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSearch").click();
                                    alert(" delete successfully.");
                                }
                                else {
                                    alert(" delete fail due to:\n\n" + data);
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

    jQuery("#btnMapCreate").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!MapgridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            Mapdialog.attr('Mode', "c");
            MapDialogSetUIByMode(Mapdialog.attr('Mode'));
            Mapdialog.dialog("option", "title", "Create").dialog("open");
        }
    });

      jQuery("#btnMapViewEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!MapgridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            SetToMapViewMode();
        }
    });

    jQuery("#btnMapdialogEditData").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = MapgridDataList.jqGrid('getGridParam', 'selrow');
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
        var r = ReleaseDataLock(Mapdialog.attr('SID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToMapViewMode();
    });
    function SetToMapViewMode() {
        var RowId = MapgridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            Mapdialog.attr('Mode', "v");
            MapDialogSetUIByMode(Mapdialog.attr('Mode'));
            Mapdialog.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnMapDelete").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!MapgridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = MapgridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (MapgridDataList.jqGrid('getRowData', RowId)).Vendor;
                //var r = AcquireDataLock(DataKey)

                //if (r == "ok") {
                if (true) {
                    if (confirm("Confirm to delete selected member (" + MapgridDataList.jqGrid('getRowData', RowId).SID + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/MRBAppove/DeleteMap",
                            data: { "SID": MapgridDataList.jqGrid('getRowData', RowId).SID },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnMapSearch").click();
                                    alert(" delete successfully.");
                                }
                                else {
                                    alert(" delete fail due to:\n\n" + data);
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


    jQuery("#btnShow").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            var MapgridDataList = $("#MapgridDataList");
            $("#MapdialogData").attr('SSID', dataRow.SID);
            MapgridDataList.jqGrid({
                url: __WebAppPathPrefix + '/MRBAppove/LoadMapJSonWithFilter',
                postData: { SearchText: dataRow.SID },
                type: "post",
                datatype: "json",
                jsonReader: {
                    root: "Rows",
                    page: "Page",
                    total: "Total",
                    records: "Records",
                    repeatitems: false
                },
                width: 800,
                height: "auto",
                colNames: ['SID',

                           '姓名','邮箱'

                ],
                colModel: [
                    { name: 'SID', index: 'SID', width: 200, sortable: false, hidden: true },

                    { name: 'Name', index: 'Name', width: 150, sortable: true, sorttype: 'text' },
                     { name: 'Email', index: 'Email', width: 150, sortable: true, sorttype: 'text' }
               
                ],
                rowNum: 10,
                //rowList: [10, 20, 30],
                sortname: 'Provider',
                viewrecords: true,
                loadonce: true,
                mtype: 'POST',
                pager: '#MapgridListPager',
                //sort by reload
                loadComplete: function () {
                    var $this = $(this);
                    if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                        if ($this.jqGrid('getGridParam', 'sortname') !== '')
                            setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
                }
            });
            MapgridDataList.jqGrid('navGrid', '#MapgridListPager', { edit: false, add: false, del: false, search: false, refresh: false });
            $('#Appove').hide();
            $('#Map').show();
            $('#tbMainMap').show();
            $('#MapdialogData').show();
            $("#btnMapSearch").click();
        }
        else {
            alert("Please select a row data to edit.");
        }
   

    })
  
    jQuery("#btnMapBack").click(function ()
    {
        $('#Appove').show();
        $('#Map').hide();
    })


});