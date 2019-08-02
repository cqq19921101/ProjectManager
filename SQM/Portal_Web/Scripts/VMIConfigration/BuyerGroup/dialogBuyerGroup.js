$(function () {
    $('#dialogBuyerGroup #txtBUYER_GROUP_ID').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });

    $('#dialogBuyerGroup').dialog({
        autoOpen: false,
        resizable: false,
        height: 300,
        width: 500,
        modal: true,
        buttons: {
            Submit: function () {
                $this = $(this);
                var BUYER_GROUP_GUID = $this.prop('BUYER_GROUP_GUID');
                var BUYER_GROUP_ID = $this.find('#txtBUYER_GROUP_ID').val();
                var BUYER_GROUP_NAME = $this.find('#txtBUYER_GROUP_NAME').val();
                var Groups = [];
                $this.find('#btnGroup').parent().find('.divGroup').each(function () {
                    Groups.push({
                        GROUP_ID: $(this).find('span.spanGroup').attr('GROUP_ID'),
                        GROUP_NAME: $(this).find('span.spanGroup').text()
                    });
                });

                $.ajax({
                    url: __WebAppPathPrefix + '/VMIConfigration/EditBuyerGroup',
                    data: {
                        BUYER_GROUP_GUID: escape($.trim(BUYER_GROUP_GUID)),
                        BUYER_GROUP_ID: escape($.trim(BUYER_GROUP_ID)),
                        BUYER_GROUP_NAME: escape($.trim(BUYER_GROUP_NAME)),
                        Groups: Groups
                    },
                    type: "post",
                    dataType: 'text',
                    async: false, // if need page refresh, please remark this option
                    success: function (data) {
                        alert(data);
                        if (data == 'Edit Buyer Group successfully.') {
                            $('#dialogBuyerGroup').dialog('close');

                            $('#gridBuyerGroup').jqGrid('clearGridData');
                            $('#gridBuyerGroup').jqGrid('setGridParam', {
                                postData: {
                                    BUYER_GROUP_ID: escape($.trim($('input#txtBUYER_GROUP_ID.tdQueryTextBox').val())),
                                    BUYER_GROUP_NAME: escape($.trim($('input#txtBUYER_GROUP_NAME.tdQueryTextBox').val()))
                                }
                            });
                            $('#gridBuyerGroup').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
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
            $this = $(this);
            $this.find('#txtBUYER_GROUP_ID').val('');
            $this.find('#txtBUYER_GROUP_NAME').val('');
            $this.find('#btnGroup').parent().find('div.divGroup').remove();
            $this.removeProp('BUYER_GROUP_GUID');
        }
    });

    $('#btnGroup').button({
        label: "Add"
    });
});