$(function () {
    var diaToDoASNHeader = $('#dialog_VMIProcess_ToDoASNHeader');
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');
    var dateformat = $('input[name$="Date2"]');
    $('#dialog_txt_VMIProcess_ETATime').inputmask("h:s", { "placeholder": "00:00" });

    //Init dialog
    diaToDoASNHeader.dialog({
        autoOpen: false,
        height: 435,
        width: 650,
        resizable: false,
        modal: true,
        buttons: {
            Submit: function () {
                var DoSuccessfully = false;
                var COMPANY_NAME_FD_ERP_VND = $('#dialog_dropbox_VMIConfig_ASNVendorProfile_Forwarder').val().split('@@');
                var COMPANY_NAME, FD_ERP_VND;
                if (COMPANY_NAME_FD_ERP_VND.length > 0) {
                    COMPANY_NAME = COMPANY_NAME_FD_ERP_VND[0];
                    FD_ERP_VND = COMPANY_NAME_FD_ERP_VND[1];
                }
                $.ajax({
                    url: __WebAppPathPrefix + ((diaToDoASNHeader.attr('Mode') == "C") ? "/VMIProcess/CreateToDoASNHeaderInfoForCreate" : "/VMIProcess/UpdateToDoASNHeaderInfo"),
                    data: {
                        ASNNO: escape($.trim($('#dialog_span_VMIProcess_ToDoASNHeader_ASNNo').html())),
                        VENDOR: escape($.trim($('#dialog_span_VMIProcess_ToDoASNHeader_Vendor').html())),
                        PLANTCODE: escape($.trim($('#dialog_span_VMIProcess_ToDoASNHeader_PlantCode').html())),
                        VENDORDNNO: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNHeader_VendorDNNo').val())),
                        INVOICENO: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNHeader_InvoiceNo').val())),
                        TRADETYPE: escape($.trim($('#dialog_dropbox_VMIProcess_ToDoASNHeader_TradeType').val())),
                        ETD: escape($.trim($('#dpToDoASN_ETDDate').val())),
                        ETA: escape($.trim($('#dpToDoASN_ETADate').val())),
                        ETA_TIME: escape($.trim($('#dialog_txt_VMIProcess_ETATime').val())),
                        TRANSFERDOCNO: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNHeader_TransferDocNo').val())),
                        VEHICLETYPEID: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNHeader_VehicleTypeAndID').val())),
                        CUSTOMERSSEALTYPE: escape($.trim($("input:radio[name=CustomerSealType]:checked").val())),
                        DEFAULTCUSTOMERSEAL: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNHeader_CustomerSealType').val())),
                        SHIPFROMTYPE: escape($.trim($("input:radio[name=ShipFromType]:checked").val())),
                        DEFAULTSHIPFROM: escape($.trim($('#dialog_span_VMIProcess_ToDoASNHeader_ShipFrom').html())),
                        OPTIONSHIPFROM: escape($.trim($('#dialog_dropbox_VMIConfig_ASNVendorProfile_ShipFrom').val())),
                        DRIVERNAME: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNHeader_DriverName').val())),
                        DRIVERPHONE: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNHeader_DriverPhone').val())),
                        DECLARATION: escape($.trim($('#dialog_dropbox_VMIProcess_ToDoASNHeader_DeclarationType').val())),
                        INCOTERMS: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNHeader_Incoterms').val())),
                        BUYERCODE: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNHeader_BuyerCode').val())),
                        //COMPANY_NAME: escape($.trim($('#dialog_dropbox_VMIConfig_ASNVendorProfile_Forwarder').val()))
                        COMPANY_NAME: escape($.trim(COMPANY_NAME)),
                        FD_ERP_VND: escape($.trim(FD_ERP_VND)),
                        CB_NAME: escape($.trim($('#dialog_dropbox_VMIConfig_ASNVendorProfile_CustomsBroker').val()))
                    },
                    type: "post",
                    dataType: 'json',
                    async: false,
                    success: function (data) {
                        if (data.Result == true) {
                            DoSuccessfully = true;
                            diaToDoASNManage.prop("ASN_NUM", $.trim(data.Message));
                            //alert(data.Message);
                            if (diaToDoASNHeader.attr('Mode') == "C")
                                alert("CreateASNHead successfully.");
                            else
                                alert("UpdateASNHeader successfully.");

                            //EnableToDoASNFunction();
                            bButtonFunctionEnable = true;
                            InitdialogToDoASNHeaderForManage();
                            ReloadToDoASNManagegridDataList();

                            diaToDoASNManage.show();
                            diaToDoASNManage.dialog("open");
                        }
                        else {
                            alert(data.Message);
                        }
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    },
                    complete: function (jqXHR, textStatus) {
                        //HideAjaxLoading();
                    }
                });

                if (DoSuccessfully) {
                    $(this).dialog("close");
                }
            },
            Close: function () {

                dateformat.datepicker('disable');
                $(this).dialog("close");
            }
        },
        close: function () {
            __DialogIsShownNow = false;
        },
        open: function (event, ui) {
            dateformat.datepicker('enable');
        }
    });

    $('#dialog_dropbox_VMIProcess_ToDoASNHeader_TradeType').change(function () {
        if ($('#hiddenImportedASN').text() == 'True' && $('#dialog_dropbox_VMIProcess_ToDoASNHeader_TradeType').val() == 'I') {
            $('.trForwarderFields').show();
            $('#dialog_VMIProcess_ToDoASNHeader').dialog('option', 'height', 600);
            $('#dialog_VMIProcess_ToDoASNHeader').dialog('option', 'width', 690);
        }
        else {
            $('.trForwarderFields').hide();
            $('#dialog_VMIProcess_ToDoASNHeader').dialog('option', 'height', 435);
            $('#dialog_VMIProcess_ToDoASNHeader').dialog('option', 'width', 650);
        }

    });

    $('#dialog_dropbox_VMIConfig_ASNVendorProfile_Forwarder').change(function () {
        var COMPANY_NAME_ERP_VND = $(this).val().split('@@');
        var COMPANY_NAME, ERP_VND;
        if (COMPANY_NAME_ERP_VND.length > 0) {
            COMPANY_NAME = COMPANY_NAME_ERP_VND[0];
            ERP_VND = COMPANY_NAME_ERP_VND[1];
        }

        //var ERP_VND = $('#dialog_span_VMIProcess_ToDoASNHeader_Vendor').text();
        //var PLANT = $('#dialog_span_VMIProcess_ToDoASNHeader_PlantCode').text();

        $.ajax({
            url: __WebAppPathPrefix + '/VMIProcess/GetForwarderInfo',
            data: {
                COMPANY_NAME: escape($.trim(COMPANY_NAME)),
                ERP_VND: escape($.trim(ERP_VND))//,
                //PLANT: escape($.trim(PLANT))
            },
            type: "post",
            dataType: 'json',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                $('#dialog_span_VMIProcess_ToDoASNHeader_ForwarderTel').text(data.TEL == null ? '' : data.TEL);
                $('#dialog_span_VMIProcess_ToDoASNHeader_ForwarderName').text(data.NAME == null ? '' : data.NAME);
                //$('#dialog_span_VMIProcess_ToDoASNHeader_ForwarderEmail').text(data.EMAIL == null ? '' : data.EMAIL);
                if (data.EMAIL != '' && data.EMAIL != null) {
                    var emails = data.EMAIL.split(';');
                    if (emails.length > 1) {
                        $('#dialog_span_VMIProcess_ToDoASNHeader_ForwarderEmail').html(emails[0] + ' [Click me]');
                        $('#dialog_span_VMIProcess_ToDoASNHeader_ForwarderEmail').attr({ 'style': 'cursor:pointer;color:blue;', 'title': 'click me to show all Email.' });

                        $('#dialog_span_VMIProcess_ToDoASNHeader_ForwarderEmail').unbind('click');

                        $('#dialog_span_VMIProcess_ToDoASNHeader_ForwarderEmail').click(function (e) {
                            e.preventDefault();
                            alert(data.EMAIL.replace(/\;/g, '\n'));
                        });
                    }
                    else {
                        $('#dialog_span_VMIProcess_ToDoASNHeader_ForwarderEmail').html(emails[0]);
                        $('#dialog_span_VMIProcess_ToDoASNHeader_ForwarderEmail').attr({ 'style': '', 'title': '' });
                        $('#dialog_span_VMIProcess_ToDoASNHeader_ForwarderEmail').unbind('click');
                    }
                }
                else {
                    $('#dialog_span_VMIProcess_ToDoASNHeader_ForwarderEmail').html(data.EMAIL);
                }
                $('#dialog_span_VMIProcess_ToDoASNHeader_ForwarderAddress').text(data.ADDRESS == null ? '' : data.ADDRESS);
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    });
});

function CheckPassToCreateASNHead() {
    var diaToDoASNHeader = $('#dialog_VMIProcess_ToDoASNHeader');

    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/CheckPassToCreateASNHeader',
        data: {
            PLANT: escape($.trim($('#txt_VMIProcess_ToDoASN_Plant').val())),
            ERP_VND: escape($.trim($('#txt_VMIProcess_ToDoASN_SBU_VDN').val()))
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.Result == true) {
                //Init Dialog ToDoASNHeader By Mode
                InitdialogToDoASNHeader(diaToDoASNHeader.attr('Mode'));
            }
            else {
                alert(data.Message);
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
            //HideAjaxLoading();
        }
    });
}

