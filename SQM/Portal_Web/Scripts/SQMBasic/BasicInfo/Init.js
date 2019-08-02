$(function () {

    //00.hide
    $("[edittpye='basic']").each(function (index) {
        $(this).hide();
    });
    $("#tbGRReport").hide();
    //01.getBasicInfoTypeList
    //Data List
    var cn = ['BasicInfoGUID',
              'VendorCode',
              'TB_SQM_Vendor_TypeCID',
              '調查表種類',
              'TB_SQM_Commodity_SubCID',
              '產品類別',
              'TB_SQM_Commodity_SubTB_SQM_CommodityCID',
              '產品子類別',
              '審核狀態'];
    var cm = [
            { "name": 'BasicInfoGUID', "index": 'BasicInfoGUID', "width": 200, "sortable": false, "hidden": true },
            { name: 'VendorCode', index: 'VendorCode', width: 150, sortable: true, sorttype: 'text', "hidden": true },
            { name: 'TB_SQM_Vendor_TypeCID', index: 'TB_SQM_Vendor_TypeCID', width: 150, sortable: true, sorttype: 'text', "hidden": true },
            { name: 'VTNAME', index: 'VTNAME', width: 250, sortable: true, sorttype: 'text' },
            { name: 'TB_SQM_Commodity_SubCID', index: 'TB_SQM_Commodity_SubCID', width: 150, sortable: true, sorttype: 'text', "hidden": true },
            { name: 'CNAME', index: 'CNAME', width: 250, sortable: true, sorttype: 'text' },
            { name: 'TB_SQM_Commodity_SubTB_SQM_CommodityCID', index: 'TB_SQM_Commodity_SubTB_SQM_CommodityCID', width: 150, sortable: true, sorttype: 'text', "hidden": true },
            { name: 'CSNAME', index: 'CSNAME', width: 250, sortable: true, sorttype: 'text' },
            { name: 'Status', index: 'Status', width: 250, sortable: true, sorttype: 'text' }
    ];
    $("#btnCommit").hide();
    $("#btnSuspend").hide();

    var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
    gridDataListBasicInfoType.jqGrid({
        url: __WebAppPathPrefix + '/SQMBasic/GetBasicInfoTypes',
        postData: { SearchText: "" },
        type: "post",
        datatype: "json",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        width: 600,
        height: "auto",
        colNames: cn,
        colModel: cm,
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'SubFuncName',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridListPagerBasicInfoType',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        },
        onSelectRow: function (id) {
            var $this = $(this);
            var selRow = $this.jqGrid('getGridParam', 'selrow');

            if (selRow) {
                var rowData = $this.jqGrid('getRowData', selRow);
                $('#ihTB_SQM_Vendor_TypeCID').val(rowData.TB_SQM_Vendor_TypeCID);

                if (rowData.Status == "None" || rowData.Status == "Reject") {
                    $("#btnCommit").show();
                    $("#btnSuspend").hide();
                } else if (rowData.Status == "Pending") {
                    $("#btnCommit").hide();
                    $("#btnSuspend").show();
                } else if (rowData.Status == "Finished") {
                    $("#btnCommit").hide();
                    $("#btnSuspend").hide();
                }
            }
        }
    });
    gridDataListBasicInfoType.jqGrid('navGrid', '#gridListPagerBasicInfoType', { edit: false, add: false, del: false, search: false, refresh: false });

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
    $('#btnCommit').button({
        label: '提交送審',
        icons: { primary: 'ui-icon-arrowthickstop-1-s' }
    });
    $('#btnSuspend').button({
        label: '取消送審',
        icons: { primary: 'ui-icon-arrowthickstop-1-s' }
    });
    $("#btnDeleteBasicInfoType").button({
        label: "Delete",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnViewEditDetail").button({
        label: "EditDetail",
        icons: { primary: "ui-icon-pencil" }
    });

    $("#btnViewEditDetail").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            loadBasicDataDetail(gridDataListBasicInfoType.jqGrid('getRowData', RowId).BasicInfoGUID);
        } else { alert("Please select a row data to edit."); }

    });

    $("#btnSetCategory").button({
        label: "保存/變更",
        icons: { primary: "ui-icon-pencil" }
    });

    $("#btnBasic").button({
        label: "基本資訊",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnBasicSave").button({
        label: "保存/變更",
        icons: { primary: "ui-icon-pencil" }
    });

    $("#btnGenral").button({
        label: "一般資訊",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnGenralSave").button({
        label: "保存/變更",
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
    $("#btnTraders").button({
        label: "貿易商產品",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnCustomer").button({
        label: "顧客 /供應商 / 分包商",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnHR").button({
        label: "人力資源",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnCertification").button({
        label: "證書",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnEquipmentMain").button({
        label: "設備",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnCriticism").button({
        label: "Criticism",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnSQMContact").button({
        label: "通訊錄",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnSQMEquipment").button({
        label: "設備",
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
        label: "製程",
        icons: { primary: "ui-icon-pencil" }
    });
    //$("#btnCertification").button({
    //    label: "體系認證",
    //    icons: {primary: "ui-icon-pencil"}
    //});
    $("#btnAbility").button({
        label: "技術能力",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnAbilitySave").button({
        label: "保存/變更",
        icons: { primary: "ui-icon-pencil" }
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
                    options += '<option value=' + data[idx].CID + '>' + data[idx].CID + ' ' + data[idx].CNAME + '</option>';
                }
                $('#ddlCategorySub').html(options);
                var dialog = $("#dialogDataBasicInfoType");
                if (dialog.attr("mode") == "v") {
                    var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
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

function loadBasicDataDetail(BasicInfoGUID) {
    //load Types
    var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
    var RowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
    $('#ihBasicInfoGUID').val(BasicInfoGUID);
    if (RowId) {
        //tbGRReport    spVendorType    spCommodity     spCommoditySub
        $('#ihBasicInfoGUID').val(gridDataListBasicInfoType.jqGrid('getRowData', RowId).BasicInfoGUID);
        $('#ihTB_SQM_Vendor_TypeCID').val(gridDataListBasicInfoType.jqGrid('getRowData', RowId).TB_SQM_Vendor_TypeCID);
        $("#spVendorType").html(gridDataListBasicInfoType.jqGrid('getRowData', RowId).VTNAME);
        $("#spCommodity").html(gridDataListBasicInfoType.jqGrid('getRowData', RowId).CNAME);
        $("#spCommoditySub").html(gridDataListBasicInfoType.jqGrid('getRowData', RowId).CSNAME);
    }
    $("#tbBasicInfoType").hide();
    $("[edittpye='basic']").each(function (index) {
        $(this).hide();
    });
    $("#divBasic").show();
    $("#tbMain1").show();
    $("#tbGRReport").show();
    loadByVendorType("");
    //getBasicData
    var BasicData;
    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetBasicData',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        data: { "BasicInfoGUID": BasicInfoGUID },
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
            $("#txt" + "DateInfo").val((new Date(BasicData.DateInfo).yyyy_mm_dd()));
            $("#txt" + "ProvidedName").val(BasicData.ProvidedName);
            $("#txt" + "JobTitle").val(BasicData.JobTitle);
            if (BasicData.IsTrader) {
                $("#cb" + "IsTrader").attr('checked', 'checked');
            }
            if (BasicData.IsSpotTrader) {
                $("#cb" + "IsSpotTrader").attr('checked', 'checked');
            }

            //split
            $("#txt" + "EnterpriseCategory").val(BasicData.EnterpriseCategory);
            $("#txt" + "OwnerShip").val(BasicData.OwnerShip);
            $("#txt" + "FoundedYear").val((new Date(BasicData.FoundedYear).yyyy_mm_dd()));
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
            try {
                $("#txt" + "TradingCurrency").val(BasicData.TradingCurrency.replace("CNY;", "").replace("USD;", ""));
            } catch (e) {

            }

            try {
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
            } catch (e) {

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
}

function loadByVendorType(vType) {

    var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
    var RowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
    var vSub;
    if (RowId) {
        vType = gridDataListBasicInfoType.jqGrid('getRowData', RowId).TB_SQM_Vendor_TypeCID;
        vSub = gridDataListBasicInfoType.jqGrid('getRowData', RowId).TB_SQM_Commodity_SubCID;
    }

    //if (vType == "1") {
    //    //商品
    //    $("[ptype='product']").hide();
    //    $("#btnProduct").show();
    //} else if (vType == "2") {
    //    $("[ptype='product']").hide();
    //    $("#btnTraders").show();

    //} else if (vType == "3") {
    //    $("[ptype='product']").hide();
    //    $("#btnAgents").show();
    //}
    $("[isHideType='Y']").hide();
    //商品
    //基本資訊    
    $("[vType" + vType + "='Y']").show();
    if (vSub == "DL-ME02" || vSub == "DL-ME03" || vSub == "DL-ME04") {
        $("#btnAbility").show();
    }
    else {
        $("#btnAbility").hide();
    }
    try {
        $("#SQMCustomersgridDataList").jqGrid("GridUnload");
    } catch (e) {

    }
    setTimeout(function () {

    }, 200);

    $('#btnBasic').click();

}

