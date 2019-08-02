$(function () {
    jQuery("#btnCombSearch").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataList = $("#gridDataListKeyInfoType");
        gridDataList.jqGrid('clearGridData');
        var searchText = escape($.trim($("#txtERPFilterText").val()));
        var plantText = escape($.trim($("#ddlPlant").val()));
        gridDataList.jqGrid('setGridParam', { postData: { SearchText: searchText, PlantText: plantText } })
        gridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});