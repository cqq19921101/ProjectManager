$(function () {
    // Init button
    $('#btnNewImportList').button({
        label: "New",
        icons: { primary: 'ui-icon-plus' }
    });

    $('#btnQueryImportList').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    // Init jqGrid
    $('#gridImportList').jqGrid({
        url: __WebAppPathPrefix + '/VMIProcess/QueryImportList',
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
            'Import List No.'
            , 'Forwarder Company Name'
            , 'Receiver'
            , 'Plan Arrival Date'
        ],
        colModel: [
            { name: 'IMPORT_LIST_NUM', index: 'IMPORT_LIST_NUM', sortable: false, width: 102, align: 'left', classes: "jqGridColumnDataAsLinkWithBlue" },
            { name: 'COMPANY_NAME', index: 'COMPANY_NAME', sortable: false, width: 200, align: 'left' },
            { name: 'RECEIVER', index: 'RECEIVER', sortable: false, width: 130, align: 'left' },
            { name: 'PLAN_ARRIVAL_TIME', index: 'PLAN_ARRIVAL_TIME', sortable: false, width: 120, align: 'left' }
        ],
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            var IMPORT_LIST_NUM = $(this).jqGrid('getCell', rowid, 'IMPORT_LIST_NUM');
            
            if (iCol == 0) {
                $('#dialogImportListForm').prop('IMPORT_LIST_NUM', IMPORT_LIST_NUM);
                $('#dialogImportListForm').dialog('open');
                $('.ui-dialog :button').blur();
                //if (!__DialogIsShownNow) {
                //    __DialogIsShownNow = true;
                //    //diaToDoVDSManage.prop({ 'PLANT': $.trim(PLANT), 'BUYER': BUYER, 'VENDOR': VENDOR, 'VENDOR_NAME': VENDOR_NAME, 'CURRENT_DATEITME': getCurrentDateTime().toString() });
                //    //diaToDoVDSManage.show();
                //    //diaToDoVDSManage.dialog('open');
                    
                //}
            }
        },
        pager: '#gridImportListPager',
        viewrecords: true,
        shrinkToFit: false,
        scrollrows: true,
        width: 572,
        height: 232,
        rowNum: 10,
        hoverrows: false,
        loadonce: true
    });

    $('#gridImportList').jqGrid('navGrid', '#gridImportListPager', { edit: false, add: false, del: false, search: false, refresh: false });
});