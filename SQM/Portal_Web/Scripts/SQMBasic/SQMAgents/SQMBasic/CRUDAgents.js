$(function () {
    var gridDataListA = $("#gridDataListA");
    var dialog = $("#dialogDataA");

    jQuery("#btnCreateA").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListA.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            dialog.attr('Mode', "c");
            DialogSetUIByModeA(dialog.attr('Mode'));
            dialog.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnViewEditA").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListA.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            SetToViewMode();
        }
    });

    jQuery("#btndialogEditDataA").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataListA.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('VendorCode'))
            if (true) {
                dialog.attr('Mode', "e");
                DialogSetUIByModeA(dialog.attr('Mode'));
                dialog.dialog("option", "title", "Edit").dialog("open");
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

    jQuery("#btndialogCancelEditA").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialog.attr('BasicInfoGUID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewMode();
    });

    function SetToViewMode()
    {
        var RowId = gridDataListA.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            dialog.attr('Mode', "v");
            DialogSetUIByModeA(dialog.attr('Mode'));
            dialog.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnDeleteA").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListA.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            var RowId = gridDataListA.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                //var DataKey = (gridDataListA.jqGrid('getRowData', RowId)).SubFuncGUID;
                //var r = AcquireDataLock(DataKey)
                if (true) {
                    if (confirm("Confirm to delete selected member (" + gridDataListA.jqGrid('getRowData', RowId).PrincipalProducts + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMBasic/DeleteAgents",
                            data: { "PrincipalProducts": gridDataListA.jqGrid('getRowData', RowId).PrincipalProducts },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSearchA").click();
                                    alert("PrincipalProducts delete successfully.");
                                }
                                else {
                                    alert("PrincipalProducts delete fail due to:\n\n" + data);
                                }
                            },
                            error: function (xhr, textStatus, thrownError) {
                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                            },
                            complete: function (jqXHR, textStatus) {
                                //$("#ajaxLoading").hide();
                            }
                        });
                    }
                    else {
                        var r = ReleaseDataLock(DataKey);
                        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
                    }
                }
                else {
                    switch (r) {
                        case "timeout": $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); break;
                        case "l": alert("Data already lock by other user."); break;
                        default: alert("Data lock fail or application error."); break;
                    }
                }
            } else { alert("Please select a row data to delete."); }
        }
    });
});



$(function () {
    var gridDataListA2 = $("#gridDataListA2");
    var dialog = $("#dialogDataA2");

    jQuery("#btnCreateA2").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListA2.jqGrid('getGridParam', 'multiselect')) {   //single select
            dialog.attr('Mode', "c");
            DialogSetUIByModeA2(dialog.attr('Mode'));
            dialog.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnViewEditA2").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListA2.jqGrid('getGridParam', 'multiselect')) {   //single select
            SetToViewModeA2();
        }
    });

    jQuery("#btndialogEditDataA2").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataListA2.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('VendorCode'))
            if (true) {
                dialog.attr('Mode', "e");
                DialogSetUIByModeA2(dialog.attr('Mode'));
                dialog.dialog("option", "title", "Edit").dialog("open");
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

    jQuery("#btndialogCancelEditA2").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialog.attr('VendorCode'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewModeA2();
    });

    function SetToViewModeA2() {
        var RowId = gridDataListA2.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            dialog.attr('Mode', "v");
            DialogSetUIByModeA2(dialog.attr('Mode'));
            dialog.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnDeleteA2").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListA2.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowId = gridDataListA2.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                //var DataKey = (gridDataListA.jqGrid('getRowData', RowId)).SubFuncGUID;
                //var r = AcquireDataLock(DataKey)
                if (true) {
                    if (confirm("Confirm to delete selected member (" + gridDataListA2.jqGrid('getRowData', RowId).FactoryName + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMBasic/DeleteAgents2",
                            data: { "FactoryName": gridDataListA2.jqGrid('getRowData', RowId).FactoryName },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSearchA2").click();
                                    alert("FactoryName delete successfully.");
                                }
                                else {
                                    alert("FactoryName delete fail due to:\n\n" + data);
                                }
                            },
                            error: function (xhr, textStatus, thrownError) {
                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                            },
                            complete: function (jqXHR, textStatus) {
                                //$("#ajaxLoading").hide();
                            }
                        });
                    }
                    else {
                        var r = ReleaseDataLock(DataKey);
                        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
                    }
                }
                else {
                    switch (r) {
                        case "timeout": $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); break;
                        case "l": alert("Data already lock by other user."); break;
                        default: alert("Data lock fail or application error."); break;
                    }
                }
            } else { alert("Please select a row data to delete."); }
        }
    });
});


$(function () {
    $("#btnPlace").click(function () {
       
        $("#PlaceA2").show();
    });

    $("#btnPlaceA2").click(function () {

        $("#PlaceA2").hide();

    });

});