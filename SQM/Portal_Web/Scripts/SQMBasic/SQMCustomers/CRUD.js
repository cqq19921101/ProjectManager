$(function () {
  
    var dialog = $("#SQMCustomersdialogData");

    jQuery("#btnSQMCustomersCreate").click(function () {
        $(this).removeClass('ui-state-focus');
          var gridDataList = $("#SQMCustomersgridDataList");
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            dialog.attr('Mode', "c");
            CustomersDialogSetUIByMode(dialog.attr('Mode'));
            dialog.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnSQMCustomersViewEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataList = $("#SQMCustomersgridDataList");
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            SetToCustomersViewMode();
        }
    });

    jQuery("#btnSQMCustomersdialogEditData").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataList = $("#SQMCustomersgridDataList");
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('SID'))
            
            //if (r == "ok") {
            if (true) {                
                dialog.attr('Mode', "e");
                CustomersDialogSetUIByMode(dialog.attr('Mode'));
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

    jQuery("#btnSQMCustomersdialogCancelEdit").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialog.attr('VendorCode'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToCustomersViewMode();
    });

    function SetToCustomersViewMode()
    {
        var gridDataList = $("#SQMCustomersgridDataList");
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            dialog.attr('Mode', "v");
            CustomersDialogSetUIByMode(dialog.attr('Mode'));
            dialog.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnSQMCustomersDelete").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataList = $("#SQMCustomersgridDataList");
        if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
        {   //single select
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            if (RowId) {
                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).BasicInfoGUID;
                //var r = AcquireDataLock(DataKey)
                
                //if (r == "ok") {
                if (true) {
                    if (confirm("Confirm to delete selected member (" + gridDataList.jqGrid('getRowData', RowId).OEMCustomerName + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMBasic/DeleteCustomers",
                            data: { "BasicInfoGUID": gridDataList.jqGrid('getRowData', RowId).BasicInfoGUID, "OEMCustomerName": gridDataList.jqGrid('getRowData', RowId).OEMCustomerName },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSQMCustomersSearch").click();
                                    alert("Customers delete successfully.");
                                }
                                else {
                                    alert("Customers delete fail due to:\n\n" + data);
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

    jQuery("#btnHPVendorNumUpdate").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
        var RowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
       $.ajax({
                            url: __WebAppPathPrefix + "/SQMBasic/HPVendorNumUpdate",
                            data: { "HPVendorNum": escape($.trim($("#txtHPVendorNum").val())), "BasicInfoGUID": gridDataListBasicInfoType.jqGrid('getRowData', RowId).BasicInfoGUID },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSQMCustomersSearch").click();
                                    alert("HPVendorNum Update successfully.");
                                }
                                else {
                                    alert("HPVendorNum Update fail due to:\n\n" + data);
                                }
                            },
                            error: function (xhr, textStatus, thrownError) {
                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                            },
                            complete: function (jqXHR, textStatus) {
                                //$("#ajaxLoading").hide();
                            }
                        });

              
               
    });
});