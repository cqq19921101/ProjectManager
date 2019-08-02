$(function () {
    var gridDataList = $("#gridDataList");

    var dialogFunctionRoles = $("#dialogFunctionRoles");
    var gridFunctionRoleList = $("#gridFunctionRoleList");

    jQuery("#btnSetRoles").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                gridFunctionRoleList.jqGrid('setGridParam', { datatype: 'json' });
                gridFunctionRoleList.jqGrid('setGridParam', { postData: { RolesString: (gridDataList.jqGrid('getRowData', RowId)).Roles } });
                gridFunctionRoleList.trigger('reloadGrid');
                dialogFunctionRoles.dialog("option", "title", "Set Function Roles").dialog("open");
            } else { alert("Please select a row data to edit."); }
        }
    });
});
