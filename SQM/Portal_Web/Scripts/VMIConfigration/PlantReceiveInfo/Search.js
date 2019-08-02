$(function () {
    $('#btnQueryPlantReceiveInfo').click(function () {
        $(this).removeClass('ui-state-focus');

        $('#gridPlantReceiveInfo').jqGrid('clearGridData');
        $('#gridPlantReceiveInfo').jqGrid('setGridParam', {
            postData: {
                PLANT: escape($.trim($('#txtPlant.tdQueryTextBox').val()))
            }
        });
        $('#gridPlantReceiveInfo').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });

    $('#btnNewPlantReceiveInfo').click(function () {
        $(this).removeClass('ui-state-focus');

        var buttons = $('#dialogPlantReceiveInfo').dialog('option', 'buttons')
        delete buttons['Delete'];
        $('#dialogPlantReceiveInfo').dialog('option', 'buttons', buttons);

        $('#dialogPlantReceiveInfo').prop('Action', 'N');
        $('#dialogPlantReceiveInfo').dialog('open');
    });
});