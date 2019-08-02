$(function () {
    // Init button
    $('#btnNewPlantReceiveInfo').button({
        label: "New",
        icons: { primary: 'ui-icon-plus' }
    });

    $('#btnQueryPlantReceiveInfo').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    // Init jqGrid
    $('#gridPlantReceiveInfo').jqGrid({
        url: __WebAppPathPrefix + '/VMIConfigration/QueryPlantReceiveInfoJsonWithFilter',
        mtype: 'POST',
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false,
        },
        colNames: [
            'Plant'
            , 'Receiver'
            , 'Receive Address'
            , 'Receiver Telephone'
        ],
        colModel: [
            { name: 'PLANT', index: 'PLANT', sortable: false, width: 55, classes: "jqGridColumnDataAsLinkWithBlue" },
            { name: 'RECEIVER', index: 'RECEIVER', sortable: false, width: 75 },
            { name: 'RECEIVE_ADDR', index: 'RECEIVE_ADDR', sortable: false, width: 215 },
            { name: 'RECEIVER_TEL', index: 'RECEIVER_TEL', sortable: false, width: 140 }
        ],
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            var PLANT = $(this).jqGrid('getCell', rowid, 'PLANT');
            var RECEIVER = $(this).jqGrid('getCell', rowid, 'RECEIVER');
            var RECEIVE_ADDR = $(this).jqGrid('getCell', rowid, 'RECEIVE_ADDR');
            var RECEIVER_TEL = $(this).jqGrid('getCell', rowid, 'RECEIVER_TEL');

            if (iCol == 0) {
                var dialogPlantReceiveInfo = $('#dialogPlantReceiveInfo');
                dialogPlantReceiveInfo.find('#txtPlant').val(PLANT);
                dialogPlantReceiveInfo.find('#txtPlant').prop('disabled', true);
                dialogPlantReceiveInfo.find('#txtReceiver').val(RECEIVER);
                dialogPlantReceiveInfo.find('#txtReceiveAddr').val(RECEIVE_ADDR);
                dialogPlantReceiveInfo.find('#txtReceiverTel').val(RECEIVER_TEL);
                dialogPlantReceiveInfo.dialog('open');
                dialogPlantReceiveInfo.prop('Action', 'U');

                var buttons = dialogPlantReceiveInfo.dialog("option", "buttons"); // getter
                $.extend(buttons, {
                    Delete: function () {
                        var ACTION = 'D';
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
                    }
                });
                dialogPlantReceiveInfo.dialog("option", "buttons", buttons); // setter
            }
        },
        pager: '#gridPlantReceiveInfoPager',
        viewrecords: true,
        shrinkToFit: false,
        scrollrows: true,
        width: 505,
        height: 232,
        rowNum: 10,
        hoverrows: false,
        loadonce: true
    });

    $('#gridPlantReceiveInfo').jqGrid('navGrid', '#gridPlantReceiveInfoPager', { edit: false, add: false, del: false, search: false, refresh: false });
});