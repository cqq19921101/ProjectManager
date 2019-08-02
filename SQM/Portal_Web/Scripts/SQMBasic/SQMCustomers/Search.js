$(function () {
    $("#btnSQMCustomersSearch").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataList = $("#SQMCustomersgridDataList");
        gridDataList.jqGrid('clearGridData');
        var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
        var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
        gridDataList.jqGrid('setGridParam', { postData: { SearchText: gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID } })
        gridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});
