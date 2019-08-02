$(function () {
    var dialog = $("#dialogMemberRoles");
    var gridDataList = $("#gridDataList");
    var gridMemberRoleList = $("#gridMemberRoleList");

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
                var $this = gridMemberRoleList, ids = $this.jqGrid('getDataIDs'), i, l = ids.length;
                for (i = l; i > -1; i--) { $this.jqGrid('saveRow', ids[i], { successfunc: function (response) { return true; } }); }

                //Read all role data and retrieve checked roles
                var r = gridMemberRoleList.jqGrid('getGridParam', 'data');
                var MemberRoles = [];
                for (var iCnt = 0; iCnt < r.length; iCnt++)
                    if ((r[iCnt].Belongs == true) || (r[iCnt].Belongs == "true")) MemberRoles.push(r[iCnt].RoleGUID)

                //Update DB
                var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
                var DataKey = gridDataList.jqGrid('getRowData', RowId).AccountGUID;
                var DoSuccessfully = false;
                $.ajax({
                    url: __WebAppPathPrefix + "/SystemMgmt/UpdateMemberRoles",
                    contentType: 'application/json',
                    data: JSON.stringify({ MemberGUID: DataKey, MemberRoles: MemberRoles }),
                    type: "post",
                    dataType: 'text',
                    async: false,
                    success: function (data) {
                        if (data == "") {
                            //$("#btnSearch").click();
                            DoSuccessfully = true;
                            alert("Member role update successfully.");
                        }
                        else {
                            alert("Member role update fail due to:\n\n" + data);
                        }
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    },
                    complete: function (jqXHR, textStatus) {
                        //$("#ajaxLoading").hide();
                    }
                });
                if (DoSuccessfully)
                    $(this).dialog("close");
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var r = ReleaseDataLock((gridDataList.jqGrid('getRowData', RowId)).AccountGUID);
            if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
        }
    });

    //Init Member Role List (jqGrid)
    var dialog = $("#dialogData");
    var gridMemberRoleList = $("#gridMemberRoleList");
    gridMemberRoleList.jqGrid({
        url: __WebAppPathPrefix + '/SystemMgmt/GetMemberRoles',
        datatype: "json",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        width: 400,
        height: "auto",
        colNames: ['Belongs',
                    'Role GUID',
                    'Role Name'],
        colModel: [
            { name: 'Belongs', index: 'Belongs', sortable: true, width: 90, editable: true, formatter: 'checkbox', edittype: 'checkbox', editoptions: { value: "true:false" }, align: 'center' },
            { name: 'RoleGUID', index: 'RoleGUID', sortable: false, hidden: true, editable: false },
            { name: 'RoleName', index: 'RoleName', sortable: true, width: 210, sorttype: 'text', editable: false }
        ],
        rowNum: 10,
        viewrecords: true,
        loadonce: true,
        pager: '#gridMemberRoleListPager',
        onPaging: function (p) {
            //set all rows finish editing
            var $this = gridMemberRoleList, ids = $this.jqGrid('getDataIDs'), i, l = ids.length;
            for (i = l; i > -1; i--) { $this.jqGrid('saveRow', ids[i], { successfunc: function (response) { return true; } }); }
        },
        gridComplete: function () {
            //all row in inline edit mode
            var $this = gridMemberRoleList, ids = $this.jqGrid('getDataIDs'), i, l = ids.length;
            for (i = l; i > -1; i--) { $this.jqGrid('editRow', ids[i], true); }
        }
    });
    gridMemberRoleList.jqGrid('navGrid', '#gridMemberRoleListPager', { edit: false, add: false, del: false, search: false, refresh: false });
});