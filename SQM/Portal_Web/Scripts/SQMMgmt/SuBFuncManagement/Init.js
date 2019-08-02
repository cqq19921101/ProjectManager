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

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var cn = ['子功能 GUID',
              '子功能名稱'];
    var cm = [
            { "name": 'SubFuncGUID', "index": 'SubFuncGUID', "width": 200, "sortable": false, "hidden": false },
            { name: 'SubFuncName', index: 'SubFuncName', width: 150, sortable: true, sorttype: 'text' }
    ];

    
    var gridDataList = $("#gridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMMgmt/LoadSubFuncJSonWithFilter',
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
        width: 300,
        height: "auto",
        colNames: cn,
        colModel: cm,
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'SubFuncName',
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
