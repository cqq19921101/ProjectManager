$(function () {
    $('#btnQueryImportList').click(function () {
        $(this).removeClass('ui-state-focus');
        $('#gridImportList').jqGrid('clearGridData');
        $('#gridImportList').jqGrid('setGridParam', {
            postData: {
                IMPORT_LIST_NUM_FM: escape($.trim($('#txtImportListNoFM').val())),
                IMPORT_LIST_NUM_TO: escape($.trim($('#txtImportListNoTo').val())),
                IDN_NUM_FM: escape($.trim($('#txtDNNoFM').val())),
                IDN_NUM_TO: escape($.trim($('#txtDNNoTO').val())),
                IS_CLOSE: escape($.trim($('#ddlIsClose').val()))
            }
        });
        $('#gridImportList').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });

    var diaPlant = $('#dialog_VMI_QueryPlantInfo');

    $('#btnQueryPlant').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtPlant';

            InitdialogPlant();
            ReloadDiaPlantCodegridDataList();

            diaPlant.show();
            diaPlant.dialog("open");
            // div with class ui-dialog
            $('.ui-dialog :button').blur();
        }
    });
});