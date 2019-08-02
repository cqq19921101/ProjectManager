$(function () {
    var gridDataList = $("#gridDataList");
    
    jQuery("#btnAssignRoleAdmins").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).RoleGUID;
                SetUI("a");
            } else { alert("Please select a row data to assign admins."); }
        }
    });

    jQuery("#btnsraFinish").click(function () {
        $(this).removeClass('ui-state-focus');
        SetUI('i');
    });

    jQuery("#btnsraLeftSearch").click(function () {
        $("#gridsraLeftResult").jqGrid('setGridParam', { postData: { SearchText: escape($.trim($("#txtsraLeftSearchText").val())), MemberType: "", FieldSets: "s" } })
            .jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });

    jQuery("#btnsraRightSearch").click(function () {
        RefreshRoleAdminUIInfo();
    });

    jQuery("#btnsraAdd").click(function () {
        $.ajax({
            url: __WebAppPathPrefix + "/SystemMgmt/AddaRoleAdmin",
            data: {
                RoleGUID: $("#divSelectRoleAdmins").attr("SelectedRoleGUID"),
                MemberGUID: $("#gridsraLeftResult").jqGrid('getRowData', ($("#gridsraLeftResult").jqGrid('getGridParam', 'selrow'))).AccountGUID
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    RefreshRoleAdminUIInfo();
                    alert("Add role admin successfully.");
                }
                else {
                    alert("Add role admin fail due to:\n\n" + data);
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

    jQuery("#btnsraRemoveAll").click(function () {
        if(confirm("Confirm to remove all selected role admins?"))
        {
            $.ajax({
                url: __WebAppPathPrefix + "/SystemMgmt/RemoveAllRoleAdmins",
                data: { RoleGUID: $("#divSelectRoleAdmins").attr("SelectedRoleGUID") },
                type: "post",
                dataType: 'text',
                async: false,
                success: function (data) {
                    if (data == "") {
                        RefreshRoleAdminUIInfo();
                        alert("Remove role admin successfully.");
                    }
                    else {
                        alert("Remove role admin fail due to:\n\n" + data);
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

function RefreshRoleAdminUIInfo()
{
    $('#gridsraRightResult').jqGrid('clearGridData');
    $("#gridsraRightResult").jqGrid('setGridParam', { postData: { RoleGUID: $("#divSelectRoleAdmins").attr("SelectedRoleGUID"), SearchText: escape($.trim($("#txtsraRightSearchText").val())) } }).jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    $("#spansraTotal").html(GetRoleAdminCount(__WebAppPathPrefix + "/SystemMgmt/LoadRoleAdminCount", $("#divSelectRoleAdmins").attr("SelectedRoleGUID")));
}

function GetRoleAdminCount(Url, RoleGUID) {
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

function RemoveARoleAdmin(MemberGUID)
{
    $.ajax({
        url: __WebAppPathPrefix + "/SystemMgmt/RemoveaRoleAdmin",
        data: {
            RoleGUID: $("#divSelectRoleAdmins").attr("SelectedRoleGUID"),
            MemberGUID: MemberGUID
        },
        type: "post",
        dataType: 'text',
        async: false,
        success: function (data) {
            if (data == "") {
                RefreshRoleAdminUIInfo();
                alert("Remove role admin successfully.");
            }
            else {
                alert("Remove role admin fail due to:\n\n" + data);
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