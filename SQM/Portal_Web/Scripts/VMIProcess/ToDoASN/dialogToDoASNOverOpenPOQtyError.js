$(function () {
    var diaToDoASNOpenPOQtyError = $('#dialog_VMIProcess_ToDoASNOverOpenPOQtyError');
    var VMI_ToDoASNOpenPOQtyErrorgridDataList = $('#VMI_Process_ToDoASNOverOpenPOQtyError_gridDataList');

    

    //Init dialog
    diaToDoASNOpenPOQtyError.dialog({
        autoOpen: false,
        height: 410,
        width: 640,
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
    VMI_ToDoASNOpenPOQtyErrorgridDataList.jqGrid({
        //url: __WebAppPathPrefix + "/VMIProcess/QueryToDoASNFileDetailInfo",
        //postData: { },
        //mtype: "POST",
        data: "",
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false,
        },
        colNames: ["PO NUM",
                    "PO LINE",
                    "Material",
                    "ASN QTY",
                    "Message"],
        colModel: [
                    { name: "PONUM", index: "PONUM", width: 120, sortable: false, sorttype: "text" },
                    { name: "POLINE", index: "POLINE", width: 70, sortable: false, sorttype: "text" },
                    { name: "MATERIAL", index: "MATERIAL", width: 150, sortable: false, sorttype: "text" },
                    { name: "ASNQTY", index: "ASNQTY", width: 110, sortable: false, sorttype: "text" },
                    { name: "MESSAGE", index: "MESSAGE", width: 300, sortable: false, sorttype: "text" }
        ],
        width: 610,
        height: 240,
        rowNum: 10,
        //rowList: [10, 20, 30],
        //sortname: "ASNLINE",
        viewrecords: true,
        loadonce: true,
        pager: '#VMI_Process_ToDoASNOverOpenPOQtyError_gridListPager',
        loadComplete: function () {
            //var $this = $(this);

            //if ($this.jqGrid('getGridParam', 'datatype') === 'json')
            //    if ($this.jqGrid('getGridParam', 'sortname') !== '') {
            //        setTimeout(function () {
            //            $this.triggerHandler('reloadGrid');
            //        }, 50);
            //    }
        }
    });

    VMI_ToDoASNOpenPOQtyErrorgridDataList.jqGrid('navGrid', '#VMI_Process_ToDoASNOverOpenPOQtyError_gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

})