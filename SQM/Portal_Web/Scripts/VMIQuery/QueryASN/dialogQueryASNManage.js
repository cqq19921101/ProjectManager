var jqGridMinusWidth = 84;
var jqGridMinusHeight = 270;

$(function () {
    var diaQueryASNManage = $('#dialog_VMIQuery_QueryASNManage');
    var diaQueryASNDetailError = $('#dialog_VMIQuery_QueryASNDetailError');
    var VMI_QueryASNManagegridDataList = $('#VMI_Query_QueryASNManage_gridDataList');

    //Init Function Button
    //Button
    $('#dia_btn_VMIQuery_QueryASNManage_EditShipInfo').button({
        label: "Edit Shipping Info",
        icons: { primary: 'ui-icon-pencil' }
    });

    $('#dia_btn_VMIQuery_QueryASNManage_FileExport').button({
        label: "Attach File",
        icons: { primary: 'ui-icon-document' }
    });

    $('#dia_btn_VMIQuery_QueryASNManage_ExcelExport').button({
        label: "Export",
        icons: { primary: 'ui-icon-arrowthickstop-1-s' }
    });

    $('#dia_btn_VMIQuery_QueryASNManage_EditImportDate').button({
        label: "Edit Import Date",
        icons: { primary: 'ui-icon-pencil' }
    });

    $('#dia_btn_VMIQuery_QueryASNManage_PrintImportedNoticeForm').button({
        label: "Print Imported Notice Form",
        icons: { primary: 'ui-icon-print' }
    });

    //After Init. to Show Menu Function Button
    $('#dialog_VMIQuery_QueryASNManage_tbTopToolBar').show();

    $(window).resize(function () {
        diaQueryASNManage.dialog('option', 'width', $(window).width() - 50);
        VMI_QueryASNManagegridDataList.jqGrid('setGridWidth', ($(window).width() < 100) ? 100 : $(window).width() - jqGridMinusWidth);
    });

    //Init dialog
    diaQueryASNManage.dialog({
        autoOpen: false,
        resizable: false,
        height: 670,
        modal: true,
        open: function (event, ui) {
            $this = $(this);
            /* adjusting the dialog size for current browser */
            $this.dialog('option', 'width', ($(window).width() < 100) ? 100 : $(window).width() - 50);
            VMI_QueryASNManagegridDataList.jqGrid('setGridWidth', ($(window).width() < 100) ? 100 : $(window).width() - jqGridMinusWidth);
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
    VMI_QueryASNManagegridDataList.jqGrid({
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
        colNames: ["-",
                    "NO",
                    "Status",
                    "Log.Handing Grp.",
                    "Material No",
                    "Custom Desc.",
                    "Desciption",
                    "Product No",
                    "PO/SA No",
                    "PO Line",
                    "Qty",
                    "GR Qty",
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
                    "Vendor Material Number(供應商料號)",
                    "Vendor Custom Serial Number(供應商項號)",
                    "Vendor HS Code(供應商HS代碼)",
                    "Vendor Custom Part Name(供應商品名)",
                    "Vendor UOM(供應商單位)",
                    "Inv. Unit Price"],
        colModel: [
                    { name: "STATUSICON", index: "STATUSICON", width: 20, sortable: false, sorttype: "text" },
                    { name: "ASNLINE", index: "ASNLINE", width: 40, sortable: false, sorttype: "text" },
                    { name: "STATUS", index: "STATUS", width: 50, sortable: false, sorttype: "text" },
                    { name: "KEEPER", index: "KEEPER", width: 120, sortable: false, sorttype: "text" },
                    { name: "MATERIAL", index: "MATERIAL", width: 120, sortable: false, sorttype: "text" },
                    { name: "CUSTOMDESCRIPTION", index: "CUSTOMDESCRIPTION", width: 160, sortable: false, sorttype: "text" },
                    { name: "DESCRIPTION", index: "DESCRIPTION", width: 180, sortable: false, sorttype: "text" },
                    { name: "PRODUCTNO", index: "PRODUCTNO", width: 80, sortable: false, sorttype: "text" },
                    { name: "PONUM", index: "PONUM", width: 80, sortable: false, sorttype: "text" },
                    { name: "POLINE", index: "POLINE", width: 70, sortable: false, sorttype: "text" },
                    { name: "ASNQTY", index: "ASNQTY", width: 60, align: 'right', sortable: false, sorttype: "text" },
                    { name: "GRQTY", index: "GRQTY", width: 60, align: 'right', sortable: false, sorttype: "text" },
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
                    { name: "VENDORMATERIALNUMBER", index: "VENDORMATERIALNUMBER", width: 260, sortable: false, sorttype: "text" },
                    { name: "VENDORCUSTOMSERIALNO", index: "VENDORCUSTOMSERIALNO", width: 260, sortable: false, sorttype: "text" },
                    { name: "VENDORHSCODE", index: "VENDORHSCODE", width: 260, sortable: false, sorttype: "text" },
                    { name: "VENDORCUSTOMPARTNAME", index: "VENDORCUSTOMPARTNAME", width: 260, sortable: false, sorttype: "text" },
                    { name: "VENDORUOM", index: "VENDORUOM", width: 260, sortable: false, sorttype: "text" },
                    { name: "INVOICEUNITPRICE", index: "INVOICEUNITPRICE", width: 100, sortable: false, sorttype: "text" }
        ],
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            var $this = $(this);
            var ASN_LINE = $this.jqGrid('getCell', rowid, 'ASNLINE');
            if (ASN_LINE != "") {
                if (iCol == 2) {
                    __DialogIsShownNow = false;
                    if ($(this).jqGrid("getCell", rowid, "STATUS").toLowerCase().indexOf("span") >= 0) {
                        if (!__DialogIsShownNow) {
                            __DialogIsShownNow = true;
                            diaQueryASNDetailError.attr("ASNNUM", $.trim(diaQueryASNManage.prop("ASN_NUM")));
                            diaQueryASNDetailError.attr("ASNLINE", $.trim(ASN_LINE));
                            InitdialogQueryASNDetailError();
                        }
                    }
                }
            }
        },
        shrinkToFit: false,
        scrollrows: true,
        //width: 1150,
        height: 252,
        rowNum: 10,
        //rowList: [10, 20, 30],
        //sortname: "ASNLINE",
        viewrecords: true,
        loadonce: true,
        rowattr: function (rd) {
            if (rd.STATUS == "R") {
                rd.STATUS = "Release";
            }
            else if (rd.STATUS == "C") {
                rd.STATUS = "Cancel";
                rd.STATUSICON = "<span class='ui-icon ui-icon-trash'></span>";
            }
            else if (rd.STATUS == "G") {
                rd.STATUS = "Close";
            }
            else if (rd.STATUS == "E") {
                rd.STATUS = "Error";
            }
            else if (rd.STATUS == "I") {
                rd.STATUS = "In-Process";
            }
            else if (rd.STATUS == "N") {
                rd.STATUS = "New";
            }
        },
        pager: '#VMI_Query_QueryASNManage_gridListPager',
        loadComplete: function () {
            var $this = $(this);

            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '') {
                    setTimeout(function () {
                        $this.triggerHandler('reloadGrid');
                    }, 50);
                }
        }
    });

    VMI_QueryASNManagegridDataList.jqGrid('navGrid', '#VMI_Query_QueryASNManage_gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });
});

function InitdialogQueryASNHeaderForManage() {
    var diaQueryASNManage = $('#dialog_VMIQuery_QueryASNManage');

    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/QueryToDoASNHeaderInfoForManage',
        data: {
            ASN_NUM: escape($.trim(diaQueryASNManage.prop("ASN_NUM")))
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.HASRESULT == true) {
                $('#dialog_span_VMIQuery_QueryASNManage_ItemCategory').html(data.PSTYP);
                $('#dialog_span_VMIQuery_QueryASNManage_ASNNo').html(data.ASNNO);
                $('#dialog_span_VMIQuery_QueryASNManage_ASNCreatedDate').html(data.ASNCREATEDATE);
                $('#dialog_span_VMIQuery_QueryASNManage_InboundDNNo').html(data.INBOUNDDNNO);
                $('#dialog_span_VMIQuery_QueryASNManage_Vendor').html(data.VENDOR + " [" + data.DEFAULTSHIPFROM + "]");
                $('#dialog_span_VMIQuery_QueryASNManage_CustomerCode').html(data.CUSTOMERCODE);
                $('#dialog_span_VMIQuery_QueryASNManage_VendorDNNo').html(data.VENDORDNNO);
                $('#dialog_span_VMIQuery_QueryASNManage_CustomerName').html(data.CUSTOMERNAME);
                $('#dialog_span_VMIQuery_QueryASNManage_InvoiceNo').html(data.INVOICENO);
                $('#dialog_span_VMIQuery_QueryASNManage_PlantCode').html(data.PLANTCODE);
                $('#dialog_span_VMIQuery_QueryASNManage_TradeType').html(data.TRADETYPE);
                $('#dialog_span_VMIQuery_QueryASNManage_ETD').html(data.ETD);
                $('#dialog_span_VMIQuery_QueryASNManage_ETA').html(data.ETA);
                $('#dialog_span_VMIQuery_QueryASNManage_TransferDocNo').html(data.TRANSFERDOCNO);
                $('#dialog_span_VMIQuery_QueryASNManage_VehicleTypeAndID').html(data.VEHICLETYPEID);
                $('#dialog_span_VMIQuery_QueryASNManage_DeclarationType').html(data.DECLARATION);

                $('#dialog_span_VMIQuery_QueryASNManage_Incoterms').html(data.INCOTERMS);
                $('#dialog_span_VMIQuery_QueryASNManage_BuyerCode').html(data.BUYERCODE);
                $('#dialog_span_VMIQuery_QueryASNManage_CompanyName').html(data.COMPANY_NAME);
                $('#dialog_span_VMIQuery_QueryASNManage_TEL').html(data.TEL);
                $('#dialog_span_VMIQuery_QueryASNManage_Name').html(data.NAME);
                $('#dialog_span_VMIQuery_QueryASNManage_CustomsBroker').html(data.CB_NAME);
                //$('#dialog_span_VMIQuery_QueryASNManage_Email').html(data.EMAIL);
                if (data.EMAIL != '') {
                    var emails = data.EMAIL.split(';');
                    if (emails.length > 1) {
                        $('#dialog_span_VMIQuery_QueryASNManage_Email').html(emails[0] + ' [Click me]');
                        $('#dialog_span_VMIQuery_QueryASNManage_Email').attr({ 'style': 'cursor:pointer;color:blue;', 'title': 'click me to show all Email.' });

                        $('#dialog_span_VMIQuery_QueryASNManage_Email').unbind('click');

                        $('#dialog_span_VMIQuery_QueryASNManage_Email').click(function (e) {
                            e.preventDefault();
                            alert(data.EMAIL.replace(/\;/g, '\n'));
                        });
                    }
                    else {
                        $('#dialog_span_VMIQuery_QueryASNManage_Email').html(emails[0]);
                        $('#dialog_span_VMIQuery_QueryASNManage_Email').attr({ 'style': '', 'title': '' });
                        $('#dialog_span_VMIQuery_QueryASNManage_Email').unbind('click');
                    }
                }
                else {
                    $('#dialog_span_VMIQuery_QueryASNManage_Email').html(data.EMAIL);
                }
                $('#dialog_span_VMIQuery_QueryASNManage_Address').html(data.ADDRESS);
                $('#dialog_span_VMIQuery_QueryASNManage_ArrivalDate').html(data.ARRIVAL_DATE);
                $('#dialog_span_VMIQuery_QueryASNManage_PlanImportDate').html(data.PLAN_IMPORT_DATE);
                $('#dialog_span_VMIQuery_QueryASNManage_DriverName').html(data.DRIVERNAME);
                $('#dialog_span_VMIQuery_QueryASNManage_DriverPhone').html(data.DRIVERPHONE);

                diaQueryASNManage.attr('VENDOR', data.VENDOR);
                diaQueryASNManage.attr('PLANT', data.PLANTCODE);
                diaQueryASNManage.attr('ETA', data.ETA);

                if (data.TRADETYPE == 'I:Import' && data.CIMPORTEDASN == 'True') {
                    $('.tdImportedASN').show();
                    $($('.tdImportedASN').parent().parent().parent()[0]).attr('style', 'width: 950px');

                    if (diaQueryASNManage.prop('STATUS') == 'Release') {
                        $('#dia_btn_VMIQuery_QueryASNManage_EditImportDate').show();
                        $('#dia_btn_VMIQuery_QueryASNManage_PrintImportedNoticeForm').show();
                    }
                    else {
                        $('#dia_btn_VMIQuery_QueryASNManage_EditImportDate').hide();
                        $('#dia_btn_VMIQuery_QueryASNManage_PrintImportedNoticeForm').hide();
                    }
                }
                else {
                    $('.tdImportedASN').hide();
                    $($('.tdImportedASN').parent().parent().parent()[0]).attr('style', 'width: 700px');

                    $('#dia_btn_VMIQuery_QueryASNManage_EditImportDate').hide();
                    $('#dia_btn_VMIQuery_QueryASNManage_PrintImportedNoticeForm').hide();
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
