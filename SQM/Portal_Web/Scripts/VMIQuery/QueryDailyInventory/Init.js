$(function () {
    var gridQueryDailyInventory = $('#gridQueryDailyInventory');

    //Init Function Button
    $('#btnQueryDailyInventory').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQueryPlantDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQueryVendorCodeDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnExportExcel').button({
        label: 'Export All',
        icons: { primary: 'ui-icon-arrowreturnthick-1-n' }
    });
    
    $('#tbTopToolBarQueryDailyInventory').show();

    // Query character to upper
    $('input#txtPlant').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });

    $('input#txtVendorCode').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
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

    gridQueryDailyInventory.jqGrid({
        url: __WebAppPathPrefix + "/VMIQuery/GetQueryDailyInventoryList",
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
                    "Vendor"
        ],
        colModel: [
                    {
                        name: "plant", index: "plant", width: 80, sortable: false, sorttype: "text", classes: "jqGridColumnDataAsLinkWithBlue",
                        formatter: function (cellvalue, option, rowobject) {
                            return cellvalue;
                        }
                    },                  
                    { name: "vendor", index: "vendor", width: 380, sortable: false, sorttype: "text" }
        ],
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            $this = $(this);
            vendorCodeName = $this.jqGrid('getCell', rowid, 'vendor');
            vendorCode = vendorCodeName.split(' ')[0];
            vendorName = vendorCodeName.replace(vendorCode + ' ', '');
            $('#dialogQueryDailyInventory').prop({
                plant:  $this.jqGrid('getCell', rowid, 'plant'),
                vendorCode: vendorCode,
                vendorName: vendorName,
                fromMaterial: $.trim($('#txtMaterialFrom').val()),
                toMaterial: $.trim($('#txtMaterialTo').val())
                });
            $('#dialogQueryDailyInventory').dialog('open');
        },
        width: 490,
        height: 232,
        rowNum: 10,
        viewrecords: true,
        loadonce: true,
        shrinkToFit: false,
        pager: '#gridQueryDailyInventoryListPager'
    });

    gridQueryDailyInventory.jqGrid('navGrid', '#gridQueryDailyInventoryListPager', { edit: false, add: false, del: false, search: false, refresh: false });
});