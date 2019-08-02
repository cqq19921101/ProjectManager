//initial dialog
$(function () {
    var dialog = $("#dialogDataT");
    
    //Toolbar Buttons
    $("#btndialogEditDataT").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogCancelEditT").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#dialogDataT").dialog({
        autoOpen: false,
        height: 550,
        width: 700,
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
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMBasic/CreateTraders" : "/SQMBasic/EditTraders"),
                        data: {
                            "BasicInfoGUID": dialog.attr("BasicInfoGUID"),
                            "PrincipalProducts": escape($.trim($("#txtPrincipalProductsT").val())),
                            "RevenuePer": escape($.trim($("#txtRevenuePerT").val())),
                            "MOQ": escape($.trim($("#txtMOQT").val())),
                            "SampleTime": escape($.trim($("#txtSampleTimeT").val())),
                            "LeadTime": escape($.trim($("#txtLeadTimeT").val())),
                            "SupAndOriName": escape($.trim($("#txtSupAndOriNameT").val())),
                            "OfferPlaceCertify": escape($("input:radio[name='C']:checked").val()),
                            "OfferPlaceFGUID": escape($.trim($("#txtOfferPlaceFGUID").val())),
                            "OfferSellCertify": escape($("input:radio[name='B']:checked").val()),
                            "OfferSellFGUID": escape($.trim($("#txtOfferSellFGUID").val())),
                            "MajorCompetitor": escape($.trim($("#txtMajorCompetitorT").val()))
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialog.attr('Mode') == "c")
                                    alert("PrincipalProducts create successfully.");
                                else
                                    alert("PrincipalProducts edit successfully.");
                            }
                            else {
                                if ((dialog.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblDiaErrMsgT").html(data);
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
                        $("#btnSearchT").click();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialog.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialog.attr('BasicInfoGUID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });
});

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByModeT(Mode) {
    var dialog = $("#dialogDataT");
    var gridDataListT = $("#gridDataListT");
    var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
    var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
    switch (Mode) {
        case "c": //Create
            $("#dialogDataTToolBarT").hide();
            $("input:radio[name = 'C']:checked").attr("checked", false);
            $("input:radio[name = 'B']:checked").attr("checked", false);
            dialog.attr('ItemRowId', "");
            dialog.attr('BasicInfoGUID', gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID);

            //$("#txtVendorCode").val("");
            //$("#txtVendorCode").removeAttr('disabled');
            $("#txtPrincipalProductsT").val("");
            $("#txtPrincipalProductsT").removeAttr('disabled');
            $("#txtRevenuePerT").val("");
            $("#txtRevenuePerT").removeAttr('disabled');
            $("#txtMOQT").val("");
            $("#txtMOQT").removeAttr('disabled');
            $("#txtSampleTimeT").val("");
            $("#txtSampleTimeT").removeAttr('disabled');
            $("#txtLeadTimeT").val("");
            $("#txtLeadTimeT").removeAttr('disabled');

            $("#txtSupAndOriNameT").val("");
            $("#txtSupAndOriNameT").removeAttr('disabled');
            $("input:radio[name='C']:checked").val("");
            $("#OfferPlaceCertify").removeAttr('disabled');
            $("#txtOfferPlaceFGUID").val("");
            $("#txtOfferPlaceFGUID").removeAttr('disabled');
            $("input:radio[name='B']:checked").val("");
            $("#OfferSellCertify").removeAttr('disabled');
            $("#txtOfferSellFGUID").val("");
            $("#txtOfferSellFGUID").removeAttr('disabled');
            $("#txtMajorCompetitorT").val("");
            $("#txtMajorCompetitorT").removeAttr('disabled');

            $("#txtFile").hide();
            $("#UpDateFile").hide();
            $("#txtFile2").hide();
            $("#UpDateFile2").hide();
            $("#ProcessTraders1").hide();
            $("#ProcessTraders2").hide();

            $("#lblDiaErrMsgT").html("");

            break;
        case "v": //View
            $("#btndialogEditDataT").button("option", "disabled", false);
            $("#btndialogCancelEditT").button("option", "disabled", true);
            $("#dialogDataTToolBarT").show();

            var RowId = gridDataListT.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataListT.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('BasicInfoGUID', gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID);

            $("#txtVendorCode").val(dataRow.BasicInfoGUID);
            //$("#txtVendorCode").attr("disabled", "disabled");
            $("#txtPrincipalProductsT").val(dataRow.PrincipalProducts);
            $("#txtPrincipalProductsT").attr("disabled", "disabled");
            $("#txtRevenuePerT").val(dataRow.RevenuePer);
            $("#txtRevenuePerT").attr("disabled", "disabled");
            $("#txtMOQT").val(dataRow.MOQ);
            $("#txtMOQT").attr("disabled", "disabled");
            $("#txtSampleTimeT").val(dataRow.SampleTime);
            $("#txtSampleTimeT").attr("disabled", "disabled");
            $("#txtLeadTimeT").val(dataRow.LeadTime);
            $("#txtLeadTimeT").attr("disabled", "disabled");
            $("#txtSupAndOriNameT").val(dataRow.SupAndOriName);
            $("#txtSupAndOriNameT").attr("disabled", "disabled");
            $('input:radio[name="C"]').eq(dataRow['OfferPlaceCertify'] == 'true' ? 0 : 1).attr("checked", true);
            $("#OfferPlaceCertify").attr("disabled", "disabled");
            $("#OfferPlaceCertify1").attr("disabled", "disabled");
            $("#txtOfferPlaceFGUID").val(dataRow.OfferPlaceFGUID);
            $("#txtOfferPlaceFGUID").attr("disabled", "disabled");
            $('input:radio[name="B"]').eq(dataRow['OfferSellCertify'] == 'true' ? 0 : 1).attr("checked", true);
            $("#OfferSellCertify").attr("disabled", "disabled");
            $("#OfferSellCertify1").attr("disabled", "disabled");
            $("#txtOfferSellFGUID").val(dataRow.OfferSellFGUID);
            $("#txtOfferSellFGUID").attr("disabled", "disabled");

            $("#txtMajorCompetitorT").val(dataRow.MajorCompetitor);
            $("#txtMajorCompetitorT").attr("disabled", "disabled");


            $("#txtFile").show();
            $("#UpDateFile").show();
            $("#txtFile2").show();
            $("#UpDateFile2").show();
            $("#ProcessTraders1").show();
            $("#ProcessTraders2").show();
            $("#btnProcessT").attr("disabled", "disabled");
            $("#btnProcessT2").attr("disabled", "disabled");
            $("#UpDateFile").attr("disabled", "disabled");
            $("#UpDateFile2").attr("disabled", "disabled");
            $("#lblDiaErrMsgT").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditDataT").button("option", "disabled", true);
            $("#btndialogCancelEditT").button("option", "disabled", false);
            $("#dialogDataTToolBarT").show();

            var RowId = gridDataListT.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataListT.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('BasicInfoGUID', gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID);

            $("#txtVendorCode").val(dataRow.BasicInfoGUID);
            //$("#txtVendorCode").removeAttr('disabled');
            $("#txtPrincipalProductsT").val(dataRow.PrincipalProducts);
            //$("#txtPrincipalProductsT").removeAttr('disabled');
            $("#txtRevenuePerT").val(dataRow.RevenuePer);
            $("#txtRevenuePerT").removeAttr('disabled');
            $("#txtMOQT").val(dataRow.MOQ);
            $("#txtMOQT").removeAttr('disabled');
            $("#txtSampleTimeT").val(dataRow.SampleTime);
            $("#txtSampleTimeT").removeAttr('disabled');
            $("#txtLeadTimeT").val(dataRow.LeadTime);
            $("#txtLeadTimeT").removeAttr('disabled');
            $("#txtSupAndOriNameT").val(dataRow.SupAndOriName);
            $("#txtSupAndOriNameT").removeAttr('disabled');
            $('input:radio[name="C"]').eq(dataRow['OfferPlaceCertify'] == 'true' ? 0 : 1).attr("checked", true);
            $("#OfferPlaceCertify").removeAttr('disabled');
            $("#OfferPlaceCertify1").removeAttr('disabled');

            $("#txtOfferPlaceFGUID").val(dataRow.OfferPlaceFGUID);
            $("#txtOfferPlaceFGUID").removeAttr('disabled');
            $('input:radio[name="B"]').eq(dataRow['OfferSellCertify'] == 'true' ? 0 : 1).attr("checked", true);
            $("#OfferSellCertify").removeAttr('disabled');
            $("#OfferSellCertify1").removeAttr('disabled');
            $("#txtOfferSellFGUID").val(dataRow.OfferSellFGUID);
            $("#txtOfferSellFGUID").removeAttr('disabled');
            $("#txtMajorCompetitorT").val(dataRow.MajorCompetitor);
            $("#txtMajorCompetitorT").removeAttr('disabled');

            $("#txtFile").show();
            $("#UpDateFile").show();
            $("#txtFile2").show();
            $("#UpDateFile2").show();
            $("#UpDateFile").removeAttr('disabled');
            $("#UpDateFile2").removeAttr('disabled');

            $("#ProcessTraders1").show();
            $("#ProcessTraders2").show();
            $("#btnProcessT").removeAttr('disabled');
            $("#btnProcessT2").removeAttr('disabled');

            $("#lblDiaErrMsgT").html("");

            break;
    }
}