$(function () {

    //Toolbar Buttons
    //$("#btnSearch").button({
    //    label: "Search Member",
    //    icons: { primary: "ui-icon-search" }
    //});
    $("#btnSubmit").button({
        label: "Full Update"//,
        //icons: { primary: "ui-icon-person" }
    });
    $("#btnCreateSibling").button({
        label: "Create Sibling"//,
        //icons: { primary: "ui-icon-person" }
    });
    $("#btnCreateChild").button({
        label: "Create Child"//,
        //icons: { primary: "ui-icon-person" }
    });
    $("#btnDelete").button({
        label: "Delete"//,
        //icons: { primary: "ui-icon-person" }
    });
    $("#btnEdit").button({
        label: "Edit"//,
        //icons: { primary: "ui-icon-person" }
    });
    $("#btnMoveUp").button({
        label: "Move Up"//,
        //icons: { primary: "ui-icon-person" }
    });
    $("#btnMoveDown").button({
        label: "Move Down"//,
        //icons: { primary: "ui-icon-person" }
    });
    $("#btnMoveLeft").button({
        label: "Level Up"//,
        //icons: { primary: "ui-icon-person" }
    });
    $("#btnMoveRight").button({
        label: "Level Down"//,
        //icons: { primary: "ui-icon-person" }
    });
    $("#btnSetRoles").button({
        label: "Set Roles"//,
        //icons: { primary: "ui-icon-person" }
    });
    $("#btnSetControllerActions").button({
        label: "Set Controller Actions"//,
        //icons: { primary: "ui-icon-person" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var gridDataList = $("#gridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SystemMgmt/LoadMenuJSon',
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
    $('#tbMain2').show();
    $('#dialogData').show();
    $('#dialogFunctionRoles').show();
    $('#dialogFunctionControllerActions').show();
    $('#dialogFunctionControllerActionsItem').show();
});