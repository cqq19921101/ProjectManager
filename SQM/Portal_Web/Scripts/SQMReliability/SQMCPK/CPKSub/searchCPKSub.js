$(function () {
    jQuery("#btnSearchCPKSub").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataListCPKSub = $("#gridDataListCPKSub");
        gridDataListCPKSub.jqGrid('clearGridData');
        var searchText = escape($.trim($("#txtFilterTextCPKSub").val()));

        gridDataListCPKSub.jqGrid('setGridParam', { postData: { SearchText: searchText } })
        gridDataListCPKSub.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});