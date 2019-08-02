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
    $('#btnOpenQueryVendorCodeDialog').button({
        icons: { primary: 'ui-icon-search' }
    }); 
    $("#btnShow").button({
        label: "Show",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnBack").button({
        label: "Back",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnSave").button({
        label: "保存/變更",
        icons: { primary: "ui-icon-pencil" }
    });
    setBasicEvent("MRB");

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var cn = ['SID',
              '编号',               
               '申请日期',                
                '料号',
                '品名',
                
                '供应商',
                '批量',
                '判退/特採原因',
                
                '申请人',
                'MRB决议',
                '是否產生費用',
                '扣款單號',
                'MRB執行結果'

    ];
    var cm = [
            { name: 'SID', index: 'SID', width: 200, sortable: false, hidden: true },
            { name: 'MRBNo', index: 'MRBNo', width: 150, sortable: true, sorttype: 'text' },
            { name: 'CreateTime', index: 'CreateTime', width: 100, sortable: true, sorttype: 'date' },
            { name: 'LitNo', index: 'LitNo', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Size', index: 'Size', width: 150, sortable: true, sorttype: 'text' },
            { name: 'VenderCode', index: 'VenderCode', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Batch', index: 'Batch', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Baddescription', index: 'Baddescription', width: 150, sortable: true, sorttype: 'text' },
           
            { name: 'Initiator', index: 'Initiator', width: 150, sortable: true, sorttype: 'text' },
            { name: 'meettype', index: 'meettype', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Ieisproduce', index: 'Ieisproduce', width: 150, sortable: true, sorttype: 'text' },
            { name: 'IEDeductionsorder', index: 'IEDeductionsorder', width: 150, sortable: true, sorttype: 'text' },
            { name: 'NOTE', index: 'NOTE', width: 150, sortable: true, sorttype: 'text' }
    ];

    
    var gridDataList = $("#gridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMMRB/LoadSQMMRBJSonWithFilter',
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
        width: 1500,
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
    $('#table1').hide();
    $('#table2').hide();
    $('#table3').hide();
    $('#back').hide();
    $('#btnCreate').hide();
});
