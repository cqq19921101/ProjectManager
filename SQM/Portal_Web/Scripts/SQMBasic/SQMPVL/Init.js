$(function () {
    $("#tbGRReport").hide();
    //01.getBasicInfoTypeList
    //Data List
    var cn = ['BasicInfoGUID',
                '編號',
              '調查表種類',
              '產品類別',
              '產品子類別',
              '部門代碼',
              '供應商代碼',
              '是否選定',
              '分數',
              '簽核狀態'
];
    var cm = [
            { "name": 'BasicInfoGUID', "index": 'BasicInfoGUID', "width": 200, "sortable": false, "hidden": true },
            { name: 'rows', index: 'rows', width: 100, sortable: true, sorttype: 'text' },
            { name: 'VTNAME', index: 'VTNAME', width: 150, sortable: true, sorttype: 'text' },
            { name: 'CNAME', index: 'CNAME', width: 150, sortable: true, sorttype: 'text' },
            { name: 'CSNAME', index: 'CSNAME', width: 350, sortable: true, sorttype: 'text '},
            { name: 'PlantCode', index: 'PlantCode', width: 150, sortable: true, sorttype: 'text' },
            { name: 'VendorCode', index: 'VendorCode', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ISChoose', index: 'ISChoose', width: 200, sortable: true, sorttype: 'text' },
            { name: 'Score', index: 'Score', width: 150, sortable: true, sorttype: 'text', "hidden": true },
            { name: 'Status', index: 'Status', width: 150, sortable: true, sorttype: 'text'},
    ];
    //$("#btnSubmit").hide();
    //$("#btnNSubmit").hide();

    var gridDataListBasicInfoType = $("#gridPVL");
    gridDataListBasicInfoType.jqGrid({
        url: __WebAppPathPrefix + '/SQMBasic/GetPVL',
        postData: {
            VTNAME: escape($("#ddlVendorType").val()),
            CNAME: escape($("#ddlCategory").val()),
            CSNAME: escape($("#ddlCategorySub").val())
        },
        mtype: "POST",
        datatype: "local",
        //datatype: "json",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        width: 800,
        height: "auto",
        colNames: cn,
        colModel: cm,
        rowNum: 20,
        //rowList: [10, 20, 30],
        sortname: 'SubFuncName',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridPVLListPager',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        },
    });
    gridDataListBasicInfoType.jqGrid('navGrid', '#gridPVLListPager', { edit: false, add: false, del: false, search: true, refresh: false });

    //tbGRReport    spVendorType    spCommodity     spCommoditySub  btnBack
    $("#btnBack").button({
        label: "Back",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnBack").click(function () {
        $("#tbGRReport").hide();
        $("#tbBasicInfoType").show();
    });

    $("#btnCreateBasicInfoType").button({
        label: "Create",
        icons: { primary: "ui-icon-pencil" }
    });

    $("#btnViewEditBasicInfoType").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });

    $("#btnViewEditDetail").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            loadBasicDataDetail(gridDataListBasicInfoType.jqGrid('getRowData', RowId).BasicInfoGUID);
        } else { alert("Please select a row data to edit."); }
        
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
            loadByVendorType("");
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {

        }
    });

    $('#ddlCategory').change(function () {
        $.ajax({
            url: __WebAppPathPrefix + '/SQMBasic/GetCommoditySubList',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
            data: { MainID: $('#ddlCategory').val() },
            type: "post",
            dataType: 'json',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                var options = '';
                for (var idx in data) {
                    options += '<option value=' + data[idx].CID + '>' + data[idx].CID +' '+ data[idx].CNAME + '</option>';
                }
                $('#ddlCategorySub').html(options);
                var dialog = $("#gridPVL");
                if (dialog.attr("mode")=="v") {
                    var gridDataListBasicInfoType = $("#gridPVL");
                    var RowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
                    if (RowId) {
                        $("#ddl" + "CategorySub").val(gridDataListBasicInfoType.jqGrid('getRowData', RowId).TB_SQM_Commodity_SubCID).change();
                    }
                }
                
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
            //$("#ddl" + "Category").val(BasicData.TB_SQM_Commodity_SubTB_SQM_CommodityCID).change();
            //$('#ddlCategory').change();
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus)
        {
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

//function loadBasicDataDetail(BasicInfoGUID) {
//    //load Types
//    var gridDataListBasicInfoType = $("#gridPVL");
//    var RowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
//    $('#ihBasicInfoGUID').val(BasicInfoGUID);
//    if (RowId) {
//        //tbGRReport    spVendorType    spCommodity     spCommoditySub
//        $('#ihBasicInfoGUID').val(gridDataListBasicInfoType.jqGrid('getRowData', RowId).TB_SQM_Vendor_TypeCID);
//        $("#spVendorType").html(gridDataListBasicInfoType.jqGrid('getRowData', RowId).VTNAME);
//        $("#spCommodity").html(gridDataListBasicInfoType.jqGrid('getRowData', RowId).CNAME);
//        $("#spCommoditySub").html(gridDataListBasicInfoType.jqGrid('getRowData', RowId).CSNAME);
//    }
//    $("#tbBasicInfoType").hide();
//    $("[edittpye='basic']").each(function (index) {
//        $(this).hide();
//    });
//    $("#divBasic").show();
//    $("#tbMain1").show();
//    $("#tbGRReport").show();
//    loadByVendorType("");
//    //getBasicData
//    var BasicData;
//    $.ajax({
//        url: __WebAppPathPrefix + '/SQMBasic/GetBasicData',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
//        data: { "BasicInfoGUID": BasicInfoGUID },
//        type: "post",
//        dataType: 'json',
//        async: false, // if need page refresh, please remark this option
//        success: function (data) {
//            BasicData = data[0];
//            //alert(BasicData.TB_SQM_Vendor_TypeCID);
//            //setDataAleadyRecord
//            $("#txt" + "VendorCode").val(BasicData.VendorCode);
//            $("#txt" + "CompanyName").val(BasicData.CompanyName);
//            $("#txt" + "CompanyAddress").val(BasicData.CompanyAddress);
//            $("#txt" + "FactoryName").val(BasicData.FactoryName);
//            $("#txt" + "FactoryAddress").val(BasicData.FactoryAddress);
//            $("#txt" + "DateInfo").val((new Date(BasicData.DateInfo).yyyy_mm_dd()));
//            $("#txt" + "ProvidedName").val(BasicData.ProvidedName);
//            $("#txt" + "JobTitle").val(BasicData.JobTitle);
//            if (BasicData.IsTrader) {
//                $("#cb" + "IsTrader").attr('checked', 'checked');
//            }
//            if (BasicData.IsSpotTrader) {
//                $("#cb" + "IsSpotTrader").attr('checked', 'checked');
//            }

//            //split
//            $("#txt" + "EnterpriseCategory").val(BasicData.EnterpriseCategory);
//            $("#txt" + "OwnerShip").val(BasicData.OwnerShip);
//            $("#txt" + "FoundedYear").val(BasicData.FoundedYear);
//            $("#txt" + "LastRevenues1").val(BasicData.LastRevenues1);
//            $("#txt" + "LastRevenues2").val(BasicData.LastRevenues2);
//            $("#txt" + "LastRevenues3").val(BasicData.LastRevenues3);
//            $("#txt" + "CurrentRevenues").val(BasicData.CurrentRevenues);
//            $("#txt" + "TurnoverAnalysis").val(BasicData.TurnoverAnalysis);
//            $("#txt" + "RevenueGrowthRate1").val(BasicData.RevenueGrowthRate1);
//            $("#txt" + "RevenueGrowthRate2").val(BasicData.RevenueGrowthRate2);
//            $("#txt" + "RevenueGrowthRate3").val(BasicData.RevenueGrowthRate3);
//            $("#txt" + "GrossProfitRate1").val(BasicData.GrossProfitRate1);
//            $("#txt" + "GrossProfitRate2").val(BasicData.GrossProfitRate2);
//            $("#txt" + "GrossProfitRate3").val(BasicData.GrossProfitRate3);
//            $("#txt" + "PlanInvestCapital").val(BasicData.PlanInvestCapital);
//            $("#txt" + "BankAndAccNumber").val(BasicData.BankAndAccNumber);
//            try {
//                $("#txt" + "TradingCurrency").val(BasicData.TradingCurrency.replace("CNY;", "").replace("USD;", ""));
//            } catch (e) {

//            }
            
//            if ((BasicData.TradingCurrency.indexOf("CNY;")) > -1) {
//                $("#" + "TradingCurrencyCNY").attr('checked', 'checked');
//            }
//            if ((BasicData.TradingCurrency.indexOf("USD;")) > -1) {
//                $("#" + "TradingCurrencyUSD").attr('checked', 'checked');
//            }
//            if ($("#txt" + "TradingCurrency").val() != "") {
//                $("#" + "TradingCurrencyOther").attr('checked', 'checked');
//            }

//            //$("#txt" + "TradeMode").val(BasicData.TradeMode);
//            if ((BasicData.TradeMode.indexOf("OUT;")) > -1) {
//                $("#" + "TradeModeOUT").attr('checked', 'checked');
//            }
//            if ((BasicData.TradeMode.indexOf("TRANS;")) > -1) {
//                $("#" + "TradeModeTRANS").attr('checked', 'checked');
//            }
//            if ((BasicData.TradeMode.indexOf("IN;")) > -1) {
//                $("#" + "TradeModeIN").attr('checked', 'checked');
//            }

//            $("#txt" + "VMIManageModel").val(BasicData.VMIManageModel);
//            $("#txt" + "Distance").val(BasicData.Distance);
//            $("#txt" + "MinMonthStateDays").val(BasicData.MinMonthStateDays);
//            $("#txt" + "BU1TurnoverName").val(BasicData.BU1TurnoverName);
//            $("#txt" + "BU2TurnoverName").val(BasicData.BU2TurnoverName);
//            $("#txt" + "BU3TurnoverName").val(BasicData.BU3TurnoverName);
//            $("#txt" + "BU1Turnover").val(BasicData.BU1Turnover);
//            $("#txt" + "BU2Turnover").val(BasicData.BU2Turnover);
//            $("#txt" + "BU3Turnover").val(BasicData.BU3Turnover);
//            $("#txt" + "CompanyAdvantage").val(BasicData.CompanyAdvantage);

function loadByVendorType(vType) {
    
    var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
    var RowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
    if (RowId) {
        vType = gridDataListBasicInfoType.jqGrid('getRowData', RowId).TB_SQM_Vendor_TypeCID;
    }
    $("[isHideType='Y']").hide();
    //商品
    //基本資訊    
    $("[vType" + vType + "='Y']").show();
        try {
            $("#SQMCustomersgridDataList").jqGrid("GridUnload");
        } catch (e) {

        }
    setTimeout(function () {

    }, 200);
    
    $('#btnBasic').click();
    
}






