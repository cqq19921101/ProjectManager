//initial dialog
$(function () {
    var dialogA = $("#dialogDataA");
    
    //Toolbar Buttons
    $("#btndialogEditDataA").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogCancelEditA").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#dialogDataA").dialog({
        autoOpen: false,
        height: 520,
        width: 700,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (dialogA.attr('Mode') == "v") {
                    $(this).dialog("close");
                }
                else {
                    var DoSuccessfully = false;
                    $.ajax({
                        url: __WebAppPathPrefix + ((dialogA.attr('Mode') == "c") ? "/SQMBasic/CreateAgents" : "/SQMBasic/EditAgents"),
                        data: {
                            "BasicInfoGUID": dialogA.attr("BasicInfoGUID"),
                            "PrincipalProducts": escape($.trim($("#txtPrincipalProductsA").val())),
                            "RevenuePer": escape($.trim($("#txtRevenuePerA").val())),
                            "MOQ": escape($.trim($("#txtMOQA").val())),
                            "SampleTime": escape($.trim($("#txtSampleTimeA").val())),
                            "LeadTime": escape($.trim($("#txtLeadTimeA").val())),
                            "ProductBrand": escape($.trim($("#txtProductBrandA").val())),
                            "SupAndOriName": escape($.trim($("#txtSupAndOriNameA").val())),
                            "SupAndOriPlace": escape($.trim($("#txtSupAndOriPlaceA").val())),
                            "OfferProxyCertify": escape($("input:radio[name='A']:checked").val()),
                            "OfferProxyFGUID": escape($.trim($("#txtOfferProxyFile").val())),
                            "MajorCompetitor": escape($.trim($("#txtMajorCompetitorA").val()))
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialogA.attr('Mode') == "c")
                                    alert("PrincipalProducts create successfully.");
                                else
                                    alert("PrincipalProducts edit successfully.");
                            }
                            else {
                                if ((dialogA.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblDiaErrMsgA").html(data);
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
                        $("#btnSearchA").click();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialogA.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialogA.attr('BasicInfoGUID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });
});

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByModeA(Mode) {
    var dialogA = $("#dialogDataA");
    var gridDataListA = $("#gridDataListA");
    var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
    var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
    switch (Mode) {
        case "c": //Create
            $("#dialogDataAToolBarA").hide();
            dialogA.attr('ItemRowId', "");
            dialogA.attr('BasicInfoGUID', gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID);

            //dialog.attr('VendorCode', dataRowA2.VendorCode);

            $("input:radio[name='A']").attr("checked", false);

            //$("#txtVendorCode").val("");
            //$("#txtVendorCode").removeAttr('disabled');
            $("#txtPrincipalProductsA").val("");
            $("#txtPrincipalProductsA").removeAttr('disabled');
            $("#txtRevenuePerA").val("");
            $("#txtRevenuePerA").removeAttr('disabled');
            $("#txtMOQA").val("");
            $("#txtMOQA").removeAttr('disabled');
            $("#txtSampleTimeA").val("");
            $("#txtSampleTimeA").removeAttr('disabled');
            $("#txtLeadTimeA").val("");
            $("#txtLeadTimeA").removeAttr('disabled');

            $("#txtProductBrandA").val("");
            $("#txtProductBrandA").removeAttr('disabled');
            $("#txtSupAndOriNameA").val("");
            $("#txtSupAndOriNameA").removeAttr('disabled');
            $("#txtSupAndOriPlaceA").val("");
            $("#txtSupAndOriPlaceA").removeAttr('disabled');
            $("input:radio[name='A']:checked").val("");
            $("#OfferProxyCertify").removeAttr('disabled');
            $("#OfferProxyCertify1").removeAttr('disabled');
            //$("#txtOfferProxyFile").val("");
            //$("#txtOfferProxyFile").removeAttr('disabled');
            $("#txtMajorCompetitorA").val("");
            $("#txtMajorCompetitorA").removeAttr('disabled');
            $("#txtFile").hide();
            $("#UpDateFile").hide();

            $("#ProcessAgents").hide();
            $("#lblDiaErrMsgA").html("");

            break;
        case "v": //View
            $("#btndialogEditDataA").button("option", "disabled", false);
            $("#btndialogCancelEditA").button("option", "disabled", true);
            $("#dialogDataAToolBarA").show();
            var RowIdA = gridDataListA.jqGrid('getGridParam', 'selrow');
            var dataRowA = gridDataListA.jqGrid('getRowData', RowIdA);
            dialogA.attr('ItemRowId', RowIdA);
            dialogA.attr('BasicInfoGUID', gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID);


            $("#txtVendorCode").val(dataRowA.BasicInfoGUID);
            $("#txtPrincipalProductsA").val(dataRowA.PrincipalProducts);
            $("#txtPrincipalProductsA").attr("disabled", "disabled");
            $("#txtRevenuePerA").val(dataRowA.RevenuePer);
            $("#txtRevenuePerA").attr("disabled", "disabled");
            $("#txtMOQA").val(dataRowA.MOQ);
            $("#txtMOQA").attr("disabled", "disabled");
            $("#txtSampleTimeA").val(dataRowA.SampleTime);
            $("#txtSampleTimeA").attr("disabled", "disabled");
            $("#txtLeadTimeA").val(dataRowA.LeadTime);
            $("#txtLeadTimeA").attr("disabled", "disabled");
            $("#txtProductBrandA").val(dataRowA.ProductBrand);
            $("#txtProductBrandA").attr("disabled", "disabled");
            $("#txtSupAndOriNameA").val(dataRowA.SupAndOriName);
            $("#txtSupAndOriNameA").attr("disabled", "disabled");
            $("#txtSupAndOriPlaceA").val(dataRowA.SupAndOriPlace);
            $("#txtSupAndOriPlaceA").attr("disabled", "disabled");
            //$("#OfferProxyCertify").val(dataRowA.OfferProxyCertify==true?1:0);
            //$('input:radio[name="A"]').eq(dataRowA.OfferProxyCertify).attr("checked", true);
            $('input:radio[name="A"]').eq(dataRowA['OfferProxyCertify'] == 'true' ? 0 : 1).attr("checked", true);
            $("#OfferProxyCertify").attr('disabled', 'disabled');
            $("#OfferProxyCertify1").attr('disabled', 'disabled');
            //$("#txtOfferProxyFile").val(dataRowA.OfferProxyFGUID);
            //$("#txtOfferProxyFile").attr("disabled", "disabled");
            $("#txtMajorCompetitorA").val(dataRowA.MajorCompetitor);
            $("#txtMajorCompetitorA").attr("disabled", "disabled");
            $("#txtFile").show();
            $("#UpDateFile").show();
            $("#ProcessAgents").show();
            $("#btnProcessA").attr("disabled", "disabled");
            $("#lblDiaErrMsgA").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditDataA").button("option", "disabled", true);
            $("#btndialogCancelEditA").button("option", "disabled", false);
            $("#dialogDataAToolBarA").show();

            var RowIdA = gridDataListA.jqGrid('getGridParam', 'selrow');
            var dataRowA = gridDataListA.jqGrid('getRowData', RowIdA);
            dialogA.attr('ItemRowId', RowIdA);
            dialogA.attr('BasicInfoGUID', gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID);

            $("#txtVendorCode").val(dataRowA.BasicInfoGUID);
            //$("#txtVendorCode").removeAttr('disabled');
            $("#txtPrincipalProductsA").val(dataRowA.PrincipalProducts);
            //$("#txtPrincipalProductsA").removeAttr('disabled');
            $("#txtRevenuePerA").val(dataRowA.RevenuePer);
            $("#txtRevenuePerA").removeAttr('disabled');
            $("#txtMOQA").val(dataRowA.MOQ);
            $("#txtMOQA").removeAttr('disabled');
            $("#txtSampleTimeA").val(dataRowA.SampleTime);
            $("#txtSampleTimeA").removeAttr('disabled');
            $("#txtLeadTimeA").val(dataRowA.LeadTime);
            $("#txtLeadTimeA").removeAttr('disabled');

            $("#txtProductBrandA").val(dataRowA.ProductBrand);
            $("#txtProductBrandA").removeAttr('disabled');
            $("#txtSupAndOriNameA").val(dataRowA.SupAndOriName);
            $("#txtSupAndOriNameA").removeAttr('disabled');
            $("#txtSupAndOriPlaceA").val(dataRowA.SupAndOriPlace);
            $("#txtSupAndOriPlaceA").removeAttr('disabled');
            $('input:radio[name="A"]').eq(dataRowA['OfferProxyCertify'] == 'true' ? 0 : 1).attr("checked", true);
            $("#OfferProxyCertify").removeAttr('disabled');
            $("#OfferProxyCertify1").removeAttr('disabled');
            //$("#txtOfferProxyFile").val(dataRowA.OfferProxyFGUID);
            //$("#txtOfferProxyFile").removeAttr('disabled');
            $("#txtMajorCompetitorA").val(dataRowA.MajorCompetitor);
            $("#txtMajorCompetitorA").removeAttr('disabled');


            $("#txtFile").show();
            $("#UpDateFile").show();
            $("#ProcessAgents").show();
            $("#UpDateFile").removeAttr('disabled');
            $("#btnProcessA").removeAttr('disabled');
            $("#lblDiaErrMsgA").html("");

            break;
    }
}


//initial dialog
$(function () {
    var dialogA = $("#dialogDataA2");

    //Toolbar Buttons
    $("#btndialogEditDataA2").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogCancelEditA2").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#dialogDataA2").dialog({
        autoOpen: false,
        height: 400,
        width: 450,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (dialogA.attr('Mode') == "v") {
                    $(this).dialog("close");
                }
                else {
                    var DoSuccessfully = false;
                    $.ajax({
                        url: __WebAppPathPrefix + ((dialogA.attr('Mode') == "c") ? "/SQMBasic/CreateAgents2" : "/SQMBasic/EditAgents2"),
                        data: {
                            "BasicInfoGUID": dialogA.attr("BasicInfoGUID"),
                            "PrincipalProducts": escape($.trim($("#txtPrincipalProductsA2").val())),
                            "FactoryName": escape($.trim($("#txtFactoryNameA2").val())),
                            "ProductBrand": escape($.trim($("#txtProductBrandA2").val())),
                            "FactoryAddress": escape($.trim($("#txtFactoryAddressA2").val())),
                            "FactoryNum": escape($.trim($("#txtFactoryNumA2").val())),
                            "FactoryDate": escape($.trim($("#txtFactoryDateA2").val())),
                            "ProductLine": escape($.trim($("#txtProductLineA2").val())),
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialogA.attr('Mode') == "c")
                                    alert("PrincipalProducts create successfully.");
                                else
                                    alert("PrincipalProducts edit successfully.");
                            }
                            else {
                                if ((dialogA.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblDiaErrMsgA2").html(data);
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
                        $("#btnSearchA2").click();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialogA.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialogA.attr('BasicInfoGUID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });
});

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByModeA2(Mode) {
    var dialogA = $("#dialogDataA2");
    var gridDataListA2 = $("#gridDataListA2");
    var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
    var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
    switch (Mode) {
        case "c": //Create
            $("#dialogDataAToolBarA2").hide();
            dialogA.attr('ItemRowId', "");
            dialogA.attr('BasicInfoGUID', gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID);

            //$("#txtVendorCode").val("");
            //$("#txtVendorCode").removeAttr('disabled');
            $("#txtPrincipalProductsA2").val("");
            $("#txtPrincipalProductsA2").removeAttr('disabled');
            $("#txtFactoryNameA2").val("");
            $("#txtFactoryNameA2").removeAttr('disabled');
            $("#txtProductBrandA2").val("");
            $("#txtProductBrandA2").removeAttr('disabled');
            $("#txtFactoryAddressA2").val("");
            $("#txtFactoryAddressA2").removeAttr('disabled');
            $("#txtFactoryNumA2").val("");
            $("#txtFactoryNumA2").removeAttr('disabled');
            $("#txtFactoryDateA2").val("");
            $("#txtFactoryDateA2").removeAttr('disabled');
            $("#txtProductLineA2").val("");
            $("#txtProductLineA2").removeAttr('disabled');
            $("#lblDiaErrMsgA2").html("");

            break;
        case "v": //View
            $("#btndialogEditDataA2").button("option", "disabled", false);
            $("#btndialogCancelEditA2").button("option", "disabled", true);
            $("#dialogDataAToolBarA2").show();
            var RowIdA2 = gridDataListA2.jqGrid('getGridParam', 'selrow');
            var dataRowA2 = gridDataListA2.jqGrid('getRowData', RowIdA2);
            dialogA.attr('ItemRowId', RowIdA2);
            dialogA.attr('BasicInfoGUID', gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID);


            $("#txtVendorCodeA2").val(dataRowA2.BasicInfoGUID);
            $("#txtPrincipalProductsA2").val(dataRowA2.PrincipalProducts);
            $("#txtPrincipalProductsA2").attr("disabled", "disabled");
            $("#txtFactoryNameA2").val(dataRowA2.FactoryName);
            $("#txtFactoryNameA2").attr("disabled", "disabled");
            $("#txtProductBrandA2").val(dataRowA2.ProductBrand);
            $("#txtProductBrandA2").attr("disabled", "disabled");
            $("#txtFactoryAddressA2").val(dataRowA2.FactoryAddress);
            $("#txtFactoryAddressA2").attr("disabled", "disabled");
            $("#txtFactoryNumA2").val(dataRowA2.FactoryNum);
            $("#txtFactoryNumA2").attr("disabled", "disabled");
            $("#txtFactoryDateA2").val(dataRowA2.FactoryDate);
            $("#txtFactoryDateA2").attr("disabled", "disabled");
            $("#txtProductLineA2").val(dataRowA2.ProductLine);
            $("#txtProductLineA2").attr("disabled", "disabled");
            $("#lblDiaErrMsgA2").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditDataA2").button("option", "disabled", true);
            $("#btndialogCancelEditA2").button("option", "disabled", false);
            $("#dialogDataAToolBarA2").show();

            var RowIdA2 = gridDataListA2.jqGrid('getGridParam', 'selrow');
            var dataRowA2 = gridDataListA2.jqGrid('getRowData', RowIdA2);
            dialogA.attr('ItemRowId', RowIdA2);
            dialogA.attr('BasicInfoGUID', gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID);

            $("#txtVendorCode").val(dataRowA2.BasicInfoGUID);
            //$("#txtVendorCode").removeAttr('disabled');
            $("#txtPrincipalProductsA2").val(dataRowA2.PrincipalProducts);
            //$("#txtPrincipalProductsA").removeAttr('disabled');
            $("#txtFactoryNameA2").val(dataRowA2.FactoryName);
            $("#txtFactoryNameA2").removeAttr('disabled');
            $("#txtProductBrandA2").val(dataRowA2.ProductBrand);
            $("#txtProductBrandA2").removeAttr('disabled');
            $("#txtFactoryAddressA2").val(dataRowA2.FactoryAddress);
            $("#txtFactoryAddressA2").removeAttr('disabled');
            $("#txtFactoryNumA2").val(dataRowA2.FactoryNum);
            $("#txtFactoryNumA2").removeAttr('disabled');
            $("#txtFactoryDateA2").val(dataRowA2.FactoryDate);
            $("#txtFactoryDateA2").removeAttr('disabled');
            $("#txtProductLineA2").val(dataRowA2.ProductLine);
            $("#txtProductLineA2").removeAttr('disabled');




            $("#lblDiaErrMsgA2").html("");

            break;
    }
}







