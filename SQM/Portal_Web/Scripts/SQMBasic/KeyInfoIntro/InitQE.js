$(function () {
    //Toolbar Buttons
    $("#btnSearch"+"QE").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCreate" + "QE").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnViewEdit" + "QE").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnDelete" + "QE").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //QEGUID
    //QualityEventOccurredTime//
    //QualityEventSummary
    //QualityEventProvideTime
    //QEFGUID
    //FileName
    //{ name: 'UpdateTime', index: 'UpdateTime', width: 250, sortable: true, sorttype: 'text', formatter: linkformatter }
    //Data List
    var cn = ['QEGUID',
                'QEFGUID',
                '品质事件发生时间',
                '品质事件概述',
                '保证函提供日期',
                '文件Url',
                'FileName',
                '资料上传日期'];
    var cm = [
            { name: 'QEGUID', "index": 'QEGUID', "width": 200, "sortable": false, "hidden": true },
            { name: 'QEFGUID', "index": 'QEFGUID', "width": 200, "sortable": false, "hidden": true },
            { name: 'QualityEventOccurredTime', index: 'QualityEventOccurredTime', width: 250, sortable: true, sorttype: 'text' },
            { name: 'QualityEventSummary', index: 'QualityEventSummary', width: 250, sortable: true, sorttype: 'text' },
            { name: 'QualityEventProvideTime', index: 'QualityEventProvideTime', width: 250, sortable: true, sorttype: 'text' },
            { name: 'FileUrlTag', index: 'FileUrlTag', width: 250, sortable: true, sorttype: 'text' },
            { name: 'FileName', "index": 'FileName', "width": 200, "sortable": false, "hidden": true },
            { name: 'UpdateTime', index: 'UpdateTime', width: 350, sortable: true, sorttype: 'text', formatter: timeFormatter }
    ];


    var gridDataList = $("#gridDataList" + "QE");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMBasic/GetQualityEvent',
        postData: { SearchText: "" },
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
        sortname: 'QualityEventOccurredTime',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridListPager' + "QE",
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataList.jqGrid('navGrid', '#gridListPager' + "QE", { edit: false, add: false, del: false, search: false, refresh: false });

    $('#dialogData' + "QE").show();

    setBasicQEEvent('QE');
});
function linkformatter( cellvalue, options, rowObject){
    return "<a href='" + "../SQMBasic/DownloadSQMFile?DataKey=123" + "'>" + cellvalue + '</a>';
}
function timeFormatter(cellvalue, options, rowObject) {
    var d = new Date(cellvalue).yyyymmddhhmmss();
    //var d = Date.parse(cellvalue).yyyymmdd;
    return d;
    //return d.toISOString().substring(0, 10);
}
function setBasicQEEvent(fid) {
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
    //    uploadFileQE('fileupload_upload' + fid);
    //});
}

function uploadFileQE(fid) {
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
            url: __WebAppPathPrefix + "/SQMBasic/UploadQEFile",
            data: {
                FA: ReqData
            },
            type: "post",
            //async: false,
            beforeSend: function () {
                $("#dialogDownloadSplash" + "QE").dialog({
                    title: 'Uploading Notify',
                    width: 'auto',
                    height: 'auto',
                    modal: true,
                    open: function (event, ui) {
                        $(this).parent().find('.ui-dialog-titlebar-close').hide();
                        $(this).parent().find('.ui-dialog-buttonpane').hide();
                        $("#lbDiaDownloadMsg" + "QE").html('Uploading...</br></br>Please wait for the operation a moment...');
                    }
                }).dialog("open");
            },
            success: function (data) {
                getFilesDetail();
                $("#dialogDownloadSplash" + "QE").dialog('close');
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
                        $('#tdErrorFileDownload' + "QE").children().remove();
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
                $("#dialogDownloadSplash" + "QE").dialog('close');
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, testStatus) {
                $.reInit_fileupalod(fid);
            }
        });
    }
}