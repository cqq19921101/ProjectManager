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

    $("#btnExcle1").button({
        label: "导出月度清单",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnExcle2").button({
        label: "导出年度清单",
        icons: { primary: "ui-icon-pencil" }
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
    $("#btnSave2").button({
        label: "保存/變更",
        icons: { primary: "ui-icon-pencil" }
    });

 
    
    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });
    $('input').width(100);
    //Data List
    var cn = ['SID',
              '编号',
              '供应商',
              '总分',
              '等级'
    ];
    var cm = [
            { name: 'SID', index: 'SID', width: 200, sortable: false, hidden: true },
            { name: 'BenbfitNo', index: 'BenbfitNo', width: 150, sortable: true, sorttype: 'text' },
            { name: 'VendorCode', index: 'VendorCode', width: 150, sortable: true, sorttype: 'text' },
            { name: 'TotolScore', index: 'TotolScore', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Class', index: 'Class', width: 150, sortable: true, sorttype: 'text' }
    ];

    
    var gridDataList = $("#gridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMBenefit/LoadJSonWithFilter',
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
    $('#tb1').show();
    $('#tb2').hide();
 
});
