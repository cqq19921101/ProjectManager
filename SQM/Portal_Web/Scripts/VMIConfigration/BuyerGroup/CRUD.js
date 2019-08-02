$(function () {
    $('#btnAddBuyerGroup').click(function () {
        $(this).removeClass('ui-state-focus');
        $('#dialogBuyerGroup').dialog('open');
    });

    $('#btnDeleteBuyerGroup').click(function () {
        $(this).removeClass('ui-state-focus');
        var gridBuyerGroup = $('#gridBuyerGroup'),
            selRowId = gridBuyerGroup.jqGrid('getGridParam', 'selrow'),
            BUYER_GROUP_GUID = gridBuyerGroup.jqGrid('getCell', selRowId, 'BUYER_GROUP_GUID'),
            BUYER_GROUP_ID = gridBuyerGroup.jqGrid('getCell', selRowId, 'BUYER_GROUP_ID');

        if (selRowId != null) {
            if (confirm('Are you sure you want to delete this buyer group [' + BUYER_GROUP_ID + '] ?')) {
                $.ajax({
                    url: __WebAppPathPrefix + '/VMIConfigration/DeleteBuyerGroup',
                    data: {
                        BUYER_GROUP_GUID: escape($.trim(BUYER_GROUP_GUID))
                    },
                    type: "post",
                    dataType: 'text',
                    async: false, // if need page refresh, please remark this option
                    success: function (data) {
                        alert(data);
                        if (data == "Delete Buyer Group successfully.") {
                            $('#gridBuyerGroup').jqGrid('clearGridData');
                            $('#gridBuyerGroup').jqGrid('setGridParam', {
                                postData: {
                                    BUYER_GORUP_ID: escape($.trim($('#txtBUYER_GROUP_ID.tdQueryTextBox').val())),
                                    BUYER_GROUP_NAME: escape($.trim($('#txtBUYER_GROUP_NAME.tdQueryTextBox').val()))
                                }
                            });
                            $('#gridBuyerGroup').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
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
        else {
            alert('Please select record first.');
        }
    });
});