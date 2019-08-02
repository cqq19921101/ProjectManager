$(function () {
    $('#ddlCategory').on('change', function () {
        // Clear jqGrid content
        $('#listRefAndDowload').jqGrid('clearGridData');
        // Sent params to jqGrid and reload it
        $('#listRefAndDowload').jqGrid('setGridParam', {
            postData: {
                CategoryID: escape($(this).find('option:selected').val())
            }
        });
        $('#listRefAndDowload').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
})