$(function () {
    $("#btnSQEContactSearch").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataList = $("#SQEContactgridDataList");
        gridDataList.jqGrid('clearGridData');

        gridDataList.jqGrid('setGridParam', { postData: { SearchText: escape($.trim($("#txtFilterText").val())) } })
        gridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});
