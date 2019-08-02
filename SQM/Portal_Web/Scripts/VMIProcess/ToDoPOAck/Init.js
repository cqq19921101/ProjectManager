$(function () {
    //Init Function Button
    $('#btnQueryToDoPOAck').button({
        label: "Query",
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQueryPlantDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQueryVendorCodeDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQueryBuyerCodeDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    // Query character to upper
    $('input#txtPlant').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });
    $('input#txtVendorCode').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });
    $('input#txtBuyerCode').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });

    $("#txtReleaseDateFrom").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtReleaseDateFrom").datepicker("setDate", '-31d');

    $("#txtReleaseDateTo").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            }
            catch (err) {
                $(this).datepicker("setDate", '0d');
            }
        }
    });
    $("#txtReleaseDateTo").datepicker("setDate", '0d');

    // Init jqGrid
    $('#gridToDoPOAck').jqGrid({
        url: __WebAppPathPrefix + '/VMIProcess/QueryToDoPOAck',
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
            'Ack All'
            , 'PO Number'
            , 'PO Ver.'
            , 'Type'
            , 'Plant'
            , 'Buyer Code'
            , 'Vendor Code'
            , 'Release Date'
            , 'File'
            , 'MODDATE_VAL'
        ],
        colModel: [
            {
                name: '', index: 'Ack All', sortable: false, width: 55,
                formatter: function (cellvalue, options, rowObject) {
                    return "<div class='jqGridColumnDataAsLinkWithBlue' style='margin-left:6px;' id='Ack_" + options.rowId + "' value='" + options.rowId + "'>Ack</div>";
                }
            },
            { name: 'PO_NUMBER', index: 'PO Number', sortable: false, width: 85, align: 'left' },
            { name: 'PO_VERSION', index: 'PO Ver.', sortable: false, width: 55, align: 'left' },
            { name: 'Type', index: 'Type', sortable: false, width: 95, align: 'left' },
            { name: 'PLANT', index: 'Plant', sortable: false, width: 50, align: 'left' },
            { name: 'PUR_GROUP', index: 'Buyer Code', sortable: false, width: 80, align: 'left' },
            { name: 'VENDOR', index: 'Vendor Code', sortable: false, width: 80, align: 'left' },
            { name: 'MODDATE', index: 'Release Date', sortable: false, width: 120, align: 'left' },
            {
                name: '', index: 'File', sortable: false, width: 120, align: 'left',
                formatter: function (cellvalue, options, rowObject) {
                    return "<div class='jqGridColumnDataAsLinkWithBlue' id='File_" + options.rowId + "' value='" + options.rowId + "'>" + rowObject.PO_NUMBER + '_' + rowObject.PO_VERSION + '.pdf' + "</div>";
                }
            },
            { name: 'MODDATE_VAL', index: 'Release Date', sortable: false, width: 160, hidden: true }
        ],
        pager: '#gridToDoPOAckListPager',
        viewrecords: true,
        shrinkToFit: false,
        scrollrows: true,
        width: 785,
        height: 232,
        rowNum: 10,
        hoverrows: false,
        loadonce: true,
        loadComplete: function () {
            $('div[id^="Ack_"]').unbind('click');
            $('div[id^="Ack_"]').click(function () {
                var rowId = $(this).attr('value'),
                    PO_NUMBER = $('#gridToDoPOAck').jqGrid('getCell', rowId, 'PO_NUMBER'),
                    PO_VERSION = $('#gridToDoPOAck').jqGrid('getCell', rowId, 'PO_VERSION'),
                    MODDATE = $('#gridToDoPOAck').jqGrid('getCell', rowId, 'MODDATE_VAL');

                if (confirm('Are you sure you want to Ack this\r\nPO Number: ' + PO_NUMBER + '\r\nPO Version: ' + PO_VERSION + '')) {

                    var POInfo = [{
                        PO_NUMBER: escape($.trim(PO_NUMBER)),
                        PO_VERSION: escape($.trim(PO_VERSION)),
                        MODDATE_VAL: escape($.trim(MODDATE))
                    }];
                    $.ajax({
                        url: __WebAppPathPrefix + '/VMIProcess/AckPO',
                        data: {
                            POInfo: POInfo
                        },
                        type: "post",
                        dataType: 'text',
                        async: false, // if need page refresh, please remark this option
                        success: function (data) {
                            alert(data);
                            reloadToDoPOAckGridList();
                        },
                        error: function (xhr, textStatus, thrownError) {
                            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                        },
                        complete: function (jqXHR, textStatus) {
                        }
                    });
                }
            });

            $('div[id^="File_"]').unbind('click');
            $('div[id^="File_"]').click(function () {
                var rowId = $(this).attr('value'),
                    PO_NUMBER = $('#gridToDoPOAck').jqGrid('getCell', rowId, 'PO_NUMBER'),
                    PO_VERSION = $('#gridToDoPOAck').jqGrid('getCell', rowId, 'PO_VERSION'),
                    MODDATE = $('#gridToDoPOAck').jqGrid('getCell', rowId, 'MODDATE_VAL');

                $.ajax({
                    url: __WebAppPathPrefix + '/VMIProcess/DownloadPOFile',
                    data: {
                        PO_NUMBER: escape($.trim(PO_NUMBER)),
                        PO_VERSION: escape($.trim(PO_VERSION)),
                        MODDATE: escape($.trim(MODDATE))
                    },
                    type: "post",
                    dataType: 'json',
                    success: function (data) {
                        if (data.Result) {
                            if (data.FileKey != "") {
                                $("#dialogDownloadSplash_FileKey").val(data.FileKey);
                                $("#dialogDownloadSplash_FileName").val(data.FileName);

                                setTimeout(function () {
                                    $("#dialogDownloadSplash_Form").attr('action', __WebAppPathPrefix + '/VMIProcess/RetrieveFileByFileKey').submit();
                                }, 10);
                            }
                        }
                        else
                            alert("Export failure. Please contact administrator manager.");
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    }
                });
            });

            $('#jqgh_gridToDoPOAck_').unbind('click');
            $('#jqgh_gridToDoPOAck_').addClass('jqGridColumnDataAsLinkWithBlue').click(function () {
                var POInfo = $('#gridToDoPOAck').jqGrid('getGridParam', 'data');
                if (POInfo.length == 0) {
                    alert('Please query To Do PO first.');
                    return;
                }
                if (confirm('Are you sure you want to Ack all PO (Total Count:' + POInfo.length + ')?')) {
                    $.ajax({
                        url: __WebAppPathPrefix + '/VMIProcess/AckPO',
                        data: {
                            POInfo: POInfo
                        },
                        type: "post",
                        dataType: 'text',
                        async: false, // if need page refresh, please remark this option
                        success: function (data) {
                            alert(data);
                            reloadToDoPOAckGridList();
                        },
                        error: function (xhr, textStatus, thrownError) {
                            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                        },
                        complete: function (jqXHR, textStatus) {
                        }
                    });
                }
            });
        },
        onCellSelect: function (rowid, iCol, cellcontent, e) {
        }
    });
    $('#gridToDoPOAck').jqGrid('navGrid', '#gridToDoPOAckListPager', { edit: false, add: false, del: false, search: false, refresh: false });
})