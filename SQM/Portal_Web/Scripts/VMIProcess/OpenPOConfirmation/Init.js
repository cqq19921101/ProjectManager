$(function () {
    //Init Function Button
    $('#btnQueryOpenPOConfirmation').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnExportOpenPOConfirmation').button({
        label: "Export",
        icons: { primary: 'ui-icon-arrowthickstop-1-s' }
    });

    $('#btnOpenQueryPlantDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQueryVendorCodeDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    // Query character to upper
    $('input#txtPlant').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });

    $('input#txtVendorCode').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });

    $("#txtDeliveryDateFrom").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtDeliveryDateFrom").datepicker("setDate", '-31d');

    $("#txtDeliveryDateTo").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            }
            catch (err) {
                $(this).datepicker("setDate", '0d');
            }
        }
    });
    $("#txtDeliveryDateTo").datepicker("setDate", '0d');

    $("#txtConfirmedDateFrom").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '0d');
            }
        }
    });
    //$("#txtConfirmedDateFrom").datepicker("setDate", '-31d');

    $("#txtConfirmedDateTo").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            }
            catch (err) {
                $(this).datepicker("setDate", '0d');
            }
        }
    });
    //$("#txtConfirmedDateTo").datepicker("setDate", '0d');

    // Init jqGrid
    $('#gridOpenPOConfirmation').jqGrid({
        url: __WebAppPathPrefix + '/VMIProcess/QueryOpenPOConfirmationJqGrid',
        mtype: 'POST',
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false,
        },
        colNames: [
            'CREATE_DATE'
            ,'Confirm Status'
            , 'PO Number'
            , 'PO Line'
            , 'Delivery Date'
            , 'PO Qty'
            , 'Confirmed Date'
            , 'Confirmed Qty'
            , 'Plant'
            , 'Vendor'
            , 'Material'
            , 'Material Description'
            , 'Size Dimension'
            , 'UOM'
        ],
        colModel: [
            { name: 'CREATE_DATE', index: 'CREATE_DATE', sortable: false, width: 100, align: 'left', hidden: true },
            { name: 'PO_CFM', index: 'PO_CFM', sortable: false, width: 100, align: 'left' },
            { name: 'PO_NUM', index: 'PO_NUM', sortable: false, width: 90, align: 'left' },
            { name: 'PO_LINE', index: 'PO_LINE', sortable: false, width: 55, align: 'left' },
            { name: 'DELIV_DATE', index: 'DELIV_DATE', sortable: false, width: 85, align: 'left' },
            { name: 'POQTY', index: 'POQTY', sortable: false, formatter: 'integer', formatoptions: { thousandsSeparator: ',', defaultValue: '' }, width: 70, align: 'right' },
            { name: 'PO_CFMDATE', index: 'PO_CFMDATE', sortable: false, width: 105, align: 'left' },
            { name: 'PO_CFMQTY', index: 'PO_CFMQTY', sortable: false, formatter: 'integer', formatoptions: { thousandsSeparator: ',', defaultValue: '' }, width: 95, align: 'right' },
            { name: 'PLANT', index: 'PLANT', sortable: false, width: 55, align: 'left' },
            { name: 'VENDOR', index: 'VENDOR', sortable: false, width: 95, align: 'left' },
            { name: 'MATERIAL', index: 'MATERIAL', sortable: false, width: 135, align: 'left' },
            { name: 'MTLDESC', index: 'MTLDESC', sortable: false, width: 255, align: 'left' },
            { name: 'MTLSZDM', index: 'MTLSZDM', sortable: false, width: 150, align: 'left' },
            { name: 'UOM', index: 'UOM', sortable: false, width: 55, align: 'left' }
        ],
        pager: '#gridOpenPOConfirmationListPager',
        viewrecords: true,
        shrinkToFit: false,
        scrollrows: true,
        width: 995,
        height: 248,
        rowNum: 10,
        hoverrows: false,
        loadonce: true
    });
    $('#gridOpenPOConfirmation').jqGrid('navGrid', '#gridOpenPOConfirmationListPager', { edit: false, add: false, del: false, search: false, refresh: false });
});