$(function () {
    $('#ddlEquipmentSpecialType').on('change', function () {
        var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
        var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
        // Clear jqGrid content
        $('#SQMEquipmentgridDataList').jqGrid('clearGridData');
        // Sent params to jqGrid and reload it
        $('#SQMEquipmentgridDataList').jqGrid('setGridParam', {
            postData: {
                EquipmentType: escape($('#ddlEquipmentType').val()), EquipmentSpecialType: escape($('#ddlEquipmentSpecialType').val() > 0 ? 0 : $('#ddlEquipmentSpecialType').val())
    , BasicInfoGUID: gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID
            }
        });
        $('#SQMEquipmentgridDataList').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
    $('#ddlEquipmentType').on('change', function () {
        var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
        var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
        // Clear jqGrid content
        $('#SQMEquipmentgridDataList').jqGrid('clearGridData');
        // Sent params to jqGrid and reload it
        $('#SQMEquipmentgridDataList').jqGrid('setGridParam', {
            postData: {
                EquipmentType: escape($('#ddlEquipmentType').val()), EquipmentSpecialType: escape($('#ddlEquipmentSpecialType').val() > 0 ? 0 : $('#ddlEquipmentSpecialType').val())
                    , BasicInfoGUID: gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID
            }
        });
        $('#SQMEquipmentgridDataList').jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
})