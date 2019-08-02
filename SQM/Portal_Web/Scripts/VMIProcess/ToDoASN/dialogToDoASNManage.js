var jqGridMinusWidth = 84;
var jqGridMinusHeight = 270;
var bButtonFunctionEnable = true;

$(function () {
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');
    var diaToDoASNDetail = $('#dialog_VMIProcess_ToDoASNDetail');
    var diaToDoASNDetailError = $('#dialog_VMIProcess_ToDoASNDetailError');
    var VMI_ToDoASNManagegridDataList = $('#VMI_Process_ToDoASNManage_gridDataList');

    //Init Function Button
    //Button
    $('#dia_btn_VMIProcess_ToDoASNManage_Head').button({
        label: "Head",
        icons: { primary: 'ui-icon-document' }
    });

    $('#dia_btn_VMIProcess_ToDoASNManage_AddDetail').button({
        label: "Add Detail",
        icons: { primary: 'ui-icon-pencil' }
    });

    $('#dia_btn_VMIProcess_ToDoASNManage_ImportDetail').button({
        label: "Import Detail",
        icons: { primary: 'ui-icon-document-b' }
    });

    $('#dia_btn_VMIProcess_ToDoASNManage_AttachFile').button({
        label: "Attach File",
        icons: { primary: 'ui-icon-arrowthickstop-1-n' }
    });

    $('#dia_btn_VMIProcess_ToDoASNManage_DeleteAll').button({
        label: "Delete All",
        icons: { primary: 'ui-icon-minusthick' }
    });

    $('#dia_btn_VMIProcess_ToDoASNManage_Delete').button({
        label: "Delete",
        icons: { primary: 'ui-icon-minus' }
    });

    $('#dia_btn_VMIProcess_ToDoASNManage_Release').button({
        label: "Release",
        icons: { primary: 'ui-icon-tag' }
    });

    $('#dia_btn_VMIProcess_ToDoASNManage_DownloadASN').button({
        label: "Download ASN",
        icons: { primary: 'ui-icon-arrowthickstop-1-s' }

    });

    $('#dia_btn_VMIProcess_ToDoASNManage_Print').button({
        label: "Print",
        icons: { primary: 'ui-icon-print' }
    });

    $('#dia_btn_VMIProcess_ToDoASNManage_SubmitToBuyerReview').button({
        label: "Submit to Buyer Review",
        icons: { primary: 'ui-icon-tag' }
    });

    $('#dia_btn_VMIProcess_ToDoASNManage_Reject').button({
        label: "Reject",
        icons: { primary: 'ui-icon-tag' }
    });

    //After Init. to Show Menu Function Button
    $('#dialog_VMIProcess_ToDoASNManage_tbTopToolBar').show();

    $(window).resize(function () {
        diaToDoASNManage.dialog('option', 'width', $(window).width() - 50);
        VMI_ToDoASNManagegridDataList.jqGrid('setGridWidth', ($(window).width() < 100) ? 100 : $(window).width() - jqGridMinusWidth);
    });

    //Init dialog
    diaToDoASNManage.dialog({
        autoOpen: false,
        resizable: false,
        height: 650,
        modal: true,
        open: function (event, ui) {
            if (bButtonFunctionEnable == true) {
                EnableFunctionButton();
            }
            $this = $(this);
            /* adjusting the dialog size for current browser */
            $this.dialog('option', 'width', ($(window).width() < 100) ? 100 : $(window).width() - 50);
            VMI_ToDoASNManagegridDataList.jqGrid('setGridWidth', ($(window).width() < 100) ? 100 : $(window).width() - jqGridMinusWidth);

            disableFunctionButtonforASNReviewer();
        },
        buttons: {
            Close: function () {
                $(this).dialog("close");
            }
        },
        close: function () {
            __DialogIsShownNow = false;
        }
    });

    //Init JQgrid
    VMI_ToDoASNManagegridDataList.jqGrid({
        url: __WebAppPathPrefix + "/VMIProcess/QueryToDoASNDetailInfoJsonWithFilter",
        //postData: { },
        mtype: "POST",
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        colNames: ["NO",
                    "Status",
                    "Log.Handling Grp.",
                    "Material No",
                    "Custom Desc.",
                    "Desciption",
                    "Product No",
                    "PO/SA No",
                    "PO Line",
                    "Qty",
                    "UOM",
                    "Packing Detail",
                    "Unit Net Wgt",
                    "Vendor Material No",
                    "Customs Seal",
                    "T.Packing",
                    "T.Net Wgt",
                    "T.Gross Wgt",
                    "Packing Type",
                    "Cnty.Code Of Origin",
                    "Cnty. Of Origin",
                    "HAWB",
                    "MAWB",
                    "MFG Date",
                    "Remark",
                    "BATCH",
                    "SLoc",
                    "Invoice Unit Price"],
        colModel: [
                    {
                        name: "ASNLINE", index: "ASNLINE", width: 40, sortable: false, sorttype: "text", classes: "jqGridColumnDataAsLinkWithBlue",
                        formatter: function (cellvalue, option, rowobject) {
                            return cellvalue;
                        }
                    },
                    { name: "STATUS", index: "STATUS", width: 50, sortable: false, sorttype: "text" },
                    { name: "KEEPER", index: "KEEPER", width: 120, sortable: false, sorttype: "text" },
                    { name: "MATERIAL", index: "MATERIAL", width: 120, sortable: false, sorttype: "text" },
                    { name: "CUSTOMDESCRIPTION", index: "CUSTOMDESCRIPTION", width: 160, sortable: false, sorttype: "text" },
                    { name: "DESCRIPTION", index: "DESCRIPTION", width: 160, sortable: false, sorttype: "text" },
                    { name: "PRODUCTNO", index: "PRODUCTNO", width: 80, sortable: false, sorttype: "text" },
                    { name: "PONUM", index: "PONUM", width: 80, sortable: false, sorttype: "text" },
                    { name: "POLINE", index: "POLINE", width: 70, sortable: false, sorttype: "text" },
                    { name: "ASNQTY", index: "ASNQTY", width: 60, align: 'right', sortable: false, sorttype: "text" },
                    { name: "UOM", index: "UOM", width: 40, sortable: false, sorttype: "text" },
                    { name: "PACKAGEDESCRIPTION", index: "PACKAGEDESCRIPTION", width: 90, sortable: false, sorttype: "text" },
                    { name: "NETWT", index: "NETWT", width: 90, align: 'right', sortable: false, sorttype: "text" },
                    { name: "VENDORMATERIAL", index: "VENDORMATERIAL", width: 120, sortable: false, sorttype: "text" },
                    { name: "CUSTOMSSEAL", index: "CUSTOMSSEAL", width: 100, sortable: false, sorttype: "text" },
                    { name: "TOTSET", index: "TOTSET", width: 80, align: 'right', sortable: false, sorttype: "text" },
                    { name: "TOTNW", index: "TOTNW", width: 80, align: 'right', sortable: false, sorttype: "text" },
                    { name: "TOTGW", index: "TOTGW", width: 80, align: 'right', sortable: false, sorttype: "text" },
                    { name: "PACKMATH", index: "PACKMATH", width: 120, sortable: false, sorttype: "text" },
                    { name: "ORGCNTYCODE", index: "ORGCNTYCODE", width: 120, sortable: false, sorttype: "text" },
                    { name: "ORGCNTY", index: "ORGCNTY", width: 100, sortable: false, sorttype: "text" },
                    { name: "HAWB", index: "HAWB", width: 80, sortable: false, sorttype: "text" },
                    { name: "MAWB", index: "MAWB", width: 80, sortable: false, sorttype: "text" },
                    { name: "MFGDATE", index: "MFGDATE", width: 100, sortable: false, sorttype: "text" },
                    { name: "PRODLINE", index: "PRODLINE", width: 100, sortable: false, sorttype: "text" },
                    { name: "BATCH", index: "BATCH", width: 100, sortable: false, sorttype: "text" },
                    { name: "LGORT", index: "LGORT", width: 70, sortable: false, sorttype: "text" },
                    { name: "INVOICEUNITPRICE", index: "INVOICEUNITPRICE", width: 70, align: 'right', sortable: false, sorttype: "text" }
        ],
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            var $this = $(this);
            var ASN_LINE = $this.jqGrid('getCell', rowid, 'ASNLINE');
            if (ASN_LINE != "") {
                if (iCol == 1) {
                    __DialogIsShownNow = false;
                    if (!__DialogIsShownNow) {
                        __DialogIsShownNow = true;
                        diaToDoASNDetail.attr("ASNNUM", $.trim(diaToDoASNManage.prop("ASN_NUM")));
                        diaToDoASNDetail.attr("ASNLINE", $.trim(ASN_LINE));
                        diaToDoASNDetail.attr("ROWID", $.trim(rowid));
                        InitdialogToDoASNDetail("U");
                    }
                }
                else if (iCol == 2) {
                    __DialogIsShownNow = false;
                    if ($(this).jqGrid("getCell", rowid, "STATUS").toLowerCase().indexOf("span") >= 0) {
                        if (!__DialogIsShownNow) {
                            __DialogIsShownNow = true;
                            diaToDoASNDetailError.attr("ASNNUM", $.trim(diaToDoASNManage.prop("ASN_NUM")));
                            diaToDoASNDetailError.attr("ASNLINE", $.trim(ASN_LINE));
                            InitdialogToDoASNDetailError();
                        }
                    }
                }
            }
        },
        cellEdit: true,
        cellsubmit: 'clientArray',
        editurl: 'clientArray',
        multiselect: true,
        shrinkToFit: false,
        scrollrows: true,
        //width: 1220,
        height: 252,
        rowNum: 10,
        //rowList: [10, 20, 30],
        //sortname: "ASNLINE",
        viewrecords: true,
        loadonce: true,
        pager: '#VMI_Process_ToDoASNManage_gridListPager',
        rowattr: function (rd) {
            if (rd.STATUS != "New" && rd.STATUS != "Error" && rd.STATUS != "<span class='jqGridColumnDataAsLinkWithBlue'>Error</span>" && rd.STATUS != 'Reviewing') {
                bButtonFunctionEnable = false;
            }
        },
        loadComplete: function () {
            var $this = $(this);


            if (bButtonFunctionEnable == true) {
                EnableFunctionButton();

                var rowCount = $this.jqGrid('getGridParam', 'records');
                if (rowCount > 0) {
                    bButtonFunctionEnable = true;
                    $("#dia_btn_VMIProcess_ToDoASNManage_Release").attr("disabled", false);
                }
                else {
                    bButtonFunctionEnable = false;
                    $("#dia_btn_VMIProcess_ToDoASNManage_Release").attr("disabled", true);
                }

            }
            else {
                DisableFunctionButton();
            }
        }
    });

    VMI_ToDoASNManagegridDataList.jqGrid('navGrid', '#VMI_Process_ToDoASNManage_gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });
});

