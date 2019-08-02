$(function () {
    var dialogCustomerCode = $('#dialog_Modify_CustomerCode');
    var diaCustomerCodegridDataList = $('#dialog_VMI_CustomerCodeForAllPlant_gridDataList');

    //Init dialog
    dialogCustomerCode.dialog({
        autoOpen: false,
        height: 220,
        width: 400,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (dialogCustomerCode.attr('Mode') == "C") {
                    if ($('#dialog_txt_Modify_CustomerCode_PLANT').val() == "") {
                        alert("[PLANT] is not allowed empty.");
                    }
                    else {
                        diaCustomerCodegridDataList.jqGrid('addRowData', $('#dialog_txt_Modify_CustomerCode_PLANT').val(),
                                        {
                                            "PLANT": $('<div/>').text($('#dialog_txt_Modify_CustomerCode_PLANT').val()).html(),
                                            "COSTOM_CODE": $('<div/>').text($('#dialog_txt_Modify_CustomerCode_CUSTOMERCODE').val()).html(),
                                            "COSTOM_NAME": $('<div/>').text($('#dialog_txt_Modify_CustomerCode_CUSTOMERNAME').val()).html()
                                        });
                        diaCustomerCodegridDataList.trigger("reloadGrid");
                        $(this).dialog("close");
                    }
                }
                else if (dialogCustomerCode.attr('Mode') == "E") {
                    if ($('#dialog_txt_Modify_CustomerCode_PLANT').val() == "") {
                        alert("[PLANT] is not allowed empty.");
                    }
                    else {
                        var RowID = dialogCustomerCode.attr('RowID');
                        var Olddata = diaCustomerCodegridDataList.jqGrid('getRowData', RowID);
                        Olddata.PLANT = $('<div/>').text($('#dialog_txt_Modify_CustomerCode_PLANT').val()).html();
                        Olddata.COSTOM_CODE = $('<div/>').text($('#dialog_txt_Modify_CustomerCode_CUSTOMERCODE').val()).html();
                        Olddata.COSTOM_NAME = $('<div/>').text($('#dialog_txt_Modify_CustomerCode_CUSTOMERNAME').val()).html();
                        diaCustomerCodegridDataList.jqGrid('setRowData', RowID, Olddata);
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