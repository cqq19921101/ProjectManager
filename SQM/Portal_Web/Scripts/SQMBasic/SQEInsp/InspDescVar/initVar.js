$(function () {
    //Toolbar Buttons
    $("#btnSearchDescVar").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCreateDescVar").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnViewEditDescVar").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnDeleteDescVar").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });
    $("#btnBackVar").button({
        label: "Back",
        icons: { primary: "ui-icon-pencil" }
    });
    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });
    //Data List
    var gridDataListDescVariables = $("#gridDataListDescVariables");
    gridDataListDescVariables.jqGrid({
        url: __WebAppPathPrefix + '/SQMBasic/LoadInspMap',
        postData: {
            SearchText: ""
            , SID: $("#dialogDataCode").attr('SID')
            , Insptype: 'Variables'
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
                    , '規格UCL'
                    , '規格LCL'
                    , '檢驗工具'
                    , 'AQL'
                    , '檢驗數'
        ],
        colModel: [
            { name: 'SID', index: 'SID', width: 100, sorttype: 'text', hidden: true },
            { name: 'SSID', index: 'SSID', width: 150, sortable: true, sorttype: 'text', hidden: true },
            { name: 'InspCode', index: 'InspCode', width: 150, sortable: true, sorttype: 'text', hidden: true },
             { name: 'InspDesc', index: 'InspDesc', width: 150, sortable: true, sorttype: 'text' },
            { name: 'UCL', index: 'UCL', width: 150, sortable: true, sorttype: 'text' },
            { name: 'LCL', index: 'LCL', width: 150, sortable: true, sorttype: 'text' },
            { name: 'InspTool', index: 'InspTool', width: 150, sortable: true, sorttype: 'text' },
            { name: 'AQL', index: 'AQL', width: 150, sortable: true, sorttype: 'text' },
            { name: 'InspNum', index: 'InspNum', width: 150, sortable: true, sorttype: 'text',hidden: true },
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'InspCode',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridListPagerDescVariables',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
            //var count = parseInt(gridDataListDescVariables.getGridParam("reccount"));
            //if (count>0) {
            //    $("#btnCreateDescVar").hide();
            //} else {
            //    $("#btnCreateDescVar").show();
            //}
        }
    });
    gridDataListDescVariables.jqGrid('navGrid', '#gridListPagerDescVariables', { edit: false, add: false, del: false, search: false, refresh: false });

})