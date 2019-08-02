var jqGridMinusWidth = 84;
var jqGridMinusHeight = 360;

$(function () {

    var diaASNSpecialCancel = $('#dialog_VMIProcess_ASNSpecialCancelManage');
    var VMI_ASNSpecialCancelManagegridDataList = $('#VMI_Process_ASNSpecialCancelManage_gridDataList');

    //Init Function Button
    //Button
    $('#dia_btn_VMIProcess_ASNSpecialCancelManage_Cancel').button({
        label: "Cancel",
        icons: { primary: 'ui-icon-cancel' }
    });

    $('#dia_btn_VMIProcess_ASNSpecialCancelManage_CancelAll').button({
        label: "Cancel ALL",
        icons: { primary: 'ui-icon-arrowreturnthick-1-s' }
    });

    //After Init. to Show Menu Function Button
    $('#dialog_VMIProcess_ASNSpecialCancelManage_tbTopToolBar').show();

    $(window).resize(function () {
        diaASNSpecialCancel.dialog('option', 'width', $(window).width() - 50);
        diaASNSpecialCancel.dialog('option', 'height', $(window).height() - 20);
        
        VMI_ASNSpecialCancelManagegridDataList.jqGrid('setGridWidth', ($(window).width() < 100) ? 100 : $(window).width() - jqGridMinusWidth);
        VMI_ASNSpecialCancelManagegridDataList.jqGrid('setGridHeight', ($(window).height() < 100) ? 100 : $(window).height() - jqGridMinusHeight);
    });

    //Init dialog
    diaASNSpecialCancel.dialog({
        autoOpen: false,
        resizable: false,
        modal: true,
        open: function (event, ui) {
            $this = $(this);
            /* adjusting the dialog size for current browser */
            $this.dialog('option', 'width', ($(window).width() < 100) ? 100 : $(window).width() - 50);
            $this.dialog('option', 'height', ($(window).height() < 100) ? 100 : $(window).height() - 20);

            VMI_ASNSpecialCancelManagegridDataList.jqGrid('setGridWidth', ($(window).width() < 100) ? 100 : $(window).width() - jqGridMinusWidth);
            VMI_ASNSpecialCancelManagegridDataList.jqGrid('setGridHeight', ($(window).height() < 100) ? 100 : $(window).height() - jqGridMinusHeight);
        },
        buttons: {
            Close: function () {
                $(this).dialog("close");
            }
        },
        close: function () {
            ReloadASNSpecialCancelgridDataList();
            __DialogIsShownNow = false;
        }
    });

    //Init JQgrid
    VMI_ASNSpecialCancelManagegridDataList.jqGrid({
        url: __WebAppPathPrefix + "/VMIProcess/QueryASNSpecialCancelDetailWithFilter",
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
                    "Log.Handing Grp.",
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
                    "Custom Seal",
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
                    "BATCH"],
        colModel: [
                    { name: "ASNLINE", index: "ASNLINE", width: 40, sortable: false, sorttype: "text" },
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
                    { name: "BATCH", index: "BATCH", width: 100, sortable: false, sorttype: "text" }

        ],
        multiselect: true,
        shrinkToFit: false,
        scrollrows: true,
        //width: 1150,
        //height: 232,
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: "ASNLINE",
        viewrecords: true,
        loadonce: true,
        rowattr: function (rd) {
            if (rd.STATUS == "R") {
                rd.STATUS = "Release";
            }
        },
        pager: '#VMI_Process_ASNSpecialCancelManage_gridListPager',
        loadComplete: function () {
            var $this = $(this);

            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '') {
                    setTimeout(function () {
                        $this.triggerHandler('reloadGrid');
                    }, 50);
                }
            var rowCount = $this.jqGrid('getGridParam', 'records');

            if (rowCount > 0) {
                $("#dia_btn_VMIProcess_ASNSpecialCancelManage_Cancel").attr("disabled", false);
                $("#dia_btn_VMIProcess_ASNSpecialCancelManage_CancelAll").attr("disabled", false);
            }
            else {
                $("#dia_btn_VMIProcess_ASNSpecialCancelManage_Cancel").attr("disabled", true);
                $("#dia_btn_VMIProcess_ASNSpecialCancelManage_CancelAll").attr("disabled", true);
            }
        }
    });

    VMI_ASNSpecialCancelManagegridDataList.jqGrid('navGrid', '#VMI_Process_ASNSpecialCancelManage_gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });
});

function InitdialogASNSpecialCancelForManage() {
    var diaASNSpecialCancel = $('#dialog_VMIProcess_ASNSpecialCancelManage');

    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/QueryASNSpecialCancelInfoForManage',
        data: {
            ASN_NUM: escape($.trim(diaASNSpecialCancel.prop("ASN_NUM")))
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.HASRESULT == true) {
                $('#dialog_span_VMIProcess_ASNSpecialCancelManage_ASNNo').html(data.ASNNO);
                $('#dialog_span_VMIProcess_ASNSpecialCancelManage_ASNCreatedDate').html(data.ASNCREATEDATE);
                $('#dialog_span_VMIProcess_ASNSpecialCancelManage_InboundDNNo').html(data.INBOUNDDNNO);
                $('#dialog_span_VMIProcess_ASNSpecialCancelManage_Vendor').html(data.VENDOR + " [" + data.DEFAULTSHIPFROM + "]");
                $('#dialog_span_VMIProcess_ASNSpecialCancelManage_CustomerCode').html(data.CUSTOMERCODE);
                $('#dialog_span_VMIProcess_ASNSpecialCancelManage_VendorDNNo').html(data.VENDORDNNO);
                $('#dialog_span_VMIProcess_ASNSpecialCancelManage_CustomerName').html(data.CUSTOMERNAME);
                $('#dialog_span_VMIProcess_ASNSpecialCancelManage_InvoiceNo').html(data.INVOICENO);
                $('#dialog_span_VMIProcess_ASNSpecialCancelManage_PlantCode').html(data.PLANTCODE);
                $('#dialog_span_VMIProcess_ASNSpecialCancelManage_TradeType').html(data.TRADETYPE);
                $('#dialog_span_VMIProcess_ASNSpecialCancelManage_ETD').html(data.ETD);
                $('#dialog_span_VMIProcess_ASNSpecialCancelManage_ETA').html(data.ETA);
                $('#dialog_span_VMIProcess_ASNSpecialCancelManage_TransferDocNo').html(data.TRANSFERDOCNO);
                $('#dialog_span_VMIProcess_ASNSpecialCancelManage_VehicleTypeAndID').html(data.VEHICLETYPEID);
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
