$(function () {
    $("#btnSearchP").click(function () {
        var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
        var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
        $(this).removeClass('ui-state-focus');
        var gridDataListP = $("#gridDataListP");
        gridDataListP.jqGrid('clearGridData');
        gridDataListP.jqGrid('setGridParam', {
            postData: {
                SearchText: escape($.trim($("#txtFilterTextP").val())), BasicInfoGUID: gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID
            }
        })
        gridDataListP.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});