$(function () {
    /**** Init jQuery fileupload 1 ****/
    $.init_fileupload(
        'fileupload_uploadOpenPOConfirmation', // fileUpload Control ID
        1, // maxNumberOfFiles (null: nolimit)
        30000000, // maxFileSize (ex: 10000000 // 10 MB; 0: nolimit)
        null // acceptFileTypes (ex: /(\.|\/)(gif|jpe?g|png)$/i)
        );

    $('#btnProcess').click(function () {
        if ($.get_uploadedFileInfo('fileupload_uploadOpenPOConfirmation').length == 0) {
            alert('Please select the file to upload.');
        }
        else {
            var ReqData = '';
            ReqData = $.extend(ReqData, { "Spec": JSON.stringify($.get_uploadedFileInfo('fileupload_uploadOpenPOConfirmation')) });
            ReqData = $.extend(ReqData, { "SubFolder": _SubFolderForUploadExcel });

            $.ajax({
                url: __WebAppPathPrefix + "/VMIProcess/UploadOpenPOConfirmationFile",
                data: {
                    FA: ReqData
                },
                type: "post",
                dataType: 'json',
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

                    $('#dialogUploadOpenPOConfirmationSummary #result').text(data.isSuccess ? 'Upload success.' : 'Upload fail.');
                    $('#dialogUploadOpenPOConfirmationSummary #message').append(data.message);
                    if (data.fileName != null) {
                        $('<a/>').prop('href', __WebAppPathPrefix + data.fileName).attr('style', 'color:blue;').text('Failure Excel Download').appendTo('#dialogUploadOpenPOConfirmationSummary #tdErrorFileDownload');
                    }

                    $('#dialogUploadOpenPOConfirmationSummary').dialog('open');
                },
                error: function (xhr, textStatus, thrownError) {
                    $("#dialogDownloadSplash").dialog('close');
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, testStatus) {
                    $.reInit_fileupalod('fileupload_uploadOpenPOConfirmation');
                }
            });
        }
    });
})