function InitdialogToDoASNHeader(Mode) {
    var diaToDoASNHeader = $('#dialog_VMIProcess_ToDoASNHeader');

    //datepicker
    var dateformat = $('input[name$="Date2"]');
    dateformat.datepicker({
        dateFormat: 'yy/mm/dd',
        onSelect: function () {
            diaToDoASNHeader.focus();
        }
    });

    dateformat.datepicker('disable');

    var objSelectForTradeType = $('#dialog_dropbox_VMIProcess_ToDoASNHeader_TradeType');
    var objSelectForShipFrom = $('#dialog_dropbox_VMIConfig_ASNVendorProfile_ShipFrom');
    objSelectForShipFrom.find('option').remove().end();
    var objSelectForForwarder = $('#dialog_dropbox_VMIConfig_ASNVendorProfile_Forwarder');
    objSelectForForwarder.find('option').remove().end();
    var objSelectForCustomsBroker = $('#dialog_dropbox_VMIConfig_ASNVendorProfile_CustomsBroker');
    objSelectForCustomsBroker.find('option').remove().end();
    var hiddenImportedASN = $('#hiddenImportedASN');
    hiddenImportedASN.text('');

    $('#dialog_span_VMIProcess_ToDoASNHeader_ForwarderTel').text('');
    $('#dialog_span_VMIProcess_ToDoASNHeader_ForwarderName').text('');
    $('#dialog_span_VMIProcess_ToDoASNHeader_ForwarderEmail').text('');
    $('#dialog_span_VMIProcess_ToDoASNHeader_ForwarderAddress').text('');
    $('#dialog_txt_VMIProcess_ToDoASNHeader_Incoterms').val('');
    $('#dialog_txt_VMIProcess_ToDoASNHeader_BuyerCode').val('');

    var $radiosCST = $('input:radio[name=CustomerSealType]');
    var $radiosSFT = $('input:radio[name=ShipFromType]');

    switch (Mode) {
        case 'C':
            $.ajax({
                url: __WebAppPathPrefix + '/VMIProcess/QueryToDoASNHeaderInfoForCreate',
                data: {
                    PLANT: escape($.trim($('#txt_VMIProcess_ToDoASN_Plant').val())),
                    ERP_VND: escape($.trim($('#txt_VMIProcess_ToDoASN_SBU_VDN').val()))
                },
                type: "post",
                dataType: 'json',
                async: false,
                success: function (data) {
                    if (data.HASRESULT == true) {
                        $('#dialog_span_VMIProcess_ToDoASNHeader_ASNNo').html("");
                        $('#dialog_span_VMIProcess_ToDoASNHeader_CreatedDate').html(data.ASNCREATEDATE);
                        $('#dialog_span_VMIProcess_ToDoASNHeader_InboundDNNo').html("");
                        $('#dialog_span_VMIProcess_ToDoASNHeader_Vendor').html(data.VENDOR);
                        $('#dialog_span_VMIProcess_ToDoASNHeader_CustomerCode').html(data.CUSTOMERCODE);
                        $('#dialog_txt_VMIProcess_ToDoASNHeader_VendorDNNo').val("");
                        $('#dialog_span_VMIProcess_ToDoASNHeader_CustomerName').html(data.CUSTOMERNAME);
                        $('#dialog_txt_VMIProcess_ToDoASNHeader_InvoiceNo').val("");
                        $('#dialog_span_VMIProcess_ToDoASNHeader_PlantCode').html(data.PLANTCODE);
                        $('#dialog_dropbox_VMIProcess_ToDoASNHeader_TradeType').attr('disabled', false);
                        if (data.TRADETYPE != "O") {
                            objSelectForTradeType.find('option').remove().end();
                            objSelectForTradeType.append($("<option></option>").attr("value", "A").text("A:Local"));
                            objSelectForTradeType.append($("<option></option>").attr("value", "P").text("P:Transfer"));
                            objSelectForTradeType.append($("<option></option>").attr("value", "I").text("I:Import"));
                            objSelectForTradeType.append($("<option></option>").attr("value", "B").text("B:Taxed"));
                            objSelectForTradeType.append($("<option></option>").attr("value", "C").text("C:Concentrate"));
                            if (data.TRADETYPE != "")
                                $('#dialog_dropbox_VMIProcess_ToDoASNHeader_TradeType').val(data.TRADETYPE);
                            else
                                $('#dialog_dropbox_VMIProcess_ToDoASNHeader_TradeType').val("A");
                        }
                        else {
                            if (data.bOlnyOTredeType == false) {
                                objSelectForTradeType.find('option').remove().end();
                                objSelectForTradeType.append($("<option></option>").attr("value", "A").text("A:Local"));
                                objSelectForTradeType.append($("<option></option>").attr("value", "P").text("P:Transfer"));
                                objSelectForTradeType.append($("<option></option>").attr("value", "I").text("I:Import"));
                                objSelectForTradeType.append($("<option></option>").attr("value", "B").text("B:Taxed"));
                                objSelectForTradeType.append($("<option></option>").attr("value", "C").text("C:Concentrate"));
                                $('#dialog_dropbox_VMIProcess_ToDoASNHeader_TradeType').val("A");
                            }
                            else {
                                objSelectForTradeType.find('option').remove().end();
                                objSelectForTradeType.append($("<option></option>").attr("value", "O").text("O:Opto"));
                            }
                        }
                        $('#dpToDoASN_ETDDate').datepicker('setDate', SetCurrentDate(0, 0));
                        $('#dpToDoASN_ETADate').datepicker('setDate', SetCurrentDate(0, 0));
                        $('#dialog_txt_VMIProcess_ETATime').val("");
                        $('#dialog_txt_VMIProcess_ToDoASNHeader_TransferDocNo').val("");
                        $('#dialog_txt_VMIProcess_ToDoASNHeader_VehicleTypeAndID').val("");
                        $radiosCST.filter('[value=N]').prop('checked', true);
                        $('#dialog_txt_VMIProcess_ToDoASNHeader_CustomerSealType').val("");
                        $radiosSFT.filter('[value=D]').prop('checked', true);
                        $('#dialog_span_VMIProcess_ToDoASNHeader_ShipFrom').html(data.DEFAULTSHIPFROM);
                        if (data.LPLANTSHORTNAME.length > 0) {
                            for (var iCnt = 0; iCnt < data.LPLANTSHORTNAME.length; iCnt++)
                                objSelectForShipFrom.append($("<option></option>").attr("value", data.LPLANTSHORTNAME[iCnt]).text(data.LPLANTSHORTNAME[iCnt]));
                        }
                        else {
                            objSelectForShipFrom.prop("disabled", true);
                        }
                        $('#dialog_txt_VMIProcess_ToDoASNHeader_DriverName').val("");
                        $('#dialog_txt_VMIProcess_ToDoASNHeader_DriverPhone').val("");
                        $('#dialog_dropbox_VMIProcess_ToDoASNHeader_DeclarationType').val(data.DECLARATION);

                        objSelectForForwarder.append($('<option/>').attr('value', '').text(''));
                        if (data.LCOMPANY_NAME.length > 0) {
                            for (var iCnt = 0; iCnt < data.LCOMPANY_NAME.length; iCnt++)
                                objSelectForForwarder.append($("<option></option>").attr("value", data.LCOMPANY_NAME[iCnt]).text(data.LCOMPANY_NAME[iCnt].split('@@')[0] + '(' + data.LCOMPANY_NAME[iCnt].split('@@')[1] + ')'));
                        }

                        objSelectForCustomsBroker.append($('<option/>').attr('value', '').text(''));
                        if (data.LCB_NAME.length > 0) {
                            for (var iCnt = 0; iCnt < data.LCB_NAME.length; iCnt++)
                                objSelectForCustomsBroker.append($("<option></option>").attr("value", data.LCB_NAME[iCnt]).text(data.LCB_NAME[iCnt]));
                        }
                        hiddenImportedASN.text(data.CIMPORTEDASN);
                        if (data.CIMPORTEDASN == "True" && $('#dialog_dropbox_VMIProcess_ToDoASNHeader_TradeType').val() == 'I') { // For Imported ASN
                            $('.trForwarderFields').show();
                            $('#dialog_VMIProcess_ToDoASNHeader').dialog('option', 'height', 600);
                            $('#dialog_VMIProcess_ToDoASNHeader').dialog('option', 'width', 690);
                        }
                        else {
                            $('.trForwarderFields').hide();
                            $('#dialog_VMIProcess_ToDoASNHeader').dialog('option', 'height', 435);
                            $('#dialog_VMIProcess_ToDoASNHeader').dialog('option', 'width', 650);
                        }

                        if (!__DialogIsShownNow) {
                            __DialogIsShownNow = true;
                            diaToDoASNHeader.show();
                            diaToDoASNHeader.dialog("open");
                        }
                    }
                    else {
                        alert("The function is not avaliable to use.");
                        //alert("Cannot initial the dialog because not exist data.");
                    }
                },
                error: function (xhr, textStatus, thrownError) {
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, textStatus) {
                    //HideAjaxLoading();
                }
            });
            break;
        case 'U':
            __DialogIsShownNow = false;
            $.ajax({
                url: __WebAppPathPrefix + '/VMIProcess/QueryToDoASNHeaderInfoForUpdate',
                data: {
                    ASN_NUM: escape($.trim($('#dialog_span_VMIProcess_ToDoASNManage_ASNNo').html()))
                },
                type: "post",
                dataType: 'json',
                async: false,
                success: function (data) {
                    if (data.HASRESULT == true) {
                        $('#dialog_span_VMIProcess_ToDoASNHeader_ASNNo').html(data.ASNNO);
                        $('#dialog_span_VMIProcess_ToDoASNHeader_CreatedDate').html(data.ASNCREATEDATE);
                        $('#dialog_span_VMIProcess_ToDoASNHeader_InboundDNNo').html(data.INBOUNDDNNO);
                        $('#dialog_span_VMIProcess_ToDoASNHeader_Vendor').html(data.VENDOR);
                        $('#dialog_span_VMIProcess_ToDoASNHeader_CustomerCode').html(data.CUSTOMERCODE);
                        $('#dialog_txt_VMIProcess_ToDoASNHeader_VendorDNNo').val(data.VENDORDNNO);
                        $('#dialog_span_VMIProcess_ToDoASNHeader_CustomerName').html(data.CUSTOMERNAME);
                        $('#dialog_txt_VMIProcess_ToDoASNHeader_InvoiceNo').val(data.INVOICENO);
                        $('#dialog_span_VMIProcess_ToDoASNHeader_PlantCode').html(data.PLANTCODE);
                        if (data.TRADETYPE != "O") {
                            objSelectForTradeType.find('option').remove().end();
                            objSelectForTradeType.append($("<option></option>").attr("value", "A").text("A:Local"));
                            objSelectForTradeType.append($("<option></option>").attr("value", "P").text("P:Transfer"));
                            objSelectForTradeType.append($("<option></option>").attr("value", "I").text("I:Import"));
                            objSelectForTradeType.append($("<option></option>").attr("value", "B").text("B:Taxed"));
                            objSelectForTradeType.append($("<option></option>").attr("value", "C").text("C:Concentrate"));
                            $('#dialog_dropbox_VMIProcess_ToDoASNHeader_TradeType').val(data.TRADETYPE);
                        }
                        else {
                            objSelectForTradeType.find('option').remove().end();
                            objSelectForTradeType.append($("<option></option>").attr("value", "O").text("O:Opto"));
                        }
                        if (data.ISIMGPLANT == true) {
                            $('#dialog_dropbox_VMIProcess_ToDoASNHeader_TradeType').attr('disabled', true);
                        }
                        else {
                            $('#dialog_dropbox_VMIProcess_ToDoASNHeader_TradeType').attr('disabled', false);
                        }
                        $('#dpToDoASN_ETDDate').datepicker('setDate', data.ETD);
                        $('#dpToDoASN_ETADate').datepicker('setDate', data.ETA);
                        $('#dialog_txt_VMIProcess_ETATime').val(data.ETA_TIME);
                        $('#dialog_txt_VMIProcess_ToDoASNHeader_TransferDocNo').val(data.TRANSFERDOCNO);
                        $('#dialog_txt_VMIProcess_ToDoASNHeader_VehicleTypeAndID').val(data.VEHICLETYPEID);
                        $radiosCST.filter('[value=N]').prop('checked', true);
                        $('#dialog_txt_VMIProcess_ToDoASNHeader_CustomerSealType').val(data.DEFAULTCUSTOMERSEAL);
                        $('#dialog_span_VMIProcess_ToDoASNHeader_ShipFrom').html(data.DEFAULTSHIPFROM);
                        if (data.LPLANTSHORTNAME.length > 0) {
                            for (var iCnt = 0; iCnt < data.LPLANTSHORTNAME.length; iCnt++)
                                objSelectForShipFrom.append($("<option></option>").attr("value", data.LPLANTSHORTNAME[iCnt]).text(data.LPLANTSHORTNAME[iCnt]));
                        }
                        else {
                            objSelectForShipFrom.prop("disabled", true);
                        }
                        if (data.OPTIONSHIPFROM == "")
                            $radiosSFT.filter('[value=D]').prop('checked', true);
                        else {
                            $radiosSFT.filter('[value=O]').prop('checked', true);
                            objSelectForShipFrom.val(data.OPTIONSHIPFROM);
                        }
                        $('#dialog_txt_VMIProcess_ToDoASNHeader_DriverName').val(data.DRIVERNAME);
                        $('#dialog_txt_VMIProcess_ToDoASNHeader_DriverPhone').val(data.DRIVERPHONE);
                        $('#dialog_dropbox_VMIProcess_ToDoASNHeader_DeclarationType').val(data.DECLARATION);

                        objSelectForForwarder.append($('<option/>').attr('value', '').text(''));
                        if (data.LCOMPANY_NAME.length > 0) {
                            for (var iCnt = 0; iCnt < data.LCOMPANY_NAME.length; iCnt++)
                                objSelectForForwarder.append($("<option></option>").attr("value", data.LCOMPANY_NAME[iCnt]).text(data.LCOMPANY_NAME[iCnt].split('@@')[0] + '(' + data.LCOMPANY_NAME[iCnt].split('@@')[1] + ')'));
                        }

                        objSelectForCustomsBroker.append($('<option/>').attr('value', '').text(''));
                        if (data.LCB_NAME.length > 0) {
                            for (var iCnt = 0; iCnt < data.LCB_NAME.length; iCnt++)
                                objSelectForCustomsBroker.append($("<option></option>").attr("value", data.LCB_NAME[iCnt]).text(data.LCB_NAME[iCnt]));
                        }
                        hiddenImportedASN.text(data.CIMPORTEDASN);

                        if (data.CIMPORTEDASN == "True" && $('#dialog_dropbox_VMIProcess_ToDoASNHeader_TradeType').val() == 'I') { // For Imported ASN
                            $('.trForwarderFields').show();
                            $('#dialog_VMIProcess_ToDoASNHeader').dialog('option', 'height', 600);
                            $('#dialog_VMIProcess_ToDoASNHeader').dialog('option', 'width', 690);
                            $('#dialog_txt_VMIProcess_ToDoASNHeader_Incoterms').val(data.INCOTERMS);
                            $('#dialog_txt_VMIProcess_ToDoASNHeader_BuyerCode').val(data.BUYERCODE);
                            $('#dialog_span_VMIProcess_ToDoASNHeader_ForwarderTel').text(data.TEL == null ? '' : data.TEL);
                            $('#dialog_span_VMIProcess_ToDoASNHeader_ForwarderName').text(data.NAME == null ? '' : data.NAME);
                            $('#dialog_span_VMIProcess_ToDoASNHeader_ForwarderEmail').text(data.EMAIL == null ? '' : data.EMAIL);
                            $('#dialog_span_VMIProcess_ToDoASNHeader_ForwarderAddress').text(data.ADDRESS == null ? '' : data.ADDRESS);
                            objSelectForForwarder.val(data.COMPANY_NAME + '@@' + data.FD_ERP_VND);
                            objSelectForCustomsBroker.val(data.CB_NAME);
                        }
                        else {
                            $('.trForwarderFields').hide();
                            $('#dialog_VMIProcess_ToDoASNHeader').dialog('option', 'height', 435);
                            $('#dialog_VMIProcess_ToDoASNHeader').dialog('option', 'width', 650);
                        }

                        if (!__DialogIsShownNow) {
                            __DialogIsShownNow = true;
                            diaToDoASNHeader.show();
                            diaToDoASNHeader.dialog("open");
                        }
                    }
                    else {
                        alert("The function is not avaliable to use.");
                        //alert("Cannot initial the dialog because not exist data.")
                    }
                },
                error: function (xhr, textStatus, thrownError) {
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, textStatus) {
                    //HideAjaxLoading();
                }
            });
            break;
    };
}

