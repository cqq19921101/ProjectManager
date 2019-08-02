$(function () {
    var gridDataList = $("#gridDataList");
    var dialog = $("#dialogData");

    jQuery("#btnRunAs").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                if (confirm("Confirm to Run As selected member (" + gridDataList.jqGrid('getRowData', RowId).AccountID + ")?")) {
                    var AccountGUID = gridDataList.jqGrid('getRowData', RowId).AccountGUID;
                    $("#hidRunAsMemberGUID").val(AccountGUID);
                    $("#_RunAsForm").submit();
                }
            } else { alert("Please select a member to RunAs."); }
        }
    });
});