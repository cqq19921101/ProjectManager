$(function () {
    //Toolbar Buttons
    $("#btnSearchCPKData").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCreateCPKData").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnViewEditCPKData").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnDeleteCPKData").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });
    $("#btnShowCPKData").button({
        label: "ShowCPKData",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnBackCPKData").button({
        label: "Back",
        icons: { primary: "ui-icon-pencil" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });
    //Data List
    var gridDataListCPKData = $("#gridDataListCPKData");
    gridDataListCPKData.jqGrid({
        url: __WebAppPathPrefix + '/SQMReliability/LoadCPKData',
        postData: {
            SearchText: ""
            , reportID: $("#dialogDataCPKSub").attr('reportID')
            , Designator: $("#dialogDataCPKSub").attr('Designator')
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
        width: "250",
        height: "auto",
        colNames: [
            'reportID'
            ,'產品CTQ'
            , 'Designator'
        ],
        colModel: [
            { name: 'reportID', index: 'reportID', width: 100, sorttype: 'text', hidden: true },
            { name: 'CTQ', index: 'CTQ', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Designator', index: 'Designator', width: 150, sortable: true, sorttype: 'text',hidden:true },
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'Name',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridListPagerCPKData',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
            CPKDataStatistic();
            ($("#spCTQNum").html() > 0) ?  $("#btnCreateCPKData").show():$("#btnCreateCPKData").hide() ;
        }
    });
    gridDataListCPKData.jqGrid('navGrid', '#gridListPagerCPKData', { edit: false, add: false, del: false, search: false, refresh: false });

    //$('#tbMain1CPK').show();
    //$('#dialogDataCPK').show();

})