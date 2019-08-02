$(function () {
    //Toolbar Buttons
    $("#btndialogFCACreate").button({
        label: "Create",
        //icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogFCAEdit").button({
        label: "Edit",
        //icons: { primary: "ui-icon-close" }
    });
    $("#btndialogFCADelete").button({
        label: "Delete",
        //icons: { primary: "ui-icon-close" }
    });
    $("#btndialogFCADeleteAll").button({
        label: "Delete All",
        //icons: { primary: "ui-icon-close" }
    });

    var gridDataList = $("#gridDataList");
    var dialog = $("#dialogFunctionControllerActions");
    var dialogFCAItem = $("#dialogFunctionControllerActionsItem");
    var gridFunctionCAList = $("#gridFunctionControllerActionList");

    jQuery("#btndialogFCACreate").click(function () {
        $(this).removeClass('ui-state-focus');
        dialogFCAItem.attr('Mode', "c");
        $('#txtDiaFunctionControllerActionsItemController').val("");
        $('#txtDiaFunctionControllerActionsItemAction').val("");
        $('#lblDiaFCAErrMsg').html("");
        dialogFCAItem.dialog("option", "title", "Create").dialog("open");
    });

    jQuery("#btndialogFCAEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridFunctionCAList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridFunctionCAList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                dialogFCAItem.attr('Mode', "e");
                var Row = gridFunctionCAList.jqGrid('getRowData', RowId);
                $('#txtDiaFunctionControllerActionsItemController').val(Row.Controller);
                $('#txtDiaFunctionControllerActionsItemAction').val(Row.Action);
                $('#lblDiaFCAErrMsg').html("");
                dialogFCAItem.dialog("option", "title", "Edit").dialog("open");
            } else { alert("Please select a row data to edit."); }
        }
    });

    jQuery("#btndialogFCADelete").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridFunctionCAList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridFunctionCAList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                gridFunctionCAList.jqGrid('delRowData', RowId);
                gridFunctionCAList.trigger("reloadGrid");
            } else { alert("Please select a row data to delete."); }
        }
    });

    jQuery("#btndialogFCADeleteAll").click(function () {
        $(this).removeClass('ui-state-focus');
        if (confirm("Delete all Controller/Action data item(s)?")) {
            gridFunctionCAList.jqGrid('clearGridData');
            gridFunctionCAList.trigger("reloadGrid");
        }
    });

    //initial dialog
    dialog.dialog({
        autoOpen: false,
        height: 435,
        width: 510,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                var NewCAString = "";
                var AllRowsInGrid = gridFunctionCAList.jqGrid('getGridParam', 'data');
                if (AllRowsInGrid.length > 0) {
                    for (var iCnt = 0; iCnt < AllRowsInGrid.length; iCnt++)
                        NewCAString += "," + AllRowsInGrid[iCnt].Controller + "|" + AllRowsInGrid[iCnt].Action;
                }
                if (NewCAString != "") NewCAString = NewCAString.substr(1, NewCAString.length - 1);
                if (NewCAString == "")
                    gridDataList.jqGrid('setCell', gridDataList.jqGrid('getGridParam', 'selrow'), "ControllerActions", null);
                else
                    gridDataList.jqGrid('setCell', gridDataList.jqGrid('getGridParam', 'selrow'), "ControllerActions", NewCAString);
                $(this).dialog("close");
                gridFunctionCAList.trigger('reloadGrid');
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
        }
    });

    //Init Controller Action List (jqGrid)
    gridFunctionCAList.jqGrid({
        datatype: "local",
        width: 400,
        height: "auto",
        colNames: ['Controller',
                    'Action'],
        colModel: [
            { name: 'Controller', index: 'Controller', sortable: false, width: 210, hidden: false, editable: true },
            { name: 'Action', index: 'Action', sortable: false, width: 210, hidden: false, editable: true }
        ],
        rowNum: 10,
        viewrecords: true,
        loadonce: true,
        pager: '#gridFunctionControllerActionListPager'
    });
    gridFunctionCAList.jqGrid('navGrid', '#gridFunctionControllerActionListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    //initial dialog
    dialogFCAItem.dialog({
        autoOpen: false,
        height: 420,
        width: 510,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (dialogFCAItem.attr('Mode') == "c") {
                    var m = "";
                    if ($.trim($("#txtDiaFunctionControllerActionsItemController").val()) == "") m += "<br />Must provide Controller name."
                    if ($.trim($("#txtDiaFunctionControllerActionsItemAction").val()) == "") m += "<br />Must provide Action name."

                    var bDuplicated = false;
                    var gridArr = gridFunctionCAList.getDataIDs();
                    for (var iCnt = 0; iCnt < gridArr.length; iCnt++) {
                        var d = gridFunctionCAList.jqGrid('getRowData', gridArr[iCnt]);
                        if (($.trim($("#txtDiaFunctionControllerActionsItemController").val()) == d.Controller) && ($.trim($("#txtDiaFunctionControllerActionsItemAction").val()) == d.Action)) {
                            bDuplicated = true;
                            break;
                        }
                    }
                    if (bDuplicated) m += "<br />Duplicated Controller/Action."
                    if (m != "") $("#lblDiaFCAErrMsg").html(m.substr(6, m.length - 6));
                    else {
                        var row = {
                            Controller: $.trim($("#txtDiaFunctionControllerActionsItemController").val()),
                            Action: $.trim($("#txtDiaFunctionControllerActionsItemAction").val())
                        };
                        var su = gridFunctionCAList.jqGrid('addRowData', GetTimeStamp(), row, 'last');
                        $(this).dialog("close");
                        gridFunctionCAList.trigger("reloadGrid");
                    }
                }
                else {
                    var m = "";
                    if ($.trim($("#txtDiaFunctionControllerActionsItemController").val()) == "") m += "<br />Must provide Controller name (en-US)."
                    if ($.trim($("#txtDiaFunctionControllerActionsItemAction").val()) == "") m += "<br />nMust provide Action name (zh-CN)."

                    var bDuplicated = false;
                    var gridArr = gridFunctionCAList.getDataIDs();
                    var SelRowId = gridFunctionCAList.jqGrid('getGridParam', 'selrow');
                    for (var iCnt = 0; iCnt < gridArr.length; iCnt++) {
                        if (gridArr[iCnt] != SelRowId) {
                            var d = gridFunctionCAList.jqGrid('getRowData', gridArr[iCnt]);
                            if (($.trim($("#txtDiaFunctionControllerActionsItemController").val()) == d.Controller) && ($.trim($("#txtDiaFunctionControllerActionsItemAction").val()) == d.Action)) {
                                bDuplicated = true;
                                break;
                            }
                        }
                    }
                    if (bDuplicated) m += "<br />Duplicated Controller/Action."
                    if (m != "") $("#lblDiaFCAErrMsg").html(m.substr(6, m.length - 6));
                    else {
                        gridFunctionCAList.jqGrid('setCell', SelRowId, 'Controller', $.trim($("#txtDiaFunctionControllerActionsItemController").val()));
                        gridFunctionCAList.jqGrid('setCell', SelRowId, 'Action', $.trim($("#txtDiaFunctionControllerActionsItemAction").val()));
                        $(this).dialog("close");
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () { }
    });

    function GetTimeStamp() {
        var temp = new Date();
        var dateStr = padStr(temp.getFullYear()) +
                        padStr(1 + temp.getMonth()) +
                        padStr(temp.getDate()) +
                        padStr(temp.getHours()) +
                        padStr(temp.getMinutes()) +
                        padStr(temp.getSeconds());
        return dateStr;
    }
    function padStr(i) { return (i < 10) ? "0" + i : "" + i; }
});