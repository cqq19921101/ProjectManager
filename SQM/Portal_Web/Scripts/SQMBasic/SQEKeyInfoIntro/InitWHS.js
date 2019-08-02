function initWHS() {
    setBasicEvent('WasteWater', '13', true);
    setBasicEvent('PollutionReport', '8',true);
    setBasicEvent('PollutionAgree', '9', true);
    setBasicEvent('PollutionAcceptance', '10', true);
    setBasicEvent('Waste', '11', true);
    setBasicEvent('Exhaust', '12', true);
   
    setBasicEvent('Noise', '14', true);
    //getFilesDetail();
    //setBasicEvent('Intro', '1');
    //setBasicEvent('Envir', '2');
    //setBasicEvent('WaterResponse', '3');
    //setBasicEvent('ElecResponse', '4');
    //setBasicEvent('CMControl', '5');
    //setBasicEvent('NonCMQuestionary', '6');
    //setBasicEvent('NonCMCommit', '7');

    $("#btnUpdateEHSBasic").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditCriticalFiles",
            data: {
                "EHSOwner": escape($.trim($("#txt" + "EHSOwner").val()))
                ,"EHSOwnerTitle": escape($.trim($("#txt" + "EHSOwnerTitle").val()))
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Update successfully.");
                }
                else {
                data = data.replace("<br />", "\u000d");
                    alert("error:" + data);
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                //$("#ajaxLoading").hide();
            }
        });
        if (DoSuccessfully) {

        }
    });

    $("#btnUpdatePollutionTypes").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        var PollutionTypes = "";
        $("[cbtype='PollutionType']").each(function (index) {
            if ($(this).is(":checked")) {
                PollutionTypes += $(this).val() + ";";
            }
        });
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditCriticalFiles",
            data: {
                "PollutionTypes": escape(PollutionTypes)
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Update successfully.");
                }
                else {
                    data = data.replace("<br />", "\u000d");
                    alert("error:" + data);
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                //$("#ajaxLoading").hide();
            }
        });
        if (DoSuccessfully) {

        }
    });

    $("#btnUpdateWasteTypes").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        var WasteTypes = "";
        $("[cbtype='WasteTypes']").each(function (index) {
            if ($(this).is(":checked")) {
                WasteTypes += $(this).val() + ";";
            }
        });
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditCriticalFiles",
            data: {
                "WasteTypes": escape(WasteTypes)
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Update successfully.");
                }
                else {
                    data = data.replace("<br />", "\u000d");
                    alert("error:" + data);
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                //$("#ajaxLoading").hide();
            }
        });
        if (DoSuccessfully) {

        }
    });

    $("#btnUpdateIsOutWasteHandler").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        var WasteTypes = "";
        $("#cbIsOutWasteHandler").each(function (index) {
            if ($("#cbIsOutWasteHandler").is(":checked")) {
                WasteTypes += $(this).val() + ";";
            }
        });
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditCriticalFiles",
            data: {
                "IsOutWasteHandler": $("#cbIsOutWasteHandler").is(":checked")
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Update successfully.");
                }
                else {
                    data = data.replace("<br />", "\u000d");
                    alert("error:" + data);
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                //$("#ajaxLoading").hide();
            }
        });
        if (DoSuccessfully) {

        }
    });

    $("#btnUpdateExhaustType").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        var ExhaustType = "";
        ExhaustType = $("#ddl" + "ExhaustType").val();
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditCriticalFiles",
            data: {
                "ExhaustType": ExhaustType
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Update successfully.");
                }
                else {
                    data = data.replace("<br />", "\u000d");
                    alert("error:" + data);
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                //$("#ajaxLoading").hide();
            }
        });
        if (DoSuccessfully) {

        }
    });

    $("#btnUpdateWasteWaterType").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        var WasteWaterType = "";
        WasteWaterType = $("#ddl" + "WasteWaterType").val();
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditCriticalFiles",
            data: {
                "WasteWaterType": WasteWaterType
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Update successfully.");
                }
                else {
                    data = data.replace("<br />", "\u000d");
                    alert("error:" + data);
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                //$("#ajaxLoading").hide();
            }
        });
        if (DoSuccessfully) {

        }
    });

    $("#btnUpdateNoiseType").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        var NoiseType = "";
        NoiseType = $("#ddl" + "NoiseType").val();
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditCriticalFiles",
            data: {
                "NoiseType": NoiseType
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Update successfully.");
                }
                else {
                    data = data.replace("<br />", "\u000d");
                    alert("error:" + data);
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                //$("#ajaxLoading").hide();
            }
        });
        if (DoSuccessfully) {

        }
    });

    

    $("#btnUpdatePollutionEventType").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        var PollutionEventType = "";
        PollutionEventType = $("#ddl" + "PollutionEventType").val();
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditCriticalFiles",
            data: {
                "PollutionEventType": PollutionEventType
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Update successfully.");
                }
                else {
                    data = data.replace("<br />", "\u000d");
                    alert("error:" + data);
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                //$("#ajaxLoading").hide();
            }
        });
        if (DoSuccessfully) {

        }
    });

    $("#btnUpdatePollutionEventDesc").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        var PollutionEventDesc = "";
        PollutionEventDesc = $("#txt" + "PollutionEventDesc").val();
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditCriticalFiles",
            data: {
                "PollutionEventDesc": PollutionEventDesc
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Update successfully.");
                }
                else {
                    data = data.replace("<br />", "\u000d");
                    alert("error:" + data);
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                //$("#ajaxLoading").hide();
            }
        });
        if (DoSuccessfully) {

        }
    });


    $("#btnUpdatePunishmentType").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        var PunishmentType = "";
        PunishmentType = $("#ddl" + "PunishmentType").val();
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditCriticalFiles",
            data: {
                "PunishmentType": PunishmentType
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Update successfully.");
                }
                else {
                    data = data.replace("<br />", "\u000d");
                    alert("error:" + data);
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                //$("#ajaxLoading").hide();
            }
        });
        if (DoSuccessfully) {

        }
    });

    $("#btnUpdatePunishmentDesc").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        var PunishmentDesc = "";
        PunishmentDesc = $("#txt" + "PunishmentDesc").val();
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditCriticalFiles",
            data: {
                "PunishmentDesc": PunishmentDesc
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("Update successfully.");
                }
                else {
                    data = data.replace("<br />", "\u000d");
                    alert("error:" + data);
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                //$("#ajaxLoading").hide();
            }
        });
        if (DoSuccessfully) {

        }
    });
    //$("#btnUpdateCMC").click(function () {
    //    $(this).removeClass('ui-state-focus');
    //    var DoSuccessfully = false;
    //    $.ajax({
    //        url: __WebAppPathPrefix + "/SQMBasic/EditCriticalFiles",
    //        data: {
    //            "IsCMControl": escape($.trim($("#cb" + "IsCMControl").is(':checked')))
    //        },
    //        type: "post",
    //        dataType: 'text',
    //        async: false,
    //        success: function (data) {
    //            if (data == "") {
    //                DoSuccessfully = true;
    //                alert("Update successfully.");
    //            }
    //            else {
    //                alert("error:" + data);
    //            }
    //        },
    //        error: function (xhr, textStatus, thrownError) {
    //            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
    //        },
    //        complete: function (jqXHR, textStatus) {
    //            //$("#ajaxLoading").hide();
    //        }
    //    });
    //    if (DoSuccessfully) {

    //    }
    //});

    //$("#btnUpdateCMQ").click(function () {
    //    $(this).removeClass('ui-state-focus');
    //    var DoSuccessfully = false;
    //    $.ajax({
    //        url: __WebAppPathPrefix + "/SQMBasic/EditCriticalFiles",
    //        data: {
    //            "IsNonCMQuestionary": escape($.trim($("#cb" + "IsNonCMQuestionary").is(':checked')))
    //        },
    //        type: "post",
    //        dataType: 'text',
    //        async: false,
    //        success: function (data) {
    //            if (data == "") {
    //                DoSuccessfully = true;
    //                alert("Update successfully.");
    //            }
    //            else {
    //                alert("error:" + data);
    //            }
    //        },
    //        error: function (xhr, textStatus, thrownError) {
    //            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
    //        },
    //        complete: function (jqXHR, textStatus) {
    //            //$("#ajaxLoading").hide();
    //        }
    //    });
    //    if (DoSuccessfully) {

    //    }
    //});

    //$("#btnUpdateCMCt").click(function () {
    //    $(this).removeClass('ui-state-focus');
    //    var DoSuccessfully = false;
    //    $.ajax({
    //        url: __WebAppPathPrefix + "/SQMBasic/EditCriticalFiles",
    //        data: {
    //            "IsNonCMCommit": escape($.trim($("#cb" + "IsNonCMCommit").is(':checked')))
    //        },
    //        type: "post",
    //        dataType: 'text',
    //        async: false,
    //        success: function (data) {
    //            if (data == "") {
    //                DoSuccessfully = true;
    //                alert("Update successfully.");
    //            }
    //            else {
    //                alert("error:" + data);
    //            }
    //        },
    //        error: function (xhr, textStatus, thrownError) {
    //            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
    //        },
    //        complete: function (jqXHR, textStatus) {
    //            //$("#ajaxLoading").hide();
    //        }
    //    });
    //    if (DoSuccessfully) {

    //    }
    //});
}

