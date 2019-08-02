$(function () {
    jQuery("#btnSearchCPK").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataListCPK = $("#gridDataListCPK");
        gridDataListCPK.jqGrid('clearGridData');
        var searchText = escape($.trim($("#txtFilterTextCPK").val()));

        gridDataListCPK.jqGrid('setGridParam', { postData: { SearchText: searchText } })
        gridDataListCPK.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});