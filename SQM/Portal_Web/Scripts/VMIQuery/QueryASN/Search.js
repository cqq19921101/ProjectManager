$(function () {
    var diaSBUVendor = $('#dialog_VMI_QuerySBUVendor');
    var diaPlant = $('#dialog_VMI_QueryPlantInfo');
    var diaASNFileDetail = $('#dialog_VMIQuery_ASNFileInfo');
    var diaASNModifyFileDetail = $('#dialog_VMIQuery_ModifyASNFileDetail');
    var diaQueryASNAttach = $('#dialogQueryASNFileAttach');
    var diaModifyASNAttach = $('#dialogModifyASNFileAttach');

    $('#btn_VMIQuery_QueryASN_Query').click(function () {
        $(this).removeClass('ui-state-focus');
        checkDateTimeIsEmpty();
    });

    $('#btn_VMIQuery_QueryASN_SBU_VDN_Query').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txt_VMIQuery_QueryASN_SBU_VDN';

            InitdialogSBUVendor();
            ReloadDiaSBUVendorCodegridDataList();

            diaSBUVendor.show();
            diaSBUVendor.dialog("open");
            // div with class ui-dialog
            $('.ui-dialog :button').blur();
        }
    });

    $('#btn_VMIQuery_QueryASN_Plant_Query').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txt_VMIQuery_QueryASN_Plant';

            InitdialogPlant();
            ReloadDiaPlantCodegridDataList();

            diaPlant.show();
            diaPlant.dialog("open");
            // div with class ui-dialog
            $('.ui-dialog :button').blur();

        }
    });

    $('#dia_btn_VMIQuery_QueryASNManage_ExcelExport').click(function () {
        $(this).removeClass('ui-state-focus');
        var diaQueryASNManage = $('#dialog_VMIQuery_QueryASNManage');

        $.ajax({
            url: __WebAppPathPrefix + '/VMIQuery/QueryASNInfoToExcel',
            data: {
                ASNNUM: escape($.trim(diaQueryASNManage.prop("ASN_NUM")))
            },
            type: "post",
            dataType: 'json',
            async: false,
            success: function (data) {
                if (data.Result) {
                    if (data.FileKey != "") {
                        $("#dialogDownloadSplash_FileKey").val(data.FileKey);
                        $("#dialogDownloadSplash_FileName").val(data.FileName);
                        setTimeout(function () {
                            $("#dialogDownloadSplash_Form").attr('action', __WebAppPathPrefix + '/VMIProcess/RetrieveFileByFileKey').submit();
                            $("#dialogDownloadSplash").dialog("close");
                        }, 10);
                    }
                }
                else
                    alert("ASN沒有明細資料，所以無法下載!請使用空白ASN進行上傳。");
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            }
        });
    });

    $('#dia_btn_VMIQuery_QueryASNManage_EditShipInfo').click(function () {
        $(this).removeClass('ui-state-focus');
        InitdialogToDoASNHeaderForEditShippingInfo();
        //AuthAttachFileExcuteIsPass()
    });

    $('#dia_btn_VMIQuery_QueryASNManage_FileExport').click(function () {
        $(this).removeClass('ui-state-focus');
        AuthAttachFileExcuteIsPass()   
    });

    $('#dialog_span_VMIQuery_ASNFileInfo_Download').click(function () {
        $(this).removeClass('ui-state-focus');
        window.location = __WebAppPathPrefix + "/VMIProcess/DownloadToDoASNAttachFile?DataKey=" + escape($.trim(diaQueryASNAttach.attr('FS_GUID')));
    });

    $('#dialog_span_VMIQuery_ModifyASNFileAttach_Download').click(function () {
        $(this).removeClass('ui-state-focus');
        window.location = __WebAppPathPrefix + "/VMIProcess/DownloadToDoASNAttachFile?DataKey=" + escape($.trim(diaModifyASNAttach.attr('FS_GUID')));
    });

    $('#dia_btn_VMIQuery_ModifyASNFileDetail_ViewLogs, #dia_btn_VMIQuery_ASNFileInfo_ViewLogs').click(function () {
        $(this).removeClass('ui-state-focus');
        $('#dialog_VMIQuery_ASNFileViewLogs').dialog('open');
        var diaQueryASNManage = $('#dialog_VMIQuery_QueryASNManage');
        var VMI_Query_ASNFileViewLogs_gridDataList = $('#VMI_Query_ASNFileViewLogs_gridDataList');

        VMI_Query_ASNFileViewLogs_gridDataList.jqGrid('clearGridData');
        VMI_Query_ASNFileViewLogs_gridDataList.jqGrid('setGridParam', { postData: { ASN_NUM: escape($.trim(diaQueryASNManage.prop("ASN_NUM"))) } });
        VMI_Query_ASNFileViewLogs_gridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });

    $('#dia_btn_VMIQuery_QueryASNManage_EditImportDate').click(function () {
        $(this).removeClass('ui-state-focus');
        $('#dialog_VMIQuery_EditImportDate').dialog('open');
    });

    $('#dia_btn_VMIQuery_QueryASNManage_PrintImportedNoticeForm').click(function () {
        $(this).removeClass('ui-state-focus');
        var diaQueryASNManage = $('#dialog_VMIQuery_QueryASNManage');

        $.ajax({
            url: __WebAppPathPrefix + '/VMIQuery/PrintImportedNoticeForm',
            data: {
                ASNNUM: escape($.trim(diaQueryASNManage.prop("ASN_NUM")))
            },
            type: "post",
            dataType: 'json',
            async: false,
            success: function (data) {
                if (data.Result) {
                    if (data.FileKey != "") {
                        $("#dialogDownloadSplash_FileKey").val(data.FileKey);
                        $("#dialogDownloadSplash_FileName").val(data.FileName);
                        setTimeout(function () {
                            $("#dialogDownloadSplash_Form").attr('action', __WebAppPathPrefix + '/VMIProcess/RetrieveFileByFileKey').submit();
                            $("#dialogDownloadSplash").dialog("close");
                        }, 10);
                    }
                }
                else
                    alert(data.FileKey);
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            }
        });
    });
});

