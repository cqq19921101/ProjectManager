$(function () {
    $('#dialogPlantForwarderInfo').dialog({
        autoOpen: false,
        resizable: false,
        height: 300,
        width: 500,
        modal: true,
        buttons: {
            Submit: function () {
                var ACTION = $(this).prop('Action');
                var PLANT = $(this).find('#txtPlant').val();
                var COMPANY_NAME = $(this).find('#txtCompanyName').val();
                var TEL = $(this).find('#txtTel').val();
                var NAME = $(this).find('#txtName').val();
                var EMAIL = $(this).find('#txtEmail').val();
                var ADDRESS = $(this).find('#txtAddress').val();

                $.ajax({
                    url: __WebAppPathPrefix + '/VMIConfigration/EditPlantForwarderInfo',
                    data: {
                        ACTION: escape($.trim(ACTION)),
                        PLANT: escape($.trim(PLANT)),
                        COMPANY_NAME: escape($.trim(COMPANY_NAME)),
                        TEL: escape($.trim(TEL)),
                        NAME: escape($.trim(NAME)),
                        EMAIL: escape($.trim(EMAIL)),
                        ADDRESS: escape($.trim(ADDRESS))
                    },
                    type: "post",
                    dataType: 'text',
                    async: false, // if need page refresh, please remark this option
                    success: function (data) {
                        alert(data);
                        if (data.indexOf('successfully') != -1) {
                            $('#dialogPlantForwarderInfo').dialog('close');

                            $('#gridPlantForwarderInfo').jqGrid('clearGridData');
                            $('#gridPlantForwarderInfo').jqGrid('setGridParam', {
                                postData: {
                                    PLANT: escape($.trim($('#txtPlant.tdQueryTextBox').val())),
                                    COMPANY_NAME: escape($.trim($('#txtCompanyName.tdQueryTextBox').val()))
                                }
                            });
                            $('#gridPlantForwarderInfo').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
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
            $(this).find('#txtCompanyName').val('').prop('disabled', false);
            $(this).find('#txtTel').val('').prop('disabled', false);
            $(this).find('#txtName').val('').prop('disabled', false);
            $(this).find('#txtEmail').val('').prop('disabled', false);
            $(this).find('#txtAddress').val('').prop('disabled', false);
        }
    });
});