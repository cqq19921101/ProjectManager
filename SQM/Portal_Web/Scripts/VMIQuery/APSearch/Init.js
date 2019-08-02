$(function () {
    //Init Function Button
    $('#btnAPSearch').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    // Init Report Date
    $("#fromInvoiceDate").datepicker({
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

    $("#toInvoiceDate").datepicker({
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

    $("#fromPayDate").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            }
            catch (err) {
                $(this).datepicker("setDate", '-92d');
            }
        }
    });
    $("#fromPayDate").datepicker("setDate", '-92d');

    $("#toPayDate").datepicker({
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
    $("#toPayDate").datepicker("setDate", '0d');

    $("#fromDueDate").datepicker({
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

    $("#toDueDate").datepicker({
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

    $('#btnOpenQueryVendorCodeDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnExportAllExcel').button({
        label: "Download All (Excel)",
        icons: { primary: 'ui-icon-arrowthickstop-1-s' }
    });

    $('#btnExportAllPDF').button({
        label: "Download All (PDF)",
        icons: { primary: 'ui-icon-arrowthickstop-1-s' }
    });

    $('#btnExportExcel').button({
        label: "Download",
        icons: { primary: 'ui-icon-arrowthickstop-1-s' }
    });

    $('input#txtVendorCode').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });

    $('#tbTopToolBarAPSearch').show();
});

function dateDiff(fromDate, toDate) {
    var date1 = new Date(fromDate);
    var date2 = new Date(toDate);
    var timeDiff = Math.abs(date2.getTime() - date1.getTime());
    var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
    return diffDays;
}