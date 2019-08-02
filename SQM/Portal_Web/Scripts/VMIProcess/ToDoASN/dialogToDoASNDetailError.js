$(function () {
    var diaToDoASNDetailError = $('#dialog_VMIProcess_ToDoASNDetailError');

    //Init dialog
    diaToDoASNDetailError.dialog({
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


function InitdialogToDoASNDetailError() {
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');
    var diaToDoASNDetailError = $('#dialog_VMIProcess_ToDoASNDetailError');

    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/QueryToDoASNDetailErrorInfo',
        data: {
            ASNNUM: escape($.trim(diaToDoASNDetailError.attr("ASNNUM"))),
            ASNLINE: escape($.trim(diaToDoASNDetailError.attr("ASNLINE"))),
            PLANT: escape($.trim(diaToDoASNManage.attr('PLANT'))),
            VENDOR: escape($.trim(diaToDoASNManage.attr('VENDOR')))
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.HASRESULT) {
                $('#dialog_span_VMIProcess_ToDoASNDetailError_ASNNo').html(data.ASNNUM);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_Status').html(data.STATUS);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_LogHandingGroup').html(data.KEEPER);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_MaterialNo').html(data.MATERIAL);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_CustomDesc').html(data.CUSTOMDESCRIPTION);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_Description').html(data.DESCRIPTION);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_CustomGroupNo').html(data.PRODUCTNO);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_POSANO').html(data.PONUM);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_POLine').html(data.POLINE);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_UOM').html(data.UOM);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_ASNQty').html(data.ASNQTY);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_PackingDetail').html(data.PACKAGEDESCRIPTION);;
                $('#dialog_span_VMIProcess_ToDoASNDetailError_UnitNetWeight').html(data.NETWT);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_VendorMaterialNo').html(data.VENDORMATERIAL);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_CustomesSeal').html(data.CUSTOMSSEAL);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_TPacking').html(data.TOTSET);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_TNetWeight').html(data.TOTNW);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_TGrossWeight').html(data.TOTGW);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_PackingType').html(data.PACKMATH);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_OriginCountryCode').html(data.ORGCNTYCODE);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_OriginCountry').html(data.ORGCNTY);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_HAWB').html(data.HAWB);
                $('#dialog_span_VMIProcess_ToDoASNDetailError_MAWB').html(data.MAWB);

                $('#dialog_span_VMProcess_ToDoASNDetailError_msg').html(data.MESSAGE);

                diaToDoASNDetailError.show();
                diaToDoASNDetailError.dialog("open");
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

