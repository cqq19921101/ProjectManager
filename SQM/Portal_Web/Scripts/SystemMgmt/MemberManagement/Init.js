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
    $("#btnSetRole").button({
        label: "Set Roles",
        icons: { primary: "ui-icon-person" }
    });

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
                    'PrimaryEmail'],
        colModel: [
            { name: 'AccountGUID', index: 'AccountGUID', width: 200, sortable: false, hidden: true },
            { name: 'AccountID', index: 'AccountID', width: 150, sortable: true, sorttype: 'text' },
            { name: 'MemberType', index: 'MemberType', width: 120, sortable: true, sorttype: 'text' },
            { name: 'NameInChinese', index: 'NameInChinese', width: 105, sortable: true, sorttype: 'text' },
            { name: 'NameInEnglish', index: 'NameInEnglish', width: 105, sortable: true, sorttype: 'text' },
            { name: 'PrimaryEmail', index: 'PrimaryEmail', width: 150, sortable: false }
        ],
        rowNum: 10,
        rowList: [10, 20, 30],
        sortname: 'AccountID',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridListPager',
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
    $('#dialogMemberRoles').show();
});
