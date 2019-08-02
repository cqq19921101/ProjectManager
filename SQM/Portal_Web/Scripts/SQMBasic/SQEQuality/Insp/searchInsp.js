$(function () {
    jQuery("#btnSearchInsp").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataListInsp = $("#gridDataListInsp");
        gridDataListInsp.jqGrid('clearGridData');
        var searchTextOfInspCode = escape($.trim($("#ddlSInspCode").val()));
        var searchTextOfInspDesc = escape($.trim($("#ddlSInspDesc").val()));

        gridDataListInsp.jqGrid('setGridParam', { postData: { SInspCode: searchTextOfInspCode, SInspDesc: searchTextOfInspDesc } })
        gridDataListInsp.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
    jQuery("#btnSearchInspVar").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataListInspVar = $("#gridDataListInspVar");
        gridDataListInspVar.jqGrid('clearGridData');
        var searchTextOfInspCode = escape($.trim($("#ddlSInspCodeVar").val()));
        var searchTextOfInspDesc = escape($.trim($("#ddlSInspDescVar").val()));

        gridDataListInspVar.jqGrid('setGridParam', { postData: { SInspCode: searchTextOfInspCode } })
        gridDataListInspVar.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
});