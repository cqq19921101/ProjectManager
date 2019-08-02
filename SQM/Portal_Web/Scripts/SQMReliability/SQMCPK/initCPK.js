$(function () {
    //Toolbar Buttons
    $("#btnSearchCPK").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCreateCPK").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnViewEditCPK").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnDeleteCPK").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });
    $("#btnShowCPKSub").button({
        label: "ShowCPKSub",
        icons: { primary: "ui-icon-search" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });
    //Data List
    var gridDataListCPK = $("#gridDataListCPK");
    gridDataListCPK.jqGrid({
        url: __WebAppPathPrefix + '/SQMReliability/LoadCPK',
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
            'MemberGUID'
            ,'報告編號'
            ,'光寶料號'
            ,'供應商'
            
            ,'工具編號'
            ,'單位'
            ,'復查'
            ,'審核'
            , '製表'
            ,'備註'
            , 'ERP_VNAME'
        ],
        colModel: [
            { name: 'MemberGUID', index: 'MemberGUID', width: 100, sorttype: 'text', hidden: true },
            { name: 'reportID', index: 'reportID', width: 100, sortable: true, sorttype: 'text'},
            { name: 'plantCode', index: 'plantCode', width: 100, sortable: true, sorttype: 'text' },
            { name: 'Supplier', index: 'Supplier', width: 160, sortable: true, sorttype: 'text' },
          
            { name: 'ToolNumber', index: 'ToolNumber', width: 100, sortable: true, sorttype: 'text' },
            { name: 'Inches', index: 'Inches', width: 100, sortable: true, sorttype: 'text' },
            { name: 'Revision', index: 'Revision', width: 100, sortable: true, sorttype: 'text' },
            { name: 'Reviewer', index: 'Reviewer', width: 100, sortable: true, sorttype: 'text' },
            { name: 'Prepared', index: 'Prepared', width: 100, sortable: true, sorttype: 'text' },
            { name: 'ReMark', index: 'ReMark', width: 100, sortable: true, sorttype: 'text' },
            { name: 'ERP_VNAME', index: 'ERP_VNAME', width: 100, sortable: true, sorttype: 'text',hidden:true}
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'Name',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridListPagerCPK',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataListCPK.jqGrid('navGrid', '#gridListPagerCPK', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#tbMain1CPK').show();

    $('#dialogDataCPK').hide();
    $('#dialogDataCPKSub').hide();
    $('#inspCPKSub').hide();
    $('#inspCPKData').hide();
    

})