function checkDateTimeIsEmpty() {
    if ($.trim($("#dpASNDate_FM").val()) == "") {
        alert("Please input Date(FM)");
    }
    else if ($.trim($("#dpASNDate_TO").val()) == "") {
        alert("Please input Date(TO)");
    }
    else {
        ReloadQueryASNgridDataList();
    }
}

function ReloadQueryASNgridDataList() {
    var VMI_QueryASNgridDataList = $('#VMI_VMIQuery_QueryASN_gridDataList');

    VMI_QueryASNgridDataList.jqGrid('clearGridData');
    VMI_QueryASNgridDataList.jqGrid('setGridParam', {
        postData: {
            ASNNoFM: escape($.trim($("#txt_VMIQuery_QueryASN_ASNNoFM").val()))
            , ASNNoTO: escape($.trim($("#txt_VMIQuery_QueryASN_ASNNoTO").val()))
            , Plant: escape($.trim($("#txt_VMIQuery_QueryASN_Plant").val()))
            , VendorCode: escape($.trim($("#txt_VMIQuery_QueryASN_SBU_VDN").val()))
            , ASNDateFM: escape($.trim($("#dpASNDate_FM").val()))
            , ASNDateTO: escape($.trim($("#dpASNDate_TO").val()))
            , MaterialFM: escape($.trim($("#txt_VMIQuery_QueryASN_MaterialFM").val()))
            , MaterialTO: escape($.trim($("#txt_VMIQuery_QueryASN_MaterialTO").val()))
            , Status: escape($.trim($("#dropbox_VMIQuery_QueryASN_Status").val()))
            , TradeType: escape($.trim($("#dropbox_VMIQuery_QueryASN_TradeType").val()))
            , DataType: escape($.trim($("input:radio[name=QueryType]:checked").val()))
            , UserID: escape($.trim($("#txt_VMIQuery_QueryASN_UserID").val()))
        }
    });
    if ($("input:radio[name=QueryType]:checked").val() == "W") {
        VMI_QueryASNgridDataList.jqGrid('hideCol', VMI_QueryASNgridDataList.getGridParam("colModel")[1].name);
    }
    else {
        VMI_QueryASNgridDataList.jqGrid('showCol', VMI_QueryASNgridDataList.getGridParam("colModel")[1].name);
    }
    VMI_QueryASNgridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}

function ReloadQueryASNManagegridDataList() {
    var VMI_QueryASNManagegridDataList = $('#VMI_Query_QueryASNManage_gridDataList');
    var diaQueryASNManage = $('#dialog_VMIQuery_QueryASNManage');

    VMI_QueryASNManagegridDataList.jqGrid('clearGridData');
    VMI_QueryASNManagegridDataList.jqGrid('setGridParam', { postData: { ASN_NUM: escape($.trim(diaQueryASNManage.prop("ASN_NUM"))) } });
    VMI_QueryASNManagegridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}

