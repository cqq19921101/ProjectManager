$(function () {
    $("#btnSQMHRSearch").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridSQMHRDataList = $("#gridSQMHRDataList");
        var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
        var rowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
        var gridBasicInfoGUID = gridDataListBasicInfoType.jqGrid('getRowData', rowId).BasicInfoGUID;
        gridSQMHRDataList.jqGrid('clearGridData');

        gridSQMHRDataList.jqGrid('setGridParam', { postData: { SearchText: escape($.trim($("#txtSQMHRFilterText").val())), BasicInfoGUID: gridBasicInfoGUID } })
        gridSQMHRDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});