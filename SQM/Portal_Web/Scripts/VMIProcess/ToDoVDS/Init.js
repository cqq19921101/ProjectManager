$(function () {
    var gridVDS = $('#gridVDS');
    var diaToDoVDSManage = $('#dialog_VMIProcess_ToDoVDSManage');

    //Init Function Button
    $('#btnQueryDemand').button({
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

    $('#btnVDSCommit').button({
        label: 'Commit',
        icons: { primary: 'ui-icon-check' }
    });

    $('#btnVDSCommitAll').button({
        label: 'Commit All',
        icons: { primary: 'ui-icon-check' }
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

    //Init jqGrid
    var lang = "en-US";
    var langShort = lang.split('-')[0].toLowerCase();

    if ($.jgrid.hasOwnProperty("regional") && $.jgrid.regional.hasOwnProperty(lang)) {
        $.extend($.jgrid, $.jgrid.regional[lang]);
    } else if ($.jgrid.hasOwnProperty("regional") && $.jgrid.regional.hasOwnProperty(langShort)) {
        $.extend($.jgrid, $.jgrid.regional[langShort]);
    }

    gridVDS.jqGrid({
        url: __WebAppPathPrefix + "/VMIProcess/GetToDoVDSList",
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
                    "VENDOR_ID",
                    "ERP_VNAME",
                    "BUYER_ID"],
        colModel: [
                    {
                        name: "PLANT", index: "PLANT", width: 15, sortable: false, sorttype: "text", classes: "jqGridColumnDataAsLinkWithBlue",
                        formatter: function (cellvalue, option, rowobject) {
                            return cellvalue;
                        }
                    },
                    { name: "BUYER", index: "BUYER", width: 50, sortable: false, sorttype: "text" },
                    { name: "VENDOR", index: "VENDOR", width: 110, sortable: false, sorttype: "text" },
                    { name: "VENDOR_ID", index: "VENDOR_ID", width: 110, sortable: false, sorttype: "text", hidden: true },
                    { name: "ERP_VNAME", index: "ERP_VNAME", width: 110, sortable: false, sorttype: "text", hidden: true },
                    { name: "BUYER_ID", index: "BUYER_ID", width: 110, sortable: false, sorttype: "text", hidden: true }
        ],
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            var $this = $(this);
            var PLANT = $this.jqGrid('getCell', rowid, 'PLANT');
            var BUYER = $this.jqGrid('getCell', rowid, 'BUYER_ID');
            var VENDOR = $this.jqGrid('getCell', rowid, 'VENDOR_ID');
            var VENDOR_NAME = $this.jqGrid('getCell', rowid, 'ERP_VNAME');

            // Only when clicking column Plant then open vds detail.
            if (iCol == 0) {
                if (!__DialogIsShownNow) {
                    __DialogIsShownNow = true;
                    diaToDoVDSManage.prop({ 'PLANT': $.trim(PLANT), 'BUYER': BUYER, 'VENDOR': VENDOR, 'VENDOR_NAME': VENDOR_NAME, 'CURRENT_DATEITME': getCurrentDateTime().toString() });
                    diaToDoVDSManage.show();
                    diaToDoVDSManage.dialog('open');
                    // Remove focus on all buttons within the
                    // div with class ui-dialog
                    $('.ui-dialog :button').blur();
                }
            }
        },
        width: 700,
        height: 232,
        rowNum: 10,
        viewrecords: true,
        loadonce: true,
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