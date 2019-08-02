$(function () {
    //Toolbar Buttons
    $("#btnSearchDesc").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCreateDesc").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnViewEditDesc").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnDeleteDesc").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });
    $("#btnShowDesc").button({
        label: "ShowDesc",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnBack").button({
        label: "Back",
        icons: { primary: "ui-icon-pencil" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });
    //Data List
    var gridDataListDesc = $("#gridDataListDesc");
    gridDataListDesc.jqGrid({
        url: __WebAppPathPrefix + '/SQMInsp/LoadInspMap',
        postData: {
            SearchText: ""
            , SID: $("#dialogDataCode").attr('SID')
        },
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
                    'SID'
                    , 'SSID'
                    , '檢測項目:'
                    , '檢測說明'
                    , '判定標準'
                    , 'CR'
                    , 'MA'
                    , 'MI'
                    , '自定義項'
                    , '檢測量'
                    , 'isOther'
        ],
        colModel: [
            { name: 'SID', index: 'SID', width: 100, sorttype: 'text', hidden: true },
            { name: 'SSID', index: 'SSID', width: 150, sortable: true, sorttype: 'text', hidden: true },
            { name: 'InspCode', index: 'InspCode', width: 150, sortable: true, sorttype: 'text', hidden: true },
            { name: 'InspDesc', index: 'InspDesc', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Standard', index: 'Standard', width: 150, sortable: true, sorttype: 'text' },
            { name: 'CR', index: 'CR', width: 150, sortable: true, sorttype: 'text' },
            { name: 'MA', index: 'MA', width: 150, sortable: true, sorttype: 'text' },
            { name: 'MI', index: 'MI', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Other', index: 'Other', width: 150, sortable: true, sorttype: 'text' },
            { name: 'InspNum', index: 'InspNum', width: 150, sortable: true, sorttype: 'text' },
            { name: 'isOther', index: 'isOther', width: 150, sortable: true, sorttype: 'text', hidden: true }
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'Name',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridListPagerCode',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataListDesc.jqGrid('navGrid', '#gridListPagerCode', { edit: false, add: false, del: false, search: false, refresh: false });

    //$('#tbMain1Code').show();
    //$('#dialogDataCode').show();

})