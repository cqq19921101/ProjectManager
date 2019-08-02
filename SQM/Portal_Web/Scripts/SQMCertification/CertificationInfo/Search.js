$(function () {
    $("#btnSQMCertificationSearch").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridSQMCertificationDataList = $("#gridSQMCertificationDataList");
        var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
        var rowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
        var gridBasicInfoGUID = gridDataListBasicInfoType.jqGrid('getRowData', rowId).BasicInfoGUID;
        gridSQMCertificationDataList.jqGrid('clearGridData');

        gridSQMCertificationDataList.jqGrid('setGridParam', { postData: { SearchText: escape($.trim($("#txtSQMCertificationFilterText").val())), BasicInfoGUID: gridBasicInfoGUID} })
        gridSQMCertificationDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});