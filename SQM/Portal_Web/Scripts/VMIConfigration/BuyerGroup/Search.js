$(function () {
    $('#btnQueryBuyerGroup').click(function () {
        $(this).removeClass('ui-state-focus');

        $('#gridBuyerGroup').jqGrid('clearGridData');
        $('#gridBuyerGroup').jqGrid('setGridParam', {
            postData: {
                BUYER_GORUP_ID: escape($.trim($('#txtBUYER_GROUP_ID.tdQueryTextBox').val())),
                BUYER_GROUP_NAME: escape($.trim($('#txtBUYER_GROUP_NAME.tdQueryTextBox').val()))
            }
        });
        $('#gridBuyerGroup').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });

    $('#dialogBuyerGroup #btnGroup').click(function () {
        $('#dialogGroup').dialog('open');
    });
});