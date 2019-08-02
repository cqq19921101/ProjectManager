$(function () {
    jQuery("#btnSearch").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataList = $("#gridDataList");
        gridDataList.jqGrid('clearGridData');
        var searchText = escape($.trim($("#txtFilterText").val()));

        gridDataList.jqGrid('setGridParam', { postData: { SearchText: searchText } })
        gridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});