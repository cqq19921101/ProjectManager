$(function () {
    jQuery("#btnSearchDescVar").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataListDescVar = $("#gridDataListDescVariables");
        gridDataListDescVar.jqGrid('clearGridData');
        var searchText = escape($.trim($("#txtFilterTextDescVar").val()));

        gridDataListDescVar.jqGrid('setGridParam', { postData: { SearchText: searchText } })
        gridDataListDescVar.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});