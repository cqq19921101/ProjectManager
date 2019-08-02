$(function () {
    var diaToDoASNAttach = $('#dialogToDoASNFileAttach');
    var diaToDoASNFileDetail = $('#dialog_VMIProcess_ToDoASNFileDetail');
    var VMI_ToDoASNFileDetailgridDataList = $('#VMI_Process_ToDoASNFileDetail_gridDataList');

    //Init Function Button
    //Button
    $('#dia_btn_VMIProcess_ToDoASNFileDetail_Add').button({
        label: "Add",
        icons: { primary: 'ui-icon-pencil' }
    });

    $('#dia_btn_VMIProcess_ToDoASNFileDetail_Delete').button({
        label: "Delete",
        icons: { primary: 'ui-icon-minus' }
    });

    $('#dia_btn_VMIProcess_ToDoASNFileDetail_ViewLogs').button({
        label: "View Logs",
        icons: { primary: 'ui-icon-search' }
    });

    //After Init. to Show Menu Function Button
    $('#dialog_VMIProcess_ToDoASNFileDetail_tbTopToolBar').show();

    //Init dialog
    diaToDoASNFileDetail.dialog({
        autoOpen: false,
        height: 500,
        width: 500,
        resizable: false,
        modal: true,
        buttons: {
            Close: function () {
                $(this).dialog("close");
            }
        },
        close: function () {
            __DialogIsShownNow = false;
        }
    });

    //Init JQgrid
    VMI_ToDoASNFileDetailgridDataList.jqGrid({
        url: __WebAppPathPrefix + "/VMIProcess/QueryToDoASNFileDetailInfo",
        //postData: { },
        mtype: "POST",
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        colNames: ["FileID",
                    "File Name",
                    "Update Date",
                    "Remark"],
        colModel: [
                    { name: "FS_GUID", index: "FS_GUID", width: 50, sortable: true, sorttype: "text", hidden : true },
                    {
                        name: "FS_FILENAME", index: "FS_FILENAME", width: 120, sortable: true, sorttype: "text", classes: "jqGridColumnDataAsLinkWithBlue",
                        formatter: function (cellvalue, option, rowobject) {
                            return cellvalue;
                        }
                    },                  
                    { name: "UPLOADDATETIME", index: "UPLOADDATETIME", width: 160, sortable: true, sorttype: "text" },
                    { name: "REMARK", index: "REMARK", width: 150, sortable: true, sorttype: "text" }

        ],
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            var $this = $(this);
            var FileName = $this.jqGrid('getCell', rowid, 'FS_FILENAME');
            var FSGUID = $this.jqGrid('getCell', rowid, 'FS_GUID');
            var REMARK = $this.jqGrid('getCell', rowid, 'REMARK');
            if (FSGUID != "") {
                if (iCol == 2) {
                    __DialogIsShownNow = false;
                    if (!__DialogIsShownNow) {
                        __DialogIsShownNow = true;
                        diaToDoASNAttach.attr("FILENAME", $.trim(FileName));
                        diaToDoASNAttach.attr("FS_GUID", $.trim(FSGUID));
                        diaToDoASNAttach.attr("REMARK", $.trim(REMARK));
                        diaToDoASNAttach.attr('Mode', 'U');
                        InitdialogToDoASNFileAttach("U");
                        diaToDoASNAttach.dialog("option", "title", "File Attach").dialog("open");
                    }
                }
            }
        },
        multiselect: true,
        shrinkToFit: false,
        scrollrows: true,
        width: 480,
        height: 280,
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: "ASNLINE",
        viewrecords: true,
        loadonce: true,
        pager: '#VMI_Process_ToDoASNFileDetail_gridListPager',
        loadComplete: function () {
            var $this = $(this);

            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '') {
                    setTimeout(function () {
                        $this.triggerHandler('reloadGrid');
                    }, 50);
                }
        }
    });

    VMI_ToDoASNFileDetailgridDataList.jqGrid('navGrid', '#VMI_Process_ToDoASNFileDetail_gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

});