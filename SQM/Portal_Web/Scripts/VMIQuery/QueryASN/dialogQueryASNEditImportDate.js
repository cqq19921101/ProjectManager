$(function () {
    var dialog_VMIQuery_EditImportDate = $('#dialog_VMIQuery_EditImportDate');

    //Init dialog
    dialog_VMIQuery_EditImportDate.dialog({
        autoOpen: false,
        height: 220,
        width: 300,
        resizable: false,
        modal: true,
        open: function () {
            $("#ARRIVAL_DATE").val($('#dialog_span_VMIQuery_QueryASNManage_ArrivalDate').text());
            $("#PLAN_IMPORT_DATE").val($('#dialog_span_VMIQuery_QueryASNManage_PlanImportDate').text());
        },
        buttons: {
            Submit: function () {
                var ASN_NUM = $('#dialog_VMIQuery_QueryASNManage').prop('ASN_NUM');
                var ARRIVAL_DATE = $(this).find('#ARRIVAL_DATE').val();
                var PLAN_IMPORT_DATE = $(this).find('#PLAN_IMPORT_DATE').val();                
                
                $.ajax({
                    url: __WebAppPathPrefix + '/VMIQuery/EditImportDate',
                    data: {
                        ASN_NUM: escape($.trim(ASN_NUM)),
                        ARRIVAL_DATE: escape($.trim(ARRIVAL_DATE)),
                        PLAN_IMPORT_DATE: escape($.trim(PLAN_IMPORT_DATE))
                    },
                    type: "post",
                    dataType: 'text',
                    async: false, // if need page refresh, please remark this option
                    success: function (data) {
                        alert(data);
                        InitdialogQueryASNHeaderForManage();
                        dialog_VMIQuery_EditImportDate.dialog("close");
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    },
                    complete: function (jqXHR, textStatus) {
                    }
                });
            },
            Close: function () {
                $(this).dialog("close");
            }
        },
        close: function () {
            __DialogIsShownNow = false;
        }
    });

    $("#ARRIVAL_DATE").datepicker({
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

    $("#PLAN_IMPORT_DATE").datepicker({
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
});