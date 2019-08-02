$(function () {
    var gridDeptList = $('#gridDeptList');
    var dialogGroupName = $('#dialogGroupName');

    $("#btnAdd").click(function () {
        $(this).removeClass('ui-state-focus');
        var SelRowId = gridDeptList.jqGrid('getGridParam', 'selrow');
        if (SelRowId) {
            var SelRow = gridDeptList.jqGrid('getRowData', SelRowId);
            var PARENT_GROUP_ID = SelRow.GROUP_ID;

            dialogGroupName.prop('PARENT_GROUP_ID', PARENT_GROUP_ID);
            dialogGroupName.prop('GROUP_TYPE', 'Department');
            dialogGroupName.prop('Action', 'Add');
            dialogGroupName.dialog('open');
        }
        else {
            alert('Please select one record first.');
        }
    });

    $("#btnEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        var SelRowId = gridDeptList.jqGrid('getGridParam', 'selrow');
        if (SelRowId) {
            var SelRow = gridDeptList.jqGrid('getRowData', SelRowId);
            var GROUP_ID = SelRow.GROUP_ID;
            var PARENT_GROUP_ID = SelRow.PARENT_GROUP_ID;
            var GROUP_NAME = SelRow.GROUP_NAME;

            dialogGroupName.find('#name').val(GROUP_NAME);
            dialogGroupName.prop('GROUP_ID', GROUP_ID);
            dialogGroupName.prop('PARENT_GROUP_ID', PARENT_GROUP_ID);
            dialogGroupName.prop('GROUP_TYPE', 'Department');
            dialogGroupName.prop('Action', 'Edit');
            dialogGroupName.dialog('open');
        }
        else {
            alert('Please select one record first.');
        }
    });

    $("#btnDelete").click(function () {
        $(this).removeClass('ui-state-focus');
        var SelRowId = gridDeptList.jqGrid('getGridParam', 'selrow');
        if (SelRowId) {
            var SelRow = gridDeptList.jqGrid('getRowData', SelRowId);

            var GROUP_ID = SelRow.GROUP_ID;

            $.ajax({
                url: __WebAppPathPrefix + '/VMIConfigration/DeleteGroup',
                data: {
                    GROUP_ID: escape($.trim(GROUP_ID)),
                    GROUP_TYPE: escape($.trim('Department'))
                },
                type: "post",
                dataType: 'text',
                async: false, // if need page refresh, please remark this option
                success: function (data) {
                    if (data.indexOf('Success') == -1) {
                        alert(data);
                    }

                    $('#gridDeptList').jqGrid('resetSelection');
                    $('#gridDeptList').trigger('reloadGrid');
                },
                error: function (xhr, textStatus, thrownError) {
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, textStatus) {
                }
            });
        }
        else {
            alert('Please select one record first.');
        }
    });

    $("#btnMoveUp").click(function () {
        $(this).removeClass('ui-state-focus');
        var SelRowId = gridDeptList.jqGrid('getGridParam', 'selrow');
        if (SelRowId) {
            var SelRow = gridDeptList.jqGrid('getRowData', SelRowId);

            var PARENT_GROUP_ID = SelRow.PARENT_GROUP_ID;
            var GROUP_ID = SelRow.GROUP_ID;
            var NEW_POSITION = parseInt(SelRow.Position) - 1;

            $.ajax({
                url: __WebAppPathPrefix + '/VMIConfigration/MoveGroup',
                data: {
                    PARENT_GROUP_ID: escape($.trim(PARENT_GROUP_ID)),
                    GROUP_ID: escape($.trim(GROUP_ID)),
                    NEW_POSITION: escape($.trim(NEW_POSITION))
                },
                type: "post",
                dataType: 'text',
                async: false, // if need page refresh, please remark this option
                success: function (data) {
                    if (data.indexOf('Success') == -1) {
                        alert(data);
                    }

                    $('#gridDeptList').jqGrid('resetSelection');
                    $('#gridDeptList').trigger('reloadGrid');
                },
                error: function (xhr, textStatus, thrownError) {
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, textStatus) {
                }
            });
        }
        else {
            alert('Please select one record first.');
        }
    });

    $("#btnMoveDown").click(function () {
        $(this).removeClass('ui-state-focus');
        var SelRowId = gridDeptList.jqGrid('getGridParam', 'selrow');
        if (SelRowId) {
            var SelRow = gridDeptList.jqGrid('getRowData', SelRowId);

            var PARENT_GROUP_ID = SelRow.PARENT_GROUP_ID;
            var GROUP_ID = SelRow.GROUP_ID;
            var NEW_POSITION = parseInt(SelRow.Position) + 1;

            $.ajax({
                url: __WebAppPathPrefix + '/VMIConfigration/MoveGroup',
                data: {
                    PARENT_GROUP_ID: escape($.trim(PARENT_GROUP_ID)),
                    GROUP_ID: escape($.trim(GROUP_ID)),
                    NEW_POSITION: escape($.trim(NEW_POSITION))
                },
                type: "post",
                dataType: 'text',
                async: false, // if need page refresh, please remark this option
                success: function (data) {
                    if (data.indexOf('Success') == -1) {
                        alert(data);
                    }

                    $('#gridDeptList').jqGrid('resetSelection');
                    $('#gridDeptList').trigger('reloadGrid');
                },
                error: function (xhr, textStatus, thrownError) {
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, textStatus) {
                }
            });
        }
        else {
            alert('Please select one record first.');
        }
    });
});