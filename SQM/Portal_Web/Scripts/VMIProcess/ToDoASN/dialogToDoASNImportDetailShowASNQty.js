$(function () {
    var diaToDoASNImportDetailShowASNQty = $('#dialog_VMIProcess_ToDoASNImportDetailShowASNQty');
    var VMI_ToDoASNImportDetailShowASNQtygridDataList = $('#VMI_Process_ToDoASNImportDetailShowASNQty_gridDataList');

    //Init dialog
    diaToDoASNImportDetailShowASNQty.dialog({
        autoOpen: false,
        height: 580,
        width: 400,
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
    VMI_ToDoASNImportDetailShowASNQtygridDataList.jqGrid({
        url: __WebAppPathPrefix + "/VMIProcess/QueryToDoASNImportDetailShowASNQty",
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
        colNames: ["ASN Num",
                    "ASN Line",
                    "ASN Qty"],
        colModel: [
                    { name: "ASNNUM", index: "ASNNUM", width: 90, sortable: false, sorttype: "text" },
                    { name: "ASNLINE", index: "ASNLINE", width: 90, sortable: false, sorttype: "text" },
                    { name: "ASNQTY", index: "ASNQTY", width: 120, align: 'right', sortable: false, sorttype: "text" },
        ],
        shrinkToFit: false,
        scrollrows: true,
        width: 370,
        height: 380,
        rowNum: 100,
        sortname: "ASN_NUM,ASN_LINE",
        viewrecords: true,
        loadonce: true,
        //pager: '#VMI_Process_ToDoASNImportDetailShowASNQty_gridListPager',
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

    VMI_ToDoASNImportDetailShowASNQtygridDataList.jqGrid('navGrid', '#VMI_Process_ToDoASNImportDetailShowASNQty_gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });
})