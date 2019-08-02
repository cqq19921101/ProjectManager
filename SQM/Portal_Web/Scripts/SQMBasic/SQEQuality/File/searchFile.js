$(function () {
    jQuery("#btnSearchFile").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataListFile = $("#gridDataListFile");
        gridDataListFile.jqGrid('clearGridData');

        gridDataListFile.jqGrid('setGridParam', { postData: { SearchText: escape($.trim($("#txtSearchFile").val())) } })
        gridDataListFile.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});