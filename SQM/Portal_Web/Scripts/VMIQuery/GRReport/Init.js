$(function () {
    $('#btnExportToExcel').button({
        label: 'Export to Excel',
        icons: { primary: 'ui-icon-arrowthickstop-1-s' }
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

    $("#fromGRDate").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-7d');
            }
        }
    });
    $("#fromGRDate").datepicker("setDate", '-7d');

    $("#toGRDate").datepicker({
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
    $("#toGRDate").datepicker("setDate", '0d');

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
});
