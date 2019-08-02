function getSQMTradersProductFile2() {
    getFilesDetailT2();

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
        'fileupload_uploadVDST2', // fileUpload Control ID
        1, // maxNumberOfFiles (null: nolimit)
        5000000, // maxFileSize (ex: 10000000 // 10 MB; 0: nolimit)
        null // acceptFileTypes (ex: /(\.|\/)(gif|jpe?g|png)$/i)
        );

    $('#btnProcessT2').click(function () {
        var uploadType = '';

        if ($.get_uploadedFileInfo('fileupload_uploadVDST2').length == 0) {
            alert('Please select the file to upload.');
        }
        else if (uploadType == '123123') {
            alert('Please check at least one type.');
        }
        else {
            var ReqData = '';
            //ReqData = $.extend(ReqData, { "Spec": JSON.stringify($.UploadFiles_UPloadifyF32_Type1_RetrieveFileInfo("Spec_FileUpload_FileList")) });
            ReqData = $.extend(ReqData, { "Spec": JSON.stringify($.get_uploadedFileInfo('fileupload_uploadVDST2')) });
            ReqData = $.extend(ReqData, { "SubFolder": _SubFolderForUploadExcel });
            var pro2 = escape($.trim($("#txtPrincipalProductsT").val()));
            var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
            var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
            var BasicInfoGUID = gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID;
            $.ajax({
                url: __WebAppPathPrefix + "/SQMTraders/UploadTraders2",
                data: {
                    FA: ReqData,
                    PrincipalProducts: pro2,
                    "BasicInfoGUID": BasicInfoGUID
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
                    });

                    if (errorMessage != '') {
                        alert(errorMessage);
                    }
                    else {
                        if (isSuccess) {
                            $('#dialogUploadVDSSummary #result').css('color', 'green').text('Process Successfully');
                            getFilesDetailT2();
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
                    $.reInit_fileupalod('fileupload_uploadVDST2');
                }
            });
        }
    });
}
function getFilesDetailT2() {
    //load esist data
    var CritialFile;
    var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
    var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
    var BasicInfoGUID = gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID;
    $.ajax({
        url: __WebAppPathPrefix + '/SQMTraders/GetTradersDetail2',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        data: {
            "BasicInfoGUID": BasicInfoGUID
        },
        type: "post",
        dataType: 'json',
        async: true, // if need page refresh, please remark this option
        success: function (data) {
            if (data.length>0) {
                CritialFile = data[0];
                if (CritialFile.OfferSellFGUID != "" && CritialFile.OfferSellFGUID != "undefine") {
                    $("#sp" + "BasicInfoFileT2").html("<a href='" + __WebAppPathPrefix + "/SQMTraders/DownloadTraders2?DataKey=" + escape($.trim(CritialFile.OfferSellFGUID)) + "'>" + CritialFile.PlaceFN1 + "</a>");
                }
            }
         
            $("#sp" + "BasicInfoUploadTimeT2").html(CritialFile.PlaceTime1);

        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {

        }
    });
}
