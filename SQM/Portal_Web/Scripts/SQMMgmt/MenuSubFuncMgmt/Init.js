$(function () {

    //btnAssignMenuSubFunc
    $("#btnAssignMenuSubFunc").button({
        label: "AssignSubFunc"//,
        //icons: { primary: "ui-icon-person" }
    });
    $("#btnAssignRoleMenuSubFunc").button({
        label: "AssignRoleSubFunc"//,
        //icons: { primary: "ui-icon-person" }
    });
    $("#btnsrmFinish").button({
        label: "Finish"//,
        //icons: { primary: "ui-icon-person" }
    });
    $("#btnrmfFinish").button({
        label: "Finish"//,
        //icons: { primary: "ui-icon-person" }
    });
    
    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var gridDataList = $("#gridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMMgmt/LoadMenuJSon',
        mtype: "post",
        datatype: "json",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },

        treeReader: {
            level: "level",
            parent_id_field: "parent",
            leaf_field: "isLeaf",
            expanded_field: "expanded"
        },

        height: "auto",
        colNames: ['Function GUID',
                    'Menu Level',
                    'SortCode',
                    'ParentFunctionGUID',
                    'Title_en_US',
                    'Title_zh_CN',
                    'Title_zh_TW',
                    'IntranetHref',
                    'InternetHref',
                    'HrefTarget',
                    'Roles',
                    'ControllerActions'],
        colModel: [
            { name: 'FunctionGUID', index: 'FunctionGUID', width: 200, sortable: false, hidden: true, key: true },
            { name: 'MenuLevel', index: 'MenuLevel', width: 150, sortable: false, hidden: true, sorttype: 'int' },
            { name: 'SortCode', index: 'SortCode', width: 105, sortable: true, hidden: true, sorttype: 'int' },
            { name: 'ParentFunctionGUID', index: 'ParentFunctionGUID', width: 150, sortable: false, hidden:true },
            { name: 'Title_en_US', index: 'Title_en_US', width: 200, sortable: false, hidden: false },
            { name: 'Title_zh_CN', index: 'Title_zh_CN', width: 200, sortable: false, hidden: false },
            { name: 'Title_zh_TW', index: 'Title_zh_TW', width: 200, sortable: false, hidden: false },
            { name: 'IntranetHref', index: 'IntranetHref', width: 200, sortable: true, sorttype: 'text' },
            { name: 'InternetHref', index: 'InternetHref', width: 200, sortable: true, sorttype: 'text' },
            { name: 'HrefTarget', index: 'HrefTarget', width: 60, sortable: true, sorttype: 'text', formatter: formatter_AnchorTarget },
            { name: 'Roles', index: 'Roles', width: 90, sortable: true, sorttype: 'text', hidden: true },
            { name: 'ControllerActions', index: 'ControllerActions', width: 290, sortable: true, sorttype: 'text', hidden: true }],
        rowNum: 9999,

        treeGrid: true,
        treeGridModel: 'adjacency',
        ExpandColumn: 'Title_en_US',
        treedatatype: "json",

        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridListPager',
        sortname: 'SortCode',
        loadComplete: function () { }
    });
    gridDataList.jqGrid('navGrid', '#gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    function formatter_AnchorTarget(cellvalue, options, rowObject) {
        if (cellvalue == "1")
            return "_self";
        else
            return "_blank";
    }

    function formatter_MenuRoles(cellvalue, options, rowObject) {
        if (cellvalue.length == 0)
            return "(n/a)";
        else
            return "Total " + cellvalue.length + " role(s).";
    }

    $('#tbMain1').show();
    $('#dialogData').show();
    //$('#dialogFunctionRoles').show();
    //$('#dialogFunctionControllerActions').show();
    //$('#dialogFunctionControllerActionsItem').show();
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
        url: __WebAppPathPrefix + '/SQMMgmt/LoadSubFuncsJSonWithFilter',
        postData: { SearchText: "",  FieldSets: "s" },
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
        colNames: ['SubFunc GUID',
                    'SubFunc Name'],
        colModel: [
            { name: 'SubFuncGUID', index: 'SubFuncGUID', width: 200, sortable: false, hidden: true },
            { name: 'SubFuncName', index: 'SubFuncName', width: 350, sortable: true, sorttype: 'text' }
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'SubFuncName',
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
        url: __WebAppPathPrefix + '/SQMMgmt/LoadSubFuncsMapJSonWithFilter',
        postData: { FunctionGUID: "", SearchText: "" },
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
        colNames: ['Function GUID',
                   'SubFunc GUID',
                   'Remove',
                    'SubFunc Name'],
        colModel: [
            { name: 'FunctionGUID', index: 'FunctionGUID', width: 200, sortable: false, hidden: true },
            
            { name: 'SubFuncGUID', index: 'SubFuncGUID', width: 75, sortable: false, hidden: true },
            { name: 'Remove', index: 'Remove', width: 75, sortable: false },
            { name: 'SubFuncName', index: 'SubFuncName', width: 350, sortable: true, sorttype: 'text' }
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'SubFuncName',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridsrmRightResultPager',
        gridComplete: function () {

            var $this = $(this);
            var ids = $this.jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                re = "<input type='button' value='Remove' onclick=\"RemoveASubFunc('" + $(this).jqGrid('getRowData', ids[i]).SubFuncGUID + "');\"  />";
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

//For Assign Role Menu Sub Funcs
$(function () {
    //Data List
    var gridRoleDataList = $("#gridRoleResult");
    gridRoleDataList.jqGrid({
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
            { name: 'RoleName', index: 'RoleName', width: 200, sortable: true, sorttype: 'text' }
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'RoleName',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridRoleResultPager',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        },
        onSelectRow: function (id) {
            var $this = $(this);
            var selRow = $this.jqGrid('getGridParam', 'selrow');

            if (selRow) {
                var rowData = $this.jqGrid('getRowData', selRow);
                //alert(rowData.RoleGUID);
                RefreshRoleSubFuncMapUIInfo();
            }
        },
    });
    gridRoleDataList.jqGrid('navGrid', '#gridRoleResultPager', { edit: false, add: false, del: false, search: false, refresh: false });


    var gridDataList = $("#gridrmfLeftResult");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMMgmt/LoadSubFuncsMapJSonWithFilter',
        postData: { FunctionGUID: "", SearchText: "" },
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
        colNames: ['Function GUID',
                   'SubFunc GUID',
                    'SubFunc Name'],
        colModel: [
            { name: 'FunctionGUID', index: 'FunctionGUID', width: 200, sortable: false, hidden: true },
            { name: 'SubFuncGUID', index: 'SubFuncGUID', width: 75, sortable: false, hidden: true },
            { name: 'SubFuncName', index: 'SubFuncName', width: 350, sortable: true, sorttype: 'text' }
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'SubFuncName',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridrmfLeftResultPager',
        gridComplete: function () {

            var $this = $(this);
            var ids = $this.jqGrid('getDataIDs');
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
    gridDataList.jqGrid('navGrid', '#gridrmfLeftResultPager', { edit: false, add: false, del: false, search: false, refresh: false });

    //right role menu func map
    var gridDataList = $("#gridrmfRightResult");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMMgmt/LoadRoleSubFuncsMapJSonWithFilter',
        postData: { FunctionGUID: "", RoleGUID: "", SearchText: "" },
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
        colNames: ['Function GUID',
                   'SubFunc GUID',
                   'Remove',
                    'SubFunc Name'],
        colModel: [
            { name: 'FunctionGUID', index: 'FunctionGUID', width: 200, sortable: false, hidden: true },

            { name: 'SubFuncGUID', index: 'SubFuncGUID', width: 75, sortable: false, hidden: true },
            { name: 'Remove', index: 'Remove', width: 75, sortable: false },
            { name: 'SubFuncName', index: 'SubFuncName', width: 350, sortable: true, sorttype: 'text' }
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'SubFuncName',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridrmfRightResultPager',
        gridComplete: function () {

            var $this = $(this);
            var ids = $this.jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                re = "<input type='button' value='Remove' onclick=\"RemoveARoleSubFunc('" + $(this).jqGrid('getRowData', ids[i]).SubFuncGUID + "');\"  />";
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
    gridDataList.jqGrid('navGrid', '#gridrmfRightResultPager', { edit: false, add: false, del: false, search: false, refresh: false });

});