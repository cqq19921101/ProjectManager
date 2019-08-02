$(function () {
    $("#btnSQMProcessSearch").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridSQMProcessDataList = $("#gridSQMProcessDataList");
        var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
        var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
        gridSQMProcessDataList.jqGrid('clearGridData');

        gridSQMProcessDataList.jqGrid('setGridParam', { postData: { SearchText: escape($.trim($("#txtSQMProcessFilterText").val())), BasicInfoGUID: gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID } })
        gridSQMProcessDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
        loadProcessList();
    });
});