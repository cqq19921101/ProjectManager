$(function () {
    jQuery("#btnSearchMap").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataListMap = $("#gridDataListMap");
        gridDataListMap.jqGrid('clearGridData');
        var searchText = escape($.trim($("#txtFilterTextMap").val()));

        gridDataListMap.jqGrid('setGridParam', { postData: { SearchText: searchText } })
        gridDataListMap.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});