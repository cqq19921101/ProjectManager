//initial dialog
$(function () {
    var dialog = $("#dialogData" + "WasteHandler");
    
    //Toolbar Buttons
    $("#btndialogEditData" + "WasteHandler").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogCancelEdit"+"WasteHandler").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#dialogData" + "WasteHandler").dialog({
        autoOpen: false,
        height: 345,
        width: 450,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (dialog.attr('Mode') == "v") {
                    $(this).dialog("close");
                }
                else {
                    var fid = 'fileupload_upload' + 'License';
                    var fidBL = 'fileupload_upload' + 'BusinessLicense';
                    var uploadType = '';

                    if ($.get_uploadedFileInfo(fid).length == 0 && dialog.attr('Mode') == "c") {
                        alert('Please select the file to upload.');
                    }
                    else if ($.get_uploadedFileInfo(fidBL).length == 0 && dialog.attr('Mode') == "c") {
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
                        //ReqData = $.extend(ReqData, { "Spec": (dialog.attr('Mode') == "c" ? JSON.stringify($.get_uploadedFileInfo(fid)) : "") });
                        //ReqData = $.extend(ReqData, { "SubFolder": (dialog.attr('Mode') == "c" ? _SubFolderForUploadExcel : "") });
                        var ReqDataBL = '';
                        //ReqData = $.extend(ReqData, { "Spec": JSON.stringify($.UploadFiles_UPloadifyF32_Type1_RetrieveFileInfo("Spec_FileUpload_FileList")) });
                        ReqDataBL = $.extend(ReqDataBL, { "Spec": JSON.stringify($.get_uploadedFileInfo(fidBL)) });
                        ReqDataBL = $.extend(ReqDataBL, { "SubFolder": _SubFolderForUploadExcel });

                        var DoSuccessfully = false;
                        var DataItem = {
                            "WHGUID": dialog.attr("WHGUID")
                           ,"LicenseType": escape($.trim($("#ddl" + "LicenseType").val()))
                        };
                        $.ajax({
                            url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMBasic/CreateWasteHandler" : "/SQMBasic/EditWasteHandler"),
                            data: {
                                "DataItem": DataItem,
                                "FA": ReqData,
                                "FABL": ReqDataBL
                            },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    DoSuccessfully = true;
                                    if (dialog.attr('Mode') == "c")
                                        alert("WasteHandler create successfully.");
                                    else
                                        alert("WasteHandler edit successfully.");
                                }
                                else {
                                    if ((dialog.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                        alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                        DoSuccessfully = true;
                                    }
                                    else
                                        $("#lblDiaErrMsg" + "WasteHandler").html(data);
                                }
                            },
                            error: function (xhr, textStatus, thrownError) {
                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                            },
                            complete: function (jqXHR, textStatus) {
                                //$("#ajaxLoading").hide();
                                $("#fileupload_upload" + "WasteHandler").find("button[class=delete]").click();
                            }
                        });
                        if (DoSuccessfully) {
                            $(this).dialog("close");
                            $("#btnSearch" + "WasteHandler").click();
                        }
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialog.attr('Mode') == "e") {
                //var r = ReleaseDataLock(dialog.attr('WHGUID'));
                //if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });
});

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByModeWasteHandler(Mode) {
    var dialog = $("#dialogData" + "WasteHandler");
    var gridDataList = $("#gridDataList" + "WasteHandler");
    switch (Mode) {
        case "c": //Create
            $("#dialogDataToolBar" + "WasteHandler").hide();

            dialog.attr('ItemRowId', "");
            dialog.attr('WHGUID', "");

            $("#ddl" + "LicenseType").removeAttr('disabled');

            $("#txt" + "LicenseFileName").attr("disabled", "disabled");
            $("#txt" + "LicenseFileName").val("");
            $("#txt" + "LicenseUpdateTime").attr("disabled", "disabled");
            $("#txt" + "LicenseUpdateTime").val("");

            $("#txt" + "BusinessLicenseFileName").attr("disabled", "disabled");
            $("#txt" + "BusinessLicenseFileName").val("");
            $("#txt" + "BusinessLicenseUpdateTime").attr("disabled", "disabled");
            $("#txt" + "BusinessLicenseUpdateTime").val("");

            $("#lblDiaErrMsg").html("");
            $("#fileupload_upload" + "License").find("[type=file]").removeAttr('disabled');
            $("#fileupload_upload" + "BusinessLicense").find("[type=file]").removeAttr('disabled');
            break;
        case "v": //View
            $("#btndialogEditData" + "WasteHandler").button("option", "disabled", false);
            $("#btndialogCancelEdit" + "WasteHandler").button("option", "disabled", true);
            $("#dialogDataToolBar" + "WasteHandler").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('WHGUID', dataRow.WHGUID);

            $("#ddl" + "LicenseType").attr('disabled', 'disabled');
            $("#ddl" + "LicenseType").val(dataRow.LicenseType);

            $("#txt" + "LicenseFileName").val(dataRow.FileName1);
            $("#txt" + "LicenseUpdateTime").val(dataRow.UpdateTime1);
            $("#txt" + "BusinessLicenseFileName").val(dataRow.FileName2);
            $("#txt" + "BusinessLicenseUpdateTime").val(dataRow.UpdateTime2);

            $("#txt" + "WasteHandlerOccurredTime").attr("disabled", "disabled");

            $("#txt" + "LicenseFileName").attr("disabled", "disabled");
            $("#txt" + "LicenseUpdateTime").attr("disabled", "disabled");
            $("#txt" + "BusinessLicenseFileName").attr("disabled", "disabled");
            $("#txt" + "BusinessLicenseUpdateTime").attr("disabled", "disabled");

            $("#lblDiaErrMsg" + "WasteHandler").html("");
            $("#fileupload_upload" + "License").find("[type=file]").attr("disabled", "disabled");
            $("#fileupload_upload" + "BusinessLicense").find("[type=file]").attr("disabled", "disabled");
            break;
        default: //Edit("e")
            $("#btndialogEditData" + "WasteHandler").button("option", "disabled", true);
            $("#btndialogCancelEdit" + "WasteHandler").button("option", "disabled", false);
            $("#dialogDataToolBar" + "WasteHandler").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('WHGUID', dataRow.WHGUID);

            $("#ddl" + "LicenseType").removeAttr('disabled');
            $("#ddl" + "LicenseType").val(dataRow.LicenseType);

            $("#txt" + "LicenseFileName").val(dataRow.FileName1);
            $("#txt" + "LicenseUpdateTime").val(dataRow.UpdateTime1);
            $("#txt" + "BusinessLicenseFileName").val(dataRow.FileName2);
            $("#txt" + "BusinessLicenseUpdateTime").val(dataRow.UpdateTime2);

            $("#txt" + "WasteHandlerOccurredTime").removeAttr('disabled');

            $("#txt" + "LicenseFileName").removeAttr('disabled');
            $("#txt" + "LicenseUpdateTime").removeAttr('disabled');
            $("#txt" + "BusinessLicenseFileName").removeAttr('disabled');
            $("#txt" + "BusinessLicenseUpdateTime").removeAttr('disabled');

            $("#lblDiaErrMsg" + "WasteHandler").html("");
            $("#fileupload_upload" + "License").find("[type=file]").removeAttr('disabled');
            $("#fileupload_upload" + "BusinessLicense").find("[type=file]").removeAttr('disabled');

            //$("#fileupload_upload" + "WasteHandler").find("[type=file]").removeAttr('disabled');
            break;
    }
}