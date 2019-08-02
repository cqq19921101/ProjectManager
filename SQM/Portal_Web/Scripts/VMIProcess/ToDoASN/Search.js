$(function () {
    var diaSBUVendor = $('#dialog_VMI_QuerySBUVendor');
    var diaPlant = $('#dialog_VMI_QueryPlantInfo');
    var diaConfirm = $('#dialog_VMI_Confirm');
    var VMI_ToDoASNgridDataList = $('#VMI_Process_ToDoASN_gridDataList');

    $('#btn_VMIProcess_ToDoASN_Query').click(function () {
        $(this).removeClass('ui-state-focus');
        checkDateTimeIsEmpty();
    });

    $('#btnVMIProcess_ToDoASN_SBU_VDN_Query').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txt_VMIProcess_ToDoASN_SBU_VDN';

            InitdialogSBUVendor();
            ReloadDiaSBUVendorCodegridDataList();

            diaSBUVendor.show();
            diaSBUVendor.dialog("open");
            // div with class ui-dialog
            $('.ui-dialog :button').blur();
        }
    });

    $('#btn_VMIProcess_ToDoASN_Plant_Query').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txt_VMIProcess_ToDoASN_Plant';

            InitdialogPlant();
            ReloadDiaPlantCodegridDataList();

            diaPlant.show();
            diaPlant.dialog("open");
            // div with class ui-dialog
            $('.ui-dialog :button').blur();
        }
    });

    $('#btn_VMIProcess_ToDoASN_Plant_Query2').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txt_VMIProcess_ToDoASN_Plant2';

            InitdialogPlant();
            ReloadDiaPlantCodegridDataList();

            diaPlant.show();
            diaPlant.dialog("open");
            // div with class ui-dialog
            $('.ui-dialog :button').blur();
        }
    });

    $('#btn_VMIProcess_ToDoASN_SBU_VDN_Query2').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txt_VMIProcess_ToDoASN_SBU_VDN2';

            InitdialogSBUVendor();
            ReloadDiaSBUVendorCodegridDataList();

            diaSBUVendor.show();
            diaSBUVendor.dialog("open");
            // div with class ui-dialog
            $('.ui-dialog :button').blur();
        }
    });

    $('#btn_VMIProcess_ToDoASNImportDetail_Query').click(function () {
        $(this).removeClass('ui-state-focus');
        var diaToDoASNImportDetail = $('#dialog_VMIProcess_ToDoASNImportDetail');
        diaToDoASNImportDetail.attr('Mode', 'B');
        if ($("input:radio[name=ImportDetail_QueryType]:checked").val() == "M") {
            if ($.trim($("#txt_VMIProcess_ToDoASNImportDetail_Material").val()) == "") {
                alert("Please Enter Material.");
            }
            else {
                ReloadToDoASNImportDetailgridDataList();
            }
        }
        else {
            ReloadToDoASNImportDetailgridDataList();
        }
    });

    $('#btn_VMIProcess_ToDoASNImportDetail_AdvanceQuery').click(function () {
        $(this).removeClass('ui-state-focus');
        var diaToDoASNImportDetail = $('#dialog_VMIProcess_ToDoASNImportDetail');
        diaToDoASNImportDetail.attr('Mode', 'R');
        if ($("input:radio[name=ImportDetail_QueryType]:checked").val() == "M") {
            if ($.trim($("#txt_VMIProcess_ToDoASNImportDetail_Material").val()) == "") {
                alert("Please Enter Material.");
            }
            else {
                $("#dialog_VMI_Confirm").dialog({
                    modal: true,
                    autoOpen: false,
                    resizable: false,
                    width: 370,
                    height: 270,
                    buttons: {
                        Yes: function () {
                            ReloadToDoASNImportDetailgridDataList();
                            $(this).dialog("close");
                        },
                        Cancel: function () {
                            $(this).dialog("close");
                        }
                    },
                    open: function () {
                        $(this).parents().find('.ui-dialog-buttonpane button:contains("Yes")').blur();
                        $(this).parents().find('.ui-dialog-buttonpane button:contains("Cancel")').focus();
                    }
                });
                $("#dialog_VMI_Confirm_Content").html("使用Realtime Query功能查詢即時資料時，<br>當後端系統服務繁忙時所需時間較久；<br>若非需要取得當日PO資料，<br>建議請使用Fast Query功能查詢即可.<br><br>請確認是否仍要執行Realtime Query功能<br>(需時較久)?");
                diaConfirm.show();
                diaConfirm.dialog("open");
            }
        }
        else { 
            $("#dialog_VMI_Confirm").dialog({
                modal: true,
                autoOpen: false,
                resizable: false,
                width: 370,
                height: 270,
                buttons: {
                    Yes: function () {
                        ReloadToDoASNImportDetailgridDataList();
                        $(this).dialog("close");
                    },
                    Cancel: function () {
                        $(this).dialog("close");
                    }
                },
                open: function () {
                    $(this).parents().find('.ui-dialog-buttonpane button:contains("Yes")').blur();
                    $(this).parents().find('.ui-dialog-buttonpane button:contains("Cancel")').focus();
                }
            });
           
            $("#dialog_VMI_Confirm_Content").html("使用Realtime Query功能查詢即時資料時，<br>當後端系統服務繁忙時所需時間較久；<br>若非需要取得當日PO資料，<br>建議請使用Fast Query功能查詢即可.<br><br>請確認是否仍要執行Realtime Query功能<br>(需時較久)?");
            diaConfirm.show();
            diaConfirm.dialog("open");
        }
    });

    $('#dia_btn_VMIProcess_ToDoASNFileDetail_ViewLogs').click(function () {
        $(this).removeClass('ui-state-focus');
        $('#dialog_VMIProcess_ToDoASNFileViewLogs').dialog('open');
        var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');
        var VMI_Process_ToDoASNFileViewLogs_gridDataList = $('#VMI_Process_ToDoASNFileViewLogs_gridDataList');

        VMI_Process_ToDoASNFileViewLogs_gridDataList.jqGrid('clearGridData');
        VMI_Process_ToDoASNFileViewLogs_gridDataList.jqGrid('setGridParam', { postData: { ASN_NUM: escape($.trim(diaToDoASNManage.prop("ASN_NUM"))) } });
        VMI_Process_ToDoASNFileViewLogs_gridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
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
        ReloadToDoASNgridDataList();
    }
}

function ReloadToDoASNgridDataList() {
    var VMI_ToDoASNgridDataList = $('#VMI_Process_ToDoASN_gridDataList');

    VMI_ToDoASNgridDataList.jqGrid('clearGridData');
    VMI_ToDoASNgridDataList.jqGrid('setGridParam', {
        postData: {
            ASNNoFM: escape($.trim($("#txt_VMIProcess_ToDoASN_ASNNoFM").val()))
            , ASNNoTO: escape($.trim($("#txt_VMIProcess_ToDoASN_ASNNoTO").val()))
            , Plant: escape($.trim($("#txt_VMIProcess_ToDoASN_Plant2").val()))
            , VendorCode: escape($.trim($("#txt_VMIProcess_ToDoASN_SBU_VDN2").val()))
            , ASNDateFM: escape($.trim($("#dpASNDate_FM").val()))
            , ASNDateTO: escape($.trim($("#dpASNDate_TO").val()))
            , Status: escape($.trim($("#dropbox_VMIProcess_ToDoASN_Status").val()))
            , TradeType: escape($.trim($("#dropbox_VMIProcess_ToDoASN_TradeType").val()))
            , DataType: escape($.trim($("input:radio[name=QueryType]:checked").val()))
            , UserID: escape($.trim($("#txt_VMIProcess_ToDoASN_UserID").val()))
        }
    });
    if ($("input:radio[name=QueryType]:checked").val() == "W") {
        VMI_ToDoASNgridDataList.jqGrid('hideCol', VMI_ToDoASNgridDataList.getGridParam("colModel")[1].name);
    }
    else {
        VMI_ToDoASNgridDataList.jqGrid('showCol', VMI_ToDoASNgridDataList.getGridParam("colModel")[1].name);
    }
    VMI_ToDoASNgridDataList.jqGrid('setGridParam', { datatype: 'json', width: 650 }).trigger('reloadGrid');
}

function ReloadToDoASNManagegridDataList() {
    var VMI_ToDoASNManagegridDataList = $('#VMI_Process_ToDoASNManage_gridDataList');
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');
    VMI_ToDoASNManagegridDataList.jqGrid('clearGridData');
    VMI_ToDoASNManagegridDataList.jqGrid('setGridParam', { postData: { ASN_NUM: escape($.trim(diaToDoASNManage.prop("ASN_NUM"))) } });
    VMI_ToDoASNManagegridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}

function ReloadToDoASNImportDetailgridDataList() {
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');
    var VMI_ToDoASNImportDetailgridDataList = $('#VMI_Process_ToDoASNImportDetail_gridDataList');
    var diaToDoASNImportDetail = $('#dialog_VMIProcess_ToDoASNImportDetail');
    VMI_ToDoASNImportDetailgridDataList.jqGrid('clearGridData');

    $("#btn_VMIProcess_ToDoASNImportDetail_Query").attr("disabled", true);
    $("#btn_VMIProcess_ToDoASNImportDetail_AdvanceQuery").attr("disabled", true);

    if ((diaToDoASNImportDetail.attr('Mode') == "B")) {
        VMI_ToDoASNImportDetailgridDataList.jqGrid('hideCol', ["PSTYP", "KEEPER", "ZCUSTOMSCHK", "ZFREEPO", "UOM", "UOM_RATE"]);
    }
    else {
        VMI_ToDoASNImportDetailgridDataList.jqGrid('showCol', ["PSTYP", "KEEPER"]).jqGrid('hideCol', ["ZCUSTOMSCHK", "ZFREEPO", "UOM", "UOM_RATE"]);
    }

    VMI_ToDoASNImportDetailgridDataList.jqGrid('setGridParam', {
        url: __WebAppPathPrefix + ((diaToDoASNImportDetail.attr('Mode') == "B") ? "/VMIProcess/QueryToDoASNImportDetailByBatchInfo" : "/VMIProcess/QueryToDoASNImportDetailByRealTimeInfo"),
        postData: {
            PLANT: escape($.trim(diaToDoASNManage.attr("PLANT"))),
            VENDOR: escape($.trim(diaToDoASNManage.attr("VENDOR"))),
            ETA: escape($.trim(diaToDoASNManage.attr("ETA"))),
            MATERIALS: escape($.trim($("#txt_VMIProcess_ToDoASNImportDetail_Material").val())),
            QueryType: escape($.trim($("input:radio[name=ImportDetail_QueryType]:checked").val()))
        },
        //loadonce: ((diaToDoASNImportDetail.attr('Mode') == "B") ? false : true)
    });
    VMI_ToDoASNImportDetailgridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}

function ReloadToDoASNImportDetailShowASNQtygridDataList() {
    var diaToDoASNImportDetailShowASNQty = $('#dialog_VMIProcess_ToDoASNImportDetailShowASNQty');
    var VMI_ToDoASNImportDetailShowASNQtygridDataList = $('#VMI_Process_ToDoASNImportDetailShowASNQty_gridDataList');

    VMI_ToDoASNImportDetailShowASNQtygridDataList.jqGrid('clearGridData');
    VMI_ToDoASNImportDetailShowASNQtygridDataList.jqGrid('setGridParam', {
        postData: {
            PO_NUM: escape($.trim(diaToDoASNImportDetailShowASNQty.attr("PONUM"))),
            PO_LINE: escape($.trim(diaToDoASNImportDetailShowASNQty.attr("POLINE")))
        }
    });
    VMI_ToDoASNImportDetailShowASNQtygridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}

function ReloadToDoASNFileDetailInfogridDataList() {
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');
    var VMI_ToDoASNFileDetailgridDataList = $('#VMI_Process_ToDoASNFileDetail_gridDataList');

    VMI_ToDoASNFileDetailgridDataList.jqGrid('clearGridData');
    VMI_ToDoASNFileDetailgridDataList.jqGrid('setGridParam', { postData: { ASN_NUM: escape($.trim(diaToDoASNManage.prop("ASN_NUM"))) } });
    VMI_ToDoASNFileDetailgridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid')
}
