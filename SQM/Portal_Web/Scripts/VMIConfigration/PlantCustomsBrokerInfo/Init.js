$(function () {
    // Init button
    $('#btnNewPlantCustomsBrokerInfo').button({
        label: "New",
        icons: { primary: 'ui-icon-plus' }
    });

    $('#btnQueryPlantCustomsBrokerInfo').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    // Init jqGrid
    $('#gridPlantCustomsBrokerInfo').jqGrid({
        url: __WebAppPathPrefix + '/VMIConfigration/QueryPlantCBInfoJsonWithFilter',
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
            , 'Name'
            , 'Address'
            , 'Telephone'
        ],
        colModel: [
            { name: 'PLANT', index: 'PLANT', sortable: false, width: 55, classes: "jqGridColumnDataAsLinkWithBlue" },
            { name: 'NAME', index: 'NAME', sortable: false, width: 220 },
            { name: 'ADDR', index: 'ADDR', sortable: false, width: 290 },
            { name: 'TEL', index: 'TEL', sortable: false, width: 140 }
        ],
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            var PLANT = $(this).jqGrid('getCell', rowid, 'PLANT');
            var NAME = $(this).jqGrid('getCell', rowid, 'NAME');
            var ADDR = $(this).jqGrid('getCell', rowid, 'ADDR');
            var TEL = $(this).jqGrid('getCell', rowid, 'TEL');

            if (iCol == 0) {
                var dialogPlantCustomsBrokerInfo = $('#dialogPlantCustomsBrokerInfo');
                dialogPlantCustomsBrokerInfo.find('#txtPlant').val(PLANT);
                dialogPlantCustomsBrokerInfo.find('#txtPlant').prop('disabled', true);
                dialogPlantCustomsBrokerInfo.find('#txtName').val(NAME);
                dialogPlantCustomsBrokerInfo.find('#txtName').prop('disabled', true);
                dialogPlantCustomsBrokerInfo.find('#txtAddr').val(ADDR);
                dialogPlantCustomsBrokerInfo.find('#txtTel').val(TEL);
                dialogPlantCustomsBrokerInfo.dialog('open');
                dialogPlantCustomsBrokerInfo.prop('Action', 'U');

                var buttons = dialogPlantCustomsBrokerInfo.dialog("option", "buttons"); // getter
                $.extend(buttons, {
                    Delete: function () {
                        var ACTION = 'D';
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
                    }
                });
                dialogPlantCustomsBrokerInfo.dialog("option", "buttons", buttons); // setter
            }
        },
        pager: '#gridPlantCustomsBrokerInfoPager',
        viewrecords: true,
        shrinkToFit: false,
        scrollrows: true,
        width: 725,
        height: 232,
        rowNum: 10,
        hoverrows: false,
        loadonce: true
    });

    $('#gridPlantCustomsBrokerInfo').jqGrid('navGrid', '#gridPlantCustomsBrokerInfoPager', { edit: false, add: false, del: false, search: false, refresh: false });
});