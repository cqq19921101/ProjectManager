$(function () {
    //Init Function Button
    $('#btnQueryPOAck').button({
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
    $('#gridQueryPOAck').jqGrid({
        url: __WebAppPathPrefix + '/VMIQuery/QueryPOAckList',
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
             'PO Number'
            , 'PO Ver.'
            , 'Type'
            , 'Plant'
            , 'Buyer Code'
            , 'Vendor Code'
            , 'Release Date'
            , 'Ack User'
            , 'Ack Date'
            , 'File'
            , 'MODDATE_VAL'
        ],
        colModel: [
            { name: 'PO_NUMBER', index: 'PO Number', sortable: false, width: 85, align: 'left' },
            { name: 'PO_VERSION', index: 'PO Ver.', sortable: false, width: 55, align: 'left' },
            { name: 'Type', index: 'Type', sortable: false, width: 95, align: 'left' },
            { name: 'PLANT', index: 'Plant', sortable: false, width: 50, align: 'left' },
            { name: 'PUR_GROUP', index: 'Buyer Code', sortable: false, width: 80, align: 'left' },
            { name: 'VENDOR', index: 'Vendor Code', sortable: false, width: 80, align: 'left' },
            { name: 'MODDATE', index: 'Release Date', sortable: false, width: 120, align: 'left' },
            { name: 'NAME', index: 'Ack User', sortable: false, width: 120, align: 'left' },
            { name: 'CONFIRM_TIME', index: 'Ack Date', sortable: false, width: 120, align: 'left' },
            {
                name: '', index: 'File', sortable: false, width: 120, align: 'left',
                formatter: function (cellvalue, options, rowObject) {
                    return "<div class='jqGridColumnDataAsLinkWithBlue' id='File_" + options.rowId + "' value='" + options.rowId + "'>" + rowObject.PO_NUMBER + '_' + rowObject.PO_VERSION + '.pdf' + "</div>";
                }
            },
            { name: 'MODDATE_VAL', index: 'Release Date', sortable: false, width: 160, hidden: true }
        ],
        pager: '#gridQueryPOAckListPager',
        viewrecords: true,
        shrinkToFit: false,
        scrollrows: true,
        width: 975,
        height: 232,
        rowNum: 10,
        hoverrows: false,
        loadonce: true,
        loadComplete: function () {
            $('div[id^="File_"]').unbind('click');
            $('div[id^="File_"]').click(function () {
                var rowId = $(this).attr('value'),
                    PO_NUMBER = $('#gridQueryPOAck').jqGrid('getCell', rowId, 'PO_NUMBER'),
                    PO_VERSION = $('#gridQueryPOAck').jqGrid('getCell', rowId, 'PO_VERSION'),
                    MODDATE = $('#gridQueryPOAck').jqGrid('getCell', rowId, 'MODDATE_VAL');

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
        },
        onCellSelect: function (rowid, iCol, cellcontent, e) {
        }
    });
    $('#gridQueryPOAck').jqGrid('navGrid', '#gridQueryPOAckListPager', { edit: false, add: false, del: false, search: false, refresh: false });
})