$(function () {

    var VMI_ASNgridDataList = $('#VMI_ASNVendor_gridDataList');
    var VMI_ASNVendorProfile = $('#dialog_VMIConfig_ASNVendorProfile');

    //Init Function Button
    //Button
    $('#btn_VMIConfig_ASN_Corp_VDN_Info_Query').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    //After Init. to Show Menu Function Button
    $('#tbTopToolBar').show();


    //Init. ASNVendorProfile UI by with SBUVendorCode or not
    var sSBUVendorCode = CheckIsVMIAdmin();
    InitASNVendorUIByRole(sSBUVendorCode);

    $("#txt_VMIConfig_ASN_Corp_VDN").bind('keyup', function (e) {
        if (e.which >= 97 && e.which <= 122) {
            var newKey = e.which - 32;
            // I have tried setting those
            e.keyCode = newKey;
            e.charCode = newKey;
        }

        $("#txt_VMIConfig_ASN_Corp_VDN").val(($("#txt_VMIConfig_ASN_Corp_VDN").val()).toUpperCase());
    });

    //Init jqGrid
    var lang = "en-US";
    var langShort = lang.split('-')[0].toLowerCase();

    if ($.jgrid.hasOwnProperty("regional") && $.jgrid.regional.hasOwnProperty(lang)) {
        $.extend($.jgrid, $.jgrid.regional[lang]);
    } else if ($.jgrid.hasOwnProperty("regional") && $.jgrid.regional.hasOwnProperty(langShort)) {
        $.extend($.jgrid, $.jgrid.regional[langShort]);
    }

    VMI_ASNgridDataList.jqGrid({
        url: __WebAppPathPrefix + "/VMIConfigration/QueryASNVendorListJsonWithFilter",
        postData: { CorpVendorCode: "", SBUVendorCode: "" },
        mtype: "POST",
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        colNames: ["SBU Vendor Code",
                    "SBU Vendor Name",
                    "Corp.Vendor",
                    "Corp Vendor Code"],
        colModel: [
                    {
                        name: "ERP_VND", index: "ERP_VND", width: 80, sortable: true, sorttype: "text", classes: "jqGridColumnDataAsLinkWithBlue",
                        formatter: function (cellvalue, option, rowobject) {
                            return cellvalue;
                        }
                    },
                    { name: "ERP_VNAME", index: "ERP_VNAME", width: 220, sortable: true, sorttype: "text" },
                    { name: "CORP_VND_FullInfo", index: "CORP_VND_FullInfo", width: 240, sortable: true, sorttype: "text" },
                    { name: "CORP_VND", index: "CORP_VND", width: 20, sortable: true, sorttype: "text", hidden:true }
        ],
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            var $this = $(this);
            var ERP_VND = $this.jqGrid('getCell', rowid, 'ERP_VND');
            var CORP_VND = $this.jqGrid('getCell', rowid, 'CORP_VND');
            if (ERP_VND != "") {
                if (iCol == 0) {
                    if (!__DialogIsShownNow) {
                        __DialogIsShownNow = true;

                        //VMI_ASNVendorProfile.prop("CORP_VND", $.trim($('#txt_VMIConfig_ASN_Corp_VDN').val()) == "" ? $.trim($('#span_VMIConfig_ASN_Corp_VDN').html()) : $.trim($('#txt_VMIConfig_ASN_Corp_VDN').val()));
                        VMI_ASNVendorProfile.prop("CORP_VND", $.trim(CORP_VND));
                        VMI_ASNVendorProfile.prop("ERP_VND", $.trim(ERP_VND));

                        InitdialogASNVendorProfile();
                        LoadDiaASNVendorProfileHeaderInfo();
                        ReloadDiaCustomerCodeForAllPlantgridDataList();
                        ReloadDiaShipFromgridDataList();
                        ReloadDiaForwardergridDataList();

                        setTimeout(function () {
                            VMI_ASNVendorProfile.show();
                            VMI_ASNVendorProfile.dialog("open");
                        }, 250);
                    }
                }
            }
        },
        width: 950,
        height: 465,
        rowNum: 20,
        rowList: [10, 20, 30],
        sortname: "ERP_VND",
        viewrecords: true,
        loadonce: true,
        pager: '#VMI_ASNVendor_gridListPager',
        loadComplete: function () {
            var $this = $(this);

            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '') {
                    //setTimeout(function () {
                    //    $this.triggerHandler('reloadGrid');
                    //}, 50);
                }
        }
    });

    VMI_ASNgridDataList.jqGrid('navGrid', '#VMI_ASNVendor_gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

});

//自動依瀏覽器大小調整Grid大小
function __BrowserResize() {
    var RefWidth = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
    var RefHeight = document.documentElement.clientHeight;

    var NewWidth = RefWidth - 11;
    var NewHeight = RefHeight - 390;
    if (NewWidth < 100) NewWidth = 100;
    if (NewHeight < 100) NewHeight = 100;

    var gridDataList = $("#gridDataList");
    gridDataList.jqGrid('setGridWidth', NewWidth);
    gridDataList.jqGrid('setGridHeight', NewHeight);
}

//Init ASNVendorProfile UI
function InitASNVendorUIByRole(CorpVendor) {
    if (CorpVendor != "") {
        $("#txt_VMIConfig_ASN_Corp_VDN").hide();
        $("#btn_VMIConfig_ASN_Corp_VDN_Query").hide();
        $("#span_VMIConfig_ASN_Corp_VDN").html(CorpVendor);
        $("#span_VMIConfig_ASN_Corp_VDN").show();
    }
    else {
        $("#txt_VMIConfig_ASN_Corp_VDN").show();
        $("#btn_VMIConfig_ASN_Corp_VDN_Query").show();
        $("#span_VMIConfig_ASN_Corp_VDN").html("");
        $("#span_VMIConfig_ASN_Corp_VDN").hide();
    }
}

//Init dialogASNVendorProfile UI
function InitdialogASNVendorProfile() {
    $('#dialog_span_VMIConfig_ASNVendorProfile_SBUVendorCode').html("");
    $('#dialog_span_VMIConfig_ASNVendorProfile_SBUVendorName').html("");
    $('#dialog_span_VMIConfig_ASNVendorProfile_CorpVendorCode').html("");
    $('#dialog_span_VMIConfig_ASNVendorProfile_CorpVendorName').html("");
    $('#dialog_txt_VMIConfig_ASNVendorProfile_VendorOfficialName').val("");
    $("#dialog_dropbox_VMIConfig_ASNVendorProfile_TradeType").val("A");
    $('#dialog_txt_VMIConfig_ASNVendorProfile_Fax').val("");
    $('#dialog_txt_VMIConfig_ASNVendorProfile_Address').val("");
    $('#dialog_txt_VMIConfig_ASNVendorProfile_Phone').val("");
    $("#dialog_dropbox_VMIConfig_ASNVendorProfile_DeclarationType").val("0");
}
