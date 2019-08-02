$(function () {
    var gridDataList = $("#gridDataList");

    var dialogMemberRoles = $("#dialogMemberRoles");
    var gridMemberRoleList = $("#gridMemberRoleList");

    jQuery("#btnSetRole").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).AccountGUID;
                var r = AcquireDataLock(DataKey)
                if (r == "ok") {
                    gridMemberRoleList.jqGrid('setGridParam', { datatype: 'json' });
                    gridMemberRoleList.jqGrid('setGridParam', { url: __WebAppPathPrefix + '/SubSystemMgmt/GetMemberRolesByMember?MemberGUID=' + DataKey });
                    gridMemberRoleList.trigger('reloadGrid');
                    dialogMemberRoles.dialog("option", "title", "Set Member Roles").dialog("open");
                }
                else {
                    switch (r) {
                        case "timeout": $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); break;
                        case "l": alert("Data already lock by other user."); break;
                        default: alert("Data lock fail or application error."); break;
                    }
                }
            } else { alert("Please select a row data to edit."); }
        }
    });
});
