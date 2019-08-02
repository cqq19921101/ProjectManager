$(function () {
    $('#btnQueryPlantCustomsBrokerInfo').click(function () {
        $(this).removeClass('ui-state-focus');

        $('#gridPlantCustomsBrokerInfo').jqGrid('clearGridData');
        $('#gridPlantCustomsBrokerInfo').jqGrid('setGridParam', {
            postData: {
                PLANT: escape($.trim($('#txtPlant.tdQueryTextBox').val()))
            }
        });
        $('#gridPlantCustomsBrokerInfo').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });

    $('#btnNewPlantCustomsBrokerInfo').click(function () {
        $(this).removeClass('ui-state-focus');

        var buttons = $('#dialogPlantCustomsBrokerInfo').dialog('option', 'buttons')
        delete buttons['Delete'];
        $('#dialogPlantCustomsBrokerInfo').dialog('option', 'buttons', buttons);

        $('#dialogPlantCustomsBrokerInfo').prop('Action', 'N');
        $('#dialogPlantCustomsBrokerInfo').dialog('open');
    });
});