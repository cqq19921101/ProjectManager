function getSQMTradersT() {
    //Toolbar Buttons
    $("#btnSearchT").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCreateT").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnViewEditT").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnDeleteT").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var gridDataListT = $("#gridDataListT");
    var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
    var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
    var BasicInfoGUID = gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID;
    gridDataListT.jqGrid({
        url: __WebAppPathPrefix + '/SQMTraders/GetTradersDetailLoad',
        postData: { "BasicInfoGUID": BasicInfoGUID },
        type: "post",
        datatype: "json",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        width:1306,
        height: "auto",
        colNames: [
            'BasicInfoGUID',
                    'PrincipalProducts',
                    'RevenuePer',
                    'MOQ',
                    'SampleTime',
                    'LeadTime',
                    'SupAndOriName',
                    'OfferPlaceCertify',
                    //'OfferPlaceFGUID',
                    'OfferSellCertify',
                    //'OfferSellFGUID',
                    'MajorCompetitor',
                    'FileUrlP',
                    'FileUrlS',
                    'FileName'
        ],
        colModel: [
            { name: 'BasicInfoGUID', index: 'BasicInfoGUID', width: 100, sorttype: 'text',hidden:true },
            { name: 'PrincipalProducts', index: 'PrincipalProducts', width: 150, sortable: true, sorttype: 'text' },
            { name: 'RevenuePer', index: 'RevenuePer', width: 80, sortable: true, sorttype: 'text' },
            { name: 'MOQ', index: 'MOQ', width: 100, sortable: true, sorttype: 'text' },
            { name: 'SampleTime', index: 'SampleTime', width: 80, sortable: true, sorttype: 'text' },
            { name: 'LeadTime', index: 'LeadTime', width: 80, sortable: true, sorttype: 'text' },
            { name: 'SupAndOriName', index: 'SupAndOriName', width: 100, sortable: true, sorttype: 'text' },
            { name: 'OfferPlaceCertify', index: 'OfferPlaceCertify', width: 100, sortable: true, sorttype: 'text' },
            //{ name: 'OfferPlaceFGUID', index: 'OfferPlaceFGUID', width: 100, sortable: true, sorttype: 'text' },
            { name: 'OfferSellCertify', index: 'OfferSellCertify', width: 100, sortable: true, sorttype: 'text' },
            //{ name: 'OfferSellFGUID', index: 'OfferSellFGUID', width: 103, sortable: true, sorttype: 'text' },
            { name: 'MajorCompetitor', index: 'MajorCompetitor', width: 100, sortable: true, sorttype: 'text' },
            { name: 'FileUrlP', index: 'FileUrlP', "width": 80, sortable: true, sorttype: 'text' },
            { name: 'FileUrlS', index: 'FileUrlS', "width": 80, sortable: true, sorttype: 'text' },
            { name: 'FileName', "index": 'FileName', "width": 80, "sortable": false, "hidden": true }
        ],
        rowNum: 20,
        //rowList: [10, 20, 30],
        sortname: 'PrincipalProducts',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridListPagerT',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataListT.jqGrid('navGrid', '#gridListPagerT', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#tbMain1T').show();
    $('#dialogDataT').show();
}
