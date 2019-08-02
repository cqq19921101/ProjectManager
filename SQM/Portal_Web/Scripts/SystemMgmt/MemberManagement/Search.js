$(function () {
    $("#btnSearch").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataList = $("#gridDataList");
        gridDataList.jqGrid('clearGridData');

        var sMemberType = "";
        if ($('#radFilterMemberTypeInternalOnly').is(':checked'))
            sMemberType = "2";
        else if ($('#radFilterMemberTypeExternalOnly').is(':checked'))
            sMemberType = "1";
        
        gridDataList.jqGrid('setGridParam', { postData: { SearchText: escape($.trim($("#txtFilterText").val())), MemberType: sMemberType } })
        gridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});