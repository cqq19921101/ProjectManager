$(function () {
    var dialogInspVar = $("#dialogDataInspVar");
    var gridDataListInspVar = $("#gridDataListInspVar");
    //Toolbar Buttons
    $("#btndialogEditDataInspVar").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogCancelEditInspVar").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });
  
   

    $("#dialogDataInspVar").dialog({
        autoOpen: false,
        height: 400,
        width: 500,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (dialogInspVar.attr('Mode') == "v") {
                    $(this).dialog("close");
                }
                else {
                    var DoSuccessfully = false;
                    var RowIdInsp = gridDataListInspVar.jqGrid('getGridParam', 'selrow');
                    $.ajax({
                        url: __WebAppPathPrefix + ((dialogInspVar.attr('Mode') == "c") ? "/SQMReliability/CreateQualityInsp" : "/SQMReliability/EditQualityInsp"),
                        data: {
                            "ReportSID": $("#gridDataListInspVar").attr("ReportSID")
                            , "InspCodeID": $("#dialogDataInspVar").attr("InspCodeID")
                             , "InspDescID": $("#dialogDataInspVar").attr("InspDescID")
                             , "UCL": escape($.trim($("#vtxtUCL").val()))
                             , "LCL": escape($.trim($("#vtxtLCL").val()))
                             , "InspTool": escape($.trim($("#vtxtInspTool").val()))
                             , "AQL": escape($.trim($("#vtxtAQL").val()))
                            , "InspNum": escape($.trim($("#vtxtInspNum").val()))
                            , "Insptype": $("#dialogDataInspVar").attr("Insptype")
                            , "Judge": escape($.trim($("#vtxtJudge").val()))
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialogInspVar.attr('Mode') == "c")
                                    alert("create successfully.");
                                else
                                    alert("edit successfully.");
                            }
                            else {
                                if ((dialogInspVar.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblDiaErrMsgInspVar").html(data);
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
                        $("#btnSearchInspVar").click();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialogInspVar.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialogInspVar.attr('SID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });


})