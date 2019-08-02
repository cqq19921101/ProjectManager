$(function () {
    jQuery("#btnCustomer").click(function () {
        $(this).removeClass('ui-state-focus');
        $("[edittpye='basic']").each(function (index) {
            $(this).hide();
        });
        $("#divSQMCustomers").show();
   
        loadCustomData();
    });

    //Toolbar Buttons
    $("#btnSQMCustomersSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnSQMCustomersCreate").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnSQMCustomersViewEdit").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnSQMCustomersDelete").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });
    $("#btnHPVendorNumUpdate").button({
        label: "Update",
        icons: { primary: "ui-icon-pencil" }
    });
    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List

    //var gridDataList = $("#SQMCustomersgridDataList");
    //if (escape($.trim($("#ddl" + "VendorType").val()))==1) {
    //    gridDataList.jqGrid({
    //        url: __WebAppPathPrefix + '/SQMBasic/LoadCustomersJSonWithFilter',
    //        postData: { SearchText: "" },
    //        type: "post",
    //        datatype: "json",
    //        jsonReader: {
    //            root: "Rows",
    //            page: "Page",
    //            total: "Total",
    //            records: "Records",
    //            repeatitems: false
    //        },
    //        width: 800,
    //        height: "auto",
    //        colNames: ['VendorCode',
    //                    'OEMCustomerName',
    //                   'BusinessCategory',
    //                   'RevenuePer',
    //                   'MajorMaterials',
    //                   'MajorSupplier',
    //                   'PurchaseRevenuePer'

    //        ],
    //        colModel: [
    //            { name: 'VendorCode', index: 'VendorCode', width: 200, sortable: false, hidden: true },
    //            { name: 'OEMCustomerName', index: 'OEMCustomerName', width: 150, sortable: true, sorttype: 'text' },
    //            { name: 'BusinessCategory', index: 'BusinessCategory', width: 150, sortable: true, sorttype: 'text' },
    //            { name: 'RevenuePer', index: 'RevenuePer', width: 150, sortable: true, sorttype: 'text' },
    //            { name: 'MajorMaterials', index: 'MajorMaterials', width: 150, sortable: true, sorttype: 'text' },
    //            { name: 'MajorSupplier', index: 'MajorSupplier', width: 150, sortable: true, sorttype: 'text' },
    //            { name: 'PurchaseRevenuePer', index: 'PurchaseRevenuePer', width: 150, sortable: true, sorttype: 'text' }
    //        ],
    //        rowNum: 10,
    //        //rowList: [10, 20, 30],
    //        sortname: 'OEMCustomerName',
    //        viewrecords: true,
    //        loadonce: true,
    //        mtype: 'POST',
    //        pager: '#SQMCustomersgridListPager',
    //        //sort by reload
    //        loadComplete: function () {
    //            var $this = $(this);
    //            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
    //                if ($this.jqGrid('getGridParam', 'sortname') !== '')
    //                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
    //        }
    //    });
    //}
    //else {
    //    gridDataList.jqGrid({
    //        url: __WebAppPathPrefix + '/SQMBasic/LoadCustomersJSonWithFilter',
    //        postData: { SearchText: "" },
    //        type: "post",
    //        datatype: "json",
    //        jsonReader: {
    //            root: "Rows",
    //            page: "Page",
    //            total: "Total",
    //            records: "Records",
    //            repeatitems: false
    //        },
    //        width: 800,
    //        height: "auto",
    //        colNames: ['VendorCode',
    //                    'OEMCustomerName',
    //                   'BusinessCategory',
    //                   'RevenuePer',


    //        ],
    //        colModel: [
    //            { name: 'VendorCode', index: 'VendorCode', width: 200, sortable: false, hidden: true },
    //            { name: 'OEMCustomerName', index: 'OEMCustomerName', width: 150, sortable: true, sorttype: 'text' },
    //            { name: 'BusinessCategory', index: 'BusinessCategory', width: 150, sortable: true, sorttype: 'text' },
    //            { name: 'RevenuePer', index: 'RevenuePer', width: 150, sortable: true, sorttype: 'text' },
               
    //        ],
    //        rowNum: 10,
    //        //rowList: [10, 20, 30],
    //        sortname: 'OEMCustomerName',
    //        viewrecords: true,
    //        loadonce: true,
    //        mtype: 'POST',
    //        pager: '#SQMCustomersgridListPager',
    //        //sort by reload
    //        loadComplete: function () {
    //            var $this = $(this);
    //            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
    //                if ($this.jqGrid('getGridParam', 'sortname') !== '')
    //                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
    //        }
    //    });
    //}

    //gridDataList.jqGrid('navGrid', '#SQMCustomersgridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#SQMCustomersMain1').show();
    $('#SQMCustomersdialogData').show();
});

