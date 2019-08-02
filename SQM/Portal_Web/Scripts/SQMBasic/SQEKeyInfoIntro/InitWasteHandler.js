﻿$(function () {
    //Toolbar Buttons
    $("#btnSearch"+"WasteHandler").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCreate" + "WasteHandler").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnViewEdit" + "WasteHandler").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnDelete" + "WasteHandler").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List

    $('#dialogData' + "WasteHandler").show();

    setBasicWasteHandlerEvent('License');
    setBasicWasteHandlerEvent('BusinessLicense');

    //get WasteHandler LicenseType
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
            //var options = '<option value=-1 Selected>All</option>';
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].PARAME_ITEM + '>' + '' + ' ' + data[idx].PARAME_VALUE1 + '</option>';
            }
            $("#ddl" + "LicenseType").append(options);
            //$("#ddl" + "LicenseType" + "").val(BasicData.ddlCategory).change();
            //$('#ddlCategory').change();
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
});



function GetWasteHandler(MemberGUID) {
    var cn = ['WHGUID',
            'LicenseType',
            '證書類型名稱',
            '證書資料Url',
            'FileName1',
            '资料上传日期',
            '商業證書資料Url',
            'FileName2',
            '资料上传日期'];
    var cm = [
            { name: 'WHGUID', "index": 'WHGUID', "width": 200, "sortable": false, "hidden": true },
            { name: 'LicenseType', "index": 'LicenseType', "width": 200, "sortable": false, "hidden": true },
            { name: 'LicenseTypeName', index: 'LicenseTypeName', width: 250, sortable: true, sorttype: 'text' },
            { name: 'FileUrlTag1', index: 'FileUrlTag1', width: 250, sortable: true, sorttype: 'text' },
            { name: 'FileName1', "index": 'FileName1', "width": 200, "sortable": false, "hidden": true },
            { name: 'UpdateTime1', index: 'UpdateTime1', width: 350, sortable: true, sorttype: 'text', formatter: timeFormatter },
            { name: 'FileUrlTag2', index: 'FileUrlTag2', width: 250, sortable: true, sorttype: 'text' },
            { name: 'FileName2', "index": 'FileName2', "width": 200, "sortable": false, "hidden": true },
            { name: 'UpdateTime2', index: 'UpdateTime2', width: 350, sortable: true, sorttype: 'text', formatter: timeFormatter }
    ];
var gridDataList = $("#gridDataList" + "WasteHandler");
gridDataList.jqGrid({
    url: __WebAppPathPrefix + '/SQMBasic/GetWasteHandler',
    postData: { "vendorCode": MemberGUID },
    type: "post",
    datatype: "json",
    jsonReader: {
        root: "Rows",
        page: "Page",
        total: "Total",
        records: "Records",
        repeatitems: false
    },
    width: 800,
    height: "auto",
    colNames: cn,
    colModel: cm,
    rowNum: 10,
    //rowList: [10, 20, 30],
    sortname: 'WHGUID',
    viewrecords: true,
    loadonce: true,
    mtype: 'POST',
    pager: '#gridListPager' + "WasteHandler",
    //sort by reload
    loadComplete: function () {
        var $this = $(this);
        if ($this.jqGrid('getGridParam', 'datatype') === 'json')
            if ($this.jqGrid('getGridParam', 'sortname') !== '')
                setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
    }
});
gridDataList.jqGrid('navGrid', '#gridListPager' + "WasteHandler", { edit: false, add: false, del: false, search: false, refresh: false });
}

function linkformatter( cellvalue, options, rowObject){
    return "<a href='" + "../SQMBasic/DownloadSQMFile?DataKey=123" + "'>" + cellvalue + '</a>';
}
function timeFormatter(cellvalue, options, rowObject) {
    var d = new Date(cellvalue).yyyymmddhhmmss();
    //var d = Date.parse(cellvalue).yyyymmdd;
    return d;
    //return d.toISOString().substring(0, 10);
}
function setBasicWasteHandlerEvent(fid) {
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

    //$('#btnProcess' + fid).click(function () {
    //    uploadFileEHS('fileupload_upload' + fid);
    //});
}

function uploadFileWasteHandler(fid, uploadType) {
    //var uploadType = '';

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
            url: __WebAppPathPrefix + "/SQMBasic/UploadWasteHandlerFile",
            data: {
                FA: ReqData,
                uploadType: uploadType
            },
            type: "post",
            //async: false,
            beforeSend: function () {
                $("#dialogDownloadSplash" + "").dialog({
                    title: 'Uploading Notify',
                    width: 'auto',
                    height: 'auto',
                    modal: true,
                    open: function (event, ui) {
                        $(this).parent().find('.ui-dialog-titlebar-close').hide();
                        $(this).parent().find('.ui-dialog-buttonpane').hide();
                        $("#lbDiaDownloadMsg" + "").html('Uploading...</br></br>Please wait for the operation a moment...');
                    }
                }).dialog("open");
            },
            success: function (data) {
                getFilesDetail();
                $("#dialogDownloadSplash" + "").dialog('close');
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
                        $('#tdErrorFileDownload' + "WasteHandler").children().remove();
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
                $("#dialogDownloadSplash" + "WasteHandler").dialog('close');
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, testStatus) {
                $.reInit_fileupalod(fid);
            }
        });
    }
}