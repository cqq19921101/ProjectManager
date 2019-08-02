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
    $("#btnQAApprove").button({
        label: "QAApprove",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnQAReject").button({
        label: "QAReject",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnReject").button({
        label: "Reject",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnRelease").button({
        label: "Release",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnSave1").button({
        label: "保存/變更",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnSave2").button({
        label: "保存/變更",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnSave3").button({
        label: "保存/變更",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnDelete").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var cn = ['SID',
              '编号',
              '申请日期',                
              '供应商',
              '料号', 
              '周期',
             'HOLD数量',
             'HOLD天数',
             '申请人',
             '状态',
             'ReleaseQuantity',
             'RejectQuantity',

    ];
    var cm = [
            { name: 'SID', index: 'SID', width: 200, sortable: false, hidden: true },
            { name: 'HoldNo', index: 'HoldNo', width: 150, sortable: true, sorttype: 'text' },
            { name: 'CreatTime', index: 'CreatTime', width: 150, sortable: true, sorttype: 'text', formatter: 'date', formatoptions: { srcformat: 'd/m/Y', newformat: 'd/m/Y' } },
            { name: 'vender', index: 'vender', width: 150, sortable: true, sorttype: 'text' },
            { name: 'LitNo', index: 'LitNo', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Period', index: 'Period', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Quantity', index: 'Quantity', width: 150, sortable: true, sorttype: 'text' },
            { name: 'HoldDays', index: 'HoldDays', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Owner', index: 'Owner', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Status', index: 'Status', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ReleaseQuantity', index: 'ReleaseQuantity', width: 150, sortable: true, sorttype: 'text' },
            { name: 'RejectQuantity', index: 'RejectQuantity', width: 150, sortable: true, sorttype: 'text' },
    ];

    
    var gridDataList = $("#gridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/Hold/LoadJSonWithFilter',
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
        width: 1000,
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
});
