$(function () {
    $("#btnSearch").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataList = $("#gridDataList");
        gridDataList.jqGrid('clearGridData');

        gridDataList.jqGrid('setGridParam', { postData: { SearchText: escape($.trim($("#txtFilterText").val())), ReportType:$("#ddlReportTypeSearch").val() } })
        gridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});