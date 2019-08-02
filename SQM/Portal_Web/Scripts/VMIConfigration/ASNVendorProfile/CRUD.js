$(function () {
    var diaCustomerCodegridDataList = $('#dialog_VMI_CustomerCodeForAllPlant_gridDataList');
    var diaShipFromgridDataList = $('#dialog_VMI_ShipFrom_gridDataList');
    var diaForwardergridDataList = $('#dialog_VMI_Forwarder_gridDataList');
    var dialogCustomerCode = $('#dialog_Modify_CustomerCode');
    var dialogShipFrom = $('#dialog_Modify_ShipFrom');
    var dialogForwarder = $('#dialog_Modify_Forwarder');

    $("#dialog_VMI_CustomerCodeForAllPlant_ADD").click(function () {
        $(this).removeClass('ui-state-focus');

        __DialogIsShownNow = true;
        dialogCustomerCode.attr('Mode', "C");

        $('#dialog_txt_Modify_CustomerCode_PLANT').val("");
        $('#dialog_txt_Modify_CustomerCode_CUSTOMERCODE').val("");
        $('#dialog_txt_Modify_CustomerCode_CUSTOMERNAME').val("");

        dialogCustomerCode.show();
        dialogCustomerCode.dialog("open");
    });

    $("#dialog_VMI_CustomerCodeForAllPlant_Upadte").click(function () {
        $(this).removeClass('ui-state-focus');

        if (!diaCustomerCodegridDataList.jqGrid('getGridParam', 'multiselect')) {
            var RowId = diaCustomerCodegridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = diaCustomerCodegridDataList.jqGrid('getRowData', RowId);
            if (RowId) {
                __DialogIsShownNow = true;
                dialogCustomerCode.attr('Mode', "E");
                dialogCustomerCode.attr('RowID', RowId);
                dialogCustomerCode.attr('PLANT', dataRow.PLANT);

                $('#dialog_txt_Modify_CustomerCode_PLANT').val(dataRow.PLANT);
                $('#dialog_txt_Modify_CustomerCode_CUSTOMERCODE').val(dataRow.COSTOM_CODE);
                $('#dialog_txt_Modify_CustomerCode_CUSTOMERNAME').val(dataRow.COSTOM_NAME);

                dialogCustomerCode.show();
                dialogCustomerCode.dialog("open");;
            }
            else {
                alert("Please select a data to Modify.");
            }
        };
    });

    $("#dialog_VMI_CustomerCodeForAllPlant_Delete").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!diaCustomerCodegridDataList.jqGrid('getGridParam', 'multiselect')) {
            var RowId = diaCustomerCodegridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                if (confirm("Confirm to delete selected [PLANT] : (" + diaCustomerCodegridDataList.jqGrid('getRowData', RowId).PLANT + ")?")) {
                    diaCustomerCodegridDataList.jqGrid('delRowData', RowId);
                    diaCustomerCodegridDataList.trigger("reloadGrid");
                };
            }
            else {
                alert("Please select a data to delete.");
            }
        };
    });

    $("#dialog_VMI_ShipFrom_ADD").click(function () {
        $(this).removeClass('ui-state-focus');

        __DialogIsShownNow = true;
        dialogShipFrom.attr('Mode', "C");

        $('#dialog_txt_Modify_ShipFrom_PLANTSHORTNAME').val("");
        $('#dialog_txt_Modify_ShipFrom_OFFICIALNAME').val("");
        $('#dialog_txt_Modify_ShipFrom_ADDRESS').val("");
        $('#dialog_txt_Modify_ShipFrom_Telphone').val("");
        $('#dialog_txt_Modify_ShipFrom_FAX').val("");

        dialogShipFrom.show();
        dialogShipFrom.dialog("open");
    });

    $("#dialog_VMI_ShipFrom_Upadte").click(function () {
        $(this).removeClass('ui-state-focus');

        if (!diaShipFromgridDataList.jqGrid('getGridParam', 'multiselect')) {
            var RowId = diaShipFromgridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = diaShipFromgridDataList.jqGrid('getRowData', RowId);
            if (RowId) {
                __DialogIsShownNow = true;
                dialogShipFrom.attr('Mode', "E");
                dialogShipFrom.attr('RowID', RowId);
                dialogShipFrom.attr('SHORTNAME', dataRow.PLANT_SHORT_NAME);

                $('#dialog_txt_Modify_ShipFrom_PLANTSHORTNAME').val(dataRow.PLANT_SHORT_NAME);
                $('#dialog_txt_Modify_ShipFrom_OFFICIALNAME').val(dataRow.PLANT_OFFICIAL_NAME);
                $('#dialog_txt_Modify_ShipFrom_ADDRESS').val(dataRow.PLANT_ADDRESS);
                $('#dialog_txt_Modify_ShipFrom_Telphone').val(dataRow.PLANT_TELPHONE);
                $('#dialog_txt_Modify_ShipFrom_FAX').val(dataRow.PLANT_FAX).text();

                dialogShipFrom.show();
                dialogShipFrom.dialog("open");;
            }
            else {
                alert("Please select a data to Modify.");
            }
        };
    });

    $("#dialog_VMI_ShipFrom_Delete").click(function () {
        $(this).removeClass('ui-state-focus');

        if (!diaShipFromgridDataList.jqGrid('getGridParam', 'multiselect')) {
            var RowId = diaShipFromgridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                if (confirm("Confirm to delete selected [Short Name] : (" + diaShipFromgridDataList.jqGrid('getRowData', RowId).PLANT_SHORT_NAME + ")?")) {
                    diaShipFromgridDataList.jqGrid('delRowData', RowId);
                    diaShipFromgridDataList.trigger("reloadGrid");
                };
            }
            else {
                alert("Please select a data to delete.");
            }
        };
    });

    $("#dialog_VMI_Forwarder_ADD").click(function () {
        $(this).removeClass('ui-state-focus');

        __DialogIsShownNow = true;
        dialogForwarder.attr('Mode', "C");

        $('#dialog_txt_Modify_Forwarder_COMPANY_NAME').val("");
        $('#dialog_txt_Modify_Forwarder_TEL').val("");
        $('#dialog_txt_Modify_Forwarder_NAME').val("");
        $('#dialog_txt_Modify_Forwarder_EMAIL').val("");
        $('#dialog_txt_Modify_Forwarder_ADDRESS').val("");

        dialogForwarder.show();
        dialogForwarder.dialog("open");
    });

    $("#dialog_VMI_Forwarder_Upadte").click(function () {
        $(this).removeClass('ui-state-focus');

        if (!diaForwardergridDataList.jqGrid('getGridParam', 'multiselect')) {
            var RowId = diaForwardergridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = diaForwardergridDataList.jqGrid('getRowData', RowId);
            if (RowId) {
                __DialogIsShownNow = true;
                dialogForwarder.attr('Mode', "E");
                dialogForwarder.attr('RowID', RowId);
                dialogForwarder.attr('COMPANY_NAME', dataRow.COMPANY_NAME);

                $('#dialog_txt_Modify_Forwarder_COMPANY_NAME').val(dataRow.COMPANY_NAME);
                $('#dialog_txt_Modify_Forwarder_TEL').val(dataRow.TEL);
                $('#dialog_txt_Modify_Forwarder_NAME').val(dataRow.NAME);
                $('#dialog_txt_Modify_Forwarder_EMAIL').val(dataRow.EMAIL);
                $('#dialog_txt_Modify_Forwarder_ADDRESS').val(dataRow.ADDRESS).text();

                dialogForwarder.show();
                dialogForwarder.dialog("open");;
            }
            else {
                alert("Please select a data to Modify.");
            }
        };
    });

    $("#dialog_VMI_Forwarder_Delete").click(function () {
        $(this).removeClass('ui-state-focus');

        if (!diaForwardergridDataList.jqGrid('getGridParam', 'multiselect')) {
            var RowId = diaForwardergridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                if (confirm("Confirm to delete selected [Comapny Name] : (" + diaForwardergridDataList.jqGrid('getRowData', RowId).COMPANY_NAME + ")?")) {
                    diaForwardergridDataList.jqGrid('delRowData', RowId);
                    diaForwardergridDataList.trigger("reloadGrid");
                };
            }
            else {
                alert("Please select a data to delete.");
            }
        };
    });
});