$(function () {
    var txtASNCorpMG = $('#dialog_VMI_txt_Corp_MG1');
    var diaSBUMG= $('#dialog_VMI_QueryCorpMemberGUID1');
    var diaSBUVendor2 = $('#dialog_VMI_QueryCorpMemberGUID2');
    var diaPlant = $('#dialog_VMI_QueryPlantInfo');
    var diaSBUVendor = $('#dialog_VMI_QuerySBUVendor');
    var diaRD = $('#dialog_VMI_QueryCorpRDGUID');
    var diaRDS = $('#dialog_VMI_QueryCorpRDSGUID');

    $("#btnSQMSQEPURSearch").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataList = $("#SQMSQEPURgridDataList");
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

    $('#btnOpenQueryVendorCodeDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtVendorCode';

            InitdialogSBUVendor();
            ReloadDiaSBUVendorCodegridDataList();

            diaSBUVendor.show();
            diaSBUVendor.dialog("open");
        }
    });

    $('#btnOpenQuerySourceDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtSource';
            __SelectorSource = '#txtSourceName';
            InitdialogQueryCorpMG2();
            ReloadDiaMGgridDataList2();

            diaSBUVendor2.show();
            diaSBUVendor2.dialog("open");
        }
    });

    $('#btnOpenQueryMGDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtMember';
            __SelectorMember = '#txtMemberName';
            InitdialogQueryCorpMG();
            ReloadDiaMGgridDataList();

            diaSBUMG.show();
            diaSBUMG.dialog("open");
        }
    });

    $('#btnOpenQueryRDDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtRD';
            __SelectorRD= '#txtRDName';
            InitdialogQueryCorpRD();
            ReloadDiaRDgridDataList();

            diaRD.show();
            diaRD.dialog("open");
        }
    });

    $('#btnOpenQueryRDSDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtRDS';
            __SelectorRDS = "#txtRDSName"
            InitdialogQueryCorpRDS();
            ReloadDiaRDSgridDataList();

            diaRDS.show();
            diaRDS.dialog("open");
        }
    });

});

//Init dialogPlantCode UI
//function InitdialogQueryCorpMG() {
//    $('#dialog_VMI_txt_Corp_MG1').val("");
//}
function InitdialogQueryCorpMG() {
    $('#dialog_VMI_txt_Corp_MG').val("");
}
function InitdialogQueryCorpMG2() {
    $('#dialog_VMI_txt_Corp_MG2').val("");
}
function InitdialogQueryCorpRD() {
    $('#dialog_VMI_txt_Corp_RD').val("");
}
function InitdialogQueryCorpRDS() {
    $('#dialog_VMI_txt_Corp_RDS').val("");
}

//Init dialogSBUVendor UI
function InitdialogSBUVendor() {
    $('#dialog_VMI_txt_SBU_VDN').val("");
}
//Init dialogPlantCode UI
function InitdialogPlant() {
    $('#dialog_VMI_txt_Plant').val("");
}


function ReloadDiaPlantCodegridDataList() {
    var diaPlantgridData = $('#dialog_VMI_PlantCode_gridDataList');
    var diatxtPlant = $('#dialog_VMI_txt_Plant');

    diaPlantgridData.jqGrid('clearGridData');
    diaPlantgridData.jqGrid('setGridParam', { postData: { PLANT: escape($.trim(diatxtPlant.val())) } });
    diaPlantgridData.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}

function ReloadDiaSBUVendorCodegridDataList() {
    var diaSBUVendorgridData = $('#dialog_VMI_SBUVendor_gridDataList');
    var diatxtSBUVDN = $('#dialog_VMI_txt_SBU_VDN');

    diaSBUVendorgridData.jqGrid('clearGridData');
    diaSBUVendorgridData.jqGrid('setGridParam', { postData: { ERP_VND: escape($.trim(diatxtSBUVDN.val())) } });
    diaSBUVendorgridData.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}

function ReloadDiaMGgridDataList() {
    var diaSBUVendorgridData = $('#dialog_VMI_CorpMemberGUID_gridDataList1');
    var diatxtSBUVDN = $('#dialog_VMI_txt_Corp_MG1');

    diaSBUVendorgridData.jqGrid('clearGridData');
    diaSBUVendorgridData.jqGrid('setGridParam', { postData: { NAME: escape($.trim(diatxtSBUVDN.val())) } });
    diaSBUVendorgridData.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}

function ReloadDiaMGgridDataList2() {
    var diaSBUVendorgridData2 = $('#dialog_VMI_CorpMemberGUID_gridDataList2');
    var diatxtSBUVDN2 = $('#dialog_VMI_txt_Corp_MG2');

    diaSBUVendorgridData2.jqGrid('clearGridData');
    diaSBUVendorgridData2.jqGrid('setGridParam', { postData: { NAME: escape($.trim(diatxtSBUVDN2.val())) } });
    diaSBUVendorgridData2.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}

function ReloadDiaRDgridDataList() {
    var diaRDgridData = $('#dialog_VMI_CorpRDGUID_gridDataList');
    var diatxtRD = $('#dialog_VMI_txt_Corp_RD');

    diaRDgridData.jqGrid('clearGridData');
    diaRDgridData.jqGrid('setGridParam', { postData: { NAME: escape($.trim(diatxtRD.val())) } });
    diaRDgridData.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}

function ReloadDiaRDSgridDataList() {
    var diaRDSgridData = $('#dialog_VMI_CorpRDSGUID_gridDataList');
    var diatxtRDS = $('#dialog_VMI_txt_Corp_RDS');

    diaRDSgridData.jqGrid('clearGridData');
    diaRDSgridData.jqGrid('setGridParam', { postData: { NAME: escape($.trim(diatxtRDS.val())) } });
    diaRDSgridData.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}


$(function () {
$('#dialog_btn_diaCorpRDGUID_Search').click(function () {
    $(this).removeClass('ui-state-focus');
    ReloadDiaRDgridDataList();
});

$('#dialog_btn_diaCorpRDSGUID_Search').click(function () {
    $(this).removeClass('ui-state-focus');
    ReloadDiaRDSgridDataList();
});

$('#dialog_btn_diaCorpMemberGUID_Search2').click(function () {
    $(this).removeClass('ui-state-focus');
    ReloadDiaMGgridDataList2();
});

$('#dialog_btn_diaCorpMemberGUID_Search1').click(function () {
    $(this).removeClass('ui-state-focus');
    ReloadDiaMGgridDataList();
});

$('#dialog_btn_diaSBUVendor_Search').click(function () {
    $(this).removeClass('ui-state-focus');
    ReloadDiaSBUVendorCodegridDataList();
});

$('#dialog_btn_diaPlant_Search').click(function () {
    $(this).removeClass('ui-state-focus');
    ReloadDiaPlantCodegridDataList();
});

});