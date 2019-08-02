$(function () {
    $('#dialog_btn_diaCorpVendor_Search').click(function () {
        $(this).removeClass('ui-state-focus');
        ReloadDiaCorpVendorCodegridDataList();
    });

    $('#dialog_btn_diaSBUVendor_Search').click(function () {
        $(this).removeClass('ui-state-focus');
        ReloadDiaSBUVendorCodegridDataList();
    });

    $('#dialog_btn_diaPlant_Search').click(function () {
        $(this).removeClass('ui-state-focus');
        ReloadDiaPlantCodegridDataList();
    });

    $('#dialog_btn_diaBuyer_Search').click(function () {
        $(this).removeClass('ui-state-focus');
        ReloadDiaBuyerCodegridDataList();
    });
});

function ReloadDiaCorpVendorCodegridDataList() {
    var diaCorpVendorgridData = $('#dialog_VMI_CorpVendor_gridDataList');
    var diatxtCorpVDN = $('#dialog_VMI_txt_Corp_VDN');

    diaCorpVendorgridData.jqGrid('clearGridData');
    diaCorpVendorgridData.jqGrid('setGridParam', { postData: { CorpVendorCode: escape($.trim(diatxtCorpVDN.val())) } });
    diaCorpVendorgridData.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}

function ReloadDiaSBUVendorCodegridDataList() {
    var diaSBUVendorgridData = $('#dialog_VMI_SBUVendor_gridDataList');
    var diatxtSBUVDN = $('#dialog_VMI_txt_SBU_VDN');

    diaSBUVendorgridData.jqGrid('clearGridData');
    diaSBUVendorgridData.jqGrid('setGridParam', { postData: { SBUVendorCode: escape($.trim(diatxtSBUVDN.val())) } });
    diaSBUVendorgridData.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}

function ReloadDiaPlantCodegridDataList() {
    var diaPlantgridData = $('#dialog_VMI_PlantCode_gridDataList');
    var diatxtPlant = $('#dialog_VMI_txt_Plant');

    diaPlantgridData.jqGrid('clearGridData');
    diaPlantgridData.jqGrid('setGridParam', { postData: { PLANT: escape($.trim(diatxtPlant.val())) } });
    diaPlantgridData.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}

function ReloadDiaBuyerCodegridDataList() {
    var diaBuyergridData = $('#dialog_VMI_BuyerCode_gridDataList');
    var diatxtBuyer = $('#dialog_VMI_txt_Buyer');

    diaBuyergridData.jqGrid('clearGridData');
    diaBuyergridData.jqGrid('setGridParam', { postData: { BUYER: escape($.trim(diatxtBuyer.val())) } });
    diaBuyergridData.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}