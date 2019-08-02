$(function () {
    $('#dialogPlantCustomsBrokerInfo').dialog({
        autoOpen: false,
        resizable: false,
        height: 230,
        width: 500,
        modal: true,
        buttons: {
            Submit: function () {
                var ACTION = $(this).prop('Action');
                var PLANT = $(this).find('#txtPlant').val();
                var NAME = $(this).find('#txtName').val();
                var ADDR = $(this).find('#txtAddr').val();
                var TEL = $(this).find('#txtTel').val();

                $.ajax({
                    url: __WebAppPathPrefix + '/VMIConfigration/EditPlantCustomsBrokerInfo',
                    data: {
                        ACTION: escape($.trim(ACTION)),
                        PLANT: escape($.trim(PLANT)),
                        NAME: escape($.trim(NAME)),
                        ADDR: escape($.trim(ADDR)),
                        TEL: escape($.trim(TEL))
                    },
                    type: "post",
                    dataType: 'text',
                    async: false, // if need page refresh, please remark this option
                    success: function (data) {
                        alert(data);
                        if (data.indexOf('successfully') != -1) {
                            $('#dialogPlantCustomsBrokerInfo').dialog('close');

                            $('#gridPlantCustomsBrokerInfo').jqGrid('clearGridData');
                            $('#gridPlantCustomsBrokerInfo').jqGrid('setGridParam', {
                                postData: {
                                    PLANT: escape($.trim($('#txtPlant.tdQueryTextBox').val()))
                                }
                            });
                            $('#gridPlantCustomsBrokerInfo').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
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
            $(this).find('#txtName').val('').prop('disabled', false);
            $(this).find('#txtAddr').val('').prop('disabled', false);
            $(this).find('#txtTel').val('').prop('disabled', false);
        }
    });
});