function InitdialogToDoASNHeaderForManage() {
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');

    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/QueryToDoASNHeaderInfoForManage',
        data: {
            ASN_NUM: escape($.trim(diaToDoASNManage.prop("ASN_NUM")))
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.HASRESULT == true) {
                $('#dialog_span_VMIProcess_ToDoASNManage_ItemCategory').html(data.PSTYP);
                $('#dialog_span_VMIProcess_ToDoASNManage_ASNNo').html(data.ASNNO);
                $('#dialog_span_VMIProcess_ToDoASNManage_ASNCreatedDate').html(data.ASNCREATEDATE);
                $('#dialog_span_VMIProcess_ToDoASNManage_InboundDNNo').html(data.INBOUNDDNNO);
                $('#dialog_span_VMIProcess_ToDoASNManage_Vendor').html(data.VENDOR + " [" + data.DEFAULTSHIPFROM + "]");
                $('#dialog_span_VMIProcess_ToDoASNManage_CustomerCode').html(data.CUSTOMERCODE);
                $('#dialog_span_VMIProcess_ToDoASNManage_VendorDNNo').html(data.VENDORDNNO);
                $('#dialog_span_VMIProcess_ToDoASNManage_CustomerName').html(data.CUSTOMERNAME);
                $('#dialog_span_VMIProcess_ToDoASNManage_InvoiceNo').html(data.INVOICENO);
                $('#dialog_span_VMIProcess_ToDoASNManage_PlantCode').html(data.PLANTCODE);
                $('#dialog_span_VMIProcess_ToDoASNManage_TradeType').html(data.TRADETYPE);
                $('#dialog_span_VMIProcess_ToDoASNManage_ETD').html(data.ETD);
                $('#dialog_span_VMIProcess_ToDoASNManage_ETA').html(data.ETA);
                $('#dialog_span_VMIProcess_ToDoASNManage_TransferDocNo').html(data.TRANSFERDOCNO);
                $('#dialog_span_VMIProcess_ToDoASNManage_VehicleTypeAndID').html(data.VEHICLETYPEID);

                $('#dialog_span_VMIProcess_ToDoASNManage_Incoterms').html(data.INCOTERMS);
                $('#dialog_span_VMIProcess_ToDoASNManage_BuyerCode').html(data.BUYERCODE);
                $('#dialog_span_VMIProcess_ToDoASNManage_CompanyName').html(data.COMPANY_NAME);
                $('#dialog_span_VMIProcess_ToDoASNManage_TEL').html(data.TEL);
                $('#dialog_span_VMIProcess_ToDoASNManage_Name').html(data.NAME);
                $('#dialog_span_VMIProcess_ToDoASNManage_CustomsBroker').html(data.CB_NAME);

                if (data.EMAIL != '') {
                    var emails = data.EMAIL.split(';');
                    if (emails.length > 1) {
                        $('#dialog_span_VMIProcess_ToDoASNManage_Email').html(emails[0] + ' [Click me]');
                        $('#dialog_span_VMIProcess_ToDoASNManage_Email').attr({ 'style': 'cursor:pointer;color:blue;', 'title': 'click me to show all Email.' });

                        $("#dialog_span_VMIProcess_ToDoASNManage_Email").unbind("click");

                        $('#dialog_span_VMIProcess_ToDoASNManage_Email').click(function (e) {
                            e.preventDefault();
                            alert(data.EMAIL.replace(/\;/g, '\n'));
                        });
                    }
                    else {
                        $('#dialog_span_VMIProcess_ToDoASNManage_Email').html(emails[0]);
                        $('#dialog_span_VMIProcess_ToDoASNManage_Email').attr({ 'style': '', 'title': '' });
                        $('#dialog_span_VMIProcess_ToDoASNManage_Email').unbind('click');
                    }
                }
                else {
                    $('#dialog_span_VMIQuery_QueryASNManage_Email').html(data.EMAIL);
                }
                $('#dialog_span_VMIProcess_ToDoASNManage_Address').html(data.ADDRESS);
                $('#dialog_span_VMIProcess_ToDoASNManage_RejectReason').html(data.REJECT_REASON);
                $('#dialog_span_VMIProcess_ToDoASNManage_DriverName').html(data.DRIVERNAME);
                $('#dialog_span_VMIProcess_ToDoASNManage_DriverPhone').html(data.DRIVERPHONE);                

                if (data.CIMPORTEDASN == 'True' && data.TRADETYPE == 'I:Import') {
                    $('.tdImportedASN').show();
                    $($('.tdImportedASN').parent().parent().parent()[0]).attr('style', 'width: 950px');

                    if (diaToDoASNManage.prop('STATUS') == 'Reviewing') {
                        $('#dia_btn_VMIProcess_ToDoASNManage_Release').show();
                        $('#dia_btn_VMIProcess_ToDoASNManage_Reject').show();
                        $('#dia_btn_VMIProcess_ToDoASNManage_SubmitToBuyerReview').hide();
                    }
                    else {
                        $('#dia_btn_VMIProcess_ToDoASNManage_Release').hide();
                        $('#dia_btn_VMIProcess_ToDoASNManage_Reject').hide();
                        $('#dia_btn_VMIProcess_ToDoASNManage_SubmitToBuyerReview').show();
                    }
                }
                else {
                    $('.tdImportedASN').hide();
                    $($('.tdImportedASN').parent().parent().parent()[0]).attr('style', 'width: 700px');
                    $('#dia_btn_VMIProcess_ToDoASNManage_Release').show();
                    $('#dia_btn_VMIProcess_ToDoASNManage_Reject').hide();
                    $('#dia_btn_VMIProcess_ToDoASNManage_SubmitToBuyerReview').hide();
                }

                diaToDoASNManage.prop('CIMPORTEDASN', data.CIMPORTEDASN);
                diaToDoASNManage.attr('VENDOR', data.VENDOR);
                diaToDoASNManage.attr('PLANT', data.PLANTCODE);
                diaToDoASNManage.attr('ETA', data.ETA);
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

function disableFunctionButtonforASNReviewer() {
    if ($('#addReviewingFlag') != undefined && $('#addReviewingFlag').text() != '') {
        if ($('#addReviewingFlag').text() == 'VMI: VMIASNReviewer') {
            $('#dia_btn_VMIProcess_ToDoASNManage_Head').prop('disabled', true);
            $('#dia_btn_VMIProcess_ToDoASNManage_AddDetail').prop('disabled', true);
            $('#dia_btn_VMIProcess_ToDoASNManage_ImportDetail').prop('disabled', true);
            $('#dia_btn_VMIProcess_ToDoASNManage_AttachFile').prop('disabled', true);
            $('#dia_btn_VMIProcess_ToDoASNManage_DeleteAll').prop('disabled', true);
            $('#dia_btn_VMIProcess_ToDoASNManage_Delete').prop('disabled', true);
        }
    }
}