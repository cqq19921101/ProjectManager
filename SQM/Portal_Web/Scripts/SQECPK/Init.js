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
    var cn = ['spc_id',
                'Lite-on',
                'spc_item',
                'usl',
                'lsl',
                'sl',
                'ucl',
                'lcl',
                'update_time',
                'cpk',
                 'check_6u',
                'check_6d',
                'check_9m',
                'sample',
                'datum'
              ];
    var cm = [
        
             { name: 'spc_id', index: 'spc_id', width: 200, sortable: true, hidden: true },
                { name: 'LitNo', index: 'LitNo', width: 250, sortable: true, sorttype: 'text' },
                { name: 'spc_item', index: 'spc_item', width:400, sortable: true, sorttype: 'text' },
               
             
                { name: 'usl', index: 'usl', width: 100, sortable: true, sorttype: 'text' },
                { name: 'lsl', index: 'lsl', width: 100, sortable: true, sorttype: 'text' },
                { name: 'sl', index: 'sl', width: 100, sortable: true, sorttype: 'text' },
                { name: 'ucl', index: 'ucl', width: 100, sortable: true, sorttype: 'text' },
                { name: 'lcl', index: 'lcl', width: 100, sortable: true, sorttype: 'text' },
                { name: 'update_time', index: 'update_time', width: 150, sortable: true, sorttype: 'text' },
                { name: 'cpk', index: 'cpk', width: 150, sortable: true, sorttype: 'text' },
                { name: 'check_6u', index: 'check_6u', width: 150, sortable: true, sorttype: 'text' },
                { name: 'check_6d', index: 'check_6d', width: 150, sortable: true, sorttype: 'text' },
                { name: 'check_9m', index: 'check_9m', width: 150, sortable: true, sorttype: 'text' },
                { name: 'sample', index: 'sample', width: 150, sortable: true, sorttype: 'text' },
                { name: 'datum', index: 'datum', width: 150, sortable: true, sorttype: 'text' }
    ];

    
    var gridDataList = $("#gridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQECPK/LoadCPK',
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
        sortname: 'spc_item',
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
