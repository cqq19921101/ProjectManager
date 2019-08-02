function VDSToolBar() {
    // Init Toolbar
    var tbTopToolBarQueryVDSManage = $('#tbTopToolBarQueryVDSManage');
    var tabsVDS = $('#tabsQueryVDSManage');
    var dialogQueryVDSManage = $('#dialogQueryVDSManage');
    var queryType = $("input[name='queryType']:checked").val();

    tbTopToolBarQueryVDSManage.show();

    $('#btnExportAll').click(function () {
        VDSExportVDS(queryType, 'ExportAll', dialogQueryVDSManage);
        $(this).blur();
    });

    $('#btnExportVDS').click(function () {
        VDSExportVDS(queryType, 'ExportVDS', dialogQueryVDSManage);
        $(this).blur();
    });
}

function VDSExportVDS(queryType, Status, dialogQueryVDSManage) {
    var PLANT = $.trim(dialogQueryVDSManage.prop('PLANT'));
    var VENDOR = $.trim(dialogQueryVDSManage.prop('VENDOR'));
    var BUYER = $.trim(dialogQueryVDSManage.prop('BUYER'));
    var VDS_NUM, VRSIO, TMType;
    if (queryType == 'history') {
        VDS_NUM = $.trim(dialogQueryVDSManage.prop('VDS_NUM'));
        VRSIO = $.trim(dialogQueryVDSManage.prop('VRSIO'));
        TMType = $.trim(dialogQueryVDSManage.prop('TMTYPE'));
    }
    $.ajax({
        url: __WebAppPathPrefix + '/VMIQuery/QueryVDSExportVDS',
        data: {
            QueryType: queryType,
            Status: Status,
            Plant: PLANT,
            VendorCode: VENDOR,
            BuyerCode: BUYER,
            VDS_NUM: VDS_NUM,
            VRSIO: VRSIO,
            TMType: TMType
        },
        type: "post",
        dataType: 'json',
        //async: false,
        beforeSend: function () {
            $("#dialogDownloadSplash").dialog({
                title: 'Download Notify',
                width: 'auto',
                height: 'auto',
                open: function (event, ui) {
                    $(this).parent().find('.ui-dialog-titlebar-close').hide();
                    $(this).parent().find('.ui-dialog-buttonpane').hide();
                    $("#lbDiaDownloadMsg").html('Downloading...</br></br>Please wait for the operation a moment...');
                }
            }).dialog("open");
        },
        success: function (data) {
            if (data.Result) {
                if (data.FileKey != "") {
                    $("#dialogDownloadSplash_FileKey").val(data.FileKey);
                    $("#dialogDownloadSplash_FileName").val(data.FileName);

                    setTimeout(function () {
                        $("#dialogDownloadSplash_Form").attr('action', __WebAppPathPrefix + '/VMIQuery/RetrieveFileByFileKey').submit();
                        $("#dialogDownloadSplash").dialog("close");
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
}