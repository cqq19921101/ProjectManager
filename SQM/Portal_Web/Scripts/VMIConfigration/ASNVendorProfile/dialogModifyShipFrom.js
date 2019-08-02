$(function () {
    var dialogShipFrom = $('#dialog_Modify_ShipFrom');
    var diaShipFromgridDataList = $('#dialog_VMI_ShipFrom_gridDataList');

    //Init dialog
    dialogShipFrom.dialog({
        autoOpen: false,
        height: 280,
        width: 430,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (dialogShipFrom.attr('Mode') == "C") {
                    if ($('#dialog_txt_Modify_ShipFrom_PLANTSHORTNAME').val() == "")
                        alert("[Short Name] is not allowed empty.");
                    else {
                        diaShipFromgridDataList.jqGrid('addRowData', $('#dialog_txt_Modify_ShipFrom_PLANTSHORTNAME').val(),
                                        {
                                            "PLANT_SHORT_NAME": $('<div/>').text($('#dialog_txt_Modify_ShipFrom_PLANTSHORTNAME').val()).html(),
                                            "PLANT_OFFICIAL_NAME": $('<div/>').text($('#dialog_txt_Modify_ShipFrom_OFFICIALNAME').val()).html(),
                                            "PLANT_ADDRESS": $('<div/>').text($('#dialog_txt_Modify_ShipFrom_ADDRESS').val()).html(),
                                            "PLANT_TELPHONE": $('<div/>').text($('#dialog_txt_Modify_ShipFrom_Telphone').val()).html(),
                                            "PLANT_FAX": $('<div/>').text($('#dialog_txt_Modify_ShipFrom_FAX').val()).html()
                                        });
                        diaShipFromgridDataList.trigger("reloadGrid");
                        $(this).dialog("close");
                    }
                }
                else if (dialogShipFrom.attr('Mode') == "E") {
                    if ($('#dialog_txt_Modify_ShipFrom_PLANTSHORTNAME').val() == "")
                        alert("[Short Name] is not allowed empty.");
                    else {
                        var RowID = dialogShipFrom.attr('RowID');
                        var Olddata = diaShipFromgridDataList.jqGrid('getRowData', RowID);
                        Olddata.PLANT_SHORT_NAME = $('<div/>').text($('#dialog_txt_Modify_ShipFrom_PLANTSHORTNAME').val()).html();
                        Olddata.PLANT_OFFICIAL_NAME = $('<div/>').text($('#dialog_txt_Modify_ShipFrom_OFFICIALNAME').val()).html();
                        Olddata.PLANT_ADDRESS = $('<div/>').text($('#dialog_txt_Modify_ShipFrom_ADDRESS').val()).html();
                        Olddata.PLANT_TELPHONE = $('<div/>').text($('#dialog_txt_Modify_ShipFrom_Telphone').val()).html();
                        Olddata.PLANT_FAX = $('<div/>').text($('#dialog_txt_Modify_ShipFrom_FAX').val()).html();
                        diaShipFromgridDataList.jqGrid('setRowData', RowID, Olddata);
                        $(this).dialog("close");
                    }
                }
            },
            Close: function () {
                $(this).dialog("close");
            }
        },
        close: function () {
            __DialogIsShownNow = true;
        }
    });
});