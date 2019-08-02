$(function () {
    $("#btnSetCategory").button({
        label: "設置/變更",
        icons: { primary: "ui-icon-pencil" }
    });

    $("#btnBasic").button({
        label: "Basic",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnBasicSave").button({
        label: "設置/變更",
        icons: { primary: "ui-icon-pencil" }
    });

    $("#btnGenral").button({
        label: "Genral",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnGenralSave").button({
        label: "設置/變更",
        icons: { primary: "ui-icon-pencil" }
    });

    $("#btnProduct").button({
        label: "製造商產品",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnAgents").button({
        label: "代理商產品",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnTraders").button({
        label: "貿易商產品",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnCSS").button({
        label: "CSS",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnHR").button({
        label: "HR",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnCertification").button({
        label: "Certification",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnEquipmentMain").button({
        label: "EquipmentMain",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnEquipmentMainTest").button({
        label: "EquipmentMainTest",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnEquipmentReliability").button({
        label: "EquipmentReliability",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnEquipmentHSF").button({
        label: "EquipmentHSF",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnEquipmentLogisitic").button({
        label: "EquipmentLogisitic",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnProcess").button({
        label: "Process",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnAbility").button({
        label: "Ability",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnAbilitySave").button({
        label: "設置/變更",
        icons: { primary: "ui-icon-pencil" }
    });

    $("[edittpye='basic']").each(function (index) {
        $(this).hide();
    });
    $("#divBasic").show();

    //getBasicData
    var BasicData;
    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetBasicData',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            BasicData = data[0];
            //alert(BasicData.TB_SQM_Vendor_TypeCID);
            //setDataAleadyRecord
            $("#txt" + "VendorCode").val(BasicData.VendorCode);
            $("#txt" + "CompanyName").val(BasicData.CompanyName);
            $("#txt" + "CompanyAddress").val(BasicData.CompanyAddress);
            $("#txt" + "FactoryName").val(BasicData.FactoryName);
            $("#txt" + "FactoryAddress").val(BasicData.FactoryAddress);
            $("#txt" + "DateInfo").val(BasicData.DateInfo);
            $("#txt" + "ProvidedName").val(BasicData.ProvidedName);
            $("#txt" + "JobTitle").val(BasicData.JobTitle);
            
            //split
            $("#txt" + "EnterpriseCategory").val(BasicData.EnterpriseCategory);
            $("#txt" + "OwnerShip").val(BasicData.OwnerShip);
            $("#txt" + "FoundedYear").val(BasicData.FoundedYear);
            $("#txt" + "LastRevenues1").val(BasicData.LastRevenues1);
            $("#txt" + "LastRevenues2").val(BasicData.LastRevenues2);
            $("#txt" + "LastRevenues3").val(BasicData.LastRevenues3);
            $("#txt" + "CurrentRevenues").val(BasicData.CurrentRevenues);
            $("#txt" + "TurnoverAnalysis").val(BasicData.TurnoverAnalysis);
            $("#txt" + "RevenueGrowthRate1").val(BasicData.RevenueGrowthRate1);
            $("#txt" + "RevenueGrowthRate2").val(BasicData.RevenueGrowthRate2);
            $("#txt" + "RevenueGrowthRate3").val(BasicData.RevenueGrowthRate3);
            $("#txt" + "GrossProfitRate1").val(BasicData.GrossProfitRate1);
            $("#txt" + "GrossProfitRate2").val(BasicData.GrossProfitRate2);
            $("#txt" + "GrossProfitRate3").val(BasicData.GrossProfitRate3);
            $("#txt" + "PlanInvestCapital").val(BasicData.PlanInvestCapital);
            $("#txt" + "BankAndAccNumber").val(BasicData.BankAndAccNumber);
            $("#txt" + "TradingCurrency").val(BasicData.TradingCurrency.replace("CNY;", "").replace("USD;", ""));
            if ((BasicData.TradingCurrency.indexOf("CNY;")) > -1) {
                $("#" + "TradingCurrencyCNY").attr('checked', 'checked');
            }
            if ((BasicData.TradingCurrency.indexOf("USD;")) > -1) {
                $("#" + "TradingCurrencyUSD").attr('checked', 'checked');
            }
            if ($("#txt" + "TradingCurrency").val() != "") {
                $("#" + "TradingCurrencyOther").attr('checked', 'checked');
            }

            //$("#txt" + "TradeMode").val(BasicData.TradeMode);
            if ((BasicData.TradeMode.indexOf("OUT;")) > -1) {
                $("#" + "TradeModeOUT").attr('checked', 'checked');
            }
            if ((BasicData.TradeMode.indexOf("TRANS;")) > -1) {
                $("#" + "TradeModeTRANS").attr('checked', 'checked');
            }
            if ((BasicData.TradeMode.indexOf("IN;")) > -1) {
                $("#" + "TradeModeIN").attr('checked', 'checked');
            }

            $("#txt" + "VMIManageModel").val(BasicData.VMIManageModel);
            $("#txt" + "Distance").val(BasicData.Distance);
            $("#txt" + "MinMonthStateDays").val(BasicData.MinMonthStateDays);
            $("#txt" + "BU1TurnoverName").val(BasicData.BU1TurnoverName);
            $("#txt" + "BU2TurnoverName").val(BasicData.BU2TurnoverName);
            $("#txt" + "BU3TurnoverName").val(BasicData.BU3TurnoverName);
            $("#txt" + "BU1Turnover").val(BasicData.BU1Turnover);
            $("#txt" + "BU2Turnover").val(BasicData.BU2Turnover);
            $("#txt" + "BU3Turnover").val(BasicData.BU3Turnover);
            $("#txt" + "CompanyAdvantage").val(BasicData.CompanyAdvantage);

            //Ability
            if (BasicData.Is3DUG) $("#" + "Is3DUG").attr('checked', 'checked');
            if (BasicData.Is3DProE) $("#" + "Is3DProE").attr('checked', 'checked');
            if (BasicData.Is2DAutoCAD) $("#" + "Is2DAutoCAD").attr('checked', 'checked');
            if (BasicData.IsPhotoShop) $("#" + "IsPhotoShop").attr('checked', 'checked');
            if (BasicData.IsIDMapAbility) $("#" + "IsIDMapAbility").attr('checked', 'checked');
            if (BasicData.Is3DMapAbility) $("#" + "Is3DMapAbility").attr('checked', 'checked');
            if (BasicData.Is2DMapAbility) $("#" + "Is2DMapAbility").attr('checked', 'checked');
            if (BasicData.IsMoldflowAbility) $("#" + "IsMoldflowAbility").attr('checked', 'checked');
            if (BasicData.IsTAAbility) $("#" + "IsTAAbility").attr('checked', 'checked');
            if (BasicData.IsDesignGuildline) $("#" + "IsDesignGuildline").attr('checked', 'checked');
            if (BasicData.IsFMEA) $("#" + "IsFMEA").attr('checked', 'checked');
            if (BasicData.IsLessonLearnt) $("#" + "IsLessonLearnt").attr('checked', 'checked');

            $("#ddl" + "MoldProduceCapacity").val(BasicData.MoldProduceCapacity).change();
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {

        }
    });


    //getVendor Type
    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetVendorType',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            //var options = '<option value=-1 Selected>All</option>';
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].CID + '>' + data[idx].CNAME + '</option>';
            }
            $('#ddlVendorType').append(options);
            //load now data
            $("#ddlVendorType").val(BasicData.TB_SQM_Vendor_TypeCID).change();
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {

        }
    });
    //$.ajax({
    //    url: __WebAppPathPrefix + '/SQMBasic/GetVendorType',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
    //    type: "post",
    //    dataType: 'json',
    //    async: false, // if need page refresh, please remark this option
    //    success: function (data) {
    //        //var options = '<option value=-1 Selected>All</option>';
    //        var options = '';
    //        for (var idx in data) {
    //            options += '<option value=' + data[idx].CID + '>' + data[idx].CID + ' ' + data[idx].CNAME + '</option>';
    //        }
    //        $('#ddlVendorType').append(options);
    //    },
    //    error: function (xhr, textStatus, thrownError) {
    //        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
    //    },
    //    complete: function (jqXHR, textStatus) {
    //    }
    //});

    $('#ddlCategory').change(function () {
        $.ajax({
            url: __WebAppPathPrefix + '/SQMBasic/GetCommoditySubList',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
            data: { MainID: $('#ddlCategory').val() },
            type: "post",
            dataType: 'json',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                //var options = '<option value=-1 Selected>All</option>';
                var options = '';
                for (var idx in data) {
                    options += '<option value=' + data[idx].CID + '>' + data[idx].CID +' '+ data[idx].CNAME + '</option>';
                }
                //$('#ddlCategory').append(options);
                $('#ddlCategorySub').html(options);
                $("#ddl" + "ddlCategorySub").val(BasicData.ddlCategorySub).change();
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    });

    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetCommodityList',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            //var options = '<option value=-1 Selected>All</option>';
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].CID + '>' + data[idx].CID + ' ' + data[idx].CNAME + '</option>';
            }
            $('#ddlCategory').append(options);
            $("#ddl" + "ddlCategory").val(BasicData.ddlCategory).change();
            //$('#ddlCategory').change();
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
    
   
    //GetCommoditySubList
    //$.ajax({
    //    url: __WebAppPathPrefix + '/SQMBasic/GetCommoditySubList',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
    //    data: { MainID: $('#ddlCategory').val() },
    //    type: "post",
    //    dataType: 'json',
    //    async: false, // if need page refresh, please remark this option
    //    success: function (data) {
    //        //var options = '<option value=-1 Selected>All</option>';
    //        var options = '';
    //        for (var idx in data) {
    //            options += '<option value=' + data[idx].CID + '>' + data[idx].CID +' '+ data[idx].CNAME + '</option>';
    //        }
    //        //$('#ddlCategory').append(options);
    //        $('#ddlCategorySub').html(options);
    //    },
    //    error: function (xhr, textStatus, thrownError) {
    //        $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
    //    },
    //    complete: function (jqXHR, textStatus) {
    //    }
    //});
});
