$(function () {
    //var dialog = $("#dialogData");
    $("#btnBasic").click(function () {
        $(this).removeClass('ui-state-focus');
        $("[edittpye='basic']").each(function (index) {
            $(this).hide();
        });
        $("#divBasic").show();
    });
    $("#btnBasicSave").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditBasicInfo",
            data: {
                "VendorCode": '',
                "CompanyName": escape($.trim($("#txt" + "CompanyName").val())),
                "BasicInfoGUID": escape($.trim( $('#ihBasicInfoGUID').val())),
                "CompanyAddress": escape($.trim($("#txt" + "CompanyAddress").val())),
                "IsTrader": escape($.trim($("#cb" + "IsTrader").is(":checked"))),
                "IsSpotTrader": escape($.trim($("#cb" + "IsSpotTrader").is(":checked"))),
                "FactoryName": escape($.trim($("#txt" + "FactoryName").val())),
                "FactoryAddress": escape($.trim($("#txt" + "FactoryAddress").val())),
                "TB_SQM_Vendor_TypeCID": escape($.trim($("#ihTB_SQM_Vendor_TypeCID").val())),
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
    $("#txtDateInfo").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtDateInfo").datepicker("setDate", '-31d');
    $("#txtFoundedYear").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtFoundedYear").datepicker("setDate", '-31d');
    $("#btnGenral").click(function () {
        $(this).removeClass('ui-state-focus');
        $("[edittpye='basic']").each(function (index) {
            $(this).hide();
        });

        $("#divGenral").show();
    });
    $("#btnGenralSave").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        var TradingCurrency = "";
        $("[cbtype='TradingCurrency']:checked").each(function (index) {
            TradingCurrency += $(this).val()+";";
        });
        $("#TradingCurrencyOther:checked").each(function (index) {
            TradingCurrency += $.trim($("#txt" + "TradingCurrency").val()) + "";
        });

        var TradeMode = "";
        $("[cbtype='TradeMode']:checked").each(function (index) {
            TradeMode += $(this).val() + ";";
        });

        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditBasicInfo2",
            data: {
                "VendorCode": '',
                "BasicInfoGUID": escape($.trim($('#ihBasicInfoGUID').val())),
                "TB_SQM_Vendor_TypeCID": escape($.trim($("#ihTB_SQM_Vendor_TypeCID").val())),
                "EnterpriseCategory": escape($.trim($("#txt" + "EnterpriseCategory").val())),
                "OwnerShip": escape($.trim($("#txt" + "OwnerShip").val())),
                "FoundedYear": escape($.trim($("#txt" + "FoundedYear").val())),
                "LastRevenues1": escape($.trim($("#txt" + "LastRevenues1").val())),
                "LastRevenues2": escape($.trim($("#txt" + "LastRevenues2").val())),
                "LastRevenues3": escape($.trim($("#txt" + "LastRevenues3").val())),
                "CurrentRevenues": escape($.trim($("#txt" + "CurrentRevenues").val())),
                "TurnoverAnalysis": escape($.trim($("#txt" + "TurnoverAnalysis").val())),
                "RevenueGrowthRate1": escape($.trim($("#txt" + "RevenueGrowthRate1").val())),
                //"RevenueGrowthRate2": escape($.trim($("#txt" + "RevenueGrowthRate2").val())),
                //"RevenueGrowthRate3": escape($.trim($("#txt" + "RevenueGrowthRate3").val())),
                "GrossProfitRate1": escape($.trim($("#txt" + "GrossProfitRate1").val())),
                //"GrossProfitRate2": escape($.trim($("#txt" + "GrossProfitRate2").val())),
                //"GrossProfitRate3": escape($.trim($("#txt" + "GrossProfitRate3").val())),
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

    $("#btnAbility").click(function () {
        $(this).removeClass('ui-state-focus');
        $("[edittpye='basic']").each(function (index) {
            $(this).hide();
        });
        $("#divAbility").show();
    });
    $("#btnAbilitySave").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;

        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/EditBasicInfoAbility",
            data: {
                "VendorCode": '',
                "BasicInfoGUID": escape($.trim($('#ihBasicInfoGUID').val())),
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

    $("#btnSetCategory").click(function () {
        $(this).removeClass('ui-state-focus');
        var DoSuccessfully = false;
        $.ajax({
            url: __WebAppPathPrefix + "/SQMBasic/SendMail",
            data: {
                "content":'test123'
            },
            type: "post",
            dataType: 'text',
            async: false,
            success: function (data) {
                if (data == "") {
                    alert("send mail successfully.");
                }
                else {
                    alert("send mail error:" + data);
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



    $("#btnProduct").click(function () {
        $(this).removeClass('ui-state-focus');
        $("[edittpye='basic']").each(function (index) {
            $(this).hide();
        });
        getSQMProducesProduct();
        $("#btnSearchP").click();
        $("#divProduct").show();


    });


    $("#btnAgents").click(function () {
        $(this).removeClass('ui-state-focus');
        $("[edittpye='basic']").each(function (index) {
            $(this).hide();
        });
        $("#divAgents").show();
        getAgentsProductVendor();
        getSQMAgentsProduct();
        $("#btnSearchA").click();
    });


    $("#btnTraders").click(function () {
        $(this).removeClass('ui-state-focus');
        $("[edittpye='basic']").each(function (index) {
            $(this).hide();
        });
        $("#divTraders").show();
        getSQMTradersT();
        getSQMTradersProductFile();
        getSQMTradersProductFile2();
        $("#btnSearchT").click();
    });


    $("#btnCriticism").click(function () {
        $(this).removeClass('ui-state-focus');
        $("[edittpye='basic']").each(function (index) {
            $(this).hide();
        });
        $("#divCriticism").show();
    });
    $("#btnSQMContact").click(function () {
        $(this).removeClass('ui-state-focus');
        $("[edittpye='basic']").each(function (index) {
            $(this).hide();
        });
        $("#divSQMContact").show();
    });
    $("#btnSQMEquipment").click(function () {
        $(this).removeClass('ui-state-focus');
        $("[edittpye='basic']").each(function (index) {
            $(this).hide();
        });
        $("#divSQMEquipment").show();
        $('#ddlEquipmentType').change();
      
    });
    
    $("#btnHR").click(function () {
        //window.location.href = "/VMIP2/SQMHR/HRInfo";
        $(this).removeClass('ui-state-focus');
        $("[edittpye='basic']").each(function (index) {
            $(this).hide();
        });
        $("#divSQMHR").show();
        getHR();
        getHRdialog();
        $("#btnSQMHRSearch").click();
    });
    $("#btnProcess").click(function () {
        //window.location.href = "/VMIP2/SQMProcess/ProcessInfo";
        $(this).removeClass('ui-state-focus');
        $("[edittpye='basic']").each(function (index) {
            $(this).hide();
        });
        $("#divSQMProcess").show();
        getProcess();
        $("#btnSQMProcessSearch").click();
    });
    $("#btnCertification").click(function () {
        $(this).removeClass('ui-state-focus');
        $("[edittpye='basic']").each(function (index) {
            $(this).hide();
        });
        $("#divSQMCertification").show();
        
        getCriticism2();
        $("#btnSQMCertificationSearch").click();
    });
});