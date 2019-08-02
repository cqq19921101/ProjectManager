$(function () {
    jQuery("#btnSearchCode").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataListCode = $("#gridDataListCode");
        gridDataListCode.jqGrid('clearGridData');
        var searchText = escape($.trim($("#txtFilterTextCode").val()));

        gridDataListCode.jqGrid('setGridParam', { postData: { SearchText: searchText, Insptype:$("#ddlSInsptype").val() } })
        gridDataListCode.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});