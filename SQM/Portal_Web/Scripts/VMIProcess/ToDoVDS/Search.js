$(function () {
    var diaPlant = $('#dialog_VMI_QueryPlantInfo');
    var diaSBUVendor = $('#dialog_VMI_QuerySBUVendor');
    var diaBuyer = $('#dialog_VMI_QueryBuyerInfo');

    $('#btnQueryDemand').click(function () {
        $(this).removeClass('ui-state-focus');
        ReloadToDoVDSgridDataList();
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

    $('#btnOpenQueryBuyerCodeDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtBuyerCode';

            InitdialogBuyer();
            ReloadDiaBuyerCodegridDataList();

            diaBuyer.show();
            diaBuyer.dialog("open");
        }
    });
})

function ReloadToDoVDSgridDataList() {
    var gridVDS = $('#gridVDS');

    gridVDS.jqGrid('clearGridData');
    gridVDS.jqGrid('setGridParam', {
        postData: {
            Plant: escape($.trim($("#txtPlant").val())),
            VendorCode: escape($.trim($("#txtVendorCode").val())),
            BuyerCode: escape($.trim($("#txtBuyerCode").val())),
            Status: escape($.trim($('#ddlStatus').val()))
        }
    });

    gridVDS.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}
