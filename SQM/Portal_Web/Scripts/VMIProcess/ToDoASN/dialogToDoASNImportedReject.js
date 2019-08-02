$(function () {
    var dialog_VMIProcess_ToDoASNImportedReject = $('#dialog_VMIProcess_ToDoASNImportedReject');
    
    //Init dialog
    dialog_VMIProcess_ToDoASNImportedReject.dialog({
        autoOpen: false,
        height: 220,
        width: 350,
        resizable: false,
        modal: true,
        buttons: {
            Submit: function () {
                var ASN_NUM = $('#dialog_VMIProcess_ToDoASNManage').prop('ASN_NUM');
                var REJECT_REASON = $.trim($(this).find('#REJECT_REASON').val());
                
                $.ajax({
                    url: __WebAppPathPrefix + '/VMIProcess/RejectImportedASN',
                    data: {
                        ASN_NUM: escape($.trim(ASN_NUM)),
                        REJECT_REASON: escape($.trim(REJECT_REASON))
                    },
                    type: "post",
                    dataType: 'text',
                    async: false, // if need page refresh, please remark this option
                    success: function (data) {
                        if (data == 'Success') {
                            alert('Reject successfully.');
                            $('#dia_btn_VMIProcess_ToDoASNManage_Reject').hide();
                            DisableFunctionButton();
                            ReloadToDoASNManagegridDataList();
                            $('#dialog_VMIProcess_ToDoASNManage').dialog('close');
                        }
                        else {
                            alert('Reject failure, plase contact administartor manager.');
                        }
                        dialog_VMIProcess_ToDoASNImportedReject.dialog('close');
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
});