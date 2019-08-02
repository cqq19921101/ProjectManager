$(function () {
    $.ajax({
        url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '<option value=-1 Selected>All</option>';
            for (var idx in data) {
                options += '<option value=' + data[idx].CategoryID + '>' + data[idx].CategoryTitle + '</option>';
            }
            $('#ddlCategory').append(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });

    $('#listRefAndDowload').jqGrid({
        url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinList',
        mtype: 'POST',
        datatype: "local",
        jsonReader: {
            root: "colData",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false,
        },
        colNames: ['ID', 'Category', 'Publish Date', 'Subject'],
        colModel: [
            { name: "ID", sortable: false, width: 100, hidden: true },
            { name: "CategoryTitle", sortable: false, width: 100 },
            { name: "PublishTime", sortable: false, width: 80 },
            { name: "Subject", sortable: false, width: 280, classes: "jqGridColumnDataAsLinkWithBlue" }
        ],
        pager: '#pagerRefAndDowload',
        viewrecords: true,
        shrinkToFit: false,
        scrollrows: true,
        width: 498,
        height: 232,
        loadonce: false,
        hoverrows: false,
        loadComplete: function () {
        },
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            $this = $(this);
            if (iCol == 3) {
                var dialogRefAndDownload = $('#dialogRefAndDownload');
                dialogRefAndDownload.prop('ID', $this.jqGrid('getCell', rowid, 'ID'));
                dialogRefAndDownload.prop('Category', $this.jqGrid('getCell', rowid, 'CategoryTitle'));
                dialogRefAndDownload.prop('PublishDate', $this.jqGrid('getCell', rowid, 'PublishTime'));
                dialogRefAndDownload.prop('Subject', $this.jqGrid('getCell', rowid, 'Subject'));
                dialogRefAndDownload.dialog('open');

                // Remove auto focus
                $('.ui-dialog :button').blur();
                $.ui.dialog.prototype._keepFocus=$.noop
            }
        }
    });
    // Clear jqGrid content
    $('#listRefAndDowload').jqGrid('clearGridData');
    // Sent params to jqGrid and reload it
    $('#listRefAndDowload').jqGrid('setGridParam', {
        postData: {
            CategoryID: escape($(this).find('option:selected').val())
        }
    });
    $('#listRefAndDowload').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');

    //Check PO Ack Info
    $.ajax({
        url: __WebAppPathPrefix + '/VMIBulletin/CheckPOAckInfo',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            if (data.ShowPOAckInfo) {
                if (data.POCnt != 0) {
                    $('#POAckInfo #POAck').show();
                    $('#POAckInfo #POAck #POAckNum').text(data.POCnt);
                }
                else {
                    $('#POAckInfo #NonPOAck').show();
                }
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
});