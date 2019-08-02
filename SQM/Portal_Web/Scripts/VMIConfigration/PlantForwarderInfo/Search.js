$(function () {
    $('#btnQueryPlantForwarderInfo').click(function () {
        $(this).removeClass('ui-state-focus');

        $('#gridPlantForwarderInfo').jqGrid('clearGridData');
        $('#gridPlantForwarderInfo').jqGrid('setGridParam', {
            postData: {
                PLANT: escape($.trim($('#txtPlant.tdQueryTextBox').val())),
                COMPANY_NAME: escape($.trim($('#txtCompanyName.tdQueryTextBox').val()))
            }
        });
        $('#gridPlantForwarderInfo').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });

    $('#btnNewPlantForwarderInfo').click(function () {
        $(this).removeClass('ui-state-focus');

        var buttons = $('#dialogPlantForwarderInfo').dialog('option', 'buttons')
        delete buttons['Delete'];
        $('#dialogPlantForwarderInfo').dialog('option', 'buttons', buttons);

        $('#dialogPlantForwarderInfo').prop('Action', 'N');
        $('#dialogPlantForwarderInfo').dialog('open');
    });
});