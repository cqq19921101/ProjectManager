$(function () {
    var gridDataList = $("#ContactgridDataList");
    var gridDataListInfo = $("#SQEContactgridDataList");
    jQuery("#btnShowContact").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {   //single select
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            gridDataListInfo.jqGrid('clearGridData');
            gridDataListInfo.jqGrid('setGridParam', { postData: { MemberGUID: dataRow.MemberGUID } })
            gridDataListInfo.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
            $("#SQEContactdialogData").attr('MemberGUID', dataRow.MemberGUID);
            $("#spVendor").html(dataRow.NameInChinese)

            $("#Contact").hide()
            $("#ContacttbMain1").hide();
            $("#ContactgridDataList").hide();
            $("#ContactInfo").show()
            $('#SQEContacttbMain1').show();
            $('#SQEContactdialogData').show();

        } else { alert("Please select a row data to show."); }

    });
})