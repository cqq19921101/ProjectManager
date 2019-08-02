$(function () {
    var gridDataList = $("#gridDataList");

    var dialogFunctionControllerActions = $("#dialogFunctionControllerActions");
    var gridFunctionControllerActionList = $("#gridFunctionControllerActionList");

    jQuery("#btnSetControllerActions").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataList.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var cacol = (gridDataList.jqGrid('getRowData', RowId)).ControllerActions;
                var cas = [];
                if (cacol != "")
                {
                    var caarray = cacol.split(",");
                    for(var iCnt =0; iCnt<caarray.length; iCnt++)
                    {
                        var castring = caarray[iCnt].split("|");
                        var ca = new Object();
                        ca.Controller = castring[0];
                        ca.Action = castring[1];
                        cas.push(ca);
                    }
                }
                gridFunctionControllerActionList.jqGrid('clearGridData');
                gridFunctionControllerActionList.jqGrid('setGridParam', { data: cas });
                gridFunctionControllerActionList.trigger('reloadGrid');

                dialogFunctionControllerActions.dialog("option", "title", "Set Function Controller Actions").dialog("open");
                $("#btndialogFCACreate").removeClass('ui-state-focus');

            } else { alert("Please select a row data to edit."); }
        }
    });
});
