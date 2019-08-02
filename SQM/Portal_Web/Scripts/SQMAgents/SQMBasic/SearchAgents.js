$(function () {
    $("#btnSearchA").click(function () {
        var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
        var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
        $(this).removeClass('ui-state-focus');
        var gridDataListA = $("#gridDataListA");
        gridDataListA.jqGrid('clearGridData');

        gridDataListA.jqGrid('setGridParam', {
            postData: {
                SearchText: escape($.trim($("#txtFilterTextA").val())),
                BasicInfoGUID: gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID
            }
        })
        gridDataListA.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
}); 
$(function () {
    $("#btnSearchA2").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataListA2 = $("#gridDataListA2");
        gridDataListA2.jqGrid('clearGridData');

        gridDataListA2.jqGrid('setGridParam', { postData: { SearchText: escape($.trim($("#txtFilterTextA2").val())) } })
        gridDataListA2.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});

