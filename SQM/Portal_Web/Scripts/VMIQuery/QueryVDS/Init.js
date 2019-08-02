$(function () {
    var gridVDS = $('#gridVDS');

    //Init Function Button
    $('#btnQueryVDS').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQueryPlantDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQueryVendorCodeDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQueryBuyerCodeDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnExportAll').button({
        label: 'Export All',
        icons: { primary: 'ui-icon-arrowreturnthick-1-n' }
    });

    $('#btnExportVDS').button({
        label: 'Export VDS Only',
        icons: { primary: 'ui-icon-arrowreturnthick-1-n' }
    });

    // Query character to upper
    $('input#txtPlant').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });

    $('input#txtVendorCode').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });

    $('input#txtBuyerCode').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });

    // Init Report Date
    $("#from").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            }
            catch (err) {
                $(this).val('');
            }
        }
    });

    $("#to").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            }
            catch (err) {
                $(this).val('');
            }
        }
    });

    $('input[name="queryType"]').on('click', function () {
        if ($(this).val() == 'history') {
            $('#from').val('').datepicker("refresh");
            $('#to').val('').datepicker("refresh");
            $('tr#trReportDate').show(500);
        }
        else {
            $('#from').val('').datepicker("refresh");
            $('#to').val('').datepicker("refresh");
            $('tr#trReportDate').hide(500);
        }
        gridVDS.jqGrid('clearGridData');
    });

    $('input#txtMaterialFrom').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });

    $('input#txtMaterialTo').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });

    //Init jqGrid
    var lang = "en-US";
    var langShort = lang.split('-')[0].toLowerCase();

    if ($.jgrid.hasOwnProperty("regional") && $.jgrid.regional.hasOwnProperty(lang)) {
        $.extend($.jgrid, $.jgrid.regional[lang]);
    } else if ($.jgrid.hasOwnProperty("regional") && $.jgrid.regional.hasOwnProperty(langShort)) {
        $.extend($.jgrid, $.jgrid.regional[langShort]);
    }

    gridVDS.jqGrid({
        url: __WebAppPathPrefix + "/VMIQuery/GetQueryVDSList",
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
                    "Buyer",
                    "Vendor",
                    "VDS NUM",
                    "VRSIO",
                    "TMTYPE",
                    "VENDOR_ID",
                    "ERP_VNAME",
                    "BUYER_ID"
        ],
        colModel: [
                    {
                        name: "PLANT", index: "PLANT", width: 80, sortable: false, sorttype: "text", classes: "jqGridColumnDataAsLinkWithBlue",
                        formatter: function (cellvalue, option, rowobject) {
                            return cellvalue;
                        }
                    },
                    { name: "BUYER", index: "BUYER", width: 200, sortable: false, sorttype: "text" },
                    { name: "VENDOR", index: "VENDOR", width: 300, sortable: false, sorttype: "text" },
                    { name: "VDS_NUM", index: "VDS_NUM", width: 120, sortable: false, sorttype: "text", hidden: true },
                    { name: "VRSIO", index: "VRSIO", width: 80, sortable: false, sorttype: "text", hidden: true },
                    { name: "TMTYPE", index: "TMTYPE", sortable: false, sorttype: "text", hidden: true },
                    { name: "VENDOR_ID", index: "VENDOR_ID", width: 80, sortable: false, sorttype: "text", hidden: true },
                    { name: "ERP_VNAME", index: "ERP_VNAME", width: 80, sortable: false, sorttype: "text", hidden: true },
                    { name: "BUYER_ID", index: "BUYER_ID", width: 80, sortable: false, sorttype: "text", hidden: true }
        ],
        gridComplete: function () {
            if ($("input[name='queryType']:checked").val() == 'history') {
                gridVDS.showCol('VDS_NUM');
                gridVDS.showCol('VRSIO');
            }
            else {
                gridVDS.hideCol('VDS_NUM');
                gridVDS.hideCol('VRSIO');
            }
        },
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            var $this = $(this);
            var PLANT = $this.jqGrid('getCell', rowid, 'PLANT');
            var BUYER = $this.jqGrid('getCell', rowid, 'BUYER_ID');
            var VENDOR = $this.jqGrid('getCell', rowid, 'VENDOR_ID');
            var VENDOR_NAME = $this.jqGrid('getCell', rowid, 'ERP_VNAME');
            var queryType = $("input[name='queryType']:checked").val();

            // Only when clicking column Plant then open vds detail.
            if (iCol == 0) {
                if (!__DialogIsShownNow) {
                    __DialogIsShownNow = true;
                    switch (queryType) {
                        case 'lastVer':
                        case 'verCompare':
                            $('#dialogQueryVDSManage').prop({
                                'PLANT': $.trim(PLANT),
                                'BUYER': BUYER,
                                'VENDOR': VENDOR,
                                'VENDOR_NAME': VENDOR_NAME,
                                'QUERY_TYPE': queryType,
                                'CURRENT_DATETIME': getCurrentDateTime().toString()
                            });
                            $('#dialogQueryVDSManage').show();
                            $('#dialogQueryVDSManage').dialog('open');
                            break;
                        case 'history':
                            var VDS_NUM = $this.jqGrid('getCell', rowid, 'VDS_NUM');
                            var VRSIO = $this.jqGrid('getCell', rowid, 'VRSIO');
                            var TMTYPE = $this.jqGrid('getCell', rowid, 'TMTYPE');
                            $('#dialogQueryVDSManage').prop({
                                'PLANT': $.trim(PLANT),
                                'BUYER': BUYER,
                                'VENDOR': VENDOR,
                                'VENDOR_NAME': VENDOR_NAME,
                                'VDS_NUM': VDS_NUM,
                                'VRSIO': VRSIO,
                                'QUERY_TYPE': queryType,
                                'TMTYPE': TMTYPE,
                                'CURRENT_DATETIME': getCurrentDateTime().toString()
                            });
                            $('#dialogQueryVDSManage').show();
                            $('#dialogQueryVDSManage').dialog('open');
                            break;
                    }
                    // Remove focus on all buttons within the
                    // div with class ui-dialog
                    $('.ui-dialog :button').blur();
                }
            }
        },
        width: 810,
        height: 232,
        rowNum: 10,
        viewrecords: true,
        loadonce: true,
        shrinkToFit: false,
        pager: '#gridVDSListPager'
    });

    gridVDS.jqGrid('navGrid', '#gridVDSListPager', { edit: false, add: false, del: false, search: false, refresh: false });
});

function getCurrentDateTime() {
    var d = new Date,
        dformat = [d.getFullYear(),
            (d.getMonth() + 1).padLeft(),
            d.getDate().padLeft()].join('/') +
                    ' ' +
                  [d.getHours().padLeft(),
                    d.getMinutes().padLeft()/*,
                    d.getSeconds().padLeft()*/].join(':');
    return dformat;
}