$(function () {
    var gridDataList = $("#gridDataList");

    jQuery("#btnAssignRoleMembers").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).RoleGUID;
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
        RefreshRoleMemberUIInfo();
    });

    jQuery("#btnsrmAdd").click(function () {
        $.ajax({
            url: __WebAppPathPrefix + "/SystemMgmt/AddaRoleMember",
            data: {
                RoleGUID: $("#divSelectRoleMembers").attr("SelectedRoleGUID"),
                MemberGUID: $("#gridsrmLeftResult").jqGrid('getRowData', ($("#gridsrmLeftResult").jqGrid('getGridParam', 'selrow'))).AccountGUID
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    RefreshRoleMemberUIInfo();
                    alert("Add role member successfully.");
                }
                else {
                    alert("Add role member fail due to:\n\n" + data);
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
        if (confirm("Confirm to remove all selected role members?")) {
            $.ajax({
                url: __WebAppPathPrefix + "/SystemMgmt/RemoveAllRoleMembers",
                data: { RoleGUID: $("#divSelectRoleMembers").attr("SelectedRoleGUID") },
                type: "post",
                dataType: 'text',
                async: false,
                success: function (data) {
                    if (data == "") {
                        RefreshRoleMemberUIInfo();
                        alert("Remove role member successfully.");
                    }
                    else {
                        alert("Remove role member fail due to:\n\n" + data);
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

function RefreshRoleMemberUIInfo() {
    $('#gridsrmRightResult').jqGrid('clearGridData');
    $("#gridsrmRightResult").jqGrid('setGridParam', { postData: { RoleGUID: $("#divSelectRoleMembers").attr("SelectedRoleGUID"), SearchText: escape($.trim($("#txtsrmRightSearchText").val())) } }).jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    $("#spansrmTotal").html(GetRoleMemberCount(__WebAppPathPrefix + "/SystemMgmt/LoadRoleMemberCount", $("#divSelectRoleMembers").attr("SelectedRoleGUID")));
}

function GetRoleMemberCount(Url, RoleGUID) {
    var r = "";
    $.ajax({
        url: Url,
        data: { "RoleGUID": RoleGUID },
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

function RemoveARoleMember(MemberGUID) {
    $.ajax({
        url: __WebAppPathPrefix + "/SystemMgmt/RemoveaRoleMember",
        data: {
            RoleGUID: $("#divSelectRoleMembers").attr("SelectedRoleGUID"),
            MemberGUID: MemberGUID
        },
        type: "post",
        dataType: 'text',
        async: false,
        success: function (data) {
            if (data == "") {
                RefreshRoleMemberUIInfo();
                alert("Remove role member successfully.");
            }
            else {
                alert("Remove role member fail due to:\n\n" + data);
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