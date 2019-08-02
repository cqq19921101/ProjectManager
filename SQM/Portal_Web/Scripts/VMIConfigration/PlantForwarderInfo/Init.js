$(function () {
    // Init button
    $('#btnNewPlantForwarderInfo').button({
        label: "New",
        icons: { primary: 'ui-icon-plus' }
    });

    $('#btnQueryPlantForwarderInfo').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    // Init jqGrid
    $('#gridPlantForwarderInfo').jqGrid({
        url: __WebAppPathPrefix + '/VMIConfigration/QueryPlantForwarderInfoJsonWithFilter',
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
            , 'Company Name'
            , 'Telephone'
            , 'Contact Person'
            , 'Email'
            , 'Address'
        ],
        colModel: [
            { name: 'ERP_VND', index: 'ERP_VND', sortable: false, width: 50, classes: "jqGridColumnDataAsLinkWithBlue" },
            { name: 'COMPANY_NAME', index: 'COMPANY_NAME', sortable: false, width: 150 },
            { name: 'TEL', index: 'TEL', sortable: false, width: 115 },
            { name: 'NAME', index: 'NAME', sortable: false, width: 150 },
            { name: 'EMAIL', index: 'EMAIL', sortable: false, width: 150 },
            { name: 'ADDRESS', index: 'ADDRESS', sortable: false, width: 220 }
        ],
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            var PLANT = $(this).jqGrid('getCell', rowid, 'ERP_VND');
            var COMPANY_NAME = $(this).jqGrid('getCell', rowid, 'COMPANY_NAME');
            var TEL = $(this).jqGrid('getCell', rowid, 'TEL');
            var NAME = $(this).jqGrid('getCell', rowid, 'NAME');
            var EMAIL = $(this).jqGrid('getCell', rowid, 'EMAIL');
            var ADDRESS = $(this).jqGrid('getCell', rowid, 'ADDRESS');

            if (iCol == 0) {
                var dialogPlantForwarderInfo = $('#dialogPlantForwarderInfo');
                dialogPlantForwarderInfo.find('#txtPlant').val(PLANT);
                dialogPlantForwarderInfo.find('#txtPlant').prop('disabled', true);
                dialogPlantForwarderInfo.find('#txtCompanyName').val(COMPANY_NAME);
                dialogPlantForwarderInfo.find('#txtCompanyName').prop('disabled', true);
                dialogPlantForwarderInfo.find('#txtTel').val(TEL);
                dialogPlantForwarderInfo.find('#txtName').val(NAME);
                dialogPlantForwarderInfo.find('#txtEmail').val(EMAIL);
                dialogPlantForwarderInfo.find('#txtAddress').val(ADDRESS);
                dialogPlantForwarderInfo.dialog('open');
                dialogPlantForwarderInfo.prop('Action', 'U');

                var buttons = dialogPlantForwarderInfo.dialog("option", "buttons"); // getter
                $.extend(buttons, {
                    Delete: function () {
                        var ACTION = 'D';
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
                    }
                });
                dialogPlantForwarderInfo.dialog("option", "buttons", buttons); // setter
            }
        },
        pager: '#gridPlantForwarderInfoPager',
        viewrecords: true,
        shrinkToFit: false,
        scrollrows: true,
        width: 870,
        height: 232,
        rowNum: 10,
        hoverrows: false,
        loadonce: true
    });

    $('#gridPlantForwarderInfo').jqGrid('navGrid', '#gridPlantForwarderInfoPager', { edit: false, add: false, del: false, search: false, refresh: false });
});