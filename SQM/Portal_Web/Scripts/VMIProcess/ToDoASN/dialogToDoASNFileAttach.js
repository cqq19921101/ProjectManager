$(function () {
    var diaToDoASNManage = $('#dialog_VMIProcess_ToDoASNManage');
    var diaToDoASNAttach = $('#dialogToDoASNFileAttach');
    var ReqData = "";

    diaToDoASNAttach.dialog({
        autoOpen: false,
        height: 300,
        width: 400,
        resizable: false,
        modal: true,
        buttons: {
            Ok: function () {
                ReqData = $.extend(ReqData, { "ASNNO": escape($.trim(diaToDoASNManage.prop("ASN_NUM"))) });
                ReqData = $.extend(ReqData, { "FS_GUID": escape($.trim(diaToDoASNAttach.attr("FS_GUID"))) });
                ReqData = $.extend(ReqData, { "REMARK": escape($.trim($('#dialog_txt_VMIProcess_ToDoASNFileAttach_Remark').val())) });
                //ReqData = $.extend(ReqData, { "SPEC": JSON.stringify($.UploadFiles_UPloadifyF32_Type1_RetrieveFileInfo("dialogToDoASN_Spec_FileAttach_FileList")) });
                ReqData = $.extend(ReqData, { "SPEC": JSON.stringify($.get_uploadedFileInfo('fileupload_uploadFileAttach')) });
                ReqData = $.extend(ReqData, { "SUBFOLDER": _SubFolderForAttachments });
                $.ajax({
                    //url: __WebAppPathPrefix + "/VMIProcess/UploadAttachmentForToDoASN",
                    url: __WebAppPathPrefix + ((diaToDoASNAttach.attr('Mode') == "C") ? "/VMIProcess/UploadAttachmentForToDoASN" : "/VMIProcess/UpdateAttachmentRemarkForToDoASN"),
                    data: {
                        "TDAA": ReqData
                    },
                    type: "post",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data.Result) {
                            alert("Upload successfully.");
                            ReloadToDoASNFileDetailInfogridDataList();
                        }
                        else {
                            alert(data.Message);
                        }
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    },
                    complete: function (jqXHR, testStatus) {

                    }
                });
                $(this).dialog("close");
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            $.reInit_fileupalod('dialogToDoASNFileAttach');
            //$("#dialogToDoASN_Spec_FileAttach_Component").hide();
            //try { $("#dialogToDoASN_Spec_FileAttach_Component").uploadify('destroy'); } catch (e) { }
        }
    });
});

function InitdialogToDoASNFileAttach(Mode) {
    var diaToDoASNAttach = $('#dialogToDoASNFileAttach');
    switch (Mode) {
        case 'C':
            $('#dialog_td_VMIProcess_Attach_New').show();
            $('#dialog_td_VMIProcess_Attach_Update').hide();
            $('#dialog_span_VMIProcess_ToDoASNFileAttach_Download').hide();
            $('#dialog_txt_VMIProcess_ToDoASNFileAttach_Remark').val("");
            break;
        case 'U':
            $('#dialog_td_VMIProcess_Attach_New').hide();
            $('#dialog_td_VMIProcess_Attach_Update').show();
            $('#dialog_span_VMIProcess_ToDoASNFileAttach_Download').show();
            $('#dialog_span_VMIProcess_ToDoASNFileAttach_Download').html(diaToDoASNAttach.attr("FILENAME"));
            $('#dialog_txt_VMIProcess_ToDoASNFileAttach_Remark').val(diaToDoASNAttach.attr("REMARK"));
            break;
    }
}