$(function () {
    $("#btnSearch").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataList = $("#gridDataList");
        gridDataList.jqGrid('clearGridData');

        gridDataList.jqGrid('setGridParam', { postData: { SearchText: "" } })
        gridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});
