$(function () {
    var diaQueryASNManage = $('#dialog_VMIQuery_QueryASNManage');
    var diaModifyASNAttach = $('#dialogModifyASNFileAttach');
    var ReqData = "";
    var DoSuccessfully = false;

    diaModifyASNAttach.dialog({
        autoOpen: false,
        height: 300,
        width: 400,
        resizable: false,
        modal: true,
        buttons: {
            Ok: function () {
                // if (diaModifyASNAttach.find('tbody.files tr td button').text() == "Remove") {
                ReqData = $.extend(ReqData, { "ASNNO": escape($.trim(diaQueryASNManage.prop("ASN_NUM"))) });
                ReqData = $.extend(ReqData, { "FS_GUID": escape($.trim(diaModifyASNAttach.attr("FS_GUID"))) });
                ReqData = $.extend(ReqData, { "REMARK": escape($.trim($('#dialog_txt_VMIQuery_ModifyASNFileAttach_Remark').val())) });
                //ReqData = $.extend(ReqData, { "SPEC": JSON.stringify($.UploadFiles_UPloadifyF32_Type1_RetrieveFileInfo("dialogToDoASN_Spec_FileAttach_FileList")) });
                ReqData = $.extend(ReqData, { "SPEC": JSON.stringify($.get_uploadedFileInfo('fileupload_uploadFileAttach')) });
                ReqData = $.extend(ReqData, { "SUBFOLDER": _SubFolderForAttachments });
                $.ajax({
                    //url: __WebAppPathPrefix + "/VMIProcess/UploadAttachmentForToDoASN",
                    url: __WebAppPathPrefix + ((diaModifyASNAttach.attr('Mode') == "C") ? "/VMIProcess/UploadAttachmentForToDoASN" : "/VMIProcess/UpdateAttachmentRemarkForToDoASN"),
                    data: {
                        "TDAA": ReqData
                    },
                    type: "post",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data.Result) {
                            DoSuccessfully = true;
                            alert("Upload successfully.");
                            ReloadModifyASNFileDetailInfogridDataList();
                        }
                        else {
                            DoSuccessfully = false;
                            alert(data.Message);
                        }
                    },
                    error: function (xhr, textStatus, thrownError) {
                        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                    },
                    complete: function (jqXHR, testStatus) {

                    }
                });
                //                }
                //                else {
                //                    alert("File upload is processing,please wait a moment.");
                //;                }

                if (DoSuccessfully) {
                    $(this).dialog("close");
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            $.reInit_fileupalod('dialogModifyASNFileAttach');
            //$("#dialogToDoASN_Spec_FileAttach_Component").hide();
            //try { $("#dialogToDoASN_Spec_FileAttach_Component").uploadify('destroy'); } catch (e) { }
        }
    });
});

function InitdialogModifyASNFileAttach(Mode) {
    var diaModifyASNAttach = $('#dialogModifyASNFileAttach');
    switch (Mode) {
        case 'C':
            $('#dialog_td_VMIQuery_Attach_New').show();
            $('#dialog_td_VMIQuery_Attach_Update').hide();
            $('#dialog_span_VMIQuery_ModifyASNFileAttach_Download').hide();
            $('#dialog_txt_VMIQuery_ModifyASNFileAttach_Remark').val("");
            break;
        case 'U':
            $('#dialog_td_VMIQuery_Attach_New').hide();
            $('#dialog_td_VMIQuery_Attach_Update').show();
            $('#dialog_span_VMIQuery_ModifyASNFileAttach_Download').show();
            $('#dialog_span_VMIQuery_ModifyASNFileAttach_Download').html(diaModifyASNAttach.attr("FILENAME"));
            $('#dialog_txt_VMIQuery_ModifyASNFileAttach_Remark').val(diaModifyASNAttach.attr("REMARK"));
            break;
    }
}