$(function () {
    $('#ddlVendor').change(function () {
        $('#listAPSearch').jqGrid('clearGridData');
        $('#listAPSearch').jqGrid('setGridParam', {
            postData: {
                InvoiceNo: escape($.trim($("#txtInvoiceNo").val())),
                PONo: escape($.trim($("#txtPONo").val())),
                fromInvoiceDate: escape($.trim($("#fromInvoiceDate").val())),
                toInvoiceDate: escape($.trim($("#toInvoiceDate").val())),
                fromPayDate: escape($.trim($("#fromPayDate").val())),
                toPayDate: escape($.trim($("#toPayDate").val())),
                fromDueDate: escape($.trim($("#fromDueDate").val())),
                toDueDate: escape($.trim($("#toDueDate").val())),
                CompanyCode: escape($.trim($("#txtCompanyCode").val())),
                VendorCode: escape($.trim($(this).find('option:selected').val())),
                PayStatus: escape($.trim($("#ddlPayStatus").val())),
                OrderBy: escape($.trim($("#ddlOrderBy").val()))
            }
        });
        $('#listAPSearch').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });

    $('#dialogAPSearch').dialog({
        autoOpen: false,
        resizable: false,
        modal: true,
        open: function () {
            $('#btnExportAllExcel').blur();
            $this = $(this);
            /* adjusting the jqGrid size for current browser */
            var dialogAPSearchWidth, listAPSearchWidth;
            var currentWindowWidth = $(window).width() - 50;

            if (currentWindowWidth < 100) { dialogAPSearchWidth = 100; listAPSearchWidth = 100; }
            else if (currentWindowWidth > 1345) { dialogAPSearchWidth = 1395; listAPSearchWidth = 1345; }
            else { dialogAPSearchWidth = currentWindowWidth - 50; listAPSearchWidth = currentWindowWidth - 100; }

            $('#dialogAPSearch').dialog('option', 'width', dialogAPSearchWidth);
            $('#listAPSearch').jqGrid('setGridWidth', listAPSearchWidth);
        },
        close: function () {
            $('#ddlVendor option').remove();
            $('#tdDialogVendorCode').text('');
            $('#tdDialogVendorName').text('');
        }
    });

    $('#listAPSearch').jqGrid({
        url: __WebAppPathPrefix + '/VMIQuery/QueryAPSearch',
        mtype: 'POST',
        datatype: "local",
        jsonReader: {
            root: "rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        colNames: [
            "Vendor Code",
            "Vendor Name",
            "LITEON Invoice",
            "Invoice NO",
            "Invoice Date",
            "Company Code",
            "Currency",
            "Invoice Amount",
            "G/L Date",
            "Due Date",
            "Pay Amount",
            "Pay Date",
            "Pay Status",
            "Consignment",
            "Po No"
        ],
        colModel: [
            { name: "VendorCode", sortable: false, width: 100, hidden: true },
            { name: "VendorName", sortable: false, width: 200, hidden: true },
            { name: "LITEONInvoice", sortable: false, width: 100 },
            { name: "InvoiceNO", sortable: false, width: 250 },
            { name: "InvoiceDate", sortable: false, width: 80 },
            { name: "CompanyCode", sortable: false, width: 100 },
            { name: "Currency", sortable: false, width: 60 },
            { name: "InvoiceAmount", sortable: false, width: 100 },
            { name: "GLDate", sortable: false, width: 80 },
            { name: "DueDate", sortable: false, width: 80 },
            { name: "PayAmount", sortable: false, width: 100 },
            { name: "PayDate", sortable: false, width: 80 },
            { name: "PayStatus", sortable: false, width: 70 },
            { name: "Consignment", sortable: false, width: 80 },
            { name: "PoNo", sortable: false, width: 100 }
        ],
        rowNum: 999999999,
        //pager: '#pagerAPSearch',
        shrinkToFit: false,
        loadonce: true,
        hoverrows: false,
        viewrecords: true,
        height: 'auto',
        loadComplete: function (data) {
            /* adjusting the jqGrid size for current browser */
            var dialogAPSearchHeight = $(this).height() + 180;
            var currentWindowHeight = $(window).height();
            if (currentWindowHeight < dialogAPSearchHeight) {
                $('#dialogAPSearch').dialog('option', 'height', currentWindowHeight - 20);
                $('#listAPSearch').jqGrid('setGridHeight', currentWindowHeight - 190);
            }
            else {
                $('#dialogAPSearch').dialog('option', 'height', dialogAPSearchHeight);
                $('#listAPSearch').jqGrid('setGridHeight', 'auto');
            }

            $('#dialogAPSearch').dialog('option', 'position', { my: 'top', at: 'top+5',  of: window })

            if ($('#listAPSearch').jqGrid('getDataIDs') != 0) {
                $('#tdDialogVendorCode').text(data[0].VendorCode);
                $('#tdDialogVendorName').text(data[0].VendorName);
            }
        }
    });

    $(window).resize(function () {
        /* adjusting the jqGrid size for current browser */
        var dialogAPSearchWidth, listAPSearchWidth;
        var currentWindowWidth = $(window).width() - 50;
        if (currentWindowWidth < 100) { dialogAPSearchWidth = 100; listAPSearchWidth = 100; }
        else if (currentWindowWidth > 1345) { dialogAPSearchWidth = 1395; listAPSearchWidth = 1345; }
        else { dialogAPSearchWidth = currentWindowWidth - 50; listAPSearchWidth = currentWindowWidth - 100; }

        $('#dialogAPSearch').dialog('option', 'width', dialogAPSearchWidth);
        $('#listAPSearch').jqGrid('setGridWidth', listAPSearchWidth);
    });
});