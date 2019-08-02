$(function () {
    var dialogFile = $("#dialogDataFile");
    var gridDataListFile = $("#gridDataListFile");
    //Toolbar Buttons
    $("#btndialogEditDataFile").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogCancelEditFile").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#dialogDataFile").dialog({
        autoOpen: false,
        height: 400,
        width: 500,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (dialogFile.attr('Mode') == "v") {
                    $(this).dialog("close");
                }
                else {
                    var DoSuccessfully = false;
                    var RowIdFile = gridDataListFile.jqGrid('getGridParam', 'selrow');
                    $.ajax({
                        url: __WebAppPathPrefix + ((dialogFile.attr('Mode') == "c") ? "/SQMReliability/CreateQualityFile" : "/SQMReliability/EditQualityFile"),
                        data: {
                            "ReportSID": $("#gridDataListFile").attr("ReportSID")
                            , "DocName": escape($.trim($("#txtDocName").val()))
                            , "DocInspResult": escape($.trim($("#txtDocInspResult").val()))
                            , "DocNo": $("#gridDataListFile").attr("FGuid")
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialogFile.attr('Mode') == "c")
                                    alert("create successfully.");
                                else
                                    alert("edit successfully.");
                            }
                            else {
                                if ((dialogFile.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblDiaErrMsgFile").html(data);
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
                        $(this).dialog("close");
                        $("#btnSearchFile").click();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialogFile.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialogFile.attr('ReportSID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });



})

function DialogSetUIByModeFile(Mode) {
    var dialogFile = $("#dialogFileCode");
    var gridDataListFile = $("#gridDataListFile");
    var RowIdFile = gridDataListFile.jqGrid('getGridParam', 'selrow');
    var dataRowFile = gridDataListFile.jqGrid('getRowData', RowIdFile);
    switch (Mode) {
        case "c": //Create
            $("#dialogFileToolBarFile").hide();

            dialogFile.attr('ItemRowId', "");
            dialogFile.attr('ReportSID', "");

            $("#txtDocName").val("");
            $("#txtDocName").removeAttr('disabled');

            $("#txtDocInspResult option:first").prop("selected", 'selected');
            $("#txtDocInspResult").removeAttr('disabled');

            //$("#UpDateFile").removeAttr('disabled');
            $("#fileUpload").show();

            $("#lblDiaErrMsgFile").html("");

            break;
        case "v": //View
            $("#btndialogEditDataFile").button("option", "disabled", false);
            $("#btndialogCancelEditFile").button("option", "disabled", true);
            $("#dialogFileToolBarFile").show();


            $("#txtDocName").val(dataRowFile.DocName);
            $("#txtDocName").attr("disabled", "disabled");

            $("#txtDocInspResult").val(dataRowFile.DocInspResult);
            $("#txtDocInspResult").attr("disabled", "disabled");

            //$("#UpDateFile").attr("disabled", "disabled");
            $("#fileUpload").hide();

            $("#lblDiaErrMsgFile").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditDataFile").button("option", "disabled", true);
            $("#btndialogCancelEditFile").button("option", "disabled", false);
            $("#dialogFileToolBarFile").show();

            dialogFile.attr('ItemRowId', RowIdFile);
            dialogFile.attr('ReportSID', dataRowFile.ReportSID);

            $("#txtDocName").val(dataRowFile.DocName);
            $("#txtDocName").removeAttr('disabled');

            $("#txtDocInspResult").val(dataRowFile.DocInspResult);
            $("#txtDocInspResult").removeAttr('disabled');

            //$("#UpDateFile").removeAttr('disabled');
            $("#fileUpload").show();

            $("#lblDiaErrMsgFile").html("");

            break;
    }
}