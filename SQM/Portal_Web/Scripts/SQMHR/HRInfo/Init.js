function getHR() {
    //Toolbar Buttons
    $("#btnSQMHRSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnSQMHRCreate").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnSQMHRViewEdit").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnSQMHRDelete").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var gridSQMHRDataList = $("#gridSQMHRDataList");
    var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
    var rowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
    var gridBasicInfoGUID = gridDataListBasicInfoType.jqGrid('getRowData', rowId).BasicInfoGUID;
    gridSQMHRDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMHR/LoadSQMProduct1JSonWithFilter',
        postData: { SearchText: "", BasicInfoGUID: gridBasicInfoGUID },
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
        colNames: ['BasicInfoGUID',
                    'HR Type',
                    '公司人力资源',
                    '实际員工人數',
                    '编制人数',
                    '平均工作年资 ',
                    '占总人数比率'],
        colModel: [
            { name: 'BasicInfoGUID', index: 'BasicInfoGUID', width:200, sorttype: false, hidden:true },
            { name: 'HRType', index: 'HRType', width: 200, sortable: false, hidden: true },
            { name: 'CName', index: 'CName', width: 200, sortable: true, sorttype: 'text' },
            { name: 'EmployeeQty', index: 'EmployeeQty', width: 150, sortable: true, sorttype: 'text' },
            { name: 'EmployeePlanned', index: 'EmployeePlanned', width: 150, sortable: true, sorttype: 'text' },
            { name: 'AverageJobSeniority', index: 'AverageJobSeniority', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Percent', index: 'Percent', width: 150, sortable: true, sorttype: 'text' }
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'CName',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridSQMHRListPager',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridSQMHRDataList.jqGrid('navGrid', '#gridSQMHRListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    //var gridSQMHRDataList = $("#gridSQMHRDataList");
    //var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
    //var rowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
    //var gridBasicInfoGUID = gridDataListBasicInfoType.jqGrid('getRowData', rowId).BasicInfoGUID;

    //alert($("#gridDataListBasicInfoType").jqGrid('getRowData', $("#gridDataListBasicInfoType").jqGrid('getGridParam', 'selrow')).BasicInfoGUID);

    //Data Total
    $.ajax({
        url: __WebAppPathPrefix + '/SQMHR/GetTotalData',
        data: { BasicInfoGUID: $("#gridDataListBasicInfoType").jqGrid('getRowData', $("#gridDataListBasicInfoType").jqGrid('getGridParam', 'selrow')).BasicInfoGUID },
        type: "post",
        dataType: "json",
        async: false,
        success: function (data) {
            var TotalEmployeeQty;
            var TotalEmployeePlanned;
            for (var idx in data) {
                TotalEmployeeQty = data[idx].EmployeeQty;
                TotalEmployeePlanned = data[idx].EmployeePlanned;
            }
            $('#txtTotalEmployees').val(TotalEmployeeQty);
            $('#txtNumberPlanned').val(TotalEmployeePlanned);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });

    $('#tbSQMHRMain1').show();
    $('#dialogSQMHRData').show();

    $.ajax({
        url: __WebAppPathPrefix + '/SQMHR/GetHRCategoryList',
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            var options;
            for (var idx in data) {
                options += '<option value=' + data[idx].CID + '>' + data[idx].CNAME + '</option>';
            }
            $('#ddlSQMHRCName').html(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}
   
