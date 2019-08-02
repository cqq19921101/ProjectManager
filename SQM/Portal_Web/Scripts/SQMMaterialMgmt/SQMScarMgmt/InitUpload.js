function setBasicEvent(fid) {

    $.init_fileupload(
        'fileupload_upload' + fid, // fileUpload Control ID
        1, // maxNumberOfFiles (null: nolimit)
        5000000, // maxFileSize (ex: 10000000 // 10 MB; 0: nolimit)
        null // acceptFileTypes (ex: /(\.|\/)(gif|jpe?g|png)$/i)
        );
  
        $('#btnProcess' + fid).click(function () {
            uploadFileECN(fid);
        });
}
function uploadFileECN(fid) {
    var ffid = 'fileupload_upload' + fid;
    var uploadType = '';

    if ($.get_uploadedFileInfo(ffid).length == 0) {
        alert('Please select the file to upload.');
    }
    else if (uploadType == '123123') {
        alert('Please check at least one type.');
    }
    else {
        var ReqData = '';
        //ReqData = $.extend(ReqData, { "Spec": JSON.stringify($.UploadFiles_UPloadifyF32_Type1_RetrieveFileInfo("Spec_FileUpload_FileList")) });
        ReqData = $.extend(ReqData, { "Spec": JSON.stringify($.get_uploadedFileInfo(ffid)) });
        ReqData = $.extend(ReqData, { "SubFolder": _SubFolderForUploadExcel });

        $.ajax({
            url: __WebAppPathPrefix + "/SQMMaterialMgmt/UploadFile",
            data: {
                FA: ReqData
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
                if (data != "fail") {
                    alert("upload success");
                    $("#tbMain1").attr(fid + "FGUID", data);
                    getFilesDetail(fid);
                } else {
                    alert(data);
                }
                return;

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
                $.reInit_fileupalod(ffid);
            }
        });
    }
}
function getFilesDetail(fid) {
    //load esist data
    var ScarFile;
    $.ajax({
        url: __WebAppPathPrefix + '/SQMMaterialMgmt/GetScarFilesDetail',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        data: {
            "FGUID": escape($.trim($("#tbMain1").attr(("" + fid + "FGUID"))))
        },
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            if (data.length > 0) {
                ScarFile = data[0];


                if (ScarFile.FGUID != "null" && ScarFile.FGUID != "undefined") {
                    $("#sp" + fid).html("<a href='" + __WebAppPathPrefix + "/SQMBasic/DownloadSQMFile?DataKey=" + escape($.trim(ScarFile.FGUID)) + "'>" + ScarFile.FileName + "</a>");
                } else {
                    $("#sp" + fid).html("empty");
                }
                //$("#sp" + "IntroUploadTime").html(new Date(ScarFile.IntroTime).yyyymmddhhmmss());

            } else {

            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {

        }
    });
}