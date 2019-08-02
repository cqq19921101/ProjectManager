//initial dialog
$(function () {
    var dialog = $("#dialogData");
    
    //Toolbar Buttons
    $("#btndialogEditData").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogCancelEdit").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#dialogData").dialog({
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
                    var DoSuccessfully = false;
                    $.ajax({
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMMaterialMgmt/CreateSQMSCAR" : "/SQMMaterialMgmt/EditSQMSCAR"),
                        data: {
                            "SID": dialog.attr("SID"),
                            "anomalousTime": escape($.trim($("#txtAnomalousTime").val())),
                            "LitNo": escape($.trim($("#txtLitNo").val())),
                            "Model": escape($.trim($("#txtModel").val())),
                            "BitNo": escape($.trim($("#txtBitNo").val())),
                            "BitNum": escape($.trim($("#txtBitNum").val())),
                            "badnessNum": escape($.trim($("#txtbadnessNum").val())),
                            "RejectRatio": escape($.trim($("#txtRejectRatio").val())),
                            "Abnormal": escape($.trim($("#ddlAbnormal").val())),
                            "VenderCode": escape($.trim($("#txtVendorCode").val())),
                            "badnessNote": escape($.trim($("#txtbadnessNote").val())),
                            "badnessPic": escape($.trim($("#dialogData").attr("SCARD1FGUID")))
                         
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialog.attr('Mode') == "c")
                                    alert("SCAR create successfully.");
                                else
                                    alert("SCAR edit successfully.");
                            }
                            else {
                                if ((dialog.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblDiaErrMsg").html(data);
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
                        $("#btnSearch").click();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialog.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialog.attr('SID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });
});

//change dialog UI