function ReloadASNFileDetailInfogridDataList() {
    var diaQueryASNManage = $('#dialog_VMIQuery_QueryASNManage');
    var VMI_ASNFileDetailgridDataList = $('#VMI_Query_ASNFileDetail_gridDataList');

    VMI_ASNFileDetailgridDataList.jqGrid('clearGridData');
    VMI_ASNFileDetailgridDataList.jqGrid('setGridParam', { postData: { ASN_NUM: escape($.trim(diaQueryASNManage.prop("ASN_NUM"))) } });
    VMI_ASNFileDetailgridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid')
}

function ReloadModifyASNFileDetailInfogridDataList() {
    var diaQueryASNManage = $('#dialog_VMIQuery_QueryASNManage');
    var VMI_ModifyASNFileDetailgridDataList = $('#VMI_Query_ModifyASNFileDetail_gridDataList');

    VMI_ModifyASNFileDetailgridDataList.jqGrid('clearGridData');
    VMI_ModifyASNFileDetailgridDataList.jqGrid('setGridParam', { postData: { ASN_NUM: escape($.trim(diaQueryASNManage.prop("ASN_NUM"))) } });
    VMI_ModifyASNFileDetailgridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid')
}

function AuthAttachFileExcuteIsPass() {
    var diaQueryASNManage = $('#dialog_VMIQuery_QueryASNManage');
    var diaASNFileDetail = $('#dialog_VMIQuery_ASNFileInfo');
    var diaASNModifyFileDetail = $('#dialog_VMIQuery_ModifyASNFileDetail');
    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/AuthAttachFileIsPass',
        data: {
            ASNNUM: escape($.trim(diaQueryASNManage.prop("ASN_NUM")))
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.Result) {
                ReloadModifyASNFileDetailInfogridDataList();
                diaASNModifyFileDetail.dialog("option", "title", "File Info").dialog("open");
                // div with class ui-dialog
                $('.ui-dialog :button').blur();
            }
            else {
                ReloadASNFileDetailInfogridDataList();
                diaASNFileDetail.dialog("option", "title", "File Info").dialog("open");
                // div with class ui-dialog
                $('.ui-dialog :button').blur();
            }          
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        }
    });
}


function InitdialogToDoASNHeaderForEditShippingInfo() {
    var diaQueryASNManage = $('#dialog_VMIQuery_QueryASNManage');
    var diaEditShipInfo = $('#dialog_QueryASN_EditShippingInfo');

    __DialogIsShownNow = false;
    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/QueryHeaderInfoForEditShipInfo',
        data: {
            ASNNUM: escape($.trim(diaQueryASNManage.prop("ASN_NUM")))
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.HASRESULT == true) {
                $('#dialog_span_QueryASN_EditShippingInfo_ASNNo').html(data.ASNNO);
                $('#dialog_span_QueryASN_EditShippingInfo_CreatedDate').html(data.ASNCREATEDATE);
                $('#dialog_span_QueryASN_EditShippingInfo_InboundDNNo').html(data.INBOUNDDNNO);
                $('#dialog_span_QueryASN_EditShippingInfo_Vendor').html(data.VENDOR);
                $('#dialog_span_QueryASN_EditShippingInfo_CustomerCode').html(data.CUSTOMERCODE);
                $('#dialog_span_QueryASN_EditShippingInfo_VendorDNNo').val(data.VENDORDNNO);
                $('#dialog_span_QueryASN_EditShippingInfo_CustomerName').html(data.CUSTOMERNAME);
                $('#dialog_span_QueryASN_EditShippingInfo_InvoiceNo').val(data.INVOICENO);
                $('#dialog_span_QueryASN_EditShippingInfo_PlantCode').html(data.PLANTCODE);
                $('#dialog_span_QueryASN_EditShippingInfo_TradeType').html(data.TRADETYPE);
                $('#span_dpQueryASN_ETDDate').html(data.ETD);
                $('#dpQueryASN_ETADate').datepicker('setDate', data.ETA);
                $('#dialog_txt_QueryASN_EditShippingInfo_ETATime').val(data.ETA_TIME == "00:00" ? "" : data.ETA_TIME);
                $('#dialog_span_QueryASN_EditShippingInfo_TransferDocNo').html(data.TRANSFERDOCNO);
                $('#dialog_txt_QueryASN_EditShippingInfo_VehicleTypeAndID').val(data.VEHICLETYPEID);
                $('#dialog_txt_QueryASN_EditShippingInfo_DriverName').val(data.DRIVERNAME);
                $('#dialog_txt_QueryASN_EditShippingInfo_DriverPhone').val(data.DRIVERPHONE);

                if (!__DialogIsShownNow) {
                    __DialogIsShownNow = true;
                    diaEditShipInfo.show();
                    diaEditShipInfo.dialog("open");
                }
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
            //HideAjaxLoading();
        }
    });
}