function loadCustomData() {
    var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
    var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
    var vType = gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).TB_SQM_Vendor_TypeCID;
    var gridDataList = $("#SQMCustomersgridDataList");
    try {
        $("#SQMCustomersgridDataList").jqGrid("GridUnload");
    } catch (e) {

    }
    setTimeout(function () {

    }, 200);
    if (vType == "1") {
        var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
        var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
        gridDataList.jqGrid({
            url: __WebAppPathPrefix + '/SQMBasic/LoadCustomersJSonWithFilter',
            postData: { SearchText: gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID },
            type: "post",
            datatype: "json",
            jsonReader: {
                root: "Rows",
                page: "Page",
                total: "Total",
                records: "Records",
                repeatitems: false
            },
            width: 800,
            height: "auto",
            colNames: ['BasicInfoGUID',
                        '前3大OEM客戶',
                       '交易产品名称或类别',
                       '占營業份額 百分比%',
                       '主要原材料名称',
                       '主要材料供应商(可填写多家)',
                       '占总采购金额比率%'

            ],
            colModel: [
                { name: 'BasicInfoGUID', index: 'BasicInfoGUID', width: 200, sortable: false, hidden: true },
                { name: 'OEMCustomerName', index: 'OEMCustomerName', width: 150, sortable: true, sorttype: 'text' },
                { name: 'BusinessCategory', index: 'BusinessCategory', width: 150, sortable: true, sorttype: 'text' },
                { name: 'RevenuePer', index: 'RevenuePer', width: 150, sortable: true, sorttype: 'text' },
                { name: 'MajorMaterials', index: 'MajorMaterials', width: 150, sortable: true, sorttype: 'text' },
                { name: 'MajorSupplier', index: 'MajorSupplier', width: 150, sortable: true, sorttype: 'text' },
                { name: 'PurchaseRevenuePer', index: 'PurchaseRevenuePer', width: 150, sortable: true, sorttype: 'text' }
            ],
            rowNum: 10,
            //rowList: [10, 20, 30],
            sortname: 'OEMCustomerName',
            viewrecords: true,
            loadonce: true,
            mtype: 'POST',
            pager: '#SQMCustomersgridListPager',
            //sort by reload
            loadComplete: function () {
                var $this = $(this);
                if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                    if ($this.jqGrid('getGridParam', 'sortname') !== '')
                        setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
            }
        });
    }
    else {
        var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
        var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
        gridDataList.jqGrid({
            url: __WebAppPathPrefix + '/SQMBasic/LoadCustomersJSonWithFilter',
            postData: { SearchText: gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID },
            type: "post",
            datatype: "json",
            jsonReader: {
                root: "Rows",
                page: "Page",
                total: "Total",
                records: "Records",
                repeatitems: false
            },
            width: 800,
            height: "auto",
            colNames: ['BasicInfoGUID',
                        '前3大OEM客戶',
                       '交易产品名称或类别',
                       '占營業份額 百分比%',


            ],
            colModel: [
                { name: 'BasicInfoGUID', index: 'VendorCode', width: 200, sortable: false, hidden: true },
                { name: 'OEMCustomerName', index: 'OEMCustomerName', width: 150, sortable: true, sorttype: 'text' },
                { name: 'BusinessCategory', index: 'BusinessCategory', width: 150, sortable: true, sorttype: 'text' },
                { name: 'RevenuePer', index: 'RevenuePer', width: 150, sortable: true, sorttype: 'text' },

            ],
            rowNum: 10,
            //rowList: [10, 20, 30],
            sortname: 'OEMCustomerName',
            viewrecords: true,
            loadonce: true,
            mtype: 'POST',
            pager: '#SQMCustomersgridListPager',
            //sort by reload
            loadComplete: function () {
                var $this = $(this);
                if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                    if ($this.jqGrid('getGridParam', 'sortname') !== '')
                        setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
            }
        });
    }

    gridDataList.jqGrid('navGrid', '#SQMCustomersgridListPager', { edit: false, add: false, del: false, search: false, refresh: false });
}