//function setBasicEvent(fid,type) {
//    //$.UploadFiles_UPloadifyF32_Type1_Init2(
//    //    $("#Spec_FileUpload_FileList"),
//    //    $("#Spec_FileUpload_Component"),
//    //    1, //Upload Max files
//    //    "30MB",
//    //    _SubFolderForUploadExcel,
//    //    "Browse");
//    //$.UploadFiles_UPloadifyF32_Type1_ReInit($("#Spec_FileUpload_FileList"))
//    //$("#Spec_FileUpload_Component").show();
//    //$("#lbDiaUPSEErrorMsg").html("");

//    $.init_fileupload(
//        'fileupload_upload' + fid, // fileUpload Control ID
//        1, // maxNumberOfFiles (null: nolimit)
//        5000000, // maxFileSize (ex: 10000000 // 10 MB; 0: nolimit)
//        null // acceptFileTypes (ex: /(\.|\/)(gif|jpe?g|png)$/i)
//        );

//    $('#btnProcess' + fid).click(function () {
//        uploadFileSQM('fileupload_upload' + fid, type);
//    });
//}


//function uploadFileSQM(fid, type) {
//    var uploadType = '';

//    if ($.get_uploadedFileInfo(fid).length == 0) {
//        alert('Please select the file to upload.');
//    }
//    else if (uploadType == '123123') {
//        alert('Please check at least one type.');
//    }
//    else {
//        var ReqData = '';
//        //ReqData = $.extend(ReqData, { "Spec": JSON.stringify($.UploadFiles_UPloadifyF32_Type1_RetrieveFileInfo("Spec_FileUpload_FileList")) });
//        ReqData = $.extend(ReqData, { "Spec": JSON.stringify($.get_uploadedFileInfo(fid)) });
//        ReqData = $.extend(ReqData, { "SubFolder": _SubFolderForUploadExcel });

//        $.ajax({
//            url: __WebAppPathPrefix + "/SQMBasic/UploadIntroFile",
//            data: {
//                FA: ReqData,
//                Type: type
//            },
//            type: "post",
//            //async: false,
//            beforeSend: function () {
//                $("#dialogDownloadSplash").dialog({
//                    title: 'Uploading Notify',
//                    width: 'auto',
//                    height: 'auto',
//                    modal: true,
//                    open: function (event, ui) {
//                        $(this).parent().find('.ui-dialog-titlebar-close').hide();
//                        $(this).parent().find('.ui-dialog-buttonpane').hide();
//                        $("#lbDiaDownloadMsg").html('Uploading...</br></br>Please wait for the operation a moment...');
//                    }
//                }).dialog("open");
//            },
//            success: function (data) {
//                getFilesDetail();
//                $("#dialogDownloadSplash").dialog('close');
//                alert("upload success");
//                return;

//                var results = $.parseJSON(data);
//                var isSuccess = true;
//                var errorMessage = '';

//                $('#dialogUploadVDSSummary table.defaultTable tr').eq(2).find('td').each(function (index, element) {
//                    $(element).find('p').each(function (index, element) {
//                        switch (index) {
//                            case 0:
//                                $(element).text('Total Lines:0');
//                                break;
//                            case 1:
//                                $(element).text('Correct Lines:0');
//                                break;
//                            case 2:
//                                $(element).text('Error Lines:0');
//                                break;
//                        }
//                    });
//                });

//                $(results).each(function (index, obj) {
//                    if (obj.Remark != null) {
//                        isSuccess = false;
//                        errorMessage += obj.Remark + '\n';
//                    }

//                    if (obj.fileName != null) {
//                        isSuccess = false;
//                        $('#tdErrorFileDownload').children().remove();
//                        $('<a/>').prop('href', __WebAppPathPrefix + obj.fileName).text('Failure Excel Download').appendTo('#tdErrorFileDownload');
//                    }
//                });

//                if (errorMessage != '') {
//                    alert(errorMessage);
//                }
//                else {
//                    if (isSuccess) {
//                        $('#dialogUploadVDSSummary #result').css('color', 'green').text('Process Successfully');
                        
//                    }
//                    else {
//                        $('#dialogUploadVDSSummary #result').css('color', 'red').text('Process Unsuccessfully');
//                    }

//                    $('#dialogUploadVDSSummary').dialog('open');
//                }
//            },
//            error: function (xhr, textStatus, thrownError) {
//                $("#dialogDownloadSplash").dialog('close');
//                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
//            },
//            complete: function (jqXHR, testStatus) {
//                $.reInit_fileupalod(fid);
//            }
//        });
//    }
//}


