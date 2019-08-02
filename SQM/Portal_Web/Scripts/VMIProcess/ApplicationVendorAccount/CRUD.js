$(function () {
    $('#btnAddAVA').click(function () {
        $(this).removeClass('ui-state-focus');
        $('#dialogAVAManage').prop('STATUS_ID', 'New');
        $('#dialogAVAManage').prop('NVA_ID', '');
        $('#dialogAVAManage').dialog('open');
    });

    $('#btnDeleteAVA').click(function () {
        var checkedList = {};
        $(this).removeClass('ui-state-focus');
        var ids = $("#gridAVA").jqGrid('getDataIDs');
        for (var i = 0; i < ids.length; i++) {
            var rowId = ids[i];
            var checked = $('#jqg_gridAVA_' + ids[i]).prop('checked');
            var TEXT = $("#gridAVA").jqGrid('getCell', ids[i], 'TEXT');
            if (checked === true && (TEXT == 'Downloaded' || TEXT == 'New')) {
                checkedList[ids[i]] = true;
            }
        }

        var deleteItems = '';
        for (var rowIds in checkedList) {
            deleteItems += $('#gridAVA').jqGrid('getCell', rowIds, 'COMPANY_NAME') + '\n';
        }

        if (deleteItems == '') {
            alert('Please check at least one item to delete.');
        } else {
            if (confirm('Do you want to delete the following company:\n' + deleteItems)) {
                var NVA_IDs = [];
                for (var rowIds in checkedList) {
                    NVA_IDs.push($('#gridAVA').jqGrid('getCell', rowIds, 'NVA_ID'));
                }

                $.ajax({
                    url: __WebAppPathPrefix + '/VMIProcess/DeleteAVAItems',
                    data: {
                        NVA_IDs: NVA_IDs
                    },
                    type: "post",
                    dataType: 'text',
                    async: false, // if need page refresh, please remark this option
                    success: function (data) {
                        if (data == 'Success') {
                            ReloadANAgridDataList();
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