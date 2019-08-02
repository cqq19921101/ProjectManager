$(function () {
    var gridDataList = $("#ContactgridDataList");
    var gridDataListInfo = $("#CriticismgridDataList");
    jQuery("#btnShowContact").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {   //single select
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            gridDataListInfo.jqGrid('clearGridData');
            gridDataListInfo.jqGrid('setGridParam', { postData: { MemberGUID: dataRow.MemberGUID } })
            gridDataListInfo.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
            $("#SQEContactdialogData").attr('MemberGUID', dataRow.MemberGUID);

            $("#Contact").hide()
            $("#ContacttbMain1").hide();
            $("#ContactgridDataList").hide();
            $("#CriticismMap").show()
            $('#CriticismtbMain1').show();
            $('#CriticismgridDataList').show();

        } else { alert("Please select a row data to show."); }

    });
})