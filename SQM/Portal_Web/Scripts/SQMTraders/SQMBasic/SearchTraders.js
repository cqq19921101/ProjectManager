$(function () {
    $("#btnSearchT").click(function () {
        var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
        var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
        $(this).removeClass('ui-state-focus');
        var gridDataListT = $("#gridDataListT");
        gridDataListT.jqGrid('clearGridData');

        gridDataListT.jqGrid('setGridParam', {
            postData: {
                SearchText: escape($.trim($("#txtFilterTextT").val())),
                BasicInfoGUID: gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID
            }
        })
        gridDataListT.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});