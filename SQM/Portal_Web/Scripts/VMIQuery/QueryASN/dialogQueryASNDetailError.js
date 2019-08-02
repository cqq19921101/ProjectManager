$(function () {
    var diaQueryASNDetailError = $('#dialog_VMIQuery_QueryASNDetailError');

    //Init dialog
    diaQueryASNDetailError.dialog({
        autoOpen: false,
        height: 450,
        width: 700,
        resizable: false,
        modal: true,
        buttons: {
            Close: function () {
                $(this).dialog("close");
            }
        },
        close: function () {
            __DialogIsShownNow = false;
        }
    });
});


function InitdialogQueryASNDetailError() {
    var diaQueryASNManage = $('#dialog_VMIQuery_QueryASNManage');
    var diaQueryASNDetailError = $('#dialog_VMIQuery_QueryASNDetailError');

    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/QueryToDoASNDetailErrorInfo',
        data: {
            ASNNUM: escape($.trim(diaQueryASNDetailError.attr("ASNNUM"))),
            ASNLINE: escape($.trim(diaQueryASNDetailError.attr("ASNLINE"))),
            PLANT: escape($.trim(diaQueryASNManage.attr('PLANT'))),
            VENDOR: escape($.trim(diaQueryASNManage.attr('VENDOR')))
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.HASRESULT) {
                $('#dialog_span_VMIQuery_QueryASNDetailError_ASNNo').html(data.ASNNUM);
                $('#dialog_span_VMIQuery_QueryASNDetailError_Status').html(data.STATUS);
                $('#dialog_span_VMIQuery_QueryASNDetailError_LogHandingGroup').html(data.KEEPER);
                $('#dialog_span_VMIQuery_QueryASNDetailError_MaterialNo').html(data.MATERIAL);
                $('#dialog_span_VMIQuery_QueryASNDetailError_CustomDesc').html(data.CUSTOMDESCRIPTION);
                $('#dialog_span_VMIQuery_QueryASNDetailError_Description').html(data.DESCRIPTION);
                $('#dialog_span_VMIQuery_QueryASNDetailError_CustomGroupNo').html(data.PRODUCTNO);
                $('#dialog_span_VMIQuery_QueryASNDetailError_POSANO').html(data.PONUM);
                $('#dialog_span_VMIQuery_QueryASNDetailError_POLine').html(data.POLINE);
                $('#dialog_span_VMIQuery_QueryASNDetailError_UOM').html(data.UOM);
                $('#dialog_span_VMIQuery_QueryASNDetailError_ASNQty').html(data.ASNQTY);
                $('#dialog_span_VMIQuery_QueryASNDetailError_PackingDetail').html(data.PACKAGEDESCRIPTION);;
                $('#dialog_span_VMIQuery_QueryASNDetailError_UnitNetWeight').html(data.NETWT);
                $('#dialog_span_VMIQuery_QueryASNDetailError_VendorMaterialNo').html(data.VENDORMATERIAL);
                $('#dialog_span_VMIQuery_QueryASNDetailError_CustomesSeal').html(data.CUSTOMSSEAL);
                $('#dialog_span_VMIQuery_QueryASNDetailError_TPacking').html(data.TOTSET);
                $('#dialog_span_VMIQuery_QueryASNDetailError_TNetWeight').html(data.TOTNW);
                $('#dialog_span_VMIQuery_QueryASNDetailError_TGrossWeight').html(data.TOTGW);
                $('#dialog_span_VMIQuery_QueryASNDetailError_PackingType').html(data.PACKMATH);
                $('#dialog_span_VMIQuery_QueryASNDetailError_OriginCountryCode').html(data.ORGCNTYCODE);
                $('#dialog_span_VMIQuery_QueryASNDetailError_OriginCountry').html(data.ORGCNTY);
                $('#dialog_span_VMIQuery_QueryASNDetailError_HAWB').html(data.HAWB);
                $('#dialog_span_VMIQuery_QueryASNDetailError_MAWB').html(data.MAWB);

                $('#dialog_span_VMIQuery_QueryASNDetailError_msg').html(data.MESSAGE);

                diaQueryASNDetailError.show();
                diaQueryASNDetailError.dialog("open");
            }
            else {
                alert("The function is not avaliable to use.");
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
            //HideAjaxLoading();
        }
    });
}

