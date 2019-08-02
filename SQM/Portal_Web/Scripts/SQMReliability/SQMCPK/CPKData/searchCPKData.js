$(function () {
    jQuery("#btnSearchCPKData").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataListCPKData = $("#gridDataListCPKData");
        gridDataListCPKData.jqGrid('clearGridData');
        var searchText = escape($.trim($("#txtFilterTextCPKData").val()));

        gridDataListCPKData.jqGrid('setGridParam', { postData: { reportID: $("#dialogDataCPKSub").attr('reportID')
            , Designator: $("#dialogDataCPKSub").attr('Designator'), SearchText: searchText } })
        gridDataListCPKData.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});