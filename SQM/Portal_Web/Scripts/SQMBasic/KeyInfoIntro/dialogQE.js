//initial dialog
$(function () {
    var dialog = $("#dialogData" + "QE");
    
    //Toolbar Buttons
    $("#btndialogEditData" + "QE").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogCancelEdit"+"QE").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#dialogData" + "QE").dialog({
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
                    var fid = 'fileupload_upload' + 'QE';
                    var uploadType = '';

                    if ($.get_uploadedFileInfo(fid).length == 0 && dialog.attr('Mode') == "c") {
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

                        var DoSuccessfully = false;
                        var DataItem = {
                            "QEGUID": dialog.attr("QEGUID"),
                            "QualityEventOccurredTime": escape($.trim($("#txt" + "QualityEventOccurredTime").val())),
                            "QualityEventSummary": escape($.trim($("#txt" + "QualityEventSummary").val())),
                            "QualityEventProvideTime": escape($.trim($("#txt" + "QualityEventProvideTime").val()))
                        };
                        $.ajax({
                            url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMBasic/CreateQualityEvent" : "/SQMBasic/EditQualityEvent"),
                            data: {
                                "DataItem": DataItem,
                                "FA": ReqData
                            },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    DoSuccessfully = true;
                                    if (dialog.attr('Mode') == "c")
                                        alert("QualityEvent create successfully.");
                                    else
                                        alert("QualityEvent edit successfully.");
                                }
                                else {
                                    if ((dialog.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                        alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                        DoSuccessfully = true;
                                    }
                                    else
                                        $("#lblDiaErrMsg" + "QE").html(data);
                                }
                            },
                            error: function (xhr, textStatus, thrownError) {
                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                            },
                            complete: function (jqXHR, textStatus) {
                                //$("#ajaxLoading").hide();
                                $("#fileupload_upload" + "QE").find("button[class=delete]").click();
                            }
                        });
                        if (DoSuccessfully) {
                            $(this).dialog("close");
                            $("#btnSearch" + "QE").click();
                        }
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialog.attr('Mode') == "e") {
                //var r = ReleaseDataLock(dialog.attr('QEGUID'));
                //if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });
});

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByModeQE(Mode) {
    var dialog = $("#dialogData" + "QE");
    var gridDataList = $("#gridDataList" + "QE");
    switch (Mode) {
        case "c": //Create
            $("#dialogDataToolBar" + "QE").hide();

            dialog.attr('ItemRowId', "");
            dialog.attr('QEGUID', "");

            $("#txt" +"QualityEventOccurredTime").val("");
            $("#txt" +"QualityEventOccurredTime").removeAttr('disabled');
            $("#txt" +"QualityEventSummary").val("");
            $("#txt" +"QualityEventSummary").removeAttr('disabled');
            $("#txt" +"QualityEventProvideTime").val("");
            $("#txt" + "QualityEventProvideTime").removeAttr('disabled');

            $("#txtQEFileName").attr("disabled", "disabled");
            $("#txtQEUpdateTime").attr("disabled", "disabled");
            $("#txtQEFileName").val("");
            $("#txtQEUpdateTime").val("");
            $("#lblDiaErrMsg").html("");
            $("#fileupload_upload" + "QE").find("[type=file]").removeAttr('disabled');
            break;
        case "v": //View
            $("#btndialogEditData" + "QE").button("option", "disabled", false);
            $("#btndialogCancelEdit" + "QE").button("option", "disabled", true);
            $("#dialogDataToolBar" + "QE").show();

            $("#QualityEventOccurredTime").attr('disabled', 'disabled');
            $("#QualityEventSummary").attr('disabled', 'disabled');
            $("#QualityEventProvideTime").attr('disabled', 'disabled');

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('QEGUID', dataRow.QEGUID);

            $("#txtQualityEventOccurredTime").val(dataRow.QualityEventOccurredTime);
            $("#txtQualityEventSummary").val(dataRow.QualityEventSummary);
            $("#txtQualityEventProvideTime").val(dataRow.QualityEventProvideTime);
            $("#txtQEFileName").val(dataRow.FileName);
            $("#txtQEUpdateTime").val(dataRow.UpdateTime);

            $("#txtQualityEventOccurredTime").attr("disabled", "disabled");
            $("#txtQualityEventSummary").attr("disabled", "disabled");
            $("#txtQualityEventProvideTime").attr("disabled", "disabled");
            $("#txtQEFileName").attr("disabled", "disabled");
            $("#txtQEUpdateTime").attr("disabled", "disabled");

            $("#lblDiaErrMsg" + "QE").html("");
            $("#fileupload_upload" + "QE").find("[type=file]").attr("disabled", "disabled");
            break;
        default: //Edit("e")
            $("#btndialogEditData" + "QE").button("option", "disabled", true);
            $("#btndialogCancelEdit" + "QE").button("option", "disabled", false);
            $("#dialogDataToolBar" + "QE").show();

            $("#txtQualityEventOccurredTime").removeAttr('disabled');
            $("#txtQualityEventSummary").removeAttr('disabled');
            $("#txtQualityEventProvideTime").removeAttr('disabled');

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('QEGUID', dataRow.QEGUID);

            $("#txtQualityEventOccurredTime").val(dataRow.QualityEventOccurredTime);
            $("#txtQualityEventSummary").val(dataRow.QualityEventSummary);
            $("#txtQualityEventProvideTime").val(dataRow.QualityEventProvideTime);

            $("#lblDiaErrMsg" + "QE").html("");
            $("#fileupload_upload" + "QE").find("[type=file]").removeAttr('disabled');
            break;
    }
}