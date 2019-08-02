$(function () {
    //$('#dialog_btn_diaSBUVendor_Search').click(function () {
    //    $(this).removeClass('ui-state-focus');
    //    ReloadDiaSBUVendorCodegridDataList();
    //});
    $(function () {
        var diaPlant = $('#dialog_VMI_QueryPlantInfo');
        var diaSBUVendor = $('#dialog_VMI_QuerySBUVendor');

        //$('#btnQueryDemand').click(function () {
        //    $(this).removeClass('ui-state-focus');
        //    ReloadToDoVDSgridDataList();
        //});

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

        //$('#btnOpenQueryBuyerCodeDialog').click(function () {
        //    $(this).removeClass('ui-state-focus');
        //    if (!__DialogIsShownNow) {
        //        __DialogIsShownNow = true;
        //        __SelectorName = '#txtBuyerCode';

        //        InitdialogBuyer();
        //        ReloadDiaBuyerCodegridDataList();

        //        diaBuyer.show();
        //        diaBuyer.dialog("open");
        //    }
        //});
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


});


function ReloadDiaSBUVendorCodegridDataList() {
    var diaSBUVendorgridData = $('#dialog_VMI_SBUVendor_gridDataList');
    var diatxtSBUVDN = $('#dialog_VMI_txt_SBU_VDN');

    diaSBUVendorgridData.jqGrid('clearGridData');
    diaSBUVendorgridData.jqGrid('setGridParam', { postData: { SBUVendorCode: escape($.trim(diatxtSBUVDN.val())) } });
    diaSBUVendorgridData.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}
