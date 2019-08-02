$(function () {
    $.init_fileupload(
        'fileupload_uploadAVA', // fileUpload Control ID
        1,                      // maxNumberOfFiles (null: nolimit)
        30000000,               // maxFileSize (ex: 10000000 // 10 MB; 0: nolimit)
        null                    // acceptFileTypes (ex: /(\.|\/)(gif|jpe?g|png)$/i)
        );
    //Init Function Button
    $('#btnQueryAVA').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnAddAVA').button({
        label: "Add",
        icons: { primary: 'ui-icon-pencil' }
    });

    $('#btnDeleteAVA').button({
        label: "Delete",
        icons: { primary: 'ui-icon-trash' }
    });

    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/IsAddByVmiUser',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            if (!data.enableAddDeleteBtn) {
                $('#btnAddAVA').prop('disabled', true);
                $('#btnDeleteAVA').prop('disabled', true);
            }
            else {
                $('#btnAddAVA').prop('disabled', false);
                $('#btnDeleteAVA').prop('disabled', false);
            }
            if (!data.checkAddByVMIUser) {
                $('#cbAddByVMIUser').prop('checked', false);
            }
            else {
                $('#cbAddByVMIUser').prop('checked', true);
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
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
    $('#gridAVA').jqGrid({
        url: __WebAppPathPrefix + '/VMIProcess/GetAVAList',
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
            'Company',
            'Site',
            'BU Vendor Code',
            'A Code',
            'G Code',
            'Status',
            'Issuer',
            'Issue Date',
            'Close Date',
            'NVA_ID'
        ],
        colModel: [
            { name: "COMPANY_NAME", index: "Company", width: 285, sortable: false, classes: "jqGridColumnDataAsLinkWithBlue" },
            { name: "SITE_NAME", index: "Site", width: 105, sortable: false },
            { name: "BU_VENDOR_CODE", index: "BU Vendor Code", width: 110, sortable: false },
            { name: "A_CODE", index: "A Code", width: 50, sortable: false },
            { name: "G_CODE", index: "G Code", width: 50, sortable: false },
            { name: "TEXT", index: "Status", width: 100, sortable: false },
            { name: "NAME", index: "Issuer", width: 120, sortable: false },
            { name: "CREATE_DATE", index: "Issue Date", width: 105, sortable: false },
            { name: "CLOSE_DATE", index: "Close Date", width: 105, sortable: false },
            { name: "NVA_ID", index: "NVA_ID", width: 285, sortable: false, hidden: true },
        ],
        pager: '#gridAVAListPager',
        viewrecords: true,
        shrinkToFit: false,
        scrollrows: true,
        width: 1100,
        height: 240,
        rowNum: 10,
        loadonce: true,
        multiselect: true,
        hoverrows: false,
        // to save selection state
        onSelectAll: function (rowIds, status) {
            if (status === true) {
                for (var i = 0; i < rowIds.length; i++) {
                    if ($('#jqg_gridAVA_' + rowIds[i]).css('display') != 'none')
                        selectedRows[rowIds[i]] = true;
                    else
                        $('#gridAVA [role=row]#' + rowIds[i]).removeClass('ui-state-highlight');
                }
            } else {
                for (var i = 0; i < rowIds.length; i++) {
                    if ($('#jqg_gridAVA_' + rowIds[i]).css('display') != 'none')
                        delete selectedRows[rowIds[i]];
                }
            }
        },
        onSelectRow: function (rowId, status, e) {
            if ($(e.target).attr('role') == 'checkbox') {
                if ($('#jqg_gridAVA_' + rowId).css('display') != 'none') {
                    if (status === false) {
                        delete selectedRows[rowId];
                    } else {
                        selectedRows[rowId] = status;
                    }
                }
                else {
                    $('#gridAVA [role=row]#' + rowId).removeClass('ui-state-highlight');
                }
            }
            else {
                $('#gridAVA').setSelection(rowId, false);
            }
        },
        gridComplete: function () {
            //for (var rowId in selectedRows) {
            //    $('#gridAVA').setSelection(rowId, true);
            //}

            var ids = $("#gridAVA").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var rowId = ids[i];
                var Status = $("#gridAVA").jqGrid('getCell', ids[i], 'Status');
                var TEXT = $("#gridAVA").jqGrid('getCell', ids[i], 'TEXT');
                if (TEXT != 'Downloaded' && TEXT != 'New') {
                    $('#jqg_gridAVA_' + ids[i]).css('display', 'none');
                }
            }
        },
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            if (iCol == 1) {
                $('#dialogAVAManage').prop('NVA_ID', $(this).jqGrid('getCell', rowid, 'NVA_ID'));
                $('#dialogAVAManage').prop('STATUS_ID', $(this).jqGrid('getCell', rowid, 'TEXT'));
                $('#dialogAVAManage').dialog('open');
            }
        }
    });
    $('#gridAVA').jqGrid('navGrid', '#gridAVAListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#gridAccountList').jqGrid({
        url: __WebAppPathPrefix + '/VMIProcess/GetAccountList',
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
            'A Code',
            'Group Name',
            'Account',
            'Company Name',
            'Name',
            'QQ',
            'EMail',
            'Is Locked Out',
            'Last Locked Out Date'
        ],
        colModel: [
            { name: "GROUP_ID", index: "A Code", width: 55, sortable: false },
            { name: "GROUP_NAME", index: "Group Name", width: 80, sortable: false },
            { name: "ACCOUNT", index: "Account", width: 70, sortable: false },
            { name: "COMPANY_NAME", index: "Company Name", width: 110, sortable: false },
            { name: "NAME", index: "Name", width: 90, sortable: false },
            { name: "QQ", index: "QQ", width: 80, sortable: false },
            { name: "EMAIL", index: "EMail", width: 180, sortable: false },
            { name: "IS_LOCKED_OUT", formatter: 'checkbox', index: "Is Locked Out", width: 85, sortable: false, align: 'center' },
            { name: "LAST_LOCKED_OUT_DATE", index: "Last Locked Out Date", width: 130, sortable: false }
        ],
        pager: '#gridAccountListPager',
        viewrecords: true,
        shrinkToFit: false,
        scrollrows: true,
        width: 950,
        height: 150,
        rowNum: 9999,
        loadonce: true,
        hoverrows: false,
        gridComplete: function () {
            $('#gbox_gridAccountList.ui-jqgrid tr.jqgrow td').css('white-space', 'normal');
        }
    });
    $('#gridAccountList').jqGrid('navGrid', '#gridAccountListPager', { edit: false, add: false, del: false, search: false, refresh: false });
});