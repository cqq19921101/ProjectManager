$(function () {
    //Toolbar Buttons
    $("#btnSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCreate").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnViewEdit").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnDelete").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });
    $("#btnCommit").button({
        label: "Commit",
        icons: { primary: "ui-icon-arrowthickstop-1-s" }
    });
    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var cn = ['SID',
              'UD',
              'Plan',
              'UD Type',
              '描述',
              '異動人員',
              '異動時間',
    ];
    var cm = [
            { name: 'SID', index: 'SID', width: 200, sortable: false, hidden: true },
            { name: 'UD', index: 'UD', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Plan', index: 'Plan', width: 150, sortable: true, sorttype: 'text' },
            { name: 'UDType', index: 'UDType', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ReMark', index: 'ReMark', width: 150, sortable: true, sorttype: 'text' },
            { name: 'UpdateUser', index: 'UpdateUser', width: 150, sortable: true, sorttype: 'text' },
            { name: 'updateTime', index: 'updateTime', width: 150, sortable: true, sorttype: 'date', formatter: "date", formatoptions: { srcformat: "Y/m/d H:i:s", newformat: "Y/m/d H:i" } },
    ];

    
    var gridDataList = $("#gridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMBasic/LoadUDJSonWithFilter',
        postData: { SearchText: "" },
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
        colNames: cn,
        colModel: cm,
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'SID',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridListPager',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataList.jqGrid('navGrid', '#gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#tbMain1').show();
    $('#dialogData').show();
});