//20160419 Edward Haung Add for replace digit by index
//start : index
//whichcode : type which code
//value : old value
//reurn new value
function replaceNumericAt(start, whichcode, value) {
    var tempStr;
    var number;

    switch (whichcode) {
        case 48:
            number = 0;
            break;
        case 49:
            number = 1;
            break;
        case 50:
            number = 2;
            break;
        case 51:
            number = 3;
            break;
        case 52:
            number = 4;
            break;
        case 53:
            number = 5;
            break;
        case 54:
            number = 6;
            break;
        case 55:
            number = 7;
            break;
        case 56:
            number = 8;
            break;
        case 57:
            number = 9;
            break;
    }

    switch (start) {
        case 0:
            if (number < 3)
                tempStr = number + value.substring(1, 5);
            else
                tempStr = "_" + value.substring(1, 5);
            break;
        case 1:
            if (value.substring(0, 1) == "0" || value.substring(0, 1) == "1") {
                tempStr = value.substring(0, 1) + number + value.substring(2, 5);
            }
            else if (value.substring(0, 1) == "2") {
                if (number < 5)
                    tempStr = value.substring(0, 1) + number + value.substring(2, 5);
                else
                    tempStr = value.substring(0, 1) + "_" + value.substring(2, 5);
            }
            break;
        case 3:
            if (number < 7)
                tempStr = value.substring(0, 3) + number + value.substring(3, 4);
            else
                tempStr = value.substring(0, 3) + "_" + value.substring(3, 4);
            break;
        case 4:
            tempStr = value.substring(0, 4) + number;
            break;
        default:
            tempStr = value;
            break;
    }

    return tempStr;
}

function replaceSpecialKeys(start, whichcode, value) {
    var tempStr;
    var sign;

    switch (whichcode) {
        case 8:
            sign = "_";
            break;
    }

    switch (start) {
        case 1:
            tempStr = sign + value.substring(1, 5);
            break;
        case 2:
        case 3:
            tempStr = value.substring(0, 1) + sign + value.substring(2, 5);
            break;
        case 4:
            tempStr = value.substring(0, 3) + sign + value.substring(4, 5);
            break;
        case 5:
            tempStr = value.substring(0, 4) + sign;
            break;
        default:
            tempStr = value;
            break;
    }

    return tempStr;
}
