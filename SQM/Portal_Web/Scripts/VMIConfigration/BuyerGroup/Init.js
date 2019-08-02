$(function () {
    $('#txtBUYER_GROUP_ID').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });

    // Init button
    $('#btnAddBuyerGroup').button({
        label: "Add",
        icons: { primary: 'ui-icon-plus' }
    });

    $('#btnDeleteBuyerGroup').button({
        label: "Delete",
        icons: { primary: 'ui-icon-trash' }
    });

    $('#btnQueryBuyerGroup').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    // Init jqGrid
    $('#gridBuyerGroup').jqGrid({
        url: __WebAppPathPrefix + '/VMIConfigration/QueryBuyerGroupJsonWithFilter',
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
            'BUYER_GROUP_GUID'
            , 'Buyer Group'
            , 'Name'
        ],
        colModel: [
            { name: 'BUYER_GROUP_GUID', index: 'BUYER_GROUP_GUID', sortable: false, width: 250, hidden: true },
            { name: 'BUYER_GROUP_ID', index: 'BUYER_GROUP_ID', sortable: false, width: 160, classes: "jqGridColumnDataAsLinkWithBlue" },
            { name: 'BUYER_GROUP_NAME', index: 'BUYER_GROUP_NAME', sortable: false, width: 270 }
        ],
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            if (iCol == 1) {
                var dialogBuyerGroup = $('#dialogBuyerGroup');
                var BUYER_GROUP_GUID = $(this).jqGrid('getCell', rowid, 'BUYER_GROUP_GUID');
                var BUYER_GROUP_ID = $(this).jqGrid('getCell', rowid, 'BUYER_GROUP_ID');
                var BUYER_GROUP_NAME = $(this).jqGrid('getCell', rowid, 'BUYER_GROUP_NAME');

                dialogBuyerGroup.find('#txtBUYER_GROUP_ID').val(BUYER_GROUP_ID);
                dialogBuyerGroup.find('#txtBUYER_GROUP_NAME').val(BUYER_GROUP_NAME);

                $.ajax({
                    url: __WebAppPathPrefix + '/VMIConfigration/QueryBuyerGroupJsonWithFilter',
                    data: {
                        BUYER_GROUP_GUID: escape($.trim(BUYER_GROUP_GUID))
                    },
                    type: "post",
                    dataType: 'json',
                    async: false, // if need page refresh, please remark this option
                    success: function (data) {
                        if (data != '') {
                            var btnGroup = $('#dialogBuyerGroup #btnGroup');
                            btnGroup.parent('div.divGroup').remove();

                            for (var i in data.Rows[0].Groups) {
                                var GROUP_ID = data.Rows[0].Groups[i].GROUP_ID;
                                var GROUP_NAME = data.Rows[0].Groups[i].GROUP_NAME;

                                var divGroup = $('<div/>').addClass('divGroup');
                                var btnRemove = $('<button/>').addClass('btnRemove').text('x').attr({ 'id': i, 'group_id': GROUP_ID });
                                var spanGroup = $('<span/>').text(GROUP_NAME).addClass('spanGroup').attr('GROUP_ID', GROUP_ID).css('text-decoration', 'underline');

                                divGroup.append(btnRemove).append('&nbsp;').append(spanGroup);

                                btnGroup.parent().append(divGroup);
                            }

                            $('.btnRemove').unbind('click');
                            $('.btnRemove').click(function () {
                                $(this).parent().remove();
                            });
                        }
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    },
                    complete: function (jqXHR, textStatus) {
                    }
                });

                dialogBuyerGroup.prop('BUYER_GROUP_GUID', BUYER_GROUP_GUID);
                dialogBuyerGroup.dialog('open');
            }
        },
        pager: '#gridBuyerGroupPager',
        viewrecords: true,
        shrinkToFit: false,
        scrollrows: true,
        width: 450,
        height: 232,
        rowNum: 10,
        hoverrows: false,
        loadonce: true
    });

    $('#gridGroupBuyer').jqGrid('navGrid', '#gridGroupBuyerPager', { edit: false, add: false, del: false, search: false, refresh: false });
});