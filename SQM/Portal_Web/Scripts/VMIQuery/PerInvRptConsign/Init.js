$(function () {
    $('#btnDownloadPDF').button({
        label: 'Download PDF',
        icons: { primary: 'ui-icon-arrowthickstop-1-s' }
    });

    $('input#txtFromPlant').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });

    $('input#txtToPlant').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });

    $('input#txtFromVendorCode').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });

    $('input#txtToVendorCode').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });

    $('#btnOpenQueryFromPlantDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQueryToPlantDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQueryFromVendorCodeDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQueryToVendorCodeDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $("#fromPostDate").datepicker({
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
    $("#fromPostDate").datepicker("setDate", '-31d');

    $("#toPostDate").datepicker({
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
    $("#toPostDate").datepicker("setDate", '0d');
})