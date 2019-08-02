function getAgentsProductVendor() {
    //Toolbar Buttons
    $("#btnSearchA").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCreateA").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnViewEditA").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnDeleteA").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });
    $("#btnSearchA2").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCreateA2").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnViewEditA2").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnDeleteA2").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });
    $("#btnPlace").button({
        label: "供貨商原廠地址填寫",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnPlaceA2").button({
        label: "隱藏",
        icons: { primary: "ui-icon-plus" }
    });


    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var gridDataListA = $("#gridDataListA");
    var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
    var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
    var BasicInfoGUID = gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID;
        //try {
        //    $("#gridDataListA").jqGrid("GridUnload");
        //} catch (e) {

        //}
    gridDataListA.jqGrid({
        url: __WebAppPathPrefix + '/SQMBasic/GetAgentsDetailLoad',
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
        width: "auto",
        height: "auto",
        colNames: [
                    'BasicInfoGUID',
                    'PrincipalProducts',
                    'RevenuePer',
                    'MOQ',
                    'SampleTime',
                    'LeadTime',
                    'ProductBrand',
                    'SupAndOriName',
                    'SupAndOriPlace',
                    'OfferProxyCertify',
                    //'OfferProxyFGUID',
                    'MajorCompetitor',
                    'FileUrlTag',
                    'FileName'
        ],
        colModel: [
            { name: 'BasicInfoGUID', index: 'BasicInfoGUID', width: 100, sorttype: 'text',hidden:true },
            { name: 'PrincipalProducts', index: 'PrincipalProducts', width: 150, sortable: true, sorttype: 'text' },
            { name: 'RevenuePer', index: 'RevenuePer', width: 80, sortable: true, sorttype: 'text' },
            { name: 'MOQ', index: 'MOQ', width: 100, sortable: true, sorttype: 'text' },
            { name: 'SampleTime', index: 'SampleTime', width: 80, sortable: true, sorttype: 'text' },
            { name: 'LeadTime', index: 'LeadTime', width: 80, sortable: true, sorttype: 'text' },
            { name: 'ProductBrand', index: 'ProductBrand', width: 100, sorttype: 'text' },
            { name: 'SupAndOriName', index: 'SupAndOriName', width: 150, sortable: true, sorttype: 'text' },
            { name: 'SupAndOriPlace', index: 'SupAndOriPlace', width: 100, sortable: true, sorttype: 'text' },
            { name: 'OfferProxyCertify', index: 'OfferProxyCertify', width: 100, sortable: true, sorttype: 'text' },
            //{ name: 'OfferProxyFGUID', index: 'OfferProxyFGUID', width: 100, sortable: true, sorttype: 'text' },
            { name: 'MajorCompetitor', index: 'MajorCompetitor', width: 100, sortable: true, sorttype: 'text' },
            { name: 'FileUrlTag', index: 'FileUrlTag', "width": 80, sortable: true, sorttype: 'text' },
            { name: 'FileName', "index": 'FileName', "width": 80, "sortable": false,"hidden":true}


        ],
        rowNum: 20,
        //rowList: [10, 20, 30],
        sortname: 'PrincipalProducts',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridListPagerA',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataListA.jqGrid('navGrid', '#gridListPagerA', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#tbMain1A').show();
    $('#dialogDataA').show();







    var gridDataListA2 = $("#gridDataListA2");
    gridDataListA2.jqGrid({
        url: __WebAppPathPrefix + '/SQMBasic/LoadAgents2',
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
        width: "auto",
        height: "auto",
        colNames: [
            'BasicInfoGUID',
                    'PrincipalProducts',
                    'FactoryName',
                    'ProductBrand',
                    'FactoryAddress',
                    'FactoryNum',
                    'FactoryDate',
                    'ProductLine'
        ],
        colModel: [
            { name: 'BasicInfoGUID', index: 'BasicInfoGUID', width: 100, sorttype: 'text',hidden:true },
            { name: 'PrincipalProducts', index: 'PrincipalProducts', width: 150, sortable: true, sorttype: 'text' },
            { name: 'FactoryName', index: 'FactoryName', width: 80, sortable: true, sorttype: 'text' },
            { name: 'ProductBrand', index: 'ProductBrand', width: 100, sortable: true, sorttype: 'text' },
            { name: 'FactoryAddress', index: 'FactoryAddress', width: 150, sortable: true, sorttype: 'text' },
            { name: 'FactoryNum', index: 'FactoryNum', width: 80, sortable: true, sorttype: 'text' },
            { name: 'FactoryDate', index: 'FactoryDate', width: 180, sorttype: 'text' },
            { name: 'ProductLine', index: 'ProductLine', width: 150, sortable: true, sorttype: 'text' }
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'PrincipalProducts',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridListPagerA2',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataListA2.jqGrid('navGrid', '#gridListPagerA2', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#dialogDataA2').show();
}



