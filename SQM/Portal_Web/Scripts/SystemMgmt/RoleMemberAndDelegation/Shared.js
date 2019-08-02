function SetUI(mode) {
    switch (mode) {
        case "i": //initial state (wait user pick a role)
            $("#divSelectRoleAdmins").hide();
            $("#divSelectRoleMembers").hide();
            $("#divSelectRole").show();
            break;
        case "a": //Maintain role admins
            $("#divSelectRoleAdmins").attr("SelectedRoleGUID",
                $("#gridDataList").jqGrid('getRowData', ($("#gridDataList").jqGrid('getGridParam', 'selrow'))).RoleGUID);
            $("#divSelectRoleAdmins").attr("SelectedRoleName",
                escape($("#gridDataList").jqGrid('getRowData', ($("#gridDataList").jqGrid('getGridParam', 'selrow'))).RoleName));
            
            $("#divSelectRole").hide();

            $("#spansraSelectedRole").text(unescape($("#divSelectRoleAdmins").attr("SelectedRoleName")));

            //Left grid
            $("#txtsraLeftSearchText").val("");
            $("#gridsraLeftResult").jqGrid('setGridParam', { postData: { SearchText: $("#txtsraLeftSearchText").val(), MemberType: "", FieldSets: "s" } }).jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
            
            //Right grid
            $("#txtsraRightSearchText").val("");
            RefreshRoleAdminUIInfo();

            $("#divSelectRoleAdmins").show();
            
            break;
        default: //Maintain role members
            $("#divSelectRoleMembers").attr("SelectedRoleGUID",
                $("#gridDataList").jqGrid('getRowData', ($("#gridDataList").jqGrid('getGridParam', 'selrow'))).RoleGUID);
            $("#divSelectRoleMembers").attr("SelectedRoleName",
                escape($("#gridDataList").jqGrid('getRowData', ($("#gridDataList").jqGrid('getGridParam', 'selrow'))).RoleName));

            $("#divSelectRole").hide();

            $("#spansrmSelectedRole").text(unescape($("#divSelectRoleMembers").attr("SelectedRoleName")));

            //Left grid
            $("#txtsrmLeftSearchText").val("");
            $("#gridsrmLeftResult").jqGrid('setGridParam', { postData: { SearchText: $("#txtsrmLeftSearchText").val(), MemberType: "", FieldSets: "s" } }).jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');

            //Right grid
            $("#txtsrmRightSearchText").val("");
            RefreshRoleMemberUIInfo();

            $("#divSelectRoleMembers").show();

            break;
    }
}