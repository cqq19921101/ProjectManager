$(function () {

    //Toolbar Buttons
    $("#btnSearch").button({
        label: "Search Member",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnRunAs").button({
        label: "Run As Member",
        icons: { primary: "ui-icon-person" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var gridDataList = $("#gridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SystemMgmt/LoadMemberJSonWithFilter',
        postData: { SearchText: "", MemberType: "" },
        type: "post",
        datatype: "json",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        height: "auto",
        colNames: ['Account GUID',
                    'Account ID',
                    'Member Type',
                    'Name (Chinese)',
                    'Name (English)',
                    'PrimaryEmail',
                    'Password Hash'],
        colModel: [
            { name: 'AccountGUID', index: 'AccountGUID', width: 200, sortable: false, hidden: true },
            { name: 'AccountID', index: 'AccountID', width: 150, sortable: true, sorttype: 'text' },
            { name: 'MemberType', index: 'MemberType', width: 120, sortable: true, sorttype: 'text' },
            { name: 'NameInChinese', index: 'NameInChinese', width: 105, sortable: true, sorttype: 'text' },
            { name: 'NameInEnglish', index: 'NameInEnglish', width: 105, sortable: true, sorttype: 'text' },
            { name: 'PrimaryEmail', index: 'PrimaryEmail', width: 150, sortable: false },
            { name: 'PasswordHash', index: 'PasswordHash', width: 80, sortable: false, hidden: true }
        ],
        rowNum: 10,
        rowList: [10, 20, 30],
        sortname: 'AccountID',
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
});