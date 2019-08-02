$(function () {
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');
    var diaToDoASNDetail = $('#dialog_VMIProcess_ToDoASNDetail');
    var diaToDoASNDetailError = $('#dialog_VMIProcess_ToDoASNDetailError');

    $('.jqTextForNumeric').keypress(function (e) {
        if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {
            e.preventDefault();
        }
        else {
            $(this).each(function () {
                if ($(this).val().indexOf('.') >= 0 && e.which == 46)
                    e.preventDefault();
            });
        }
    });

    $('.jqTextForSelectAll').focus(function (e) {
        this.select();
    });

    $('#dialog_txt_VMIProcess_ToDoASNDetail_POSANO').focusout(function () {

        if ($('#dialog_txt_VMIProcess_ToDoASNDetail_POSANO').val() != "")
            $('#dialog_span_VMIProcess_ToDoASNDetail_TNetWeight').html("0.00")

        if ($('#dialog_txt_VMIProcess_ToDoASNDetail_POSANO').val() != "" && $('#dialog_txt_VMIProcess_ToDoASNDetail_POLine').val() != "") {
            GetCreateDetailInfo();
            ComputeTotalNetWeight();
        }
    });

    $('#dialog_txt_VMIProcess_ToDoASNDetail_POLine').focusout(function () {
        if ($('#dialog_txt_VMIProcess_ToDoASNDetail_POLine').val() != "") {
            $('#dialog_txt_VMIProcess_ToDoASNDetail_POLine').val(padLeft($('#dialog_txt_VMIProcess_ToDoASNDetail_POLine').val(), 5));
            $('#dialog_span_VMIProcess_ToDoASNDetail_TNetWeight').html("0.00")
        }

        if ($('#dialog_txt_VMIProcess_ToDoASNDetail_POSANO').val() != "" && $('#dialog_txt_VMIProcess_ToDoASNDetail_POLine').val() != "") {
            GetCreateDetailInfo();
            ComputeTotalNetWeight();
        }
    });

    $('#dialog_txt_VMIProcess_ToDoASNDetail_ASNQty, #dialog_txt_VMIProcess_ToDoASNDetail_UnitNetWeight').bind('change keyup', function () {
        if(diaToDoASNDetail.attr('Mode') == "C") GetCreateUOM();
        ComputeTotalNetWeight();
    });

    //$('#dialog_txt_VMIProcess_ToDoASNDetail_ASNQty').keyup(function () {
    //    GetCreateUOM();
    //    ComputeTotalNetWeight();
    //});

    //$('#dialog_txt_VMIProcess_ToDoASNDetail_UnitNetWeight').keyup(function () {
    //    GetCreateUOM();
    //    ComputeTotalNetWeight();
    //});

    //Init dialog
    diaToDoASNDetail.dialog({
        autoOpen: false,
        height: 580,
        width: 700,
        resizable: false,
        modal: true,
        buttons: {
            Submit: function () {
                if ($('#addReviewingFlag') != undefined && $('#addReviewingFlag').text() != '') {
                    if ($('#addReviewingFlag').text() == 'VMI: VMIASNReviewer') {
                        alert('You have no right to modify detail.');
                        return false;
                    }
                }

                var DoSuccessfully = false;
                $.ajax({
                    url: __WebAppPathPrefix + ((diaToDoASNDetail.attr('Mode') == "C") ? "/VMIProcess/CreateToDoASNDetailInfoForCreate" : "/VMIProcess/UpdateToDoASNDetailInfo"),
                    data: {
                        PLANT: escape($.trim(diaToDoASNManage.attr('PLANT'))),
                        VENDOR: escape($.trim(diaToDoASNManage.attr('VENDOR'))),
                        ETA: escape($.trim(diaToDoASNManage.attr('ETA'))),
                        ASNNUM: escape($.trim($('#dialog_span_VMIProcess_ToDoASNDetail_ASNNo').html())),
                        ASNLINE: escape($.trim(diaToDoASNDetail.attr("ASNLINE"))),
                        STATUS: escape($.trim($('#dialog_span_VMIProcess_ToDoASNDetail_Status').html())),
                        KEEPER: escape($.trim($('#dialog_span_VMIProcess_ToDoASNDetail_LogHandingGroup').html())),
                        MATERIAL: escape($.trim($('#dialog_span_VMIProcess_ToDoASNDetail_MaterialNo').html())),
                        CUSTOMDESCRIPTION: escape($.trim($('#dialog_span_VMIProcess_ToDoASNDetail_CustomDesc').html())),
                        DESCRIPTION: escape($.trim($('#dialog_span_VMIProcess_ToDoASNDetail_Description').html())),
                        PRODUCTNO: escape($.trim($('#dialog_span_VMIProcess_ToDoASNDetail_CustomGroupNo').html())),
                        PONUM: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_POSANO').val())),
                        POLINE: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_POLine').val())),
                        UOM: escape($.trim($('#dialog_span_VMIProcess_ToDoASNDetail_UOM').html())),
                        ASNQTY: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_ASNQty').val())),
                        PACKAGEDESCRIPTION: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_PackingDetail').val())),
                        NETWT: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_UnitNetWeight').val())),
                        VENDORMATERIAL: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_VendorMaterialNo').val())),
                        CUSTOMSSEAL: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_CustomesSeal').val()) == "" ? $.trim($('#dialog_dropbox_VMIProcess_ToDoASNDetail_CustomesSeal').val()) : $.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_CustomesSeal').val())),
                        TOTSET: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_TPacking').val())),
                        TOTNW: escape($.trim($('#dialog_span_VMIProcess_ToDoASNDetail_TNetWeight').html())),
                        TOTGW: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_TGrossWeight').val())),
                        PACKMATH: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_PackingType').val())),
                        ORGCNTYCODE: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_OriginCountryCode').val())),
                        ORGCNTY: escape($.trim($('#dialog_span_VMIProcess_ToDoASNDetail_OriginCountry').val())),
                        HAWB: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_HAWB').val())),
                        MAWB: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_MAWB').val())),
                        MFGDATE: escape($.trim($('#dpToDoASN_MFGDate').val())),
                        PRODLINE: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_Remark').val())),
                        BATCH: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_Batch').val())),
                        INVOICEUNITPRICE: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_InvoiceUnitPrice').val())),
                        VENDORMATERIALNUMBER: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_VendorMaterialNumber').val())),
                        VENDORCUSTOMSERIALNO: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_VendorCustomSerialNo').val())),
                        VENDORHSCODE: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_VendorHSCode').val())),
                        VENDORCUSTOMPARTNAME: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_VendorCustomPartName').val())),
                        VENDORUOM: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_VendorUOM').val()))
                    },
                    type: "post",
                    dataType: 'json',
                    async: false,
                    success: function (data) {
                        if (data.Result == true) {
                            DoSuccessfully = true;

                            alert(data.Message);
                            if ((diaToDoASNDetail.attr('Mode') == "C")) {
                                ReloadToDoASNManagegridDataList();
                            }
                            else {
                                var updateDataRow = {
                                    ASNQTY: $.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_ASNQty').val()),
                                    PACKAGEDESCRIPTION: $.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_PackingDetail').val()),
                                    NETWT: $.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_UnitNetWeight').val()),
                                    VENDORMATERIAL: $.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_VendorMaterialNo').val()),
                                    CUSTOMSSEAL: $.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_CustomesSeal').val()) == "" ? $.trim($('#dialog_dropbox_VMIProcess_ToDoASNDetail_CustomesSeal').val()) : $.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_CustomesSeal').val()),
                                    TOTSET: $.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_TPacking').val()),
                                    TOTNW: $.trim($('#dialog_span_VMIProcess_ToDoASNDetail_TNetWeight').html()),
                                    TOTGW: $.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_TGrossWeight').val()),
                                    PACKMATH: $.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_PackingType').val()),
                                    ORGCNTYCODE: $.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_OriginCountryCode').val()),
                                    HAWB: $.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_HAWB').val()),
                                    MAWB: $.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_MAWB').val()),
                                    MFGDATE: $.trim($('#dpToDoASN_MFGDate').val()),
                                    PRODLINE: $.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_Remark').val()),
                                    BATCH: $.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_Batch').val())
                                };
                                $('#VMI_Process_ToDoASNManage_gridDataList').jqGrid("setRowData", diaToDoASNDetail.attr("ROWID"), updateDataRow);
                            }

                            InitdialogToDoASNHeaderForManage();
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
                $(this).dialog("close");
            }
        },
        close: function () {
            __DialogIsShownNow = false;
        }
    });
});

function InitdialogToDoASNDetail(Mode) {
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');
    var diaToDoASNDetail = $('#dialog_VMIProcess_ToDoASNDetail');

    __DialogIsShownNow = false;

    //datepicker
    var dateformat = $('input[name$="Date"]');
    dateformat.datepicker({
        dateFormat: 'yy/mm/dd',
    });

    switch (Mode) {
        case 'C':
            diaToDoASNDetail.attr('Mode', 'C');
            $.ajax({
                url: __WebAppPathPrefix + '/VMIProcess/CheckIsEnableToDoASNFunctionByType',
                data: {
                    ASN_NUM: escape($.trim($('#dialog_span_VMIProcess_ToDoASNManage_ASNNo').html())),
                    bIsAllCheck: false
                },
                type: "post",
                dataType: 'json',
                async: false,
                success: function (data) {
                    if (data.Result) {
                        $('#dialog_span_VMIProcess_ToDoASNDetail_ASNNo').html($('#dialog_span_VMIProcess_ToDoASNManage_ASNNo').html());
                        $('#dialog_span_VMIProcess_ToDoASNDetail_Status').html("New");
                        $('#dialog_span_VMIProcess_ToDoASNDetail_LogHandingGroup').html("");
                        $('#dialog_span_VMIProcess_ToDoASNDetail_MaterialNo').html("");
                        $('#dialog_span_VMIProcess_ToDoASNDetail_CustomDesc').html("");
                        $('#dialog_span_VMIProcess_ToDoASNDetail_Description').html("");
                        $('#dialog_span_VMIProcess_ToDoASNDetail_CustomGroupNo').html("");

                        $("#dialog_span_VMIProcess_ToDoASNDetail_POSANO").hide();
                        $('#dialog_span_VMIProcess_ToDoASNDetail_POSANO').html("");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_POSANO').show();
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_POSANO').val("");

                        $("#dialog_span_VMIProcess_ToDoASNDetail_POLine").hide();
                        $('#dialog_span_VMIProcess_ToDoASNDetail_POLine').html("");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_POLine').show();
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_POLine').val("");

                        $('#dialog_span_VMIProcess_ToDoASNDetail_UOM').html("");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_ASNQty').val("0.000");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_PackingDetail').val("");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_UnitNetWeight').prop('disabled', false);
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_UnitNetWeight').val("0.000000");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_VendorMaterialNo').val("");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_CustomesSeal').show();
                        $('#dialog_dropbox_VMIProcess_ToDoASNDetail_CustomesSeal').hide();
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_CustomesSeal').val("");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_CustomesSeal').prop('disabled', false);
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_TPacking').val("0");
                        $('#dialog_span_VMIProcess_ToDoASNDetail_TNetWeight').html("");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_TGrossWeight').val("0.00");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_PackingType').val("");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_OriginCountryCode').val("");
                        $('#dialog_span_VMIProcess_ToDoASNDetail_OriginCountry').val("");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_HAWB').val("");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_MAWB').val("");
                        $('#dpToDoASN_MFGDate').val("");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_Remark').val("");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_Batch').val("");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_InvoiceUnitPrice').val("");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_VendorMaterialNumber').val("");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_VendorCustomSerialNo').val("");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_VendorHSCode').val("");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_VendorCustomPartName').val("");
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_VendorUOM').val("");

                        if (!__DialogIsShownNow) {
                            __DialogIsShownNow = true;
                            diaToDoASNDetail.show();
                            diaToDoASNDetail.dialog("open");
                        }
                    }
                    else {
                        alert("The function is not avaliable to use.");
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
            diaToDoASNDetail.attr('Mode', 'U');
            $.ajax({
                url: __WebAppPathPrefix + '/VMIProcess/QueryToDoASNDetailInfoForModify',
                data: {
                    ASNNUM: escape($.trim(diaToDoASNDetail.attr("ASNNUM"))),
                    ASNLINE: escape($.trim(diaToDoASNDetail.attr("ASNLINE"))),
                    PLANT: escape($.trim(diaToDoASNManage.attr('PLANT'))),
                    VENDOR: escape($.trim(diaToDoASNManage.attr('VENDOR')))
                },
                type: "post",
                dataType: 'json',
                async: false,
                success: function (data) {
                    if (data.HASRESULT) {
                        $('#dialog_span_VMIProcess_ToDoASNDetail_ASNNo').html(data.ASNNUM);
                        $('#dialog_span_VMIProcess_ToDoASNDetail_Status').html("New");
                        $('#dialog_span_VMIProcess_ToDoASNDetail_LogHandingGroup').html(data.KEEPER);
                        $('#dialog_span_VMIProcess_ToDoASNDetail_MaterialNo').html(data.MATERIAL);
                        $('#dialog_span_VMIProcess_ToDoASNDetail_CustomDesc').html(data.CUSTOMDESCRIPTION);
                        $('#dialog_span_VMIProcess_ToDoASNDetail_Description').html(data.DESCRIPTION);
                        $('#dialog_span_VMIProcess_ToDoASNDetail_CustomGroupNo').html(data.PRODUCTNO);

                        $("#dialog_span_VMIProcess_ToDoASNDetail_POSANO").show();
                        $('#dialog_span_VMIProcess_ToDoASNDetail_POSANO').html(data.PONUM);
                        $("#dialog_txt_VMIProcess_ToDoASNDetail_POSANO").hide();
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_POSANO').val(data.PONUM);

                        $("#dialog_span_VMIProcess_ToDoASNDetail_POLine").show();
                        $('#dialog_span_VMIProcess_ToDoASNDetail_POLine').html(data.POLINE);
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_POLine').hide();
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_POLine').val(data.POLINE);

                        $('#dialog_span_VMIProcess_ToDoASNDetail_UOM').html(data.UOM);
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_ASNQty').val(data.ASNQTY);
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_PackingDetail').val(data.PACKAGEDESCRIPTION);
                        if (data.NETWT == 0)
                            $('#dialog_txt_VMIProcess_ToDoASNDetail_UnitNetWeight').prop('disabled', false);
                        else
                            $('#dialog_txt_VMIProcess_ToDoASNDetail_UnitNetWeight').prop('disabled', true);
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_UnitNetWeight').val(data.NETWT);
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_VendorMaterialNo').val(data.VENDORMATERIAL);

                        if (data.lCUSTOMSSEAL !== undefined && data.lCUSTOMSSEAL != null) {
                            $('#dialog_txt_VMIProcess_ToDoASNDetail_CustomesSeal').prop('disabled', false);
                            if (data.lCUSTOMSSEAL.length > 0) {
                                if (data.lCUSTOMSSEAL.length == 1) {
                                    $('#dialog_txt_VMIProcess_ToDoASNDetail_CustomesSeal').show();
                                    $('#dialog_dropbox_VMIProcess_ToDoASNDetail_CustomesSeal').hide();
                                    $('#dialog_txt_VMIProcess_ToDoASNDetail_CustomesSeal').val(data.lCUSTOMSSEAL[0]);
                                    $('#dialog_txt_VMIProcess_ToDoASNDetail_CustomesSeal').prop('disabled', true);
                                }
                                else {
                                    $('#dialog_txt_VMIProcess_ToDoASNDetail_CustomesSeal').hide();
                                    $('#dialog_dropbox_VMIProcess_ToDoASNDetail_CustomesSeal').show();
                                    var objSelectForCustomsSeal = $('#dialog_dropbox_VMIProcess_ToDoASNDetail_CustomesSeal');
                                    objSelectForCustomsSeal.find('option').remove().end();
                                    for (var iCnt = 0; iCnt < data.lCUSTOMSSEAL.length; iCnt++)
                                        objSelectForCustomsSeal.append($("<option></option>").attr("value", data.lCUSTOMSSEAL[iCnt]).text(data.lCUSTOMSSEAL[iCnt]));

                                    if (data.CUSTOMSSEAL != "")
                                        $('#dialog_dropbox_VMIProcess_ToDoASNDetail_CustomesSeal').val(data.CUSTOMSSEAL);
                                }
                            }
                        }
                        else {
                            $('#dialog_txt_VMIProcess_ToDoASNDetail_CustomesSeal').show();
                            $('#dialog_dropbox_VMIProcess_ToDoASNDetail_CustomesSeal').hide();
                            $('#dialog_txt_VMIProcess_ToDoASNDetail_CustomesSeal').val(data.CUSTOMSSEAL);
                        }
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_TPacking').val(data.TOTSET);
                        $('#dialog_span_VMIProcess_ToDoASNDetail_TNetWeight').html(data.TOTNW);
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_TGrossWeight').val(data.TOTGW);
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_PackingType').val(data.PACKMATH);
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_OriginCountryCode').val(data.ORGCNTYCODE);
                        $('#dialog_span_VMIProcess_ToDoASNDetail_OriginCountry').html(data.ORGCNTY);
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_HAWB').val(data.HAWB);
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_MAWB').val(data.MAWB);
                        $('#dpToDoASN_MFGDate').val(data.MFGDATE);
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_Remark').val(data.PRODLINE);
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_Batch').val(data.BATCH);
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_InvoiceUnitPrice').val(data.INVOICEUNITPRICE);
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_VendorMaterialNumber').val(data.VENDORMATERIALNUMBER);
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_VendorCustomSerialNo').val(data.VENDORCUSTOMSERIALNO);
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_VendorHSCode').val(data.VENDORHSCODE);
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_VendorCustomPartName').val(data.VENDORCUSTOMPARTNAME);
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_VendorUOM').val(data.VENDORUOM);
                        diaToDoASNManage.attr('UOMRATE', data.UOMRATE);

                        if (!__DialogIsShownNow) {
                            __DialogIsShownNow = true;
                            diaToDoASNDetail.show();
                            diaToDoASNDetail.dialog("open");
                        }
                    }
                    else {
                        if (data.Message != "") {
                            alert(data.Message);
                        }
                        else {
                            alert("The function is not avaliable to use.");
                        }
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
    }
}

function GetCreateDetailInfo() {
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');

    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/QueryToDoASNDetailInfoForCreate',
        data: {
            ASN_NUM: escape($.trim($('#dialog_span_VMIProcess_ToDoASNDetail_ASNNo').html())),
            PO_NUM: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_POSANO').val())),
            PO_LINE: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_POLine').val())),
            PLANT: escape($.trim(diaToDoASNManage.attr('PLANT'))),
            VENDOR: escape($.trim(diaToDoASNManage.attr('VENDOR'))),
            ETA: escape($.trim(diaToDoASNManage.attr('ETA')))
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data != "") {
                if (data.RT == 1) {
                    alert(data.Message);
                }
                else {
                    $('#dialog_span_VMIProcess_ToDoASNDetail_LogHandingGroup').html(data.KEEPER);
                    $('#dialog_span_VMIProcess_ToDoASNDetail_MaterialNo').html(data.MATERIAL);
                    $('#dialog_span_VMIProcess_ToDoASNDetail_CustomDesc').html(data.CUSTOMDESCRIPTION);
                    $('#dialog_span_VMIProcess_ToDoASNDetail_Description').html(data.DESCRIPTION);
                    $('#dialog_span_VMIProcess_ToDoASNDetail_CustomGroupNo').html(data.PRODUCTNO);
                    $('#dialog_span_VMIProcess_ToDoASNDetail_UOM').html(data.UOM);
                    $('#dialog_txt_VMIProcess_ToDoASNDetail_UnitNetWeight').val(data.NETWT);
                    diaToDoASNManage.attr('UOMRATE', data.UOMRATE);
                    if (data.NETWT != 0) {
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_UnitNetWeight').prop('disabled', true);
                    }
                    else {
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_UnitNetWeight').prop('disabled', false);
                    }

                    if (data.lCUSTOMSSEAL !== undefined && data.lCUSTOMSSEAL != null) {
                        $('#dialog_txt_VMIProcess_ToDoASNDetail_CustomesSeal').prop('disabled', false);
                        if (data.lCUSTOMSSEAL.length > 0) {
                            if (data.lCUSTOMSSEAL.length == 1) {
                                $('#dialog_txt_VMIProcess_ToDoASNDetail_CustomesSeal').val(data.lCUSTOMSSEAL[0]);
                                $('#dialog_txt_VMIProcess_ToDoASNDetail_CustomesSeal').prop('disabled', true);
                            }
                            else {
                                $('#dialog_txt_VMIProcess_ToDoASNDetail_CustomesSeal').hide();
                                $('#dialog_dropbox_VMIProcess_ToDoASNDetail_CustomesSeal').show();
                                var objSelectForCustomsSeal = $('#dialog_dropbox_VMIProcess_ToDoASNDetail_CustomesSeal');
                                objSelectForCustomsSeal.find('option').remove().end();
                                for (var iCnt = 0; iCnt < data.lCUSTOMSSEAL.length; iCnt++)
                                    objSelectForCustomsSeal.append($("<option></option>").attr("value", data.lCUSTOMSSEAL[iCnt]).text(data.lCUSTOMSSEAL[iCnt]));
                            }
                        }
                    }
                }
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

function GetCreateUOM() {
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');

    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/QueryToDoASNDetailInfoForCreate',
        data: {
            ASN_NUM: escape($.trim($('#dialog_span_VMIProcess_ToDoASNDetail_ASNNo').html())),
            PO_NUM: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_POSANO').val())),
            PO_LINE: escape($.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_POLine').val())),
            PLANT: escape($.trim(diaToDoASNManage.attr('PLANT'))),
            VENDOR: escape($.trim(diaToDoASNManage.attr('VENDOR'))),
            ETA: escape($.trim(diaToDoASNManage.attr('ETA')))
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data != "") {
                if (data.RT == 1) {
                    alert(data.Message);
                }
                else {
                    diaToDoASNManage.attr('UOMRATE', data.UOMRATE);
                }
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

function ComputeTotalNetWeight() {
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');
    var TotalNetWeight = 0;
    var ShowTotalNetWeight = 0;

    var ASNQty = $.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_ASNQty').val());
    var UnitNetWeight = $.trim($('#dialog_txt_VMIProcess_ToDoASNDetail_UnitNetWeight').val());
    var UOMRate = diaToDoASNManage.attr('UOMRATE') === undefined ? 0 : diaToDoASNManage.attr('UOMRATE');

    //TotalNetWeight = ASNQty * formatFloat(UnitNetWeight, 2) * UOMRate / 1000;
    TotalNetWeight = ASNQty * UnitNetWeight.toString().replace(/[,]+/g, "") * UOMRate / 1000;

    ShowTotalNetWeight = formatFloat(TotalNetWeight, 2);

    if (TotalNetWeight < 0.01 && TotalNetWeight > 0) {
        $('#dialog_span_VMIProcess_ToDoASNDetail_TNetWeight').html("0.01");
    }
    else {
        $('#dialog_span_VMIProcess_ToDoASNDetail_TNetWeight').html(ShowTotalNetWeight);
    }

    if ($('#dialog_span_VMIProcess_ToDoASNManage_TradeType').html() != 'I:Import') {
        $('#dialog_txt_VMIProcess_ToDoASNDetail_TGrossWeight').val($('#dialog_span_VMIProcess_ToDoASNDetail_TNetWeight').html());
    }

    if (diaToDoASNManage.prop('CIMPORTEDASN') == 'True' && $('#dialog_span_VMIProcess_ToDoASNManage_TradeType').html() == 'I:Import') {
        $('#dialog_txt_VMIProcess_ToDoASNDetail_TGrossWeight').val('');
    }


    //if (TotalNetWeight == 0) {
    //    $('#dialog_span_VMIProcess_ToDoASNDetail_TNetWeight').html("0.00");
    //    $('#dialog_txt_VMIProcess_ToDoASNDetail_TGrossWeight').val("0.00");
    //}
    //else if (TotalNetWeight > 0 && TotalNetWeight < 0.01) {
    //    $('#dialog_span_VMIProcess_ToDoASNDetail_TNetWeight').html("0.01");
    //    $('#dialog_txt_VMIProcess_ToDoASNDetail_TGrossWeight').val("0.01");
    //}
    //else {
    //    $('#dialog_span_VMIProcess_ToDoASNDetail_TNetWeight').html(ShowTotalNetWeight);
    //    //    $('#dialog_txt_VMIProcess_ToDoASNDetail_TGrossWeight').val(ShowTotalNetWeight);
    //}

    //if (TotalNetWeight == 0 && ASNQty == 0) {
    //    $('#dialog_span_VMIProcess_ToDoASNDetail_TNetWeight').html("0.00");
    //    $('#dialog_txt_VMIProcess_ToDoASNDetail_TGrossWeight').val("0.00");
    //}
    //else if (TotalNetWeight == 0 && ASNQty > 0) {
    //    $('#dialog_span_VMIProcess_ToDoASNDetail_TNetWeight').html("0.01");
    //    $('#dialog_txt_VMIProcess_ToDoASNDetail_TGrossWeight').val("0.01");
    //}
    //else if (TotalNetWeight > 0 && TotalNetWeight < 0.01) {
    //    $('#dialog_span_VMIProcess_ToDoASNDetail_TNetWeight').html("0.01");
    //    $('#dialog_txt_VMIProcess_ToDoASNDetail_TGrossWeight').val("0.01");
    //}
    //else {
    //    $('#dialog_span_VMIProcess_ToDoASNDetail_TNetWeight').html(ShowTotalNetWeight);
    //    //    $('#dialog_txt_VMIProcess_ToDoASNDetail_TGrossWeight').val(ShowTotalNetWeight);
    //}
}

function formatFloat(num, pos) {
    var size = Math.pow(10, pos);
    num = num.toString().replace(/[,]+/g, "");
    return Math.round(num * size) / size;
}
