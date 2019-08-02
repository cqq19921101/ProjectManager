$(function () {
    $("#tbKeyInfoType").show();
    $("#tbInfoType").hide();
    $("#btnSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCombSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });

    //plantname ddl
    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetPlantList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '<option value="">-- 請選擇 --</option>';
            for (var idx in data) {
                options += '<option value=' + data[idx].PlantCode + '>' + data[idx].PlantCode + ' ' + data[idx].PlantName + '</option>';
            }
            $('#ddlPlant').append(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
    var gridDataList = $("#gridDataListKeyInfoType");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMBasic/LoadReliInfo',
        postData: {},
        type: "post",
        datatype: "json",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        width: 400,
        height: "auto",
        colNames: [
                    '部門代碼',
                    'MemberGUID',
                    '部門名稱',
                    '供應商名稱',
                    '供應商經理'

        ],
        colModel: [
            { name: 'Plant', index: 'Plant', width: 200, sortable: true, sorttype: 'text' },
            { name: 'MemberGUID', index: 'MemberGUID', width: 200, sortable: false, hidden: true },
            { name: 'plant_name', index: 'plant_name', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ERP_VNAME', index: 'ERP_VNAME', width: 150, sortable: true, sorttype: 'text' },
            { name: 'NameInChinese', index: 'NameInChinese', width: 150, sortable: true, sorttype: 'text' }
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        //sortname: '',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridListPagerKeyInfoType',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataList.jqGrid('navGrid', '#gridListPagerKeyInfoType', { edit: false, add: false, del: false, search: false, refresh: false });


    $("#btnViewEditDetail").button({
        label: "EditDetail",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnViewEditDetail").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            $("#ihBasicInfoGUID").val(gridDataList.jqGrid('getRowData', RowId).MemberGUID);
        } else { alert("Please select a row data to edit."); }
        getPollutionTypes();
        getWasteTypes();
        getCommonTypes();
        getPeriodType();
        getCritalFilesData();
        getFilesDetail();
        GetWasteHandler(gridDataList.jqGrid('getRowData', RowId).MemberGUID);
        GetQualityEvent(gridDataList.jqGrid('getRowData', RowId).MemberGUID);
        $("#tbKeyInfoType").hide();
        $("#tbInfoType").show();
    });
    $("#btnBack").button({
        label: "BACK",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnBack").click(function () {
        $("#tbKeyInfoType").show();
        $("#tbInfoType").hide();


    });
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
        $("#divQualityEvent").show();
    });

    $("#btnEHS").button({
        label: "供應商環境安全管理調查表",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnEHS").click(function () {
        $(this).removeClass('ui-state-focus');
        $("[edittpye='Files']").each(function (index) {
            $(this).hide();
        });
        $("#divEHS").show();
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

    setBasicEvent('Intro', '1',false);
    setBasicEvent('Envir', '2', false);
    setBasicEvent('WaterResponse', '3', false);
    setBasicEvent('ElecResponse', '4', false);
    setBasicEvent('CMControl', '5', false);
    setBasicEvent('NonCMQuestionary', '6', false);
    setBasicEvent('NonCMCommit', '7', false);

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
    initWHS();
})
//function setBasicEvent(fid, type) {
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

function setBasicEvent(fid, type,isValidDate) {
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
    if (isValidDate) {
        $('#btnProcess' + fid).click(function () {
            uploadFileSQM('fileupload_upload' + fid, type,'txt' + fid+'ValidDate');
        });
    } else {
        $('#btnProcess' + fid).click(function () {
            uploadFileSQM('fileupload_upload' + fid, type,'txt' + fid+'ValidDate');
        });
    }
    
}


function uploadFileSQM(fid, type,validFid) {
    var uploadType = '';

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
            url: __WebAppPathPrefix + "/SQMBasic/UploadIntroFile",
            data: {
                FA: ReqData,
                Type: type,
                ValidDate: ($('#' + validFid).length > 0 ? $('#' + validFid).val() : "")
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
                getFilesDetail();
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
    if (value == "undefined" || value == null || value == false) {
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
         data: { "vendorCode": escape($("#ihBasicInfoGUID").val()) },
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {  
          
            
            $("#txt" + "PollutionReportValidDate").attr("disabled", "disabled");
            $("#txt" + "PollutionAgreeValidDate").attr("disabled", "disabled");
            $("#txt" + "PollutionAcceptanceValidDate").attr("disabled", "disabled");
            $("#txt" + "WasteValidDate").attr("disabled", "disabled");
            $("#cb" + "IsOutWasteHandler").attr("disabled", "disabled");
            $("#ddl" + "ExhaustType").attr("disabled", "disabled");
            $("#txt" + "ExhaustValidDate").attr("disabled", "disabled");
            $("#ddl" + "WasteWaterType").attr("disabled", "disabled");
            $("#txt" + "WasteWaterValidDate").attr("disabled", "disabled");
            $("#ddl" + "NoiseType").attr("disabled", "disabled");
            $("#txt" + "NoiseValidDate").attr("disabled", "disabled");
            $("#ddl" + "PollutionEventType").attr("disabled", "disabled");
            $("#ddl" + "PunishmentType").attr("disabled", "disabled");
         
            if (data.length > 0) {
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

                if (CritialFile.PollutionReportFGUID != null && CritialFile.PollutionReportFGUID != "undefined") {
                    $("#sp" + "PollutionReportFile").html("<a href='" + __WebAppPathPrefix + "/SQMBasic/DownloadSQMFile?DataKey=" + escape($.trim(CritialFile.PollutionReportFGUID)) + "'>" + CritialFile.PollutionReportFN + "</a>");
                } else {
                    $("#sp" + "PollutionReportFile").html("empty");
                }
                $("#sp" + "PollutionReportUploadTime").html(new Date(CritialFile.PollutionReportTime).yyyymmddhhmmss());
                $("#txt" + "PollutionReportValidDate").val(new Date(CritialFile.PollutionReportValidDate).yyyymmdd());

                if (CritialFile.PollutionAgreeFGUID != null && CritialFile.PollutionAgreeFGUID != "undefined") {
                    $("#sp" + "PollutionAgreeFile").html("<a href='" + __WebAppPathPrefix + "/SQMBasic/DownloadSQMFile?DataKey=" + escape($.trim(CritialFile.PollutionAgreeFGUID)) + "'>" + CritialFile.PollutionAgreeFN + "</a>");
                } else {
                    $("#sp" + "PollutionAgreeFile").html("empty");
                }
                $("#sp" + "PollutionAgreeUploadTime").html(new Date(CritialFile.PollutionAgreeTime).yyyymmddhhmmss());
                $("#txt" + "PollutionAgreeValidDate").val(new Date(CritialFile.PollutionAgreeValidDate).yyyymmdd());

                if (CritialFile.PollutionAcceptanceFGUID != null && CritialFile.PollutionAcceptanceFGUID != "undefined") {
                    $("#sp" + "PollutionAcceptanceFile").html("<a href='" + __WebAppPathPrefix + "/SQMBasic/DownloadSQMFile?DataKey=" + escape($.trim(CritialFile.PollutionAcceptanceFGUID)) + "'>" + CritialFile.PollutionAcceptanceFN + "</a>");
                } else {
                    $("#sp" + "PollutionAcceptanceFile").html("empty");
                }
                $("#sp" + "PollutionAcceptanceUploadTime").html(new Date(CritialFile.PollutionAcceptanceTime).yyyymmddhhmmss());
                $("#txt" + "PollutionAcceptanceValidDate").val(new Date(CritialFile.PollutionAcceptanceValidDate).yyyymmdd());

                if (CritialFile.WasteFGUID != null && CritialFile.WasteFGUID != "undefined") {
                    $("#sp" + "WasteFile").html("<a href='" + __WebAppPathPrefix + "/SQMBasic/DownloadSQMFile?DataKey=" + escape($.trim(CritialFile.WasteFGUID)) + "'>" + CritialFile.WasteFN + "</a>");
                } else {
                    $("#sp" + "WasteFile").html("empty");
                }
                $("#sp" + "WasteUploadTime").html(new Date(CritialFile.WasteTime).yyyymmddhhmmss());
                $("#txt" + "WasteValidDate").val(new Date(CritialFile.WasteValidDate).yyyymmdd());

                if (CritialFile.ExhaustFGUID != null && CritialFile.ExhaustFGUID != "undefined") {
                    $("#sp" + "ExhaustFile").html("<a href='" + __WebAppPathPrefix + "/SQMBasic/DownloadSQMFile?DataKey=" + escape($.trim(CritialFile.ExhaustFGUID)) + "'>" + CritialFile.ExhaustFN + "</a>");
                } else {
                    $("#sp" + "ExhaustFile").html("empty");
                }
                $("#sp" + "ExhaustUploadTime").html(new Date(CritialFile.ExhaustTime).yyyymmddhhmmss());
                $("#txt" + "ExhaustValidDate").val(new Date(CritialFile.ExhaustValidDate).yyyymmdd());

                if (CritialFile.WasteWaterFGUID != null && CritialFile.WasteWaterFGUID != "undefined") {
                    $("#sp" + "WasteWaterFile").html("<a href='" + __WebAppPathPrefix + "/SQMBasic/DownloadSQMFile?DataKey=" + escape($.trim(CritialFile.WasteWaterFGUID)) + "'>" + CritialFile.WasteWaterFN + "</a>");
                } else {
                    $("#sp" + "WasteWaterFile").html("empty");
                }
                $("#sp" + "WasteWaterUploadTime").html(new Date(CritialFile.WasteWaterTime).yyyymmddhhmmss());
                $("#txt" + "WasteWaterValidDate").val(new Date(CritialFile.WasteWaterValidDate).yyyymmdd());

                if (CritialFile.NoiseFGUID != null && CritialFile.NoiseFGUID != "undefined") {
                    $("#sp" + "NoiseFile").html("<a href='" + __WebAppPathPrefix + "/SQMBasic/DownloadSQMFile?DataKey=" + escape($.trim(CritialFile.NoiseFGUID)) + "'>" + CritialFile.NoiseFN + "</a>");
                } else {
                    $("#sp" + "NoiseFile").html("empty");
                }
                $("#sp" + "NoiseUploadTime").html(new Date(CritialFile.NoiseTime).yyyymmddhhmmss());
                $("#txt" + "NoiseValidDate").val(new Date(CritialFile.NoiseValidDate).yyyymmdd());
            }
            else {
                $("#sp" + "IntroFile").html("empty");
                $("#sp" + "EnvirFile").html("empty");
                $("#sp" + "WaterResponseFile").html("empty");
                $("#sp" + "ElecResponseFile").html("empty");
                $("#sp" + "CMControlFile").html("empty");
                $("#sp" + "NonCMQuestionaryFile").html("empty");
                $("#sp" + "NonCMCommitFile").html("empty");
                $("#sp" + "PollutionReportFile").html("empty");
                $("#sp" + "PollutionAgreeFile").html("empty");
                $("#sp" + "PollutionAcceptanceFile").html("empty");
                $("#sp" + "WasteFile").html("empty");
                $("#sp" + "ExhaustFile").html("empty");
                $("#sp" + "WasteWaterFile").html("empty");
                $("#sp" + "NoiseFile").html("empty");
                $("#sp" + "IntroUploadTime").html("");
                $("#sp" + "EnvirUploadTime").html("");
                $("#sp" + "WaterResponseUploadTime").html("");
                $("#sp" + "ElecResponseUploadTime").html("");
                $("#sp" + "CMControlUploadTime").html("");
                $("#sp" + "NonCMQuestionaryUploadTime").html("");
                $("#sp" + "NonCMCommitUploadTime").html("");
                $("#sp" + "PollutionReportUploadTime").html("");
                $("#txt" + "PollutionReportValidDate").val("");
                $("#sp" + "PollutionAgreeUploadTime").html("");
                $("#txt" + "PollutionAgreeValidDate").val("");
                $("#sp" + "PollutionAcceptanceUploadTime").html("");
                $("#txt" + "PollutionAcceptanceValidDate").val("");
                $("#sp" + "WasteUploadTime").html("");
                $("#txt" + "WasteValidDate").val("");
                $("#sp" + "ExhaustUploadTime").html("");
                $("#txt" + "ExhaustValidDate").val("");
                $("#sp" + "WasteWaterUploadTime").html("");
                $("#txt" + "WasteWaterValidDate").val("");
                $("#sp" + "NoiseUploadTime").html("");
                $("#txt" + "NoiseValidDate").val("");


            }
            
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
        data: { "vendorCode": escape($("#ihBasicInfoGUID").val()) },
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
           
            //checkbox
            $("#cb" + "IsWaterEmergencySquad").attr("disabled", "disabled");
            $("#cb" + "IsWaterResponseProgram").attr("disabled", "disabled");
            $("#cb" + "IsElecEmergencySquad").attr("disabled", "disabled");
            $("#cb" + "IsElecResponseProgram").attr("disabled", "disabled");
            $("#cb" + "IsCMControl").attr("disabled", "disabled");
            $("#cb" + "IsNonCMQuestionary").attr("disabled", "disabled");
            $("#cb" + "IsNonCMCommit").attr("disabled", "disabled");
            $("#txt" + "EnvirSignedTime").attr("disabled", "disabled");
            $("#txt" + "EHSOwner").attr("disabled", "disabled");
            $("#txt" + "EHSOwnerTitle").attr("disabled", "disabled");
            $("#txt" + "PollutionEventDesc").attr("disabled", "disabled");
            $("#txt" + "PunishmentDesc").attr("disabled", "disabled");
            
            if (data.length > 0) {
                CriticalFile = data[0];     
                if (isNullOrFalse(CriticalFile.IsWaterEmergencySquad)) {
                    $("#cb" + "IsWaterEmergencySquad").removeAttr("checked");
                } else {
                    $("#cb" + "IsWaterEmergencySquad").attr('checked', 'checked');
                }
                
                if (isNullOrFalse(CriticalFile.IsWaterResponseProgram)) {
                    $("#cb" + "IsWaterResponseProgram").removeAttr("checked");
                } else {
                    $("#cb" + "IsWaterResponseProgram").attr('checked', 'checked');
                }
               
                if (isNullOrFalse(CriticalFile.IsElecEmergencySquad)) {
                    $("#cb" + "IsElecEmergencySquad").removeAttr("checked");
                } else {
                    $("#cb" + "IsElecEmergencySquad").attr('checked', 'checked');
                }
               
                if (isNullOrFalse(CriticalFile.IsElecResponseProgram)) {
                    $("#cb" + "IsElecResponseProgram").removeAttr("checked");
                } else {
                    $("#cb" + "IsElecResponseProgram").attr('checked', 'checked');
                }
               
                if (isNullOrFalse(CriticalFile.IsCMControl)) {
                    $("#cb" + "IsCMControl").removeAttr("checked");
                } else {
                    $("#cb" + "IsCMControl").attr('checked', 'checked');
                }
                if (isNullOrFalse(CriticalFile.IsNonCMQuestionary)) {
                    $("#cb" + "IsNonCMQuestionary").removeAttr("checked");
                } else {
                    $("#cb" + "IsNonCMQuestionary").attr('checked', 'checked');
                }
                if (isNullOrFalse(CriticalFile.IsNonCMCommit)) {
                    $("#cb" + "IsNonCMCommit").removeAttr("checked");
                } else {
                    $("#cb" + "IsNonCMCommit").attr('checked', 'checked');
                }

                //colume data
                if (isNullOrFalse(CriticalFile.EnvirSignedTime) == false) {
                    $("#txt" + "EnvirSignedTime").val(new Date(CriticalFile.EnvirSignedTime).yyyy_mm_dd());
                }
                if (isNullOrFalse(CriticalFile.EHSOwner) == false) {
                    $("#txt" + "EHSOwner").val(CriticalFile.EHSOwner);
                }
                if (isNullOrFalse(CriticalFile.EHSOwnerTitle) == false) {
                    $("#txt" + "EHSOwnerTitle").val(CriticalFile.EHSOwnerTitle);
                }
                if (isNullOrFalse(CriticalFile.PollutionEventDesc) == false) {
                    $("#txt" + "PollutionEventDesc").val(CriticalFile.PollutionEventDesc);
                }
                if (isNullOrFalse(CriticalFile.PunishmentDesc) == false) {
                    $("#txt" + "PunishmentDesc").val(CriticalFile.PunishmentDesc);
                }

                //multi checkbox
                if (isNullOrFalse(CriticalFile.PollutionTypes) == false) {
                    var aryPollutionTypes = CriticalFile.PollutionTypes.split(';');
                    for (var i = 0; i < aryPollutionTypes.length; i++) {
                        var PollutionType = aryPollutionTypes[i];
                        if (isNullOrFalse(PollutionType) == false) {
                            $("#cb" + "PollutionType" + PollutionType).attr('checked', 'checked');
                        }
                    }
                }

                if (isNullOrFalse(CriticalFile.WasteTypes) == false) {
                    var aryWasteTypes = CriticalFile.WasteTypes.split(';');
                    for (var i = 0; i < aryWasteTypes.length; i++) {
                        var WasteType = aryWasteTypes[i];
                        if (isNullOrFalse(WasteType) == false) {
                            $("#cb" + "WasteTypes" + WasteType).attr('checked', 'checked');

                        }
                    }
                }

                //ddl
                $("#ddl" + "ExhaustType").val(CriticalFile.ExhaustType).change();
                $("#ddl" + "WasteWaterType").val(CriticalFile.WasteWaterType).change();
                $("#ddl" + "NoiseType").val(CriticalFile.NoiseType).change();
                $("#ddl" + "PollutionEventType").val(CriticalFile.PollutionEventType).change();
                $("#ddl" + "PunishmentType").val(CriticalFile.PunishmentType).change();
            }
            else {
                $("#cb" + "IsWaterEmergencySquad").removeAttr("checked");
                $("#cb" + "IsWaterResponseProgram").removeAttr("checked");
                $("#cb" + "IsElecEmergencySquad").removeAttr("checked");
                $("#cb" + "IsElecResponseProgram").removeAttr("checked");
                $("#cb" + "IsCMControl").removeAttr("checked");
                $("#cb" + "IsNonCMQuestionary").removeAttr("checked");
                $("#cb" + "IsNonCMCommit").removeAttr("checked");
                $("#txt" + "EnvirSignedTime").val("");
                $("#txt" + "EHSOwner").val("");
                $("#txt" + "EHSOwnerTitle").val("");
                $("#txt" + "PollutionEventDesc").val("");
                $("#txt" + "PunishmentDesc").val("");
                
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {

        }
    });
}
function getPollutionTypes() {
    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetParam',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        type: "post",
        data: {
            "APPLICATION_NAME": "SQM"
            , "FUNCTION_NAME": "EHS"
            , "PARAME_NAME": "PollutionType"
        },
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '';
            for (var idx in data) {
                options += '<input id="cbPollutionType' + data[idx].PARAME_ITEM + '" cbtype="PollutionType" onclick="return false;" type="checkbox"  name="vehicle" value="' + data[idx].PARAME_ITEM + '" >' + data[idx].PARAME_VALUE1 + '<BR/>'
            }
            //<input id="TradingCurrencyCNY" cbtype="TradingCurrency" type="checkbox" name="vehicle" value="CNY" checked="checked">
            //$("#td" + "PollutionTypes").append(options);
            $("#td" + "PollutionTypes").html(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}
function getWasteTypes() {
    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetParam',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        type: "post",
        data: {
            "APPLICATION_NAME": "SQM"
            , "FUNCTION_NAME": "EHS"
            , "PARAME_NAME": "WasteType"
        },
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '';
            for (var idx in data) {
                options += '<input id="cbWasteTypes' + data[idx].PARAME_ITEM + '" cbtype="WasteTypes" onclick="return false;" type="checkbox" value="' + data[idx].PARAME_ITEM + '" >' + data[idx].PARAME_VALUE1 + '<BR/>'
            }
            $("#td" + "WasteTypes").html(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

function getCommonTypes() {
    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetParam',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        type: "post",
        data: {
            "APPLICATION_NAME": "SQM"
            , "FUNCTION_NAME": "EHS"
            , "PARAME_NAME": "AnswerType"
        },
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].PARAME_ITEM + '>' + data[idx].PARAME_VALUE1 + '</option>';
            }
            $("#ddl" + "ExhaustType").html(options);
            $("#ddl" + "WasteWaterType").html(options);
            $("#ddl" + "NoiseType").html(options);

        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

function getPeriodType() {
    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetParam',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        type: "post",
        data: {
            "APPLICATION_NAME": "SQM"
            , "FUNCTION_NAME": "EHS"
            , "PARAME_NAME": "PeriodType"
        },
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].PARAME_ITEM + '>' + data[idx].PARAME_VALUE1 + '</option>';
            }
            $("#ddl" + "PollutionEventType").html(options);
            $("#ddl" + "PunishmentType").html(options);

        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}