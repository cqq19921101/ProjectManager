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


    $("#btnOver").button({
        label: "捞取资料",
        icons: { primary: "ui-icon-trash" }
    });
    $("#btnShow").button({
        label: "Show",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnBack").button({
        label: "Back",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnD2Save").button({
        label: "保存/變更",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnD9Save").button({
        label: "保存/變更",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnAppove1").button({
        label: "Appove",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnAppove2").button({
        label: "Appove",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnReject1").button({
        label: "Reject",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnReject2").button({
        label: "Reject",
        icons: { primary: "ui-icon-pencil" }
    });
    
    setBasicEvent("SCARD1");
    setBasicEvent("SCARD7");
    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var cn = ['SID',
              'scarNo',
               
               '创建时间',
                '供应商',
                '料号',
                '信息提报人',
                '结案状态'
    ];
    var cm = [
            { name: 'SID', index: 'SID', width: 200, sortable: false, hidden: true },
            { name: 'scarNo', index: 'scarNo', width: 150, sortable: true, sorttype: 'text' },
             { name: 'anomalousTime', index: 'anomalousTime', width: 150, sortable: true, sorttype: 'text' },
            { name: 'VenderCode', index: 'VenderCode', width: 150, sortable: true, sorttype: 'text' },
            { name: 'LitNo', index: 'LitNo', width: 150, sortable: true, sorttype: 'text' },
            { name: 'initiator', index: 'initiator', width: 150, sortable: true, sorttype: 'text' },
             { name: 'badnessPic', index: 'badnessPic', width: 150, sortable: true, sorttype: 'text' }
    ];

    
    var gridDataList = $("#gridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMMaterialMgmt/LoadSQMSCARJSonWithFilter',
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


    $('#tbMain1').show();
    $('#dialogData').show();
    $('#back').hide();
    $('#D9').hide();
    $('#D2').hide();
    $('#D3').hide();
    $('#D4').hide();
    $('#D5').hide();
    $('#D6').hide();
    $('#D7').hide();
    $('#D8').hide();
});
