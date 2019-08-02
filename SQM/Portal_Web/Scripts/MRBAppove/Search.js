$(function () {
    $("#btnSearch").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataList = $("#gridDataList");
        gridDataList.jqGrid('clearGridData');

        gridDataList.jqGrid('setGridParam', { postData: { SearchText: "" } })
        gridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
    $("#btnMapSearch").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataList = $("#gridDataList");
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');

        var dataRow = gridDataList.jqGrid('getRowData', RowId);
        var MapgridDataList = $("#MapgridDataList");
        MapgridDataList.jqGrid('clearGridData');

        MapgridDataList.jqGrid('setGridParam', { postData: { SearchText: dataRow.SID } })
        MapgridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});
