$(function () {
    $('#btnAddABA').click(function () {
        $(this).removeClass('ui-state-focus');
        $('#dialogABAManage').prop('STATUS', 'New');
        $('#dialogABAManage').prop('NBA_ID', '');
        $('#dialogABAManage').dialog('open');
    });

    $('#btnDeleteABA').click(function () {
        var checkedList = {};
        $(this).removeClass('ui-state-focus');
        var ids = $("#gridABA").jqGrid('getDataIDs');
        for (var i = 0; i < ids.length; i++) {
            var rowId = ids[i];
            var checked = $('#jqg_gridABA_' + ids[i]).prop('checked');
            var STATUS = $("#gridABA").jqGrid('getCell', ids[i], 'STATUS');
            if (checked === true && STATUS == 'New') {
                checkedList[ids[i]] = true;
            }
        }

        var deleteItems = '';
        for (var rowIds in checkedList) {
            deleteItems += $('#gridABA').jqGrid('getCell', rowIds, 'BUYER_NAME') + '\n';
        }

        if (deleteItems == '') {
            alert('Please check at least one item to delete.');
        } else {
            if (confirm('Do you want to delete the following company:\n' + deleteItems)) {
                var NBA_IDs = [];
                for (var rowIds in checkedList) {
                    NBA_IDs.push($('#gridABA').jqGrid('getCell', rowIds, 'NBA_ID'));
                }

                $.ajax({
                    url: __WebAppPathPrefix + '/VMIProcess/DeleteABAItems',
                    data: {
                        NBA_IDs: NBA_IDs
                    },
                    type: "post",
                    dataType: 'text',
                    async: false, // if need page refresh, please remark this option
                    success: function (data) {
                        if (data == 'Delete successfully.') {
                            ReloadABAgridDataList();
                        }
                        else {
                            alert('Deleting failure, please contact system administrator manager.');
                        }
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    },
                    complete: function (jqXHR, textStatus) {
                    }
                });
            }
        }
    });
});