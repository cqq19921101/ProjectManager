$(function () {
    //Init Buttons
    $('#btnSearchApplyHistory').button({
        label: "Search Apply History",
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnQueryVendorEmail').button({
        label: "Query and Change Vendor E-mail",
        icons: { primary: 'ui-icon-search' }
    });
    //Init jqGrid
    var lblAdminFlag = $('#lblAdminFlag').text();

    $('#gridApplication').jqGrid({
        url: __WebAppPathPrefix + '/VMIProcess/GetApplicationList',
        mtype: 'POST',
        datatype: 'local',
        jsonReader: {
            root: 'Rows',
            page: 'Page',
            total: 'Total',
            records: 'Records',
            repeatitems: false,
        },
        colNames: [
            'ID',
            'Status',
            'Account',
            'BU Vendor Code',
            'Old E-mail',
            'New E-mail',
            'Applicant',
            'Application Date',
            'Close Date'
        ],
        colModel: [
            { name: "ID", index: "iD", width: 285, sortable: false, hidden: true },
            {
                name: "STATUS_ID", index: "Status", width: 50, sortable: false, formatter: function (cellvalue, options, rowObject) {
                    switch (cellvalue) {
                        case '0':
                            cellvalue = 'New';
                            break;
                        case '1':
                            cellvalue = 'Reject';
                            break;
                        case '2':
                            cellvalue = 'Close';
                            break;
                    }
                    return cellvalue;
                }
            },
            {
                name: "ACCOUNT", index: "Account", width: 80, sortable: false, formatter: function (cellvalue, options, rowObject) {
                    if (lblAdminFlag != '') {
                        options.colModel.classes = 'jqGridColumnDataAsLinkWithBlue';
                    }
                    return cellvalue;
                }
            },
            { name: "ERP_VND", index: "BU Vendor Code", width: 110, sortable: false },
            { name: "PREV_EMAIL", index: "Old E-mail", width: 185, sortable: false },
            { name: "NEW_EMAIL", index: "New E-mail", width: 185, sortable: false },
            { name: "CREATE_USER", index: "Applicant", width: 100, sortable: false },
            { name: "CREATE_DATE", index: "Application Date", width: 120, sortable: false },
            { name: "CLOSE_DATE", index: "Close Date", width: 120, sortable: false }
        ],
        pager: '#gridApplicationPager',
        viewrecords: true,
        shrinkToFit: false,
        scrollrows: true,
        width: 990,
        height: 240,
        rowNum: 10,
        loadonce: true,
        hoverrows: false,
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            if (iCol == 2 && lblAdminFlag != '') {
                var ID = $(this).jqGrid('getCell', rowid, 'ID');
                $('#dialogVendorAccountInfo').prop('ID', ID);
                $('#dialogVendorAccountInfo').prop('action', 'review');
                $('#dialogVendorAccountInfo').dialog('open');
            }
        }
    });

    $('#gridApplication').jqGrid('navGrid', '#gridApplicationPager', { edit: false, add: false, del: false, search: false, refresh: false });
});