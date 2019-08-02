$(function () {
    //Init...
    //$.UploadFiles_UPloadifyF32_Type1_Init2(
    //                   $("#UploadASN_Spec_FileAttach_FileList"),
    //                   $("#UploadASN_Spec_FileAttach_Component"),
    //                   1,
    //                   "30MB",
    //                   _SubFolderForAttachments,
    //                   "Select File...");
    //$.UploadFiles_UPloadifyF32_Type1_ReInit($("#UploadASN_Spec_FileAttach_FileList"))
    //$("#UploadASN_Spec_FileAttach_Component").show();

        /**** Init jQuery fileupload 1 ****/
    $.init_fileupload(
        'fileupload_uploadASN', // fileUpload Control ID
        1, // maxNumberOfFiles (null: nolimit)
        30000000, // maxFileSize (ex: 10000000 // 10 MB; 0: nolimit)
        null // acceptFileTypes (ex: /(\.|\/)(gif|jpe?g|png)$/i)
        );

    $.reInit_fileupalod('fileupload_uploadASN');

    $('#div_VMIProcess_ReturnFile_Block').hide();

    $('#txt_VMIProcess_UploadASN_ItemSplitValue').val("20");

    $('#dialog_span_VMIProcess_UploadASNFile_Download').click(function () {
        $(this).removeClass('ui-state-focus');
        $.ajax({
            url: __WebAppPathPrefix + '/VMIProcess/DownloadUploadASNTemplateFile',
            //data: {
            //    ASNNUM: escape($.trim(diaQueryASNManage.prop("ASN_NUM")))
            //},
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
                            //$("#dialogDownloadSplash_Form").dialog("close");
                        }, 10);
                    }
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            }
        });
    });

    $('#btn_VMIProcess_UploadASN_Process').click(function () {
        var bIsSplit = false;
        var ReqData = '';
        var diaUploadASNResult = $('#dialog_VMIProcess_UploadASNResult');
        if ($('#chk_VMIProcess_UploadASN_Split').prop('checked')) bIsSplit = true;

        //if ($.UploadFiles_UPloadifyF32_Type1_RetrieveFileInfo("UploadASN_Spec_FileAttach_FileList").length == 0) {
        if($.get_uploadedFileInfo('fileupload_uploadASN').length == 0) {
            alert('Please select the file to upload.');
        }
        else {
            //ReqData = $.extend(ReqData, { "Spec": JSON.stringify($.UploadFiles_UPloadifyF32_Type1_RetrieveFileInfo("UploadASN_Spec_FileAttach_FileList")) });
            ReqData = $.extend(ReqData, { "Spec": JSON.stringify($.get_uploadedFileInfo('fileupload_uploadASN'))});
            ReqData = $.extend(ReqData, { "SubFolder": _SubFolderForAttachments });

            $.ajax({
                url: __WebAppPathPrefix + "/VMIProcess/UploadASNFileInfo",
                data: {
                    UFI: ReqData,
                    IsSplit: bIsSplit,
                    SplitLine: escape($.trim($('#txt_VMIProcess_UploadASN_ItemSplitValue').val()))
                },
                type: "post",
                dataType: 'json',
                async: false,
                success: function (data) {
                    if (data.Result) {
                        if (data.Message != "") {
                            alert(data.Message);
                        }
                    }
                    else {
                        if (data.RT == 1) {
                            $('#div_VMIProcess_ReturnFile_Block').show();
                            $('#span_VMIProcess_ReturnASNFile_Download').html(data.FileName);
                            $('#span_VMIProcess_ReturnASNFile_Download').prop("path", data.FilePath);
                            alert(data.Message);
                        }
                        else if (data.RT == 2) {
                            if (!__DialogIsShownNow) {
                                __DialogIsShownNow = true;
                                $('#dialog_span_VMProcess_UploadASNResult_msg').css('color', 'green');
                                $('#dialog_span_VMProcess_UploadASNResult_msg').html("Process Successfully.");
                                $('#dialog_span_VMProcess_UploadASNResult_ASNNo').show();
                                $('#dialog_span_VMProcess_UploadASNResult_ASNNo').html("開啟 ASN No : " + data.Message);
                                $('#dialog_span_VMIProcess_UploadASNResult_TotalLines').html(data.TotalLines);
                                $('#dialog_span_VMIProcess_UploadASNResult_CorrectLines').html(data.CorrectLines);
                                $('#dialog_span_VMIProcess_UploadASNResult_ErrorLines').html(data.ErrorLines);
                                $('#dialog_span_VMIProcess_attachimg').hide();
                                $('#dialog_span_VMIProcess_UploadASNResult_FilePath').html("");
                                $('#dialog_span_VMIProcess_UploadASNResult_FilePath').prop("");
                                diaUploadASNResult.attr('ASNNo', data.Message);
                                diaUploadASNResult.show();
                                diaUploadASNResult.dialog("open");
                            }
                        }
                        else if (data.RT == 3) {
                            if (!__DialogIsShownNow) {
                                __DialogIsShownNow = true;
                                $('#dialog_span_VMProcess_UploadASNResult_msg').css('color', 'red');
                                $('#dialog_span_VMProcess_UploadASNResult_msg').html("Process Unsuccessfully.");
                                $('#dialog_span_VMProcess_UploadASNResult_ASNNo').hide();
                                $('#dialog_span_VMIProcess_UploadASNResult_TotalLines').html(data.TotalLines);
                                $('#dialog_span_VMIProcess_UploadASNResult_CorrectLines').html(data.CorrectLines);
                                $('#dialog_span_VMIProcess_UploadASNResult_ErrorLines').html(data.ErrorLines);
                                $('#dialog_span_VMIProcess_attachimg').show();
                                $('#dialog_span_VMIProcess_UploadASNResult_FilePath').html(data.FileName);
                                $('#dialog_span_VMIProcess_UploadASNResult_FilePath').prop("path", data.FilePath);
                                diaUploadASNResult.show();
                                diaUploadASNResult.dialog("open");
                            }
                        }
                    }
                },
                error: function (xhr, textStatus, thrownError) {
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, testStatus) { }

            });
        }
    });

    $('#span_VMIProcess_ReturnASNFile_Download').click(function () {
        window.location = __WebAppPathPrefix + $('#span_VMIProcess_ReturnASNFile_Download').prop("path");
    });

    $('#dialog_span_VMIProcess_UploadASNResult_FilePath').click(function () {
        window.location = __WebAppPathPrefix + $('#dialog_span_VMIProcess_UploadASNResult_FilePath').prop("path");
    });

    $('#span_VMIProcess_ReturnASNFile_Delete').click(function () {
        $('#div_VMIProcess_ReturnFile_Block').hide();
    });
});
