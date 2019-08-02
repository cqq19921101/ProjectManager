$(function () {
    var diaASNSpecialCancelErrorResult = $('#dialog_VMIProcess_ASNSpecialCancelErrorResult');
    var VMI_ASNSpecialCancelErrorResultgridDataList = $('#VMI_Process_ASNSpecialCancelErrorResult_gridDataList');

    //Init dialog
    diaASNSpecialCancelErrorResult.dialog({
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

    VMI_ASNSpecialCancelErrorResultgridDataList.jqGrid({
        mtype: "POST",
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        colNames: ["ASN No",
                    "IDN Num",
                    "Error Code"],
        colModel: [
                    { name: "ASNNO", index: "ASNNO", width: 90, sortable: false, sorttype: "text" },
                    { name: "IDNNUM", index: "IDNNUM", width: 90, sortable: false, sorttype: "text" },
                    { name: "ERRORCODE", index: "ERRORCODE", width: 255, sortable: false, sorttype: "text" }

        ],
        shrinkToFit: false,
        scrollrows: true,
        width: 455,
        height: 290,
        rowNum: 30,
        //rowList: [10, 20, 30],
        //sortname: "ASNLINE",
        viewrecords: true,
        loadonce: true,
        pager: '#VMI_Process_ASNSpecialCancelErrorResult_gridListPager',
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

    VMI_ASNSpecialCancelErrorResultgridDataList.jqGrid('navGrid', '#VMI_Process_ASNSpecialCancelErrorResult_gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });
})