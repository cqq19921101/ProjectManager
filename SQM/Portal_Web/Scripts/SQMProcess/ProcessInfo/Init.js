function getProcess() {
    //Toolbar Buttons
    $("#btnSQMProcessSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnSQMProcessCreate").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnSQMProcessViewEdit").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnSQMProcessDelete").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
    var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
    var BasicInfoGUID = gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID;
    var gridSQMProcessDataList = $("#gridSQMProcessDataList");
    gridSQMProcessDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMProcess/LoadSQMProduct1JSonWithFilter',
        postData: { SearchText: "", BasicInfoGUID: BasicInfoGUID },
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
                    'Process Type',
                    '公司制程状况',
                    '制程名称',
                    '是否自有制程',
                    '委外供应商名称 '],
        colModel: [
            { name: 'BasicInfoGUID', index: 'BasicInfoGUID', width: 200, sortable: false, hidden: true },
            { name: 'ProcessType', index: 'ProcessType', width: 200, sortable: false, hidden: true },
            { name: 'CName', index: 'CName', width: 200, sortable: true, sorttype: 'text' },
            { name: 'ProcessName', index: 'ProcessName', width: 150, sortable: true, sorttype: 'text' },
            { name: 'OwnOrOut', index: 'OwnOrOut', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ExternalSupplierName', index: 'ExternalSupplierName', width: 150, sortable: true, sorttype: 'text' }
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'CName',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridSQMProcessListPager',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridSQMProcessDataList.jqGrid('navGrid', '#gridSQMProcessListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#tbSQMProcessMain1').show();
    $('#dialogSQMProcessData').show();

    loadProcessList();
}
function loadProcessList() {
    $.ajax({
        url: __WebAppPathPrefix + '/SQMProcess/GetProcessCategoryList',
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            var options;
            for (var idx in data) {
                options += '<option value=' + data[idx].CID + '>' + data[idx].CNAME + '</option>';
            }
            options += '<option value=-1>Other</option>';
            $('#ddlSQMProcessCName').html(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
    $("#txtSQMProcessCNameInput").hide();
}

