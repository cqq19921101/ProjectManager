$(function () {
    $.init_fileupload(
        'fileupload_uploadABA', // fileUpload Control ID
        1,                      // maxNumberOfFiles (null: nolimit)
        30000000,               // maxFileSize (ex: 10000000 // 10 MB; 0: nolimit)
        null                    // acceptFileTypes (ex: /(\.|\/)(gif|jpe?g|png)$/i)
        );
    //Init Function Button
    $('#btnQueryABA').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnAddABA').button({
        label: "Add",
        icons: { primary: 'ui-icon-pencil' }
    });

    $('#btnDeleteABA').button({
        label: "Delete",
        icons: { primary: 'ui-icon-trash' }
    });

    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/GetAVASiteList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '<option value="" selected></option>';
            $('#ddlSite option').remove();
            $('#ddlSITE_ID option').remove();
            $(data).each(function (index, value) {
                options += '<option value="' + value.SITE_ID + '">' + value.SITE_NAME + '</option>';
            });
            $('#ddlSite').html(options);
            $('#ddlSITE_ID').html(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });

    // Init jqGrid
    var selectedRows = {};
    $('#gridABA').jqGrid({
        url: __WebAppPathPrefix + '/VMIProcess/GetABAList',
        mtype: 'POST',
        datatype: 'local',
        jsonReader: {
            root: 'Rows',
            page: 'Page',
            total: 'Total',
            records: 'Records',
            repeatitems: false,
        },
        colNames: [
            'Buyer',
            'Site',
            'Status',
            'Issuer',
            'Issue Date',
            'Close Date',
            'NBA_ID'
        ],
        colModel: [
            { name: "BUYER_NAME", index: "BUYER_NAME", width: 105, sortable: false, classes: "jqGridColumnDataAsLinkWithBlue" },
            { name: "SITE_ID", index: "SITE_ID", width: 90, sortable: false },
            { name: "STATUS", index: "STATUS", width: 70, sortable: false },
            { name: "CREATE_USER_NAME", index: "CREATE_USER_NAME", width: 105, sortable: false },
            { name: "CREATE_TIME", index: "CREATE_TIME", width: 125, sortable: false },
            { name: "CLOSE_TIME", index: "CLOSE_TIME", width: 125, sortable: false },
            { name: "NBA_ID", index: "NBA_ID", width: 285, sortable: false, hidden: true }
        ],
        pager: '#gridABAListPager',
        viewrecords: true,
        shrinkToFit: false,
        scrollrows: true,
        width: 676,
        height: 240,
        rowNum: 10,
        loadonce: true,
        multiselect: true,
        hoverrows: false,
        // to save selection state
        onSelectAll: function (rowIds, status) {
            if (status === true) {
                for (var i = 0; i < rowIds.length; i++) {
                    if ($('#jqg_gridABA_' + rowIds[i]).css('display') != 'none')
                        selectedRows[rowIds[i]] = true;
                    else
                        $('#gridABA [role=row]#' + rowIds[i]).removeClass('ui-state-highlight');
                }
            } else {
                for (var i = 0; i < rowIds.length; i++) {
                    if ($('#jqg_gridABA_' + rowIds[i]).css('display') != 'none')
                        delete selectedRows[rowIds[i]];
                }
            }
        },
        onSelectRow: function (rowId, status, e) {
            if ($(e.target).attr('role') == 'checkbox') {
                if ($('#jqg_gridABA_' + rowId).css('display') != 'none') {
                    if (status === false) {
                        delete selectedRows[rowId];
                    } else {
                        selectedRows[rowId] = status;
                    }
                }
                else {
                    $('#gridABA [role=row]#' + rowId).removeClass('ui-state-highlight');
                }
            }
            else {
                $('#gridABA').setSelection(rowId, false);
            }
        },
        gridComplete: function () {
            var ids = $("#gridABA").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var rowId = ids[i];
                var STATUS = $("#gridABA").jqGrid('getCell', ids[i], 'STATUS');

                if (STATUS != 'New') {
                    $('#jqg_gridABA_' + ids[i]).css('display', 'none');
                }
            }
        },
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            if (iCol == 1) {
                $('#dialogABAManage').prop('NBA_ID', $(this).jqGrid('getCell', rowid, 'NBA_ID'));
                $('#dialogABAManage').prop('STATUS', $(this).jqGrid('getCell', rowid, 'STATUS'));
                $('#dialogABAManage').dialog('open');
            }
        }
    });
    $('#gridABA').jqGrid('navGrid', '#gridABAListPager', { edit: false, add: false, del: false, search: false, refresh: false });
});