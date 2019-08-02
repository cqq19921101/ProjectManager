$(function () {
    var diaQueryASNManage = $('#dialog_VMIQuery_QueryASNManage');
    var diaEditShipInfo = $('#dialog_QueryASN_EditShippingInfo');

    $('#dialog_txt_QueryASN_EditShippingInfo_ETATime').inputmask("h:s", { "placeholder": "00:00" });

    //datepicker
    var dateformat = $('input[name$="Date2"]');
    dateformat.datepicker({
        dateFormat: 'yy/mm/dd',
        onSelect: function () {
            diaEditShipInfo.focus();
        }
    });
    dateformat.datepicker('disable');

    //Init dialog
    diaEditShipInfo.dialog({
        autoOpen: false,
        height: 355,
        width: 650,
        resizable: false,
        modal: true,
        buttons: {
            Submit: function () {
                var DoSuccessfully = false;
                $.ajax({
                    url: __WebAppPathPrefix + "/VMIProcess/UpdateHeaderForEditShipInfo",
                    data: {
                        ASNNO: escape($.trim($('#dialog_span_QueryASN_EditShippingInfo_ASNNo').html())),
                        VENDOR: escape($.trim($('#dialog_span_QueryASN_EditShippingInfo_Vendor').html())),
                        PLANTCODE: escape($.trim($('#dialog_span_QueryASN_EditShippingInfo_PlantCode').html())),
                        ETA: escape($.trim($('#dpQueryASN_ETADate').val())),
                        ETA_TIME: escape($.trim($('#dialog_txt_QueryASN_EditShippingInfo_ETATime').val())),
                        VEHICLETYPEID: escape($.trim($('#dialog_txt_QueryASN_EditShippingInfo_VehicleTypeAndID').val())),
                        DRIVERNAME: escape($.trim($('#dialog_txt_QueryASN_EditShippingInfo_DriverName').val())),
                        DRIVERPHONE: escape($.trim($('#dialog_txt_QueryASN_EditShippingInfo_DriverPhone').val()))
                    },
                    type: "post",
                    dataType: 'json',
                    async: false,
                    success: function (data) {
                        if (data.Result == true) {
                            DoSuccessfully = true;
                            diaQueryASNManage.prop("ASN_NUM", $.trim(data.Message));
                            alert("Update successfully.");
                            InitdialogQueryASNHeaderForManage();
                        }
                        else {
                            alert(data.Message);
                        }
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    },
                    complete: function (jqXHR, textStatus) {
                        //HideAjaxLoading();
                    }
                });

                if (DoSuccessfully) {
                    $(this).dialog("close");
                }
            },
            Cancel: function () {
                $(this).dialog("close");
            },
        },
        close: function () {
            __DialogIsShownNow = false;
            dateformat.datepicker('disable');
        },
        open: function (event, ui) {
            diaEditShipInfo.focus();
            dateformat.datepicker('enable');
            $(this).parents().find('.ui-dialog-buttonpane button:contains("Submit")').blur();
            $(this).parents().find('.ui-dialog-buttonpane button:contains("Cancel")').focus();
        }
    });
});