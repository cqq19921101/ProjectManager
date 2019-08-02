$(function () {
    $('#dialogPlantReceiveInfo').dialog({
        autoOpen: false,
        resizable: false,
        height: 230,
        width: 500,
        modal: true,
        buttons: {
            Submit: function () {
                var ACTION = $(this).prop('Action');
                var PLANT = $(this).find('#txtPlant').val();
                var RECEIVER = $(this).find('#txtReceiver').val();
                var RECEIVE_ADDR = $(this).find('#txtReceiveAddr').val();
                var RECEIVER_TEL = $(this).find('#txtReceiverTel').val();

                $.ajax({
                    url: __WebAppPathPrefix + '/VMIConfigration/EditPlantReceiveInfo',
                    data: {
                        ACTION: escape($.trim(ACTION)),
                        PLANT: escape($.trim(PLANT)),
                        RECEIVER: escape($.trim(RECEIVER)),
                        RECEIVE_ADDR: escape($.trim(RECEIVE_ADDR)),
                        RECEIVER_TEL: escape($.trim(RECEIVER_TEL))
                    },
                    type: "post",
                    dataType: 'text',
                    async: false, // if need page refresh, please remark this option
                    success: function (data) {
                        alert(data);
                        if (data.indexOf('successfully') != -1) {
                            $('#dialogPlantReceiveInfo').dialog('close');

                            $('#gridPlantReceiveInfo').jqGrid('clearGridData');
                            $('#gridPlantReceiveInfo').jqGrid('setGridParam', {
                                postData: {
                                    PLANT: escape($.trim($('#txtPlant.tdQueryTextBox').val()))
                                }
                            });
                            $('#gridPlantReceiveInfo').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                        }
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    },
                    complete: function (jqXHR, textStatus) {
                    }
                });
            },
            Close: function () {
                $(this).dialog('close');
            }
        },
        close: function () {
            $(this).find('#txtPlant').val('').prop('disabled', false);
            $(this).find('#txtReceiver').val('').prop('disabled', false);
            $(this).find('#txtReceiveAddr').val('').prop('disabled', false);
            $(this).find('#txtReceiverTel').val('').prop('disabled', false);
        }
    });
});