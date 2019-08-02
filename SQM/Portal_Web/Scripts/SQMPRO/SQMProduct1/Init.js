$(function () {
    //Toolbar Buttons
    $("#btnSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCreate").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnViewEdit").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnDelete").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var gridDataList = $("#gridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMPRO/LoadSQMProduct1JSonWithFilter',
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
        colNames: ['HR Type',
                    '公司人力资源',
                    '实际員工人數',
                    '编制人数',
                    '平均工作年资 ',
                    '占总人数比率'],
        colModel: [
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
        pager: '#gridListPager',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataList.jqGrid('navGrid', '#gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    //Data Total
    $.ajax({
        url: __WebAppPathPrefix + '/SQMPRO/GetTotalData',
        type: "post",
        dataType: 'json',
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

    $('#tbMain1').show();
    $('#dialogData').show();

    $.ajax({
        url: __WebAppPathPrefix + '/SQMPRO/GetHRCategoryList',
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            var options;
            for (var idx in data) {
                options += '<option value=' + data[idx].CID + '>' + data[idx].CNAME + '</option>';
            }
            $('#ddlCName').append(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
});
   
