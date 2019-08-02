$(function () {
    //Toolbar Buttons
    $("#btnSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });


    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var gridDataList = $("#gridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMMgmt/LoadVendorJSonWithFilter',
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
        width: "auto",
        height: "auto",
        colNames: ['供應商代碼',
                   '供應商簡稱代碼',
                   '供應商名稱',
                   '供應商簡稱',
                   '供應商官方名稱',
                   '固話',
                   '傳真',
                   '地址',
        ],
        colModel: [
                { name: 'ERPVND', index: 'ERPVND', width: 200, sortable: false, sorttype: 'text' },
                { name: 'CORPVND', index: 'CORPVND', width: 150, sortable: true, sorttype: 'text' },
                { name: 'ERPVName', index: 'ERPVName', width: 150, sortable: true, sorttype: 'text' },
                { name: 'CorpVName', index: 'CorpVName', width: 150, sortable: true, sorttype: 'text' },
                { name: 'OfficalName', index: 'OfficalName', width: 150, sortable: true, sorttype: 'text' },
                { name: 'TelPhone', index: 'TelPhone', width: 150, sortable: true, sorttype: 'text' },
                { name: 'Fax', index: 'Fax', width: 150, sortable: true, sorttype: 'text' },
                { name: 'Address', index: 'Address', width: 150, sortable: true, sorttype: 'text' },
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'ERPVND',
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

    //gridDataList.jqGrid({
    //    url: __WebAppPathPrefix + '/SQMMgmt/LoadSubFuncJSonWithFilter',
    //    postData: { SearchText: "" },
    //    type: "post",
    //    datatype: "json",
    //    jsonReader: {
    //        root: "Rows",
    //        page: "Page",
    //        total: "Total",
    //        records: "Records",
    //        repeatitems: false
    //    },
    //    width: 300,
    //    height: "auto",
    //    colNames: ['SubFunc GUID',
    //                'SubFunc Name'],
    //    colModel: [
    //        { name: 'SubFuncGUID', index: 'SubFuncGUID', width: 200, sortable: false, hidden: true },
    //        { name: 'SubFuncName', index: 'SubFuncName', width: 150, sortable: true, sorttype: 'text' }
    //    ],
    //    rowNum: 10,
    //    //rowList: [10, 20, 30],
    //    sortname: 'SubFuncName',
    //    viewrecords: true,
    //    loadonce: true,
    //    mtype: 'POST',
    //    pager: '#gridListPager',
    //    //sort by reload
    //    loadComplete: function () {
    //        var $this = $(this);
    //        if ($this.jqGrid('getGridParam', 'datatype') === 'json')
    //            if ($this.jqGrid('getGridParam', 'sortname') !== '')
    //                setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
    //    }
    //});
    //gridDataList.jqGrid('navGrid', '#gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#tbMain1').show();
    $('#dialogData').show();
});
