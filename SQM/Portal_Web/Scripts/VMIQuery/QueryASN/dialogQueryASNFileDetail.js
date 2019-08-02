$(function () {
    var diaQueryASNAttach = $('#dialogQueryASNFileAttach');
    var diaASNFileDetail = $('#dialog_VMIQuery_ASNFileInfo');
    var VMI_ASNFileDetailgridDataList = $('#VMI_Query_ASNFileDetail_gridDataList');

    //Init dialog
    diaASNFileDetail.dialog({
        autoOpen: false,
        height: 460,
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
    VMI_ASNFileDetailgridDataList.jqGrid({
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
                    { name: "FS_GUID", index: "FS_GUID", width: 50, sortable: false, sorttype: "text", hidden : true },
                    {
                        name: "FS_FILENAME", index: "FS_FILENAME", width: 120, sortable: false, sorttype: "text", classes: "jqGridColumnDataAsLinkWithBlue",
                        formatter: function (cellvalue, option, rowobject) {
                            return cellvalue;
                        }
                    },                  
                    { name: "UPLOADDATETIME", index: "UPLOADDATETIME", width: 160, sortable: false, sorttype: "text" },
                    { name: "REMARK", index: "REMARK", width: 180, sortable: false, sorttype: "text" }

        ],
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            var $this = $(this);
            var FileName = $this.jqGrid('getCell', rowid, 'FS_FILENAME');
            var FSGUID = $this.jqGrid('getCell', rowid, 'FS_GUID');
            var REMARK = $this.jqGrid('getCell', rowid, 'REMARK');
            if (FSGUID != "") {
                if (iCol == 1) {
                    __DialogIsShownNow = false;
                    if (!__DialogIsShownNow) {
                        __DialogIsShownNow = true;
                        diaQueryASNAttach.attr("FILENAME", $.trim(FileName));
                        diaQueryASNAttach.attr("FS_GUID", $.trim(FSGUID));
                        diaQueryASNAttach.attr("REMARK", $.trim(REMARK));
                        InitdialogASNFileAttach();
                        diaQueryASNAttach.dialog("option", "title", "File Info").dialog("open");
                    }
                }
            }
        },
        shrinkToFit: false,
        scrollrows: true,
        width: 480,
        height: 232,
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: "ASNLINE",
        viewrecords: true,
        loadonce: true,
        pager: '#VMI_Query_ASNFileDetail_gridListPager',
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

    VMI_ASNFileDetailgridDataList.jqGrid('navGrid', '#VMI_Query_ASNFileDetail_gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

});