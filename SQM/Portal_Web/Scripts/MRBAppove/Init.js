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
    $("#btnShow").button({
        label: "Show",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnMapSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnMapBack").button({
        label: "Back",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnMapCreate").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnMapViewEdit").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnMapDelete").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var gridDataList = $("#gridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/MRBAppove/LoadJSonWithFilter',
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
        width: 800,
        height: "auto",
        colNames: [ 'SID',
                   
                   '負責單位'
             
                    ],
        colModel: [
            { name: 'SID', index: 'SID', width: 200, sortable: false, hidden: true },
           
            { name: 'DepartmentType', index: 'DepartmentType', width: 150, sortable: true, sorttype: 'text' },
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'Provider',
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
