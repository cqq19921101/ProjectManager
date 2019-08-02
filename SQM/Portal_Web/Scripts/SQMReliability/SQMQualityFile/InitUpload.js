$(function () {
    //getFilesDetail();
    var reg = /(\.|\/)(pdf)$/i;
    /**** Init jQuery fileupload 1 ****/
    $.init_fileupload(
        'fileupload_uploadVDS', // fileUpload Control ID
        null, // maxNumberOfFiles (null: nolimit)
        5000000, // maxFileSize (ex: 10000000 // 10 MB; 0: nolimit)
        reg // acceptFileTypes (ex: /(\.|\/)(gif|jpe?g|png)$/i)
        );

    $('#btnUploadSureInfo').click(function () {
        
        var gridDataList = $("#gridDataListFile");
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        var dataRow = gridDataList.jqGrid('getRowData', RowId);
        var Keydata = dataRow.ReportSID;
        var uploadType = '';

        if ($.get_uploadedFileInfo('fileupload_uploadVDS').length == 0) {
            alert('Please select the file to upload.');
        }
        else if (uploadType == '123123') {
            alert('Please check at least one type.');
        }
        else {
            var ReqData = '';
            //ReqData = $.extend(ReqData, { "Spec": JSON.stringify($.UploadFiles_UPloadifyF32_Type1_RetrieveFileInfo("Spec_FileUpload_FileList")) });
            ReqData = $.extend(ReqData, { "Spec": JSON.stringify($.get_uploadedFileInfo('fileupload_uploadVDS')) });
            ReqData = $.extend(ReqData, { "SubFolder": _SubFolderForUploadExcel });
            //var TestProjet = escape($.trim($("#txtProject").val()));
            //var gridDataList = $("#ReliInfogridDataList");

            //var RowIdReli = gridDataList.jqGrid('getGridParam', 'selrow');

            $.ajax({
                url: __WebAppPathPrefix + "/SQMReliability/UploadFileOfQuality",
                data: {
                    //"ReportSID": Keydata,
                    FA: ReqData
                    //,TestProjet: TestProjet
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
                            $("#lblUploadDiaErrMsg").html('Uploading...</br></br>Please wait for the operation a moment...');
                        }
                    }).dialog("open");
                },
                success: function (data) {
                    $("#gridDataListFile").attr("FGuid", data);
                    $("#dialogDownloadSplash").dialog('close');
                    ////var results = $.parseJSON(data);
                    var isSuccess = true;
                    //var errorMessage = data;

                    //$('#dialogUploadVDSSummary table.defaultTable tr').eq(2).find('td').each(function (index, element) {
                    //    $(element).find('p').each(function (index, element) {
                    //        switch (index) {
                    //            case 0:
                    //                $(element).text('Total Lines:0');
                    //                break;
                    //            case 1:
                    //                $(element).text('Correct Lines:0');
                    //                break;
                    //            case 2:
                    //                $(element).text('Error Lines:0');
                    //                break;
                    //        }
                    //    });
                    //});

                    ////$(results).each(function (index, obj) {
                    ////    if (obj.Remark != null) {
                    ////        isSuccess = false;
                    ////        errorMessage += obj.Remark + '\n';
                    ////    }

                    ////    if (obj.fileName != null) {
                    ////        isSuccess = false;
                    ////        $('#tdErrorFileDownload').children().remove();
                    ////        $('<a/>').prop('href', __WebAppPathPrefix + obj.fileName).text('Failure Excel Download').appendTo('#tdErrorFileDownload');
                    ////    }
                    ////});

                    //if (errorMessage != '') {
                    //    alert(errorMessage);
                    //}
                    //else {
                        if (isSuccess) {
                            alert("Process Successfully.");
                            $('#dialogUploadVDSSummary #result').css('color', 'green').text('Process Successfully');
                            //getFilesDetail();
                            //$("#btnReliabilitySearch").click();
                            //$("#MapdialogData").dialog("close");
                            
                        }
                        else {
                            alert("Process Unsuccessfully.");
                            $('#dialogUploadVDSSummary #result').css('color', 'red').text('Process Unsuccessfully');
                        }

                    //    //$('#dialogUploadVDSSummary').dialog('open');
                    //}
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