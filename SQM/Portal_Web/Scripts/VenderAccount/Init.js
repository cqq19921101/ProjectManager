$(function () {
   
    //Toolbar Buttons
    $("#btnSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
  


    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var gridDataList = $("#gridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/VenderAccount/LoadJSonWithFilter',
        postData: { SearchText1: escape($.trim($("#txtFilterText1").val())),SearchText2: escape($.trim($("#txtFilterText2").val())), },
        type: "post",
        datatype: "json",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        width: 900,
        height: "auto",
        colNames: [
                   '供应商编码',
                   '供应商账号',
                   '中文名称',
                   '英文名称',
                   '电子邮箱'
                    ],
        colModel: [
     
           { name: 'VendorCode', index: 'VendorCode', width: 150, sortable: true, sorttype: 'text' },
            { name: 'AccountID', index: 'AccountID', width: 150, sortable: true, sorttype: 'text' },
            { name: 'NameInChinese', index: 'NameInChinese', width: 150, sortable: true, sorttype: 'text' },
            { name: 'NameInEnglish', index: 'NameInEnglish', width: 150, sortable: true, sorttype: 'text' },
            { name: 'PrimaryEmail', index: 'PrimaryEmail', width: 200, sortable: true, sorttype: 'text' },
       
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
