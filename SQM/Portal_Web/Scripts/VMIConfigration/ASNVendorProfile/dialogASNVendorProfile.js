$(function () {

    var diaASNVendorProfile = $('#dialog_VMIConfig_ASNVendorProfile');
    var diaCustomerCodegridDataList = $('#dialog_VMI_CustomerCodeForAllPlant_gridDataList');
    var diaShipFromgridDataList = $('#dialog_VMI_ShipFrom_gridDataList');
    var diaForwardergridDataList = $('#dialog_VMI_Forwarder_gridDataList');


    //Init dialog
    diaASNVendorProfile.dialog({
        autoOpen: false,
        height: 900,
        width: 750,
        resizable: false,
        modal: true,
        buttons: {
            Submit: function () {
                var DoSuccessfully = false;

                $.ajax({
                    url: __WebAppPathPrefix + '/VMIConfigration/UpdateASNVendorProfile',
                    data: {
                        ERP_VND: escape($.trim(diaASNVendorProfile.prop("ERP_VND"))),
                        CORP_VND: escape($.trim(diaASNVendorProfile.prop("CORP_VND"))),
                        OFFICIAL_NAME: escape($.trim($('#dialog_txt_VMIConfig_ASNVendorProfile_VendorOfficialName').val())),
                        CONTRACT_TYPE: escape($.trim($("#dialog_dropbox_VMIConfig_ASNVendorProfile_TradeType").val())),
                        FAX: escape($.trim($('#dialog_txt_VMIConfig_ASNVendorProfile_Fax').val())),
                        ADDRESS: escape($.trim($('#dialog_txt_VMIConfig_ASNVendorProfile_Address').val())),
                        TELPHONE: escape($.trim($('#dialog_txt_VMIConfig_ASNVendorProfile_Phone').val())),
                        DECLARATION_TYPE: escape($.trim($('#dialog_dropbox_VMIConfig_ASNVendorProfile_DeclarationType').val())),
                        DetailCC: escape(JSON.stringify(diaCustomerCodegridDataList.jqGrid('getGridParam', 'data'))),
                        DetailSF: escape(JSON.stringify(diaShipFromgridDataList.jqGrid('getGridParam', 'data'))),
                        DetailFI: escape(JSON.stringify(diaForwardergridDataList.jqGrid('getGridParam', 'data')))
                    },
                    type: "post",
                    dataType: 'text',
                    async: false,
                    success: function (data) {
                        if (data == "") {
                            DoSuccessfully = true;
                            alert("Update successfully.");
                        }
                        else {
                            alert(data);
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

    //Init Function Button
    //Button
    $('#dialog_VMI_CustomerCodeForAllPlant_ADD').button({
        label: "ADD",
        icons: { primary: 'ui-icon-plus' }
    });

    $('#dialog_VMI_CustomerCodeForAllPlant_Upadte').button({
        label: "Update",
        icons: { primary: 'ui-icon-pencil' }
    });

    $('#dialog_VMI_CustomerCodeForAllPlant_Delete').button({
        label: "Delete",
        icons: { primary: 'ui-icon-minus' }
    });

    //After Init. to Show Menu Function Button
    $('#dialog_VMI_CustomerCodeForAllPlant_tbTopToolBar').show();

    //Init CustomerCodeForAllPlant gridData
    diaCustomerCodegridDataList.jqGrid({
        url: __WebAppPathPrefix + "/VMIConfigration/QueryCustomerCodeAllPlantJsonWithFilter",
        postData: { SBUVendorCode: "" },
        mtype: "POST",
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        colNames: ["Plant",
                    "Customer Code",
                    "Customer Name"],
        colModel: [
                    { name: "PLANT", index: "PLANT", width: 80, sortable: false, sorttype: "text" },
                    { name: "COSTOM_CODE", index: "COSTOM_CODE", width: 100, sortable: false, sorttype: "text" },
                    { name: "COSTOM_NAME", index: "COSTOM_NAME", width: 200, sortable: false, sorttype: "text" }
        ],
        width: 545,
        height: 120,
        rowNum: 5,
        rowList: [5, 10, 15],
        sortname: "PLANT",
        viewrecords: true,
        loadonce: true,
        pager: '#dialog_VMI_CustomerCodeForAllPlant_gridDataPager',
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

    diaCustomerCodegridDataList.jqGrid('navGrid', '#dialog_VMI_CustomerCodeForAllPlant_gridDataPager', { edit: false, add: false, del: false, search: false, refresh: false });

    //Init Function Button
    //Button
    $('#dialog_VMI_ShipFrom_ADD').button({
        label: "ADD",
        icons: { primary: 'ui-icon-plus' }
    });

    $('#dialog_VMI_ShipFrom_Upadte').button({
        label: "Update",
        icons: { primary: 'ui-icon-pencil' }
    });

    $('#dialog_VMI_ShipFrom_Delete').button({
        label: "Delete",
        icons: { primary: 'ui-icon-minus' }
    });

    //After Init. to Show Menu Function Button
    $('#dialog_VMI_ShipFrom_tbTopToolBar').show();

    //Init ShipFrom gridData
    diaShipFromgridDataList.jqGrid({
        url: __WebAppPathPrefix + "/VMIConfigration/QueryShipFromJsonWithFilter",
        postData: { SBUVendorCode: "" },
        mtype: "POST",
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        colNames: ["Plant Short Name",
                    "Official Name",
                    "Address",
                    "Telephone",
                    "FAX"],
        colModel: [
                    { name: "PLANT_SHORT_NAME", index: "PLANT_SHORT_NAME", width: 120, sortable: false, sorttype: "text" },
                    { name: "PLANT_OFFICIAL_NAME", index: "PLANT_OFFICIAL_NAME", width: 100, sortable: false, sorttype: "text" },
                    { name: "PLANT_ADDRESS", index: "PLANT_ADDRESS", width: 100, sortable: false, sorttype: "text" },
                    { name: "PLANT_TELPHONE", index: "PLANT_TELPHONE", width: 100, sortable: false, sorttype: "text" },
                    { name: "PLANT_FAX", index: "PLANT_FAX", width: 100, sortable: false, sorttype: "text" }
        ],
        width: 545,
        height: 120,
        rowNum: 5,
        rowList: [5, 10, 15],
        sortname: "PLANT_SHORT_NAME",
        viewrecords: true,
        loadonce: true,
        pager: '#dialog_VMI_ShipFrom_gridDataPager',
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

    diaShipFromgridDataList.jqGrid('navGrid', '#dialog_VMI_ShipFrom_gridDataPager', { edit: false, add: false, del: false, search: false, refresh: false });

    //Init Function Button
    //Button
    $('#dialog_VMI_Forwarder_ADD').button({
        label: "ADD",
        icons: { primary: 'ui-icon-plus' }
    });

    $('#dialog_VMI_Forwarder_Upadte').button({
        label: "Update",
        icons: { primary: 'ui-icon-pencil' }
    });

    $('#dialog_VMI_Forwarder_Delete').button({
        label: "Delete",
        icons: { primary: 'ui-icon-minus' }
    });

    //After Init. to Show Menu Function Button
    $('#dialog_VMI_Forwarder_tbTopToolBar').show();

    //Init Forwarder gridData
    diaForwardergridDataList.jqGrid({
        url: __WebAppPathPrefix + "/VMIConfigration/QueryForwarderJsonWithFilter",
        postData: { SBUVendorCode: "" },
        mtype: "POST",
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        colNames: ["Company Name",
                    "Telephone",
                    "Contact Person",
                    "Email",
                    "Address"],
        colModel: [
                    { name: "COMPANY_NAME", index: "COMPANY_NAME", width: 120, sortable: false, sorttype: "text" },
                    { name: "TEL", index: "TEL", width: 100, sortable: false, sorttype: "text" },
                    { name: "NAME", index: "NAME", width: 100, sortable: false, sorttype: "text" },
                    { name: "EMAIL", index: "EMAIL", width: 100, sortable: false, sorttype: "text" },
                    { name: "ADDRESS", index: "ADDRESS", width: 100, sortable: false, sorttype: "text" }
        ],
        width: 545,
        height: 120,
        rowNum: 5,
        rowList: [5, 10, 15],
        sortname: "PLANT_SHORT_NAME",
        viewrecords: true,
        loadonce: true,
        pager: '#dialog_VMI_Forwarder_gridDataPager',
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

    diaForwardergridDataList.jqGrid('navGrid', '#dialog_VMI_Forwarder_gridDataPager', { edit: false, add: false, del: false, search: false, refresh: false });
});