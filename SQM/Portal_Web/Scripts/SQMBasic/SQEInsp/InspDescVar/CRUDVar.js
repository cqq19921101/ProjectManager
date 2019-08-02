$(function () {
    var dialogDescVar = $("#dialogDataDescVar");
    var gridDataListDescVariables = $("#gridDataListDescVariables");

    jQuery("#btnBackVar").click(function () {
        $(this).removeClass('ui-state-focus');
        gridDataListDescVariables.jqGrid('clearGridData');

        $('#inspCode').show();
        $('#inspDescVar').hide();
        $('#tbMain1DescVar').hide();

    });
    jQuery("#btnCreateDescVar").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListDescVariables.jqGrid('getGridParam', 'multiselect')) {   //single select
            dialogDescVar.attr('Mode', "c");
            DialogSetUIByModeDescVar(dialogDescVar.attr('Mode'));
            dialogDescVar.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnViewEditDescVar").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListDescVariables.jqGrid('getGridParam', 'multiselect')) {   //single select
            SetToViewModeDescVar();
        }
    });

    jQuery("#btndialogEditDataDescVar").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataListDescVariables.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('VendorCode'))
            if (true) {
                dialogDescVar.attr('Mode', "e");
                DialogSetUIByModeDescVar(dialogDescVar.attr('Mode'));
                dialogDescVar.dialog("option", "title", "Edit").dialog("open");
            }
            else {
                switch (r) {
                    case "timeout": $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); break;
                    case "l": alert("Data already lock by other user."); break;
                    default: alert("Data lock fail or application error."); break;
                }
            }
        } else { alert("Please select a row data to edit."); }
    });

    jQuery("#btndialogCancelEditDescVar").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialogDescVar.attr('SID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewModeDescVar();
    });

    function SetToViewModeDescVar() {
        var RowIdCode = gridDataListDescVariables.jqGrid('getGridParam', 'selrow');
        if (RowIdCode) {
            dialogDescVar.attr('Mode', "v");
            DialogSetUIByModeDescVar(dialogDescVar.attr('Mode'));
            dialogDescVar.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

})