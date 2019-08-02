$(function () {
    //Toolbar Buttons
    $("#btnSearchCPKSub").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCreateCPKSub").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnViewEditCPKSub").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnDeleteCPKSub").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });
    $("#btnShowCPKSub").button({
        label: "ShowCPKSub",
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
    var gridDataListCPKSub = $("#gridDataListCPKSub");
    gridDataListCPKSub.jqGrid({
        url: __WebAppPathPrefix + '/SQMReliability/LoadCPKSub',
        postData: {
            SearchText: ""
            , plantCode: $("#dialogDataCPK").attr('plantCode')
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
        width: "1350",
        height: "auto",
        colNames: [
            'SID'
            ,'plantCode'
            , '測量項目'
            , '單位'
            , '規格值'
            , '規格上限'
            , '規格下線'
            , '最大掌控值'
            , '最小掌控值'
            , '測量數據(pcs)'
            , '中心線'
            , 'CPK管控'
            , '區間'
            , '連續六點上升'
            , '連續六點下降'
            , '連續九點在中心線附近'
        ],
        colModel: [
            { name: 'SID', index: 'SID', width: 150, sorttype: 'text', hidden: true },
            { name: 'plantCode', index: 'plantCode', width: 150, sorttype: 'text', hidden: true },
            { name: 'Designator', index: 'Designator', width: 100, sortable: true, sorttype: 'text' },
            { name: 'Unit', index: 'Unit', width: 100, sortable: true, sorttype: 'text' },
            { name: 'Nominal', index: 'Nominal', width: 100, sortable: true, sorttype: 'text' },
            { name: 'maxNominal', index: 'maxNominal', width: 100, sortable: true, sorttype: 'text' },
            { name: 'minNominal', index: 'minNominal', width: 100, sortable: true, sorttype: 'text' },
            { name: 'UpperControlLimit', index: 'UpperControlLimit', width: 100, sortable: true, sorttype: 'text' },
            { name: 'LowerControlLimit', index: 'LowerControlLimit', width: 100, sortable: true, sorttype: 'text' }
            , { name: 'CTQNum', index: 'CTQNum', width: 100, sortable: true, sorttype: 'text' }
             , { name: 'centerline', index: 'centerline', width: 100, sortable: true, sorttype: 'text' }
             , { name: 'CPKManager', index: 'CPKManager', width: 100, sortable: true, sorttype: 'text' }
             , { name: 'section', index: 'section', width: 100, sortable: true, sorttype: 'text' }
             , { name: 'Sixpointrise', index: 'Sixpointrise', width: 100, sortable: true, sorttype: 'text' }
             , { name: 'Dropdown', index: 'Dropdown', width: 100, sortable: true, sorttype: 'text' }
             , { name: 'NineNearCenter', index: 'NineNearCenter', width: 150, sortable: true, sorttype: 'text' }
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'Name',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridListPagerCPKSub',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataListCPKSub.jqGrid('navGrid', '#gridListPagerCPKSub', { edit: false, add: false, del: false, search: false, refresh: false });

    //$('#tbMain1CPK').show();
    //$('#dialogDataCPK').show();

})