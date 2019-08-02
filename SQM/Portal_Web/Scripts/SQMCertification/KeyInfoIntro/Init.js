$(function () {
    //Init Btn
    $("#btnIntro").button({
        label: "公司簡介",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnIntro").click(function () {
        $(this).removeClass('ui-state-focus');
        $("[edittpye='Files']").each(function (index) {
            $(this).hide();
        });
        $("#divIntro").show();
    });

    $("#btnEnvir").button({
        label: "品質與環境保證承諾書",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnEnvir").click(function () {
        $(this).removeClass('ui-state-focus');
        $("[edittpye='Files']").each(function (index) {
            $(this).hide();
        });
        $("#divEnvir").show();
    });

    $("#btnResponse").button({
        label: "缺水限電應變管理",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnResponse").click(function () {
        $(this).removeClass('ui-state-focus');
        $("[edittpye='Files']").each(function (index) {
            $(this).hide();
        });
        $("#divResponse").show();
    });

    $("#btnCM").button({
        label: "不使用衝突礦產金屬",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnCM").click(function () {
        $(this).removeClass('ui-state-focus');
        $("[edittpye='Files']").each(function (index) {
            $(this).hide();
        });
        $("#divCM").show();
    });

    $("#btnQE").button({
        label: "供應商重大品質事件資訊與品質保證函",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnQE").click(function () {
        $(this).removeClass('ui-state-focus');
        $("[edittpye='Files']").each(function (index) {
            $(this).hide();
        });
        $("#divQuiltyEvent").show();
    });

    $("[edittpye='Files']").each(function (index) {
        $(this).hide();
    });
    $("#divIntro").show();

    //datepicker
    var dateformat = $('input[name$="Date"]');
    dateformat.datepicker({
        dateFormat: 'yy/mm/dd',
    });

    //getCritalFilesData();
    //getFilesDetail();
    setBasicEvent('Intro', '1');
    setBasicEvent('Envir', '2');
    setBasicEvent('WaterResponse', '3');
    setBasicEvent('ElecResponse', '4');
    setBasicEvent('CMControl', '5');
    setBasicEvent('NonCMQuestionary', '6');
    setBasicEvent('NonCMCommit', '7');

    $("#btnSignDateEnvir").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditCriticalFiles",
            data: {
                "EnvirSignedTime": escape($.trim($("#txt" + "EnvirSignedTime").val()))
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

    $("#btnUpdateWater").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditCriticalFiles",
            data: {
                "IsWaterEmergencySquad": escape($.trim($("#cb" + "IsWaterEmergencySquad").is(':checked'))),
                "IsWaterResponseProgram": escape($.trim($("#cb" + "IsWaterResponseProgram").is(':checked')))
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

    $("#btnUpdateElec").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditCriticalFiles",
            data: {
                "IsElecEmergencySquad": escape($.trim($("#cb" + "IsElecEmergencySquad").is(':checked'))),
                "IsElecResponseProgram": escape($.trim($("#cb" + "IsElecResponseProgram").is(':checked')))
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

    $("#btnUpdateCMC").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditCriticalFiles",
            data: {
                "IsCMControl": escape($.trim($("#cb" + "IsCMControl").is(':checked')))
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

    $("#btnUpdateCMQ").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditCriticalFiles",
            data: {
                "IsNonCMQuestionary": escape($.trim($("#cb" + "IsNonCMQuestionary").is(':checked')))
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

    $("#btnUpdateCMCt").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditCriticalFiles",
            data: {
                "IsNonCMCommit": escape($.trim($("#cb" + "IsNonCMCommit").is(':checked')))
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
})
function setBasicEvent(fid,type) {
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

    $.init_fileupload(
        'fileupload_upload' + fid, // fileUpload Control ID
        1, // maxNumberOfFiles (null: nolimit)
        5000000, // maxFileSize (ex: 10000000 // 10 MB; 0: nolimit)
        null // acceptFileTypes (ex: /(\.|\/)(gif|jpe?g|png)$/i)
        );

    $('#btnProcess' + fid).click(function () {
        uploadFileSQM('fileupload_upload' + fid, type);
    });
}


function uploadFileSQM(fid, type) {
    var uploadType = '';
    var gridSQMCertificationDataList = $("#gridSQMCertificationDataList");
    var RowId = gridSQMCertificationDataList.jqGrid('getGridParam', 'selrow');
    if ($.get_uploadedFileInfo(fid).length == 0) {
        alert('Please select the file to upload.');
    }
    else if (uploadType == '123123') {
        alert('Please check at least one type.');
    }
    else {
        var ReqData = '';
        //ReqData = $.extend(ReqData, { "Spec": JSON.stringify($.UploadFiles_UPloadifyF32_Type1_RetrieveFileInfo("Spec_FileUpload_FileList")) });
        ReqData = $.extend(ReqData, { "Spec": JSON.stringify($.get_uploadedFileInfo(fid)) });
        ReqData = $.extend(ReqData, { "SubFolder": _SubFolderForUploadExcel });
     

        $.ajax({
            url: __WebAppPathPrefix + "/SQMCertification/UploadIntroFile",
            data: {
                BasicInfoGUID: gridSQMCertificationDataList.jqGrid('getRowData', RowId).BasicInfoGUID,              
                FA: ReqData,
                Type: type,
                CertificationType: gridSQMCertificationDataList.jqGrid('getRowData', RowId).CertificationType,
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
               // getFilesDetail();
                $("#dialogDownloadSplash").dialog('close');
                alert("upload success");
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
                $.reInit_fileupalod(fid);
            }
        });
    }
}


function isNullOrFalse(value) {
    if (value == "undefined" || value == false) {
        return true;
    } else {
        return false;
    }
}

function getFilesDetail() {
    //load esist data
    var CritialFile;
    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetCriticalFilesDetail',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            
            CritialFile = data[0];
            if (CritialFile.IntroFGUID != "null" && CritialFile.IntroFGUID != "undefined") {
                $("#sp" + "IntroFile").html("<a href='" + __WebAppPathPrefix + "/SQMBasic/DownloadSQMFile?DataKey=" + escape($.trim(CritialFile.IntroFGUID)) + "'>" + CritialFile.IntroFN + "</a>");
            } else {
                $("#sp" + "IntroFile").html("empty");
            }
            $("#sp" + "IntroUploadTime").html(new Date(CritialFile.IntroTime).yyyymmddhhmmss());

            if (CritialFile.EnvirFGUID != null && CritialFile.EnvirFGUID != "undefined") {
                $("#sp" + "EnvirFile").html("<a href='" + __WebAppPathPrefix + "/SQMBasic/DownloadSQMFile?DataKey=" + escape($.trim(CritialFile.EnvirFGUID)) + "'>" + CritialFile.EnvirFN + "</a>");
            } else {
                $("#sp" + "EnvirFile").html("empty");
            }
            $("#sp" + "EnvirUploadTime").html(new Date(CritialFile.EnvirTime).yyyymmddhhmmss());

            if (CritialFile.WaterResponseFGUID != null && CritialFile.WaterResponseFGUID != "undefined") {
                $("#sp" + "WaterResponseFile").html("<a href='" + __WebAppPathPrefix + "/SQMBasic/DownloadSQMFile?DataKey=" + escape($.trim(CritialFile.WaterResponseFGUID)) + "'>" + CritialFile.WaterResponseFN + "</a>");
            } else {
                $("#sp" + "WaterResponseFile").html("empty");
            }
            $("#sp" + "WaterResponseUploadTime").html(new Date(CritialFile.WaterResponseTime).yyyymmddhhmmss());

            if (CritialFile.ElecResponseFGUID != null && CritialFile.ElecResponseFGUID != "undefined") {
                $("#sp" + "ElecResponseFile").html("<a href='" + __WebAppPathPrefix + "/SQMBasic/DownloadSQMFile?DataKey=" + escape($.trim(CritialFile.ElecResponseFGUID)) + "'>" + CritialFile.ElecResponseFN + "</a>");
            } else {
                $("#sp" + "ElecResponseFile").html("empty");
            }
            $("#sp" + "ElecResponseUploadTime").html(new Date(CritialFile.ElecResponseTime).yyyymmddhhmmss());

            if (CritialFile.CMControlFGUID != null && CritialFile.CMControlFGUID != "undefined") {
                $("#sp" + "CMControlFile").html("<a href='" + __WebAppPathPrefix + "/SQMBasic/DownloadSQMFile?DataKey=" + escape($.trim(CritialFile.CMControlFGUID)) + "'>" + CritialFile.CMControlFN + "</a>");
            } else {
                $("#sp" + "CMControlFile").html("empty");
            }
            $("#sp" + "CMControlUploadTime").html(new Date(CritialFile.CMControlTime).yyyymmddhhmmss());

            if (CritialFile.NonCMQuestionaryFGUID != null && CritialFile.NonCMQuestionaryFGUID != "undefined") {
                $("#sp" + "NonCMQuestionaryFile").html("<a href='" + __WebAppPathPrefix + "/SQMBasic/DownloadSQMFile?DataKey=" + escape($.trim(CritialFile.NonCMQuestionaryFGUID)) + "'>" + CritialFile.NonCMQuestionaryFN + "</a>");
            } else {
                $("#sp" + "NonCMQuestionaryFile").html("empty");
            }
            $("#sp" + "NonCMQuestionaryUploadTime").html(new Date(CritialFile.NonCMQuestionaryTime).yyyymmddhhmmss());

            if (CritialFile.NonCMCommitFGUID != null && CritialFile.NonCMCommitFGUID != "undefined") {
                $("#sp" + "NonCMCommitFile").html("<a href='" + __WebAppPathPrefix + "/SQMBasic/DownloadSQMFile?DataKey=" + escape($.trim(CritialFile.NonCMCommitFGUID)) + "'>" + CritialFile.NonCMCommitFN + "</a>");
            } else {
                $("#sp" + "NonCMCommitFile").html("empty");
            }
            $("#sp" + "NonCMCommitUploadTime").html(new Date(CritialFile.NonCMCommitTime).yyyymmddhhmmss());

            
            
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {

        }
    });
}

function getCritalFilesData() {
    //load esist data
    var CritialFile;
    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetCriticalFile',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            CritialFile = data[0];
            //checkbox
            if (isNullOrFalse(CritialFile.IsWaterEmergencySquad)) {
                $("#cb" + "IsWaterEmergencySquad").removeAttr("checked");
            } else {
                $("#cb" + "IsWaterEmergencySquad").attr('checked', 'checked');
            }

            if (isNullOrFalse(CritialFile.IsWaterResponseProgram)) {
                $("#cb" + "IsWaterResponseProgram").removeAttr("checked");
            } else {
                $("#cb" + "IsWaterResponseProgram").attr('checked', 'checked');
            }

            if (isNullOrFalse(CritialFile.IsElecEmergencySquad)) {
                $("#cb" + "IsElecEmergencySquad").removeAttr("checked");
            } else {
                $("#cb" + "IsElecEmergencySquad").attr('checked', 'checked');
            }

            if (isNullOrFalse(CritialFile.IsElecResponseProgram)) {
                $("#cb" + "IsElecResponseProgram").removeAttr("checked");
            } else {
                $("#cb" + "IsElecResponseProgram").attr('checked', 'checked');
            }

            if (isNullOrFalse(CritialFile.IsCMControl)) {
                $("#cb" + "IsCMControl").removeAttr("checked");
            } else {
                $("#cb" + "IsCMControl").attr('checked', 'checked');
            }
            if (isNullOrFalse(CritialFile.IsNonCMQuestionary)) {
                $("#cb" + "IsNonCMQuestionary").removeAttr("checked");
            } else {
                $("#cb" + "IsNonCMQuestionary").attr('checked', 'checked');
            }
            if (isNullOrFalse(CritialFile.IsNonCMCommit)) {
                $("#cb" + "IsNonCMCommit").removeAttr("checked");
            } else {
                $("#cb" + "IsNonCMCommit").attr('checked', 'checked');
            }


        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {

        }
    });
}

Date.prototype.yyyymmdd = function () {
    var yyyy = this.getFullYear();
    var mm = this.getMonth() < 9 ? "0" + (this.getMonth() + 1) : (this.getMonth() + 1); // getMonth() is zero-based
    var dd = this.getDate() < 10 ? "0" + this.getDate() : this.getDate();
    return "".concat(yyyy).concat(mm).concat(dd);
};

Date.prototype.yyyy_mm_dd = function () {
    var yyyy = this.getFullYear();
    var mm = this.getMonth() < 9 ? "0" + (this.getMonth() + 1) : (this.getMonth() + 1); // getMonth() is zero-based
    var dd = this.getDate() < 10 ? "0" + this.getDate() : this.getDate();
    return "".concat(yyyy).concat('/').concat(mm).concat('/').concat(dd);
};

Date.prototype.yyyymmddhhmm = function () {
    var yyyy = this.getFullYear();
    var mm = this.getMonth() < 9 ? "0" + (this.getMonth() + 1) : (this.getMonth() + 1); // getMonth() is zero-based
    var dd = this.getDate() < 10 ? "0" + this.getDate() : this.getDate();
    var hh = this.getHours() < 10 ? "0" + this.getHours() : this.getHours();
    var min = this.getMinutes() < 10 ? "0" + this.getMinutes() : this.getMinutes();
    return "".concat(yyyy).concat(mm).concat(dd).concat(hh).concat(min);
};

Date.prototype.yyyymmddhhmmss = function () {
    var yyyy = this.getFullYear();
    var mm = this.getMonth() < 9 ? "0" + (this.getMonth() + 1) : (this.getMonth() + 1); // getMonth() is zero-based
    var dd = this.getDate() < 10 ? "0" + this.getDate() : this.getDate();
    var hh = this.getHours() < 10 ? "0" + this.getHours() : this.getHours();
    var min = this.getMinutes() < 10 ? "0" + this.getMinutes() : this.getMinutes();
    var ss = this.getSeconds() < 10 ? "0" + this.getSeconds() : this.getSeconds();
    return "".concat(yyyy).concat('-').concat(mm).concat('-').concat(dd).concat(' ').concat(hh).concat(':').concat(min).concat(':').concat(ss);
};