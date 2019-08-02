$(function () {
  
    //Toolbar Buttons
    $("#btnSQMSQEPURSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnSQMSQEPURCreate").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnSQMSQEPUREdit").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnSQMSQEPURDelete").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });



    $('#btnOpenQueryPlantDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQuerySourceDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQueryVendorCodeDialog').button({
        icons: { primary: 'ui-icon-search' }
    });
    $('#btnOpenQueryMGDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQueryRDDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQueryRDSDialog').button({
        icons: { primary: 'ui-icon-search' }
    });


    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var gridDataList = $("#SQMSQEPURgridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMSQEPUR/LoadSQMSQEPUR',
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
        width: 1200,
        height: "auto",
        colNames: [
            '供應商代碼',
                    '供應商名稱',
                    'PlantCode',
                   '部門名稱',
                   'SourcerGUID',
                   'SOURCING經理',
                   'SQEGUID',
                   'SQE經理',
				  'RDGUID',
				   'RD經理',
                   'RDSGUID',
                    'RDS經理'
               ],
        colModel: [
            { name: 'VendorCode', index: 'VendorCode', width: 200, sortable: false },
            { name: 'ERP_VNAME', index: 'ERP_VNAME', width: 500, sortable: false },
            { name: 'PlantCode', index: 'PlantCode', width: 150, sortable: true, sorttype: 'text',hidden:true},
            { name: 'PLANT_NAME', index: 'PLANT_NAME', width: 500, sortable: true, sorttype: 'text' },
            { name: 'SourcerGUID', index: 'SourcerGUID', width: 150, sortable: true, sorttype: 'text',hidden:true },
            { name: 'SourcerName', index: 'SourcerName', width: 150, sortable: true, sorttype: 'text' },
            { name: 'SQEGUID', index: 'SQEGUID', width: 150, sortable: true, sorttype: 'text', hidden: true },
            { name: 'SQENAME', index: 'SQENAME', width: 150, sortable: true, sorttype: 'text' },
            { name: 'RDGUID', index: 'RDGUID', width: 150, sortable: true, sorttype: 'text', hidden: true },
            { name: 'RDNAME', index: 'RDNAME', width: 150, sortable: true, sorttype: 'text' },
            { name: 'RDSGUID', index: 'RDSGUID', width: 150, sortable: true, sorttype: 'text', hidden: true },
            { name: 'RDSNAME', index: 'RDSNAME', width: 150, sortable: true, sorttype: 'text' },

        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'Provider',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#SQMSQEPURgridListPager',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataList.jqGrid('navGrid', '#SQMSQEPURgridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#SQMSQEPURtbMain1').show();
    $('#SQMSQEPURdialogData').show();
});
