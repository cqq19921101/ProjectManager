$(function () {
    var gridDataList = $("#gridDataList");

    jQuery("#btnAssignMenuSubFunc").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).FunctionGUID;
                SetUI("m");
            } else { alert("Please select a row data to assign members."); }
        }
    });

    jQuery("#btnsrmFinish").click(function () {
        $(this).removeClass('ui-state-focus');
        SetUI('i');
    });

    jQuery("#btnsrmLeftSearch").click(function () {
        $("#gridsrmLeftResult").jqGrid('setGridParam', { postData: { SearchText: escape($.trim($("#txtsrmLeftSearchText").val())), MemberType: "", FieldSets: "s" } })
            .jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });

    jQuery("#btnsrmRightSearch").click(function () {
        RefreshSubFuncUIInfo();
    });

    jQuery("#btnsrmAdd").click(function () {
        if ($("#gridsrmLeftResult").jqGrid('getRowData', ($("#gridsrmLeftResult").jqGrid('getGridParam', 'selrow'))).SubFuncGUID == undefined) {
            alert("Please choose a sub func!");
            return;
        }
        $.ajax({
            url: __WebAppPathPrefix + "/SQMMgmt/AddaSubFunc",
            data: {
                FunctionGUID: $("#divSelectMenuFuncs").attr("selected-function-guid"),
                SubFuncGUID: $("#gridsrmLeftResult").jqGrid('getRowData', ($("#gridsrmLeftResult").jqGrid('getGridParam', 'selrow'))).SubFuncGUID
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    RefreshSubFuncUIInfo();
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

    jQuery("#btnsrmRemoveAll").click(function () {
        if (confirm("Confirm to remove all selected sub funcs?")) {
            $.ajax({
                url: __WebAppPathPrefix + "/SQMMgmt/RemoveAllSubFuncs",
                data: { FunctionGUID: $("#divSelectMenuFuncs").attr("selected-function-guid") },
                type: "post",
                dataType: 'text',
                async: false,
                success: function (data) {
                    if (data == "") {
                        RefreshSubFuncUIInfo();
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

function RefreshSubFuncUIInfo() {
    $('#gridsrmRightResult').jqGrid('clearGridData');
    $("#gridsrmRightResult").jqGrid('setGridParam', { postData: { FunctionGUID: $("#divSelectMenuFuncs").attr("selected-function-guid"), SearchText: escape($.trim($("#txtsrmRightSearchText").val())) } }).jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    $("#spansrmTotal").html(GetSubFuncCount(__WebAppPathPrefix + "/SQMMgmt/LoadSubFuncCount", $("#divSelectMenuFuncs").attr("selected-function-guid")));
}

function GetSubFuncCount(Url, FunctionGUID) {
    var r = "";
    $.ajax({
        url: Url,
        data: { "FunctionGUID": FunctionGUID },
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

function RemoveASubFunc(SubFuncGUID) {
    $.ajax({
        url: __WebAppPathPrefix + "/SQMMgmt/RemoveaSubFunc",
        data: {
            FunctionGUID: $("#divSelectMenuFuncs").attr("selected-function-guid"),
            SubFuncGUID: SubFuncGUID
        },
        type: "post",
        dataType: 'text',
        async: false,
        success: function (data) {
            if (data == "") {
                RefreshSubFuncUIInfo();
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