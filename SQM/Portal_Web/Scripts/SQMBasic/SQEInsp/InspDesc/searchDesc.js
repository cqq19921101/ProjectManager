$(function () {
    jQuery("#btnSearchDesc").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataListDesc = $("#gridDataListDesc");
        gridDataListDesc.jqGrid('clearGridData');
        var searchText = escape($.trim($("#txtFilterTextDesc").val()));

        gridDataListDesc.jqGrid('setGridParam', { postData: { SearchText: searchText } })
        gridDataListDesc.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});