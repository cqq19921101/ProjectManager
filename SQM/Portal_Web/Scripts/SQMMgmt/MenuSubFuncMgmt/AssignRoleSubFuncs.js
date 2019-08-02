$(function () {
    var gridDataList = $("#gridDataList");
    jQuery("#btnAssignRoleMenuSubFunc").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).FunctionGUID;
                SetUI("r");
            } else { alert("Please select a row data to assign role menu sub funcs."); }
        }
    });


    $("#btnRoleSearch").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridRoleDataList = $("#gridRoleResult");
        gridRoleDataList.jqGrid('clearGridData');
        gridRoleDataList.jqGrid('setGridParam', { postData: { SearchText: escape($.trim($("#txtRoleSearchText").val())) } })
        gridRoleDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });

    

    jQuery("#btnrmfFinish").click(function () {
        $(this).removeClass('ui-state-focus');
        SetUI('i');
    });

    jQuery("#btnrmfLeftSearch").click(function () {
        $("#gridrmfLeftResult").jqGrid('setGridParam', { postData: { SearchText: escape($.trim($("#txtrmfLeftSearchText").val())), MemberType: "", FieldSets: "s" } })
            .jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });

    jQuery("#btnrmfRightSearch").click(function () {
        RefreshRoleSubFuncMapUIInfo();
    });

    jQuery("#btnrmfAdd").click(function () {
        if ($("#gridRoleResult").jqGrid('getRowData', ($("#gridRoleResult").jqGrid('getGridParam', 'selrow'))).RoleGUID == undefined) {
            alert("Please choose a sub func!");
            return;
        }
        if ($("#gridrmfLeftResult").jqGrid('getRowData', ($("#gridrmfLeftResult").jqGrid('getGridParam', 'selrow'))).SubFuncGUID == undefined) {
            alert("Please choose a sub func!");
            return;
        }
        $.ajax({
            url: __WebAppPathPrefix + "/SQMMgmt/AddaRoleSubFunc",
            data: {
                FunctionGUID: $("#divSelectRoleMenuFuncs").attr("selected-function-guid"),
                RoleGUID: $("#gridRoleResult").jqGrid('getRowData', ($("#gridRoleResult").jqGrid('getGridParam', 'selrow'))).RoleGUID,
                SubFuncGUID: $("#gridrmfLeftResult").jqGrid('getRowData', ($("#gridrmfLeftResult").jqGrid('getGridParam', 'selrow'))).SubFuncGUID
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    RefreshRoleSubFuncMapUIInfo();
                    alert("Add sub func successfully.");
                }
                else {
                    alert("Add sub func fail due to:\n\n" + data);
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                //$("#ajaxLoading").hide();
            }
        });
    });

    jQuery("#btnrmfRemoveAll").click(function () {
        if (confirm("Confirm to remove all selected sub funcs?")) {
            $.ajax({
                url: __WebAppPathPrefix + "/SQMMgmt/RemoveAllRoleSubFuncs",
                data: { FunctionGUID: $("#divSelectRoleMenuFuncs").attr("selected-function-guid"), RoleGUID: $("#gridRoleResult").jqGrid('getRowData', ($("#gridRoleResult").jqGrid('getGridParam', 'selrow'))).RoleGUID },
                type: "post",
                dataType: 'text',
                async: false,
                success: function (data) {
                    if (data == "") {
                        RefreshRoleSubFuncMapUIInfo();
                        alert("Remove sub func successfully.");
                    }
                    else {
                        alert("Remove sub func fail due to:\n\n" + data);
                    }
                },
                error: function (xhr, textStatus, thrownError) {
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, textStatus) {
                    //$("#ajaxLoading").hide();
                }
            });
        }
    });
});

function RefreshRoleSubFuncUIInfo() {
    $('#gridrmfLeftResult').jqGrid('clearGridData');
    $("#gridrmfLeftResult").jqGrid('setGridParam', { postData: { FunctionGUID: $("#divSelectRoleMenuFuncs").attr("selected-function-guid"), SearchText: escape($.trim($("#txtrmfLeftSearchText").val())) } }).jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}

function RefreshRoleSubFuncMapUIInfo() {
    //update menu sub func map


    //update role menu sub map
    var RoleGUID = $("#gridRoleResult").jqGrid('getRowData', ($("#gridRoleResult").jqGrid('getGridParam', 'selrow'))).RoleGUID;
    $('#gridrmfRightResult').jqGrid('clearGridData');
    $("#gridrmfRightResult").jqGrid('setGridParam', { postData: { FunctionGUID: $("#divSelectRoleMenuFuncs").attr("selected-function-guid"), RoleGUID, SearchText: escape($.trim($("#txtrmfRightSearchText").val())) } }).jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    $("#spanrmfTotal").html(GetRoleSubFuncCount(__WebAppPathPrefix + "/SQMMgmt/LoadRoleSubFuncCount", $("#divSelectRoleMenuFuncs").attr("selected-function-guid"),RoleGUID));
}

function GetRoleSubFuncCount(Url, FunctionGUID, RoleGUID) {
    var r = "";
    $.ajax({
        url: Url,
        data: { "FunctionGUID": FunctionGUID, "RoleGUID": RoleGUID },
        type: "post",
        dataType: 'text',
        async: false,
        success: function (data) {
            r = JSonParse(data).count;
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
            //$("#ajaxLoading").hide();
        }
    });
    return r;
}

function RemoveARoleSubFunc(SubFuncGUID) {
    //get choose role guid
    var gridRoleDataList = $("#gridRoleResult");
    if (!gridRoleDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
        var RowId = gridRoleDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            var RoleGUID = (gridRoleDataList.jqGrid('getRowData', RowId)).RoleGUID;
            $.ajax({
                url: __WebAppPathPrefix + "/SQMMgmt/RemoveaRoleSubFunc",
                data: {
                    FunctionGUID: $("#divSelectRoleMenuFuncs").attr("selected-function-guid"),
                    RoleGUID: RoleGUID,
                    SubFuncGUID: SubFuncGUID
                },
                type: "post",
                dataType: 'text',
                async: false,
                success: function (data) {
                    if (data == "") {
                        RefreshRoleSubFuncMapUIInfo();
                        alert("Remove sub func successfully.");
                    }
                    else {
                        alert("Remove sub func fail due to:\n\n" + data);
                    }
                },
                error: function (xhr, textStatus, thrownError) {
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, textStatus) {
                    //$("#ajaxLoading").hide();
                }
            });
        } else { alert("Please select a Role to assign funcs."); }
    }

    
}