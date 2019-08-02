$(function () {
    var dialog_VMIProcess_ToDoASNFileViewLogs = $('#dialog_VMIProcess_ToDoASNFileViewLogs');
    var VMI_Process_ToDoASNFileViewLogs_gridDataList = $('#VMI_Process_ToDoASNFileViewLogs_gridDataList');
    var VMI_Process_ToDoASNFileViewLogs_gridListPager = $('#VMI_Process_ToDoASNFileViewLogs_gridListPager');

    //Init dialog
    dialog_VMIProcess_ToDoASNFileViewLogs.dialog({
        autoOpen: false,
        height: 450,
        width: 800,
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
    VMI_Process_ToDoASNFileViewLogs_gridDataList.jqGrid({
        url: __WebAppPathPrefix + "/VMIProcess/QueryToDoASNFileViewLogsDetailInfo",
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
        colNames: ["Log Time",
                    "Modify By",
                    "Action",
                    "File Name",
                    "Remark"],
        colModel: [
                    { name: "CREATE_TIME", index: "CREATE_TIME", width: 130, sortable: false, sorttype: "text" },
                    { name: "ACCOUNT", index: "ACCOUNT", width: 80, sortable: false, sorttype: "text" },
                    { name: "Log_Status", index: "Log_Status", width: 65, sortable: false, sorttype: "text" },
                    { name: "FS_FileName", index: "FS_FileName", width: 220, sortable: false, sorttype: "text" },
                    { name: "Remark", index: "Remark", width: 240, sortable: false, sorttype: "text" }
        ],
        shrinkToFit: false,
        scrollrows: true,
        width: 780,
        height: 280,
        rowNum: 999,
        //rowList: [10, 20, 30],
        //sortname: "ASNLINE",
        viewrecords: true,
        loadonce: true,
        pager: '#VMI_Process_ToDoASNFileViewLogs_gridListPager',
        loadComplete: function () {
            var $this = $(this);

            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '') {
                    setTimeout(function () {
                        $this.triggerHandler('reloadGrid');
                    }, 50);
                }
        },
        hoverrows: false
    });

    VMI_Process_ToDoASNFileViewLogs_gridDataList.jqGrid('navGrid', '#VMI_Process_ToDoASNFileViewLogs_gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });
})