$(function () {
    var ASNVendorProfile = $('#VMI_ASNVendor_gridDataList');
    var txtASNCorpVDN = $('#dialog_VMI_txt_Corp_VDN');
    var txtASNSBUVDN = $('#dialog_VMI_txt_SBU_VDN');
    var diaCorpVendor = $('#dialog_VMI_QueryCorpVendor');
    var diaSBUVendor = $('#dialog_VMI_QuerySBUVendor');


    $('#btn_VMIConfig_ASN_Corp_VDN_Info_Query').click(function () {
        $(this).removeClass('ui-state-focus');

        ASNVendorProfile.jqGrid('clearGridData');
        ASNVendorProfile.jqGrid('setGridParam', {
            postData: {
                CorpVendorCode: escape($.trim($('#txt_VMIConfig_ASN_Corp_VDN').val()) == "" ? $.trim($('#span_VMIConfig_ASN_Corp_VDN').html()) : $.trim($('#txt_VMIConfig_ASN_Corp_VDN').val())),
                SBUVendorCode: escape($.trim($('#txt_VMIConfig_ASN_SBU_VDN').val()))
            }
        });

        ASNVendorProfile.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
        
    });

    $('#btn_VMIConfig_ASN_Corp_VDN_Query').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txt_VMIConfig_ASN_Corp_VDN';

            InitdialogQueryCorpVendor();
            ReloadDiaCorpVendorCodegridDataList();

            diaCorpVendor.show();
            diaCorpVendor.dialog("open");
            // div with class ui-dialog
            $('.ui-dialog :button').blur();
        }
    });

    $('#btn_VMIConfig_ASN_SBU_VDN_Query').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txt_VMIConfig_ASN_SBU_VDN';

            InitdialogSBUVendor();
            ReloadDiaSBUVendorCodegridDataList();

            diaSBUVendor.show();
            diaSBUVendor.dialog("open");
            // div with class ui-dialog
            $('.ui-dialog :button').blur();
        }
    });


    //$('#dialog_btn_diaASNCorpVendor_Search').click(function () {
    //    $(this).removeClass('ui-state-focus');
    //    ReloadDiaCorpVendorCodegridDataList();
    //});

    //$('#dialog_btn_diaASNSBUVendor_Search').click(function () {
    //    $(this).removeClass('ui-state-focus');
    //    ReloadDiaSBUVendorCodegridDataList();
    //});

});

function LoadDiaASNVendorProfileHeaderInfo() {
    var diaASNVendorProfile = $('#dialog_VMIConfig_ASNVendorProfile');

    //Header Info
    $.ajax({
        url: __WebAppPathPrefix + '/VMIConfigration/QueryASNVendorProfileJsonWithFilter',
        data: {
            CorpVendorCode: escape($.trim(diaASNVendorProfile.prop("CORP_VND"))),
            SBUVendorCode: escape($.trim(diaASNVendorProfile.prop("ERP_VND")))
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            $('#dialog_span_VMIConfig_ASNVendorProfile_SBUVendorCode').html(data.ERP_VND);
            $('#dialog_span_VMIConfig_ASNVendorProfile_SBUVendorName').html(data.ERP_VNAME);
            $('#dialog_span_VMIConfig_ASNVendorProfile_CorpVendorCode').html(data.CORP_VND);
            $('#dialog_span_VMIConfig_ASNVendorProfile_CorpVendorName').html(data.CORP_VNAME);
            $('#dialog_txt_VMIConfig_ASNVendorProfile_VendorOfficialName').val(data.OFFICIAL_NAME);
            $('#dialog_dropbox_VMIConfig_ASNVendorProfile_TradeType').val(data.CONTRACT_TYPE);
            $('#dialog_txt_VMIConfig_ASNVendorProfile_Fax').val(data.FAX);
            $('#dialog_txt_VMIConfig_ASNVendorProfile_Address').val(data.ADDRESS);
            $('#dialog_txt_VMIConfig_ASNVendorProfile_Phone').val(data.TELPHONE);
            $('#dialog_dropbox_VMIConfig_ASNVendorProfile_DeclarationType').val(data.DECLARATION_TYPE);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
            //HideAjaxLoading();
        }
    });
}

function ReloadDiaCustomerCodeForAllPlantgridDataList() {
    var diaASNVendorProfile = $('#dialog_VMIConfig_ASNVendorProfile');
    var diaCustomerCodegridDataList = $('#dialog_VMI_CustomerCodeForAllPlant_gridDataList');

    diaCustomerCodegridDataList.jqGrid('clearGridData');
    diaCustomerCodegridDataList.jqGrid('setGridParam', { postData: { SBUVendorCode: escape($.trim(diaASNVendorProfile.prop("ERP_VND"))) } });
    diaCustomerCodegridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}

function ReloadDiaShipFromgridDataList() {
    var diaASNVendorProfile = $('#dialog_VMIConfig_ASNVendorProfile');
    var diaShipFromgridDataList = $('#dialog_VMI_ShipFrom_gridDataList');

    diaShipFromgridDataList.jqGrid('clearGridData');
    diaShipFromgridDataList.jqGrid('setGridParam', { postData: { SBUVendorCode: escape($.trim(diaASNVendorProfile.prop("ERP_VND"))) } });
    diaShipFromgridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}

function ReloadDiaForwardergridDataList() {
    var diaASNVendorProfile = $('#dialog_VMIConfig_ASNVendorProfile');
    var diaForwardergridDataList = $('#dialog_VMI_Forwarder_gridDataList');

    diaForwardergridDataList.jqGrid('clearGridData');
    diaForwardergridDataList.jqGrid('setGridParam', { postData: { SBUVendorCode: escape($.trim(diaASNVendorProfile.prop("ERP_VND"))) } });
    diaForwardergridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}