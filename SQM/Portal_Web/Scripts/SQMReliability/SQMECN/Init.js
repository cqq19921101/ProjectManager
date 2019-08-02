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

    setBasicEvent('ProposedChange', '1', false);
    setBasicEvent('DesignChange', '2', false);
    setBasicEvent('SupplierApproval', '3', false);
    //Data List
    var cn = ['SID',
              'MemberGUID',
              '編號',
              'ChangeTypeSID',
              '變更類型',
              '供應商',
              '電話',
              '起草人',
              '規格/料號',
              '品名',
              'ChangeItemTypeSID',
              '變更項目類型',
              '變更內容',
              '附件',
              '變更日期',
              '變更原因',
              '設計變更需求（確認/資格）',
              '附件',
              '消耗',
              '報廢',
              '重工',
              '全檢',
              '在製品',
              '庫存數量',
              '變更後環境影響評估',
              '是否重新提交PPAP',
              '供應商承認者',
              '職位',
              '要求日期',
              '簽核狀態',
              '簽核人員'
    ];
    var cm = [
            { name: 'SID', index: 'SID', width: 200, sortable: false, hidden: true },
            { name: 'MemberGUID', index: 'MemberGUID', width: 200, sortable: false, hidden: true },
            { name: 'TZDNo', index: 'TZDNo', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ChangeTypeSID', index: 'ChangeTypeSID', width: 200, sortable: false, hidden: true },
            { name: 'ChangeType', index: 'ChangeType', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Supplier', index: 'Supplier', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Phone', index: 'Phone', width: 150, sortable: true, sorttype: 'text' },
            { name: 'OriginatorName', index: 'OriginatorName', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Spec', index: 'Spec', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Description', index: 'Description', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ChangeItemTypeSID', index: 'ChangeItemTypeSID', width: 200, sortable: false, hidden: true },
            { name: 'ChangeItemType', index: 'ChangeItemType', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ProposedChange', index: 'ProposedChange', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ProposedChangeFile', index: 'ProposedChangeFile', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ProposedDate', index: 'ProposedDate', width: 150, sortable: true, sorttype: 'date', formatter: "date", formatoptions: { newformat: "Y/m/d" } },
            { name: 'ChangeReason', index: 'ChangeReason', width: 150, sortable: true, sorttype: 'text' },
            { name: 'DesignChange', index: 'DesignChange', width: 150, sortable: true, sorttype: 'text', formatter: function (cellValue, options, rowdata, action) { return rowdata.DesignChange == true ? 'Yes' : 'No'; } },
            { name: 'DesignChangeFile', index: 'DesignChangeFiel', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Consume', index: 'Consume', width: 150, sortable: true, sorttype: 'text', formatter: function (cellValue, options, rowdata, action) { return rowdata.Consume == true ? 'true' : 'false'; } },
            { name: 'Scrap', index: 'Scrap', width: 150, sortable: true, sorttype: 'text', formatter: function (cellValue, options, rowdata, action) { return rowdata.Scrap == true ? 'true' : 'false'; } },
            { name: 'Rework', index: 'Rework', width: 150, sortable: true, sorttype: 'text', formatter: function (cellValue, options, rowdata, action) { return rowdata.Rework == true ? 'true' : 'false'; } },
            { name: 'Sort', index: 'Sort', width: 150, sortable: true, sorttype: 'text', formatter: function (cellValue, options, rowdata, action) { return rowdata.Sort == true ? 'true' : 'false'; } },
            { name: 'WIP', index: 'WIP', width: 150, sortable: true, sorttype: 'text' },
            { name: 'QtyInStock', index: 'QtyInStock', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Environment', index: 'Environment', width: 150, sortable: true, sorttype: 'text', formatter: function (cellValue, options, rowdata, action) { return rowdata.Environment == true ? 'Yes' : 'No'; } },
            { name: 'PPAP', index: 'PPAP', width: 150, sortable: true, sorttype: 'text', formatter: function (cellValue, options, rowdata, action) { return rowdata.PPAP == true ? 'Yes' : 'No'; } },
            { name: 'SupplierApproval', index: 'SupplierApproval', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Title', index: 'Title', width: 150, sortable: true, sorttype: 'text' },
            { name: 'RequestDate', index: 'RequestDate', width: 150, sortable: true, sorttype: 'date', formatter: "date", formatoptions: { newformat: "Y/m/d" } },
            { name: 'Status', index: 'Status', width: 150, sortable: true, sorttype: 'text' },
            { name: 'NameInChinese', index: 'NameInChinese', width: 150, sortable: true, sorttype: 'text' },
    ];

    
    var gridDataList = $("#gridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMReliability/LoadECNJSonWithFilter',
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
        width: 1350,
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
