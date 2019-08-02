$(function () {

    //Toolbar Buttons
    $("#btnSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnAssignRoleMembers").button({
        label: "Assign Role Members",
        icons: { primary: "ui-icon-person" }
    });
    $("#btnAssignRoleAdmins").button({
        label: "Assign Role Admins",
        icons: { primary: "ui-icon-key" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var gridDataList = $("#gridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SystemMgmt/LoadRoleJSonWithFilter',
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
        width: 300,
        height: "auto",
        colNames: ['Role GUID',
                    'Role Name'],
        colModel: [
            { name: 'RoleGUID', index: 'RoleGUID', width: 200, sortable: false, hidden: true },
            { name: 'RoleName', index: 'RoleName', width: 150, sortable: true, sorttype: 'text' }
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'RoleName',
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
});

//For Assign Role Admins
$(function () {
    //Toolbar Buttons
    $("#btnsraFinish").button({
        label: "Finish",
        icons: { primary: "ui-icon-check" }
    });
    
    var gridDataList = $("#gridsraLeftResult");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SystemMgmt/LoadMemberJSonWithFilter',
        postData: { SearchText: "", MemberType: "", FieldSets: "s" },
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
                    'Name (Chinese)',
                    'Name (English)'],
        colModel: [
            { name: 'AccountGUID', index: 'AccountGUID', width: 200, sortable: false, hidden: true },
            { name: 'AccountID', index: 'AccountID', width: 150, sortable: true, sorttype: 'text' },
            { name: 'NameInChinese', index: 'NameInChinese', width: 105, sortable: true, sorttype: 'text' },
            { name: 'NameInEnglish', index: 'NameInEnglish', width: 105, sortable: true, sorttype: 'text' }
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'AccountID',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridsraLeftResultPager',
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataList.jqGrid('navGrid', '#gridsraLeftResultPager', { edit: false, add: false, del: false, search: false, refresh: false });

    var gridDataList = $("#gridsraRightResult");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SystemMgmt/LoadRoleAdminJSonWithFilter',
        postData: { RoleGUID: "", SearchText: "" },
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
        colNames: [ 'Account GUID',
                    'Remove',
                    'Account ID',
                    'Name (Chinese)',
                    'Name (English)'],
        colModel: [
            { name: 'AccountGUID', index: 'AccountGUID', width: 200, sortable: false, hidden: true },
            { name: 'Remove', index: 'Remove', width: 75, sortable: false },
            { name: 'AccountID', index: 'AccountID', width: 150, sortable: true, sorttype: 'text' },
            { name: 'NameInChinese', index: 'NameInChinese', width: 105, sortable: true, sorttype: 'text' },
            { name: 'NameInEnglish', index: 'NameInEnglish', width: 105, sortable: true, sorttype: 'text' }
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'AccountID',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridsraRightResultPager',
        gridComplete: function () {

            var $this = $(this);
            var ids = $this.jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                re = "<input type='button' value='Remove' onclick=\"RemoveARoleAdmin('" + $(this).jqGrid('getRowData', ids[i]).AccountGUID + "');\"  />";
                $this.jqGrid('setRowData', ids[i], { Remove: re });
            }
        },
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataList.jqGrid('navGrid', '#gridsraRightResultPager', { edit: false, add: false, del: false, search: false, refresh: false });
});

//For Assign Role Members
$(function () {
    //Toolbar Buttons
    $("#btnsrmFinish").button({
        label: "Finish",
        icons: { primary: "ui-icon-check" }
    });

    var gridDataList = $("#gridsrmLeftResult");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SystemMgmt/LoadMemberJSonWithFilter',
        postData: { SearchText: "", MemberType: "", FieldSets: "s" },
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
                    'Name (Chinese)',
                    'Name (English)'],
        colModel: [
            { name: 'AccountGUID', index: 'AccountGUID', width: 200, sortable: false, hidden: true },
            { name: 'AccountID', index: 'AccountID', width: 150, sortable: true, sorttype: 'text' },
            { name: 'NameInChinese', index: 'NameInChinese', width: 105, sortable: true, sorttype: 'text' },
            { name: 'NameInEnglish', index: 'NameInEnglish', width: 105, sortable: true, sorttype: 'text' }
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'AccountID',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridsrmLeftResultPager',
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataList.jqGrid('navGrid', '#gridsrmLeftResultPager', { edit: false, add: false, del: false, search: false, refresh: false });

    var gridDataList = $("#gridsrmRightResult");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SystemMgmt/LoadRoleMemberJSonWithFilter',
        postData: { RoleGUID: "", SearchText: "" },
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
                    'Remove',
                    'Account ID',
                    'Name (Chinese)',
                    'Name (English)'],
        colModel: [
            { name: 'AccountGUID', index: 'AccountGUID', width: 200, sortable: false, hidden: true },
            { name: 'Remove', index: 'Remove', width: 75, sortable: false },
            { name: 'AccountID', index: 'AccountID', width: 150, sortable: true, sorttype: 'text' },
            { name: 'NameInChinese', index: 'NameInChinese', width: 105, sortable: true, sorttype: 'text' },
            { name: 'NameInEnglish', index: 'NameInEnglish', width: 105, sortable: true, sorttype: 'text' }
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'AccountID',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridsrmRightResultPager',
        gridComplete: function () {

            var $this = $(this);
            var ids = $this.jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                re = "<input type='button' value='Remove' onclick=\"RemoveARoleMember('" + $(this).jqGrid('getRowData', ids[i]).AccountGUID + "');\"  />";
                $this.jqGrid('setRowData', ids[i], { Remove: re });
            }
        },
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () {
                        $this.triggerHandler('reloadGrid');
                    }, 50);
        }
    });
    gridDataList.jqGrid('navGrid', '#gridsrmRightResultPager', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#tbMain1').show();
});
