$(function () {
    //$.UploadFiles_UPloadifyF32_Type1_Init2(
    //    $("#Spec_FileUpload_FileList"),
    //    $("#Spec_FileUpload_Component"),
    //    1, //Upload Max files
    //    "30MB",
    //    _SubFolderForUploadExcel,
    //    "Browse");
    //$.UploadFiles_UPloadifyF32_Type1_ReInit($("#Spec_FileUpload_FileList"))
    //$("#Spec_FileUpload_Component").show();
    //$("#lbDiaUPSEErrorMsg").html("");

    /**** Init jQuery fileupload 1 ****/
    $.init_fileupload(
        'fileupload_uploadVDS', // fileUpload Control ID
        1, // maxNumberOfFiles (null: nolimit)
        30000000, // maxFileSize (ex: 10000000 // 10 MB; 0: nolimit)
        null // acceptFileTypes (ex: /(\.|\/)(gif|jpe?g|png)$/i)
        );

    $('#btnProcess').click(function () {
        var uploadType = '';
        if ($('#cbD').prop('checked')) uploadType += 'D:';
        if ($('#cbW').prop('checked')) uploadType += 'W:';
        if ($('#cbM').prop('checked')) uploadType += 'M:';
        if ($('#cbX').prop('checked')) uploadType += 'X:';

        //if ($.UploadFiles_UPloadifyF32_Type1_RetrieveFileInfo("Spec_FileUpload_FileList").length == 0) {
        if ($.get_uploadedFileInfo('fileupload_uploadVDS').length == 0) {
            alert('Please select the file to upload.');
        }
        else if (uploadType == '') {
            alert('Please check at least one type.');
        }
        else {
            var ReqData = '';
            //ReqData = $.extend(ReqData, { "Spec": JSON.stringify($.UploadFiles_UPloadifyF32_Type1_RetrieveFileInfo("Spec_FileUpload_FileList")) });
            ReqData = $.extend(ReqData, { "Spec": JSON.stringify($.get_uploadedFileInfo('fileupload_uploadVDS')) });
            ReqData = $.extend(ReqData, { "SubFolder": _SubFolderForUploadExcel });

            $.ajax({
                url: __WebAppPathPrefix + "/VMIProcess/UploadVDSFile",
                data: {
                    FA: ReqData,
                    Type: uploadType
                },
                type: "post",
                //async: false,
                beforeSend: function () {
                    $("#dialogDownloadSplash").dialog({
                        title: 'Uploading Notify',
                        width: 'auto',
                        height: 'auto',
                        modal: true,
                        open: function (event, ui) {
                            $(this).parent().find('.ui-dialog-titlebar-close').hide();
                            $(this).parent().find('.ui-dialog-buttonpane').hide();
                            $("#lbDiaDownloadMsg").html('Uploading...</br></br>Please wait for the operation a moment...');
                        }
                    }).dialog("open");
                },
                success: function (data) {
                    $("#dialogDownloadSplash").dialog('close');
                    var results = $.parseJSON(data);
                    var isSuccess = true;
                    var errorMessage = '';

                    $('#dialogUploadVDSSummary table.defaultTable tr').eq(2).find('td').each(function (index, element) {
                        $(element).find('p').each(function (index, element) {
                            switch (index) {
                                case 0:
                                    $(element).text('Total Lines:0');
                                    break;
                                case 1:
                                    $(element).text('Correct Lines:0');
                                    break;
                                case 2:
                                    $(element).text('Error Lines:0');
                                    break;
                            }
                        });
                    });

                    $(results).each(function (index, obj) {
                        if (obj.Remark != null) {
                            isSuccess = false;
                            errorMessage += obj.Remark + '\n';
                        }

                        if (obj.fileName != null) {
                            isSuccess = false;
                            $('#tdErrorFileDownload').children().remove();
                            $('<a/>').prop('href', __WebAppPathPrefix + obj.fileName).text('Failure Excel Download').appendTo('#tdErrorFileDownload');
                        }

                        switch (obj.VDSType) {
                            case 'Daily':
                                $('#tdDailyLines p').each(function (index, element) {
                                    switch (index) {
                                        case 0:
                                            $(element).text('Total Lines: ' + obj.TotalLine);
                                            break;
                                        case 1:
                                            $(element).text('Correct Lines: ' + obj.CorrectLine);
                                            break;
                                        case 2:
                                            $(element).text('Error Lines: ' + obj.ErrorLine);
                                            break;
                                    }
                                });
                                break;
                            case 'Weekly':
                                $('#tdWeeklyLines p').each(function (index, element) {
                                    switch (index) {
                                        case 0:
                                            $(element).text('Total Lines: ' + obj.TotalLine);
                                            break;
                                        case 1:
                                            $(element).text('Correct Lines: ' + obj.CorrectLine);
                                            break;
                                        case 2:
                                            $(element).text('Error Lines: ' + obj.ErrorLine);
                                            break;
                                    }
                                });
                                break;
                            case 'Monthly':
                                $('#tdMonthlyLines p').each(function (index, element) {
                                    switch (index) {
                                        case 0:
                                            $(element).text('Total Lines: ' + obj.TotalLine);
                                            break;
                                        case 1:
                                            $(element).text('Correct Lines: ' + obj.CorrectLine);
                                            break;
                                        case 2:
                                            $(element).text('Error Lines: ' + obj.ErrorLine);
                                            break;
                                    }
                                });
                                break;
                            case 'Daily + Weekly + Monthly':
                                $('#tdDWMLines p').each(function (index, element) {
                                    switch (index) {
                                        case 0:
                                            $(element).text('Total Lines: ' + obj.TotalLine);
                                            break;
                                        case 1:
                                            $(element).text('Correct Lines: ' + obj.CorrectLine);
                                            break;
                                        case 2:
                                            $(element).text('Error Lines: ' + obj.ErrorLine);
                                            break;
                                    }
                                });
                                break;
                        }
                    });

                    if (errorMessage != '') {
                        alert(errorMessage);
                    }
                    else {
                        if (isSuccess) {
                            $('#dialogUploadVDSSummary #result').css('color', 'green').text('Process Successfully');
                        }
                        else {
                            $('#dialogUploadVDSSummary #result').css('color', 'red').text('Process Unsuccessfully');
                        }

                        $('#dialogUploadVDSSummary').dialog('open');
                    }
                },
                error: function (xhr, textStatus, thrownError) {
                    $("#dialogDownloadSplash").dialog('close');
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, testStatus) {
                    $.reInit_fileupalod('fileupload_uploadVDS');
                }
            });
        }
    });
})
