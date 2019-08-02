$(function () {
    //var dialog = $("#dialogData");
    jQuery("#btnBasic").click(function () {
        $("[edittpye='basic']").each(function (index) {
            $(this).hide();
        });
        $("#divBasic").show();
    });
    jQuery("#btnBasicSave").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditBasicInfo",
            data: {
                "VendorCode": '',
                "CompanyName": escape($.trim($("#txt" + "CompanyName").val())),
                "CompanyAddress": escape($.trim($("#txt" + "CompanyAddress").val())),
                "FactoryName": escape($.trim($("#txt" + "FactoryName").val())),
                "FactoryAddress": escape($.trim($("#txt" + "FactoryAddress").val())),
                
                "DateInfo": escape($.trim($("#txt" + "DateInfo").val())),
                "ProvidedName": escape($.trim($("#txt" + "ProvidedName").val())),
                "JobTitle": escape($.trim($("#txt" + "JobTitle").val()))
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("EditBasicInfo successfully.");
                }
                else {
                    data = data.replace("<br />", "\u000d");
                    alert("error:" + data);
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
            
        }
    });

    jQuery("#btnGenral").click(function () {
        $("[edittpye='basic']").each(function (index) {
            $(this).hide();
        });

        $("#divGenral").show();
    });
    jQuery("#btnGenralSave").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        var TradingCurrency = "";
        jQuery("[cbtype='TradingCurrency']:checked").each(function (index) {
            TradingCurrency += $(this).val()+";";
        });
        jQuery("#TradingCurrencyOther:checked").each(function (index) {
            TradingCurrency += $.trim($("#txt" + "TradingCurrency").val()) + "";
        });

        var TradeMode = "";
        jQuery("[cbtype='TradeMode']:checked").each(function (index) {
            TradeMode += $(this).val() + ";";
        });

        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditBasicInfo2",
            data: {
                "VendorCode": '',
                "VendorCode": '',
                "EnterpriseCategory": escape($.trim($("#txt" + "EnterpriseCategory").val())),
                "OwnerShip": escape($.trim($("#txt" + "OwnerShip").val())),
                "FoundedYear": escape($.trim($("#txt" + "FoundedYear").val())),
                "LastRevenues1": escape($.trim($("#txt" + "LastRevenues1").val())),
                "LastRevenues2": escape($.trim($("#txt" + "LastRevenues2").val())),
                "LastRevenues3": escape($.trim($("#txt" + "LastRevenues3").val())),
                "CurrentRevenues": escape($.trim($("#txt" + "CurrentRevenues").val())),
                "TurnoverAnalysis": escape($.trim($("#txt" + "TurnoverAnalysis").val())),
                "RevenueGrowthRate1": escape($.trim($("#txt" + "RevenueGrowthRate1").val())),
                "RevenueGrowthRate2": escape($.trim($("#txt" + "RevenueGrowthRate2").val())),
                "RevenueGrowthRate3": escape($.trim($("#txt" + "RevenueGrowthRate3").val())),
                "GrossProfitRate1": escape($.trim($("#txt" + "GrossProfitRate1").val())),
                "GrossProfitRate2": escape($.trim($("#txt" + "GrossProfitRate2").val())),
                "GrossProfitRate3": escape($.trim($("#txt" + "GrossProfitRate3").val())),
                "PlanInvestCapital": escape($.trim($("#txt" + "PlanInvestCapital").val())),
                "BankAndAccNumber": escape($.trim($("#txt" + "BankAndAccNumber").val())),
                "TradingCurrency": escape(TradingCurrency),
                "TradeMode": escape(TradeMode),
                "VMIManageModel": escape($.trim($("#txt" + "VMIManageModel").val())),
                "Distance": escape($.trim($("#txt" + "Distance").val())),
                "MinMonthStateDays": escape($.trim($("#txt" + "MinMonthStateDays").val())),
                "BU1TurnoverName": escape($.trim($("#txt" + "BU1TurnoverName").val())),
                "BU2TurnoverName": escape($.trim($("#txt" + "BU2TurnoverName").val())),
                "BU3TurnoverName": escape($.trim($("#txt" + "BU3TurnoverName").val())),
                "BU1Turnover": escape($.trim($("#txt" + "BU1Turnover").val())),
                "BU2Turnover": escape($.trim($("#txt" + "BU2Turnover").val())),
                "BU3Turnover": escape($.trim($("#txt" + "BU3Turnover").val())),
                "CompanyAdvantage": escape($.trim($("#txt" + "CompanyAdvantage").val()))

            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("EditBasicInfo successfully.");
                }
                else {
                    data = data.replace("<br />", "\u000d");
                    alert("error:" + data);
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

        }
    });

    jQuery("#btnAbility").click(function () {
        $("[edittpye='basic']").each(function (index) {
            $(this).hide();
        });
        $("#divAbility").show();
    });
    jQuery("#btnAbilitySave").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        //var TradingCurrency = "";
        //jQuery("[cbtype='TradingCurrency']:checked").each(function (index) {
        //    TradingCurrency += $(this).val()+";";
        //});
        //jQuery("#TradingCurrencyOther:checked").each(function (index) {
        //    TradingCurrency += $.trim($("#txt" + "TradingCurrency").val()) + "";
        //});

        //var TradeMode = "";
        //jQuery("[cbtype='TradeMode']:checked").each(function (index) {
        //    TradeMode += $(this).val() + ";";
        //});

        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditBasicInfoAbility",
            data: {
                "VendorCode": '',
                "Is3DUG": escape($.trim($("#" + "Is3DUG").is(":checked"))),
                "Is3DProE": escape($.trim($("#" + "Is3DProE").is(":checked"))),
                "Is2DAutoCAD": escape($.trim($("#" + "Is2DAutoCAD").is(":checked"))),
                "IsPhotoShop": escape($.trim($("#" + "IsPhotoShop").is(":checked"))),
                "IsIDMapAbility": escape($.trim($("#" + "IsIDMapAbility").is(":checked"))),
                "Is3DMapAbility": escape($.trim($("#" + "Is3DMapAbility").is(":checked"))),
                "Is2DMapAbility": escape($.trim($("#" + "Is2DMapAbility").is(":checked"))),
                "IsMoldflowAbility": escape($.trim($("#" + "IsMoldflowAbility").is(":checked"))),
                "IsTAAbility": escape($.trim($("#" + "IsTAAbility").is(":checked"))),
                "IsDesignGuildline": escape($.trim($("#" + "IsDesignGuildline").is(":checked"))),
                "IsFMEA": escape($.trim($("#" + "IsFMEA").is(":checked"))),
                "IsLessonLearnt": escape($.trim($("#" + "IsLessonLearnt").is(":checked"))),
                "MoldProduceCapacity": escape($.trim($("#ddl" + "MoldProduceCapacity").val()))

            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    DoSuccessfully = true;
                    alert("EditBasicInfo successfully.");
                }
                else {
                    data = data.replace("<br />", "\u000d");
                    alert("error:" + data);
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

        }
    });

    jQuery("#btnSetCategory").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditVendorType",
            data: {
                "TypeID": escape($.trim($("#ddl" + "VendorType").val()))
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    alert("VendorType edit successfully.");
                    loadByVendorType($("#ddl" + "VendorType").val());
                }
                else {
                    data = data.replace("<br />", "\u000d");
                    alert("error:" + data);
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
            
        }
    });
});