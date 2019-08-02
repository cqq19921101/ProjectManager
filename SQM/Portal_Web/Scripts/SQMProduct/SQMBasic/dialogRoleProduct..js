//initial dialog
$(function () {
    var dialogP = $("#dialogDataP");
    
    //Toolbar Buttons
    $("#btndialogEditDataP").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogCancelEditP").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#dialogDataP").dialog({
        autoOpen: false,
        height: 400,
        width: 600,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (dialogP.attr('Mode') == "v") {
                    $(this).dialogP("close");
                }
                else {
                    var DoSuccessfully = false;
                    $.ajax({
                        url: __WebAppPathPrefix + ((dialogP.attr('Mode') == "c") ? "/SQMProduct/CreatePro" : "/SQMProduct/EditPro"),
                        data: {
                            "BasicInfoGUID": dialogP.attr("BasicInfoGUID"),
                            "PrincipalProducts": escape($.trim($("#txtPrincipalProductsP").val())),
                            "RevenuePer": escape($.trim($("#txtRevenuePerP").val())),
                            "MOQ": escape($.trim($("#txtMOQP").val())),
                            "SampleTime": escape($.trim($("#txtSampleTimeP").val())),
                            "LeadTime": escape($.trim($("#txtLeadTimeP").val())),
                            "AnnualCapacity": escape($.trim($("#txtAnnualCapacityP").val())),
                            "MajorCompetitor": escape($.trim($("#txtMajorCompetitorP").val()))
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialogP.attr('Mode') == "c")
                                    alert("PrincipalProducts create successfully.");
                                else
                                    alert("PrincipalProducts edit successfully.");
                            }
                            else {
                                if ((dialogP.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblDiaErrMsgP").html(data);
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
                        $("#btnSearchP").click();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialogP.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialogP.attr('BasicInfoGUID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });
});

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByModeP(Mode) {
    var dialogP = $("#dialogDataP");
    var gridDataListP = $("#gridDataListP");
    var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
    var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
    switch (Mode) {
        case "c": //Create
            $("#dialogDataPToolBarP").hide();

            dialogP.attr('ItemRowId', "");
            dialogP.attr('ItemRowId', "");
            dialogP.attr('BasicInfoGUID', gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID);

            //$("#txtVendorCode").val("");
            //$("#txtVendorCode").removeAttr('disabled');
            $("#txtPrincipalProductsP").val("");
            $("#txtPrincipalProductsP").removeAttr('disabled');
            $("#txtRevenuePerP").val("");
            $("#txtRevenuePerP").removeAttr('disabled');
            $("#txtMOQP").val("");
            $("#txtMOQP").removeAttr('disabled');
            $("#txtSampleTimeP").val("");
            $("#txtSampleTimeP").removeAttr('disabled');
            $("#txtLeadTimeP").val("");
            $("#txtLeadTimeP").removeAttr('disabled');
            $("#txtAnnualCapacityP").val("");
            $("#txtAnnualCapacityP").removeAttr('disabled');
            $("#txtMajorCompetitorP").val("");
            $("#txtMajorCompetitorP").removeAttr('disabled');

            $("#lblDiaErrMsgP").html("");

            break;
        case "v": //View
            $("#btndialogEditDataP").button("option", "disabled", false);
            $("#btndialogCancelEditP").button("option", "disabled", true);
            $("#dialogDataPToolBarP").show();

            var RowIdP = gridDataListP.jqGrid('getGridParam', 'selrow');
            var dataRowP = gridDataListP.jqGrid('getRowData', RowIdP);
            dialogP.attr('ItemRowId', RowIdP);
            dialogP.attr('BasicInfoGUID', gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID);

            $("#txtVendorCode").val(dataRowP.BasicInfoGUID);
            $("#txtVendorCode").attr("disabled", "disabled");
            $("#txtPrincipalProductsP").val(dataRowP.PrincipalProducts);
            $("#txtPrincipalProductsP").attr("disabled", "disabled");
            $("#txtRevenuePerP").val(dataRowP.RevenuePer);
            $("#txtRevenuePerP").attr("disabled", "disabled");
            $("#txtMOQP").val(dataRowP.MOQ);
            $("#txtMOQP").attr("disabled", "disabled");
            $("#txtSampleTimeP").val(dataRowP.SampleTime);
            $("#txtSampleTimeP").attr("disabled", "disabled");
            $("#txtLeadTimeP").val(dataRowP.LeadTime);
            $("#txtLeadTimeP").attr("disabled", "disabled");
            $("#txtAnnualCapacityP").val(dataRowP.AnnualCapacity);
            $("#txtAnnualCapacityP").attr("disabled", "disabled");
            $("#txtMajorCompetitorP").val(dataRowP.MajorCompetitor);
            $("#txtMajorCompetitorP").attr("disabled", "disabled");

            $("#lblDiaErrMsgP").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditDataP").button("option", "disabled", true);
            $("#btndialogCancelEditP").button("option", "disabled", false);
            $("#dialogDataPToolBarP").show();

            var RowIdP = gridDataListP.jqGrid('getGridParam', 'selrow');
            var dataRowP = gridDataListP.jqGrid('getRowData', RowIdP);
            dialogP.attr('ItemRowId', RowIdP);
            dialogP.attr('BasicInfoGUID', dataRowP.BasicInfoGUID);

            $("#txtVendorCode").val(dataRowP.BasicInfoGUID);
            //$("#txtVendorCode").removeAttr('disabled');
            $("#txtPrincipalProductsP").val(dataRowP.PrincipalProducts);
            //$("#txtPrincipalProductsP").removeAttr('disabled');
            $("#txtRevenuePerP").val(dataRowP.RevenuePer);
            $("#txtRevenuePerP").removeAttr('disabled');
            $("#txtMOQP").val(dataRowP.MOQ);
            $("#txtMOQP").removeAttr('disabled');
            $("#txtSampleTimeP").val(dataRowP.SampleTime);
            $("#txtSampleTimeP").removeAttr('disabled');
            $("#txtLeadTimeP").val(dataRowP.LeadTime);
            $("#txtLeadTimeP").removeAttr('disabled');
            $("#txtAnnualCapacityP").val(dataRowP.AnnualCapacity);
            $("#txtAnnualCapacityP").removeAttr('disabled');
            $("#txtMajorCompetitorP").val(dataRowP.MajorCompetitor);
            $("#txtMajorCompetitorP").removeAttr('disabled');

            $("#lblDiaErrMsgP").html("");

            break;
    }
}