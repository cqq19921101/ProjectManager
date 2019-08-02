$(function () {
    var gridDataList = $("#gridDataList");

    var dialog = $("#dialogFunctionRoles");
    var gridFunctionRoleList = $("#gridFunctionRoleList");

    //initial dialog
    dialog.dialog({
        autoOpen: false,
        height: 420,
        width: 510,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                //set all rows finish editing
                var $this = gridFunctionRoleList, ids = $this.jqGrid('getDataIDs'), i, l = ids.length;
                for (i = l; i > -1; i--) { $this.jqGrid('saveRow', ids[i], { successfunc: function (response) { return true; } }); }

                //Read all role data and retrieve checked roles
                var d = "";
                var r = gridFunctionRoleList.jqGrid('getGridParam', 'data');
                var MemberRoles = [];
                for (var iCnt = 0; iCnt < r.length; iCnt++) {
                    //alert(r[iCnt].RoleName + " - " + r[iCnt].Grants + " - " + (r[iCnt].Grants == true));
                    if ((r[iCnt].Grants == true) || (r[iCnt].Grants == 'true'))
                        d += "," + r[iCnt].RoleGUID;
                }
                if (d.length > 0) d = d.substr(1, d.length);

                //Update back to grid
                if (d == "")
                    gridDataList.jqGrid('setCell', gridDataList.jqGrid('getGridParam', 'selrow'), "Roles", null);
                else
                    gridDataList.jqGrid('setCell', gridDataList.jqGrid('getGridParam', 'selrow'), "Roles", d);
                $(this).dialog("close");
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () { }
    });

    //Init Member Role List (jqGrid)
    gridFunctionRoleList.jqGrid({
        url: __WebAppPathPrefix + '/SystemMgmt/GetFunctionRoles',
        mtype: "post",
        datatype: "json",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        //datatype: "local",
        width: 400,
        height: "auto",
        colNames: ['Grants',
                    'Role GUID',
                    'Role Name'],
        colModel: [
            { name: 'Grants', index: 'Grants', sortable: true, width: 90, editable: true, formatter: 'checkbox', edittype: 'checkbox', editoptions: { value: "true:false" }, align: 'center' },
            { name: 'RoleGUID', index: 'RoleGUID', sortable: false, hidden: true, editable: false },
            { name: 'RoleName', index: 'RoleName', sortable: true, width: 210, sorttype: 'text', editable: false }
        ],
        rowNum: 10,
        viewrecords: true,
        loadonce: true,
        pager: '#gridFunctionRoleListPager',
        //multiselect: true,
        onPaging: function (p) {
            //set all rows finish editing
            var $this = gridFunctionRoleList, ids = $this.jqGrid('getDataIDs'), i, l = ids.length;
            for (i = l; i > -1; i--) { $this.jqGrid('saveRow', ids[i], { successfunc: function (response) { return true; } }); }
        },
        gridComplete: function () {
            //all row in inline edit mode
            var $this = gridFunctionRoleList, ids = $this.jqGrid('getDataIDs'), i, l = ids.length;
            for (i = l; i > -1; i--) { $this.jqGrid('editRow', ids[i], true); }
        }
    });
    gridFunctionRoleList.jqGrid('navGrid', '#gridFunctionRoleListPager', { edit: false, add: false, del: false, search: false, refresh: false });
});