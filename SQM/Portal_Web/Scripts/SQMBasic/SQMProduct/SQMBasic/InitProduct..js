function getSQMProducesProduct () {
    //Toolbar Buttons
    $("#btnSearchP").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCreateP").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnViewEditP").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnDeleteP").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var gridDataListP = $("#gridDataListP");
    var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
    var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
    var BasicInfoGUID = gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID;
    gridDataListP.jqGrid({
        url: __WebAppPathPrefix + '/SQMBasic/LoadProduct',
        postData: { SearchText: "", BasicInfoGUID: BasicInfoGUID },
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
        colNames: [
                    'BasicInfoGUID',
                    '主要產品',
                    '占營業份額 百分比%',
                    'MOQ（Minimum Order Quantity ）',
                    '最快提供样品时间（天数）',
                    'L/T（Lead Time）（天数）',
                    '年产能',
                    '主要竞争对手公司名称'
                    , '产能空间'
        ],
        colModel: [
            { name: 'BasicInfoGUID', index: 'BasicInfoGUID', width: 100, sorttype: 'text', hidden: true },
            { name: 'PrincipalProducts', index: 'PrincipalProducts', width: 150, sortable: true, sorttype: 'text' },
            { name: 'RevenuePer', index: 'RevenuePer', width: 100, sortable: true, sorttype: 'text' },
            { name: 'MOQ', index: 'MOQ', width: 100, sortable: true, sorttype: 'text' },
            { name: 'SampleTime', index: 'SampleTime', width: 50, sortable: true, sorttype: 'text' },
            { name: 'LeadTime', index: 'LeadTime', width: 50, sortable: true, sorttype: 'text' },
            { name: 'AnnualCapacity', index: 'AnnualCapacity', width: 100, sortable: true, sorttype: 'text' },
            { name: 'MajorCompetitor', index: 'MajorCompetitor', width: 150, sortable: true, sorttype: 'text' }
            , { name: 'AnnualCapacitySpan', index: 'AnnualCapacitySpan', width: 150, sortable: true, sorttype: 'text' }
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'PrincipalProducts',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridListPagerP',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataListP.jqGrid('navGrid', '#gridListPagerP', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#tbMain1P').show();
    $('#dialogDataP').show();
    //$("#btnSearchP").click();

}