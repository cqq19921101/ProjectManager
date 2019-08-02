function SetUI(mode) {
    switch (mode) {
        case "i": //initial state (wait user pick a menu)
            $("#divSelectMenuFuncs").hide();
            $("#divSelectRoleMenuFuncs").hide();
            $("#tbMain1").show();
            $("#tbMain2").show();
            break;
        case "r": //initial state (wait user pick a menu)
            $("#tbMain1").hide();
            $("#tbMain2").hide();
            $("#divSelectMenuFuncs").hide();

            $("#divSelectRoleMenuFuncs").attr("selected-function-guid",
                $("#gridDataList").jqGrid('getRowData', ($("#gridDataList").jqGrid('getGridParam', 'selrow'))).FunctionGUID);
            $("#divSelectRole").hide();

            $("#spanrmfSelectedFunc").text(unescape($("#divSelectMenuFuncs").attr("selected-function-guid")));

            //Left grid
            $("#txtrmfLeftSearchText").val("");
            //$("#gridrmfLeftResult").jqGrid('setGridParam', { postData: { SearchText: $("#txtrmfLeftSearchText").val(), MemberType: "", FieldSets: "s" } }).jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');

            //Right grid
            $("#txtrmfRightSearchText").val("");

            

            //load when choose role
            //RefreshRoleSubFuncMapUIInfo();

            $("#divSelectRoleMenuFuncs").show();
            //load when show page
            RefreshRoleSubFuncUIInfo();

            break;
        default: //Maintain sub funcs
            $("#tbMain1").hide();
            $("#tbMain2").hide();
            $("#divSelectRoleMenuFuncs").hide();

            $("#divSelectMenuFuncs").attr("selected-function-guid",
                $("#gridDataList").jqGrid('getRowData', ($("#gridDataList").jqGrid('getGridParam', 'selrow'))).FunctionGUID);
            $("#divSelectMenuFuncs").attr("SelectedFunctionGUID2",
                $("#gridDataList").jqGrid('getRowData', ($("#gridDataList").jqGrid('getGridParam', 'selrow'))).FunctionGUID);
            //$("#divSelectMenuFuncs").attr("SelectedMenuName",
            //    escape($("#gridDataList").jqGrid('getRowData', ($("#gridDataList").jqGrid('getGridParam', 'selrow'))).RoleName));
            //$("divSelectMenuFuncs").show();
            $("#divSelectRole").hide();

            $("#spansrmSelectedFunc").text(unescape($("#divSelectMenuFuncs").attr("selected-function-guid")));

            //Left grid
            $("#txtsrmLeftSearchText").val("");
            $("#gridsrmLeftResult").jqGrid('setGridParam', { postData: { SearchText: $("#txtsrmLeftSearchText").val(), MemberType: "", FieldSets: "s" } }).jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');

            //Right grid
            $("#txtsrmRightSearchText").val("");
            RefreshSubFuncUIInfo();

            $("#divSelectMenuFuncs").show();

            break;
    }
}