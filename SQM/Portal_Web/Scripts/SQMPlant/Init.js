$(function () {
  
    //Toolbar Buttons
    $("#btnSQMPlantSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnSQMPlantCreate").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnSQMPlantDelete").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });
    $('#btnOpenQueryPlantDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQueryVendorCodeDialog').button({
        icons: { primary: 'ui-icon-search' }
    });
    $('#btnOpenQueryMGDialog').button({
        icons: { primary: 'ui-icon-search' }
    });
    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var gridDataList = $("#SQMPlantgridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMPlant/LoadPlantJSonWithFilter',
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
        colNames: ['PlantCode',
                    'MemberGUID',
                    '部門名稱',
                   '部門經理',
                   '郵箱'
                 
                  
             
                    ],
        colModel: [
            { name: 'PlantCode', index: 'PlantCode', width: 200, sortable: false, hidden: true },
            { name: 'MemberGUID', index: 'MemberGUID', width: 200, sortable: false, hidden: true },
            { name: 'PlantName', index: 'Plant_name', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Name', index: 'NAME', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Email', index: 'EMAIL', width: 150, sortable: true, sorttype: 'text' },
      
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'Provider',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#SQMPlantgridListPager',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataList.jqGrid('navGrid', '#SQMPlantgridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#SQMPlanttbMain1').show();
    $('#SQMPlantdialogData').show();
});
