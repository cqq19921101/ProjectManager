$(function () {
    //Toolbar Buttons
    $("#btnSearchCode").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCreateCode").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnViewEditCode").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnDeleteCode").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });
    $("#btnShowDesc").button({
        label: "ShowDesc",
        icons: { primary: "ui-icon-search" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });
    //Data List
    var gridDataListCode = $("#gridDataListCode");
    gridDataListCode.jqGrid({
        url: __WebAppPathPrefix + '/SQMInsp/LoadInsp',
        postData: { SearchText: ""},
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
                    ,'檢測項目InspCode:'
        ],
        colModel: [
            { name: 'SID', index: 'SID', width: 100, sorttype: 'text', hidden: true },
            { name: 'Name', index: 'Name', width: 150, sortable: true, sorttype: 'text' },
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
    gridDataListCode.jqGrid('navGrid', '#gridListPagerCode', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#tbMain1Code').show();

    $('#dialogDataCode').hide();
    $('#dialogDataDesc').hide();
    $('#inspDesc').hide();

    

})