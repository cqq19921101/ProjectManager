$(function () {
    var txtASNCorpMG = $('#dialog_VMI_txt_Corp_MG');
    var diaPlant = $('#dialog_VMI_QueryPlantInfo');
    var diaSBUVendor = $('#dialog_VMI_QueryCorpMemberGUID');
    $("#btnSQMPlantSearch").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataList = $("#SQMPlantgridDataList");
        gridDataList.jqGrid('clearGridData');

        gridDataList.jqGrid('setGridParam', { postData: { SearchText: escape($.trim($("#txtFilterText").val())) } })
        gridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
    $('#btnOpenQueryPlantDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtPlant';

            InitdialogPlant();
            ReloadDiaPlantCodegridDataList();

            diaPlant.show();
            diaPlant.dialog("open");
        }
    });
    $('#dialog_btn_diaCorpMemberGUID_Search').click(function () {
        $(this).removeClass('ui-state-focus');
        ReloadDiaMGgridDataList();
    });

    $('#btnOpenQueryMGDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtMG';

            InitdialogQueryCorpMG();
            ReloadDiaMGgridDataList();

            diaSBUVendor.show();
            diaSBUVendor.dialog("open");
        }
    });
});

//Init dialogPlantCode UI
function InitdialogQueryCorpMG() {
    $('#dialog_VMI_txt_Corp_MG').val("");
}



function ReloadDiaMGgridDataList() {
    var diaSBUVendorgridData = $('#dialog_VMI_CorpMemberGUID_gridDataList');
    var diatxtSBUVDN = $('#dialog_VMI_txt_Corp_MG');

    diaSBUVendorgridData.jqGrid('clearGridData');
    diaSBUVendorgridData.jqGrid('setGridParam', { postData: { CorpMemberGUIDCode: escape($.trim(diatxtSBUVDN.val())) } });
    diaSBUVendorgridData.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}