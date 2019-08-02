$(function () {
    var dialogForwarder = $('#dialog_Modify_Forwarder');
    var diaForwardergridDataList = $('#dialog_VMI_Forwarder_gridDataList');

    //Init dialog
    dialogForwarder.dialog({
        autoOpen: false,
        height: 280,
        width: 430,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (dialogForwarder.attr('Mode') == "C") {
                    if ($.trim($('#dialog_txt_Modify_Forwarder_COMPANY_NAME').val()) == "")
                        alert("[Company Name] is not allowed empty.");
                    else {
                        diaForwardergridDataList.jqGrid('addRowData', $('#dialog_txt_Modify_Forwarder_COMPANY_NAME').val(),
                                        {
                                            "COMPANY_NAME": $('<div/>').text($('#dialog_txt_Modify_Forwarder_COMPANY_NAME').val()).html(),
                                            "TEL": $('<div/>').text($('#dialog_txt_Modify_Forwarder_TEL').val()).html(),
                                            "NAME": $('<div/>').text($('#dialog_txt_Modify_Forwarder_NAME').val()).html(),
                                            "EMAIL": $('<div/>').text($('#dialog_txt_Modify_Forwarder_EMAIL').val()).html(),
                                            "ADDRESS": $('<div/>').text($('#dialog_txt_Modify_Forwarder_ADDRESS').val()).html()
                                        });
                        diaForwardergridDataList.trigger("reloadGrid");
                        $(this).dialog("close");
                    }
                }
                else if (dialogForwarder.attr('Mode') == "E") {
                    if ($.trim($('#dialog_txt_Modify_Forwarder_COMPANY_NAME').val()) == "")
                        alert("[Company Name] is not allowed empty.");
                    else {
                        var RowID = dialogForwarder.attr('RowID');
                        var Olddata = diaForwardergridDataList.jqGrid('getRowData', RowID);
                        Olddata.COMPANY_NAME = $('<div/>').text($('#dialog_txt_Modify_Forwarder_COMPANY_NAME').val()).html();
                        Olddata.TEL = $('<div/>').text($('#dialog_txt_Modify_Forwarder_TEL').val()).html();
                        Olddata.NAME = $('<div/>').text($('#dialog_txt_Modify_Forwarder_NAME').val()).html();
                        Olddata.EMAIL = $('<div/>').text($('#dialog_txt_Modify_Forwarder_EMAIL').val()).html();
                        Olddata.ADDRESS = $('<div/>').text($('#dialog_txt_Modify_Forwarder_ADDRESS').val()).html();
                        diaForwardergridDataList.jqGrid('setRowData', RowID, Olddata);
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