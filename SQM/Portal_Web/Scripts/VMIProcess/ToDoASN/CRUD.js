$(function () {
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');
    var diaToDoASNHeader = $('#dialog_VMIProcess_ToDoASNHeader');
    var diaToDoASNDetail = $('#dialog_VMIProcess_ToDoASNDetail');
    var diaToDoASNFileDetail = $('#dialog_VMIProcess_ToDoASNFileDetail');
    var diaToDoASNOpenPOQtyError = $('#dialog_VMIProcess_ToDoASNOverOpenPOQtyError');
    var VMI_ToDoASNOpenPOQtyErrorgridDataList = $('#VMI_Process_ToDoASNOverOpenPOQtyError_gridDataList');
    var diaToDoASNAttach = $('#dialogToDoASNFileAttach');
    var diaVMIReportView = $('#dialog_VMIReport_ReportForm');
    var timeoutRelease;


    $('#btn_VMIProcess_ToDoASN_CreateHeader').click(function () {
        $(this).removeClass('ui-state-focus');
        if ($('#txt_VMIProcess_ToDoASN_Plant').val() != "" && $('#txt_VMIProcess_ToDoASN_SBU_VDN').val() != "") {
            diaToDoASNHeader.attr('Mode', 'C');
            CheckPassToCreateASNHead();
        }
        else if ($('#txt_VMIProcess_ToDoASN_Plant').val() == "" && $('#txt_VMIProcess_ToDoASN_SBU_VDN').val() != "") {
            alert("Please enter Plant.")
        }
        else if ($('#txt_VMIProcess_ToDoASN_Plant').val() != "" && $('#txt_VMIProcess_ToDoASN_SBU_VDN').val() == "") {
            alert("Please enter Vendor.")
        }
        else {
            alert("Please enter Plant and Vendor.")
        }
    });

    $('#dia_btn_VMIProcess_ToDoASNManage_Head').click(function () {
        $(this).removeClass('ui-state-focus');
        diaToDoASNHeader.attr('Mode', 'U');
        InitdialogToDoASNHeader(diaToDoASNHeader.attr('Mode'));
    });

    $('#dia_btn_VMIProcess_ToDoASNManage_AddDetail').click(function () {
        $(this).removeClass('ui-state-focus');
        diaToDoASNDetail.attr('Mode', 'C');
        InitdialogToDoASNDetail(diaToDoASNDetail.attr('Mode'));
    });

    $('#dia_btn_VMIProcess_ToDoASNManage_ImportDetail').click(function () {
        $(this).removeClass('ui-state-focus');
        InitdialogToDoASNImportDetail('B');
        // div with class ui-dialog
        $('.ui-dialog :button').blur();
    });

    $('#dia_btn_VMIProcess_ToDoASNManage_Delete').click(function () {
        $(this).removeClass('ui-state-focus');
        var VMI_ToDoASNManagegridDataList = $('#VMI_Process_ToDoASNManage_gridDataList');
        var arrSelRowID = VMI_ToDoASNManagegridDataList.jqGrid("getGridParam", "selarrrow");
        var AsnLines = "";

        if (arrSelRowID.length > 0) {
            for (var i = 0 ; i < arrSelRowID.length; i++) {
                AsnLines += VMI_ToDoASNManagegridDataList.jqGrid('getRowData', arrSelRowID[i]).ASNLINE + ",";
            }

            //delete success 
            if (confirm("Delete the selected lines?")) {
                DeleteSelectedAsnLine(AsnLines);
            }
        }
        else {
            alert("Please select a data to delete.");
        }
    });

    $('#dia_btn_VMIProcess_ToDoASNManage_AttachFile').click(function () {
        $(this).removeClass('ui-state-focus');
        ReloadToDoASNFileDetailInfogridDataList();
        diaToDoASNFileDetail.dialog("option", "title", "File Attach").dialog("open");
        // div with class ui-dialog
        $('.ui-dialog :button').blur();
    });

    $('#dia_btn_VMIProcess_ToDoASNFileDetail_Add').click(function () {
        $(this).removeClass('ui-state-focus');
        diaToDoASNAttach.attr('Mode', 'C');
        InitdialogToDoASNFileAttach('C');

        /**** Init jQuery fileupload 1 ****/
        $.init_fileupload(
            'fileupload_uploadFileAttach', // fileUpload Control ID
            1, // maxNumberOfFiles (null: nolimit)
            30000000, // maxFileSize (ex: 10000000 // 10 MB; 0: nolimit)
            null // acceptFileTypes (ex: /(\.|\/)(gif|jpe?g|png)$/i)
            );

        diaToDoASNAttach.dialog("option", "title", "Upload").dialog("open");
    });

    $('#dia_btn_VMIProcess_ToDoASNFileDetail_Delete').click(function () {
        $(this).removeClass('ui-state-focus');
        var VMI_ToDoASNFileDetailgridDataList = $('#VMI_Process_ToDoASNFileDetail_gridDataList');
        var arrSelRowID = VMI_ToDoASNFileDetailgridDataList.jqGrid("getGridParam", "selarrrow");
        var AttachGUIDLines = "";

        if (arrSelRowID.length > 0) {
            for (var i = 0 ; i < arrSelRowID.length; i++) {
                AttachGUIDLines += VMI_ToDoASNFileDetailgridDataList.jqGrid('getRowData', arrSelRowID[i]).FS_GUID + ",";
            }

            //delete success 
            if (confirm("Delete the selected lines?")) {
                DeleteSelectedAttachLine(AttachGUIDLines);
            }
        }
        else {
            alert("Please select a data to delete.");
        }

    });

    $('#dialog_span_VMIProcess_ToDoASNFileAttach_Download').click(function () {
        $(this).removeClass('ui-state-focus');
        var diaToDoASNAttach = $('#dialogToDoASNFileAttach');
        window.location = __WebAppPathPrefix + "/VMIProcess/DownloadToDoASNAttachFile?DataKey=" + escape($.trim(diaToDoASNAttach.attr('FS_GUID')));
    });

    $('#dia_btn_VMIProcess_ToDoASNManage_DeleteAll').click(function () {
        $(this).removeClass('ui-state-focus');
        //delete success 
        if (confirm("Delete the selected lines?")) {
            DeleteAllAsnDetail();
        }
    });

    $('#dia_btn_VMIProcess_ToDoASNManage_Release').click(function () {
        $(this).removeClass('ui-state-focus');
        DisableFunctionButton();

        if (CheckASNDetailStatusForRelease()) {
            $.ajax({
                url: __WebAppPathPrefix + '/VMIProcess/ReleaseToDoASN',
                data: {
                    ASNNUM: escape($.trim(diaToDoASNManage.prop("ASN_NUM")))
                },
                type: "post",
                dataType: 'json',
                beforeSend: function () {
                    $("#dialogDownloadSplash").dialog({
                        title: 'Process Notify',
                        width: 'auto',
                        height: 'auto',
                        open: function (event, ui) {
                            $(this).parent().find('.ui-dialog-titlebar-close').hide();
                            $(this).parent().find('.ui-dialog-buttonpane').hide();
                            $("#lbDiaDownloadMsg").html('In Processing...</br></br>Please wait for the operation a moment...');
                        }
                    }).dialog("open");
                    //setTimeout(function () {
                    //    CheckStatusForReleaseProcessLast();
                    //}, 5000);
                    timeoutRelease = setTimeout(function () {
                        $("#dialogDownloadSplash").dialog("close");
                        $("#dialogDownloadSplash").dialog({
                            title: 'Process Notify',
                            width: 'auto',
                            height: 'auto',
                            open: function (event, ui) {
                                $(this).parent().find('.ui-dialog-titlebar-close').hide();
                                $(this).parent().find('.ui-dialog-buttonpane').hide();
                                $("#lbDiaDownloadMsg").html('In Processing...</br></br>Please wait a min for the operation done... ');
                            }
                        }).dialog("open");

                        timeoutRelease = setTimeout(function () {
                            $("#dialogDownloadSplash").dialog("close");
                        }, 5000);
                    }, 5000);
                },
                async: true,
                success: function (data) {
                    if (data.Result == true) {
                        setTimeout(function () {
                            $("#dialogDownloadSplash").dialog("close");
                        }, 1000);
                        ReloadToDoASNManagegridDataList();
                        CheckStatusForReleaseProcess();
                        DisableFunctionButton();
                    }
                    else {
                        clearTimeout(timeoutRelease);
                        $("#dialogDownloadSplash").dialog("close");
                        EnableFunctionButton();
                        if (data.RT == 3) {
                            VMI_ToDoASNOpenPOQtyErrorgridDataList.jqGrid('setGridParam', { datatype: "jsonstring", datastr: data.Message });
                            VMI_ToDoASNOpenPOQtyErrorgridDataList.trigger('reloadGrid');
                            diaToDoASNOpenPOQtyError.dialog("option", "title", "Release Error Message").dialog("open");
                            // div with class ui-dialog
                            $('.ui-dialog :button').blur();
                        }
                        else {
                            alert(data.Message);
                        }
                    }

                    disableFunctionButtonforASNReviewer();
                },
                error: function (xhr, textStatus, thrownError) {
                    setTimeout(function () {
                        $("#dialogDownloadSplash").dialog("close");
                    }, 1000);
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                }
            });
        }
        else {
            alert("All ASN Detail status must be New or Error, not allow Release.")
        }
    });

    $('#dia_btn_VMIProcess_ToDoASNManage_DownloadASN').click(function () {
        $(this).removeClass('ui-state-focus');
        var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');

        $.ajax({
            url: __WebAppPathPrefix + '/VMIProcess/QueryToDoASNDetailToExcel',
            data: {
                ASNNUM: escape($.trim(diaToDoASNManage.prop("ASN_NUM")))
            },
            type: "post",
            dataType: 'json',
            async: true,
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

    $('#dia_btn_VMIProcess_ToDoASNManage_Print').click(function () {
        $(this).removeClass('ui-state-focus');
        window.open(__WebAppPathPrefix + '/VMIReport/VMIReportForm?ASN_NUM=' + escape($.trim(diaToDoASNManage.prop("ASN_NUM"))), '_blank');

    });

    $('#dia_btn_VMIProcess_ToDoASNManage_SubmitToBuyerReview').click(function () {
        $(this).removeClass('ui-state-focus');

        $.ajax({
            url: __WebAppPathPrefix + '/VMIProcess/SubmitToBuyerReview',
            data: {
                ASN_NUM: escape($.trim(diaToDoASNManage.prop('ASN_NUM')))
            },
            type: "post",
            dataType: 'text',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                if (data == "Success") {
                    ReloadToDoASNManagegridDataList();
                    DisableFunctionButton();
                    $('#dia_btn_VMIProcess_ToDoASNManage_SubmitToBuyerReview').hide();
                    alert("Notice buyer successfully.");
                }
                else if(data == "Fail"){
                    alert("Notice buyer failuer, please contact buyer.");
                }
                else {
                    alert(data);
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    });

    $('#dia_btn_VMIProcess_ToDoASNManage_Reject').click(function () {
        $('#dialog_VMIProcess_ToDoASNImportedReject').dialog('open');
    });
});

function DeleteSelectedAsnLine(Asnlines) {
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');

    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/DeleteToDoASNDetailInfo',
        data: {
            ASN_NUM: escape($.trim(diaToDoASNManage.prop("ASN_NUM"))),
            ASN_LINES: escape($.trim(Asnlines))
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.Result == true) {
                alert(data.Message);
                ReloadToDoASNManagegridDataList();
            }
            else {
                alert(data.Message);
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

function DeleteAllAsnDetail() {
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');

    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/DeleteAllToDoASNDetailInfo',
        data: {
            ASN_NUM: escape($.trim(diaToDoASNManage.prop("ASN_NUM")))
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.Result == true) {
                alert(data.Message);
                ReloadToDoASNManagegridDataList();
                diaToDoASNManage.dialog("close");
                ReloadToDoASNgridDataList();
            }
            else {
                alert(data.Message);
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

function DeleteSelectedAttachLine(AttachGUIDLines) {

    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');

    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/DeleteToDoASNAttachFileInfo',
        data: {
            ASN_NUM: escape($.trim(diaToDoASNManage.prop("ASN_NUM"))),
            AttachGUIDs: escape($.trim(AttachGUIDLines))
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.Result == true) {
                alert(data.Message);
                ReloadToDoASNFileDetailInfogridDataList();
            }
            else {
                alert(data.Message);
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

function CheckASNDetailStatusForRelease() {
    var bIsCheckPass = true;
    var VMI_ToDoASNManagegridDataList = $('#VMI_Process_ToDoASNManage_gridDataList');
    var RowIDs = VMI_ToDoASNManagegridDataList.jqGrid('getDataIDs');
    var dataRow;
    for (var i = 0 ; i < RowIDs.length; i++) {
        dataRow = VMI_ToDoASNManagegridDataList.jqGrid('getRowData', RowIDs[i]);
        if (dataRow.STATUS != "New" && dataRow.STATUS != "Error" && dataRow.STATUS != "Reviewing" && dataRow.STATUS.toLowerCase().indexOf("error") < 0) {
            bIsCheckPass = false;
            break;
        }
    }

    return bIsCheckPass;
}


function CheckStatusForReleaseProcess() {
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');
    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/CheckProcessForReleaseStatus',
        data: {
            ASNNUM: escape($.trim(diaToDoASNManage.prop("ASN_NUM")))
        },
        type: "post",
        dataType: 'json',
        //async: false,
        beforeSend: function () {
            $("#dialogDownloadSplash").dialog({
                title: 'Process Notify',
                width: 'auto',
                height: 'auto',
                open: function (event, ui) {
                    $(this).parent().find('.ui-dialog-titlebar-close').hide();
                    $(this).parent().find('.ui-dialog-buttonpane').hide();
                    $("#lbDiaDownloadMsg").html('In Processing...</br></br>Please wait for the operation a moment...');
                }
            }).dialog("open");
        },
        success: function (data) {
            if (data.Result == true) {
                $("#lbDiaDownloadMsg").html('Release successfully.');

                setTimeout(function () {
                    $("#dialogDownloadSplash").dialog("close");
                }, 1500)
            }
            else {
                setTimeout(function () {
                    CheckStatusForReleaseProcessLast();
                }, 5000)
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

function CheckStatusForReleaseProcessLast() {
    alert()
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');
    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/CheckProcessForReleaseStatus',
        data: {
            ASNNUM: escape($.trim(diaToDoASNManage.prop("ASN_NUM")))
        },
        type: "post",
        dataType: 'json',
        //async: false,
        beforeSend: function () {
            $("#lbDiaDownloadMsg").html('In Processing...</br></br>Please wait for the operation a moment...');
        },
        success: function (data) {
            if (data.Result == true) {
                $("#lbDiaDownloadMsg").html('Release successfully.');

                setTimeout(function () {
                    $("#dialogDownloadSplash").dialog("close");
                }, 1000)
            }
            else {
                $("#lbDiaDownloadMsg").html('In Processing...</br></br>Please wait for the operation later...');

                setTimeout(function () {
                    $("#dialogDownloadSplash").dialog("close");
                }, 1500)
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

function DisableFunctionButton() {
    $('#dia_btn_VMIProcess_ToDoASNManage_Head').attr("disabled", true);
    $('#dia_btn_VMIProcess_ToDoASNManage_AddDetail').attr("disabled", true);
    $('#dia_btn_VMIProcess_ToDoASNManage_ImportDetail').attr("disabled", true);
    $('#dia_btn_VMIProcess_ToDoASNManage_AttachFile').attr("disabled", true);
    $('#dia_btn_VMIProcess_ToDoASNManage_DeleteAll').attr("disabled", true);
    $('#dia_btn_VMIProcess_ToDoASNManage_Delete').attr("disabled", true);
    $('#dia_btn_VMIProcess_ToDoASNManage_Release').attr("disabled", true);
    $('#dia_btn_VMIProcess_ToDoASNManage_DownloadASN').attr("disabled", true);
    $('#dia_btn_VMIProcess_ToDoASNManage_Print').attr("disabled", true);
}

function EnableFunctionButton() {
    $('#dia_btn_VMIProcess_ToDoASNManage_Head').attr("disabled", false);
    $('#dia_btn_VMIProcess_ToDoASNManage_AddDetail').attr("disabled", false);
    $('#dia_btn_VMIProcess_ToDoASNManage_ImportDetail').attr("disabled", false);
    $('#dia_btn_VMIProcess_ToDoASNManage_AttachFile').attr("disabled", false);
    $('#dia_btn_VMIProcess_ToDoASNManage_DeleteAll').attr("disabled", false);
    $('#dia_btn_VMIProcess_ToDoASNManage_Delete').attr("disabled", false);
    $('#dia_btn_VMIProcess_ToDoASNManage_Release').attr("disabled", false);
    $('#dia_btn_VMIProcess_ToDoASNManage_DownloadASN').attr("disabled", false);
    $('#dia_btn_VMIProcess_ToDoASNManage_Print').attr("disabled", false);
}


