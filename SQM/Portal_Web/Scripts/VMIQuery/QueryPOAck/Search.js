$(function () {
    var diaPlant = $('#dialog_VMI_QueryPlantInfo');
    var diaSBUVendor = $('#dialog_VMI_QuerySBUVendor');
    var diaBuyer = $('#dialog_VMI_QueryBuyerInfo');

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

    $('#btnQueryPOAck').click(function () {
        $(this).removeClass('ui-state-focus');
        reloadQueryPOAckGridList();
    });
})

function dateDiff(fromDate, toDate) {
    var date1 = new Date(fromDate);
    var date2 = new Date(toDate);
    var timeDiff = Math.abs(date2.getTime() - date1.getTime());
    var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
    return diffDays;
}

function reloadQueryPOAckGridList() {
    var PONumberFrom = $.trim($('#txtPONumberFrom').val());
    var PONumberTo = $.trim($('#txtPONumberTo').val());
    var Plant = $.trim($('#txtPlant').val());
    var VendorCode = $.trim($('#txtVendorCode').val());
    var BuyerCode = $.trim($('#txtBuyerCode').val());
    var ReleaseDateFrom = $.trim($('#txtReleaseDateFrom').val());
    var ReleaseDateTo = $.trim($('#txtReleaseDateTo').val());
    var Ack = $.trim($('#ddlAck option:selected').val());

    if (ReleaseDateFrom == '' || ReleaseDateTo == '') {
        alert('Please select Release Date(FM) and Release Date(TO) first.');
    }
    else if (dateDiff(ReleaseDateFrom, ReleaseDateTo) > 62) {
        alert('The interval of the Release Date is exceed 62 days, please reselect it under 62 days.');
    }
    else {
        var gridQueryPOAck = $('#gridQueryPOAck');

        gridQueryPOAck.jqGrid('clearGridData');
        gridQueryPOAck.jqGrid('setGridParam', {
            postData: {
                PONumberFrom: escape(PONumberFrom),
                PONumberTo: escape(PONumberTo),
                Plant: escape(Plant),
                VendorCode: escape(VendorCode),
                BuyerCode: escape(BuyerCode),
                ReleaseDateFrom: escape(ReleaseDateFrom),
                ReleaseDateTo: escape(ReleaseDateTo),
                Ack: escape(Ack)
            }
        });

        gridQueryPOAck.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    }
}