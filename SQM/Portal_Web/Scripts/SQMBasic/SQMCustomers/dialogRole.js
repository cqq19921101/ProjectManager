//initial dialog
$(function () {
    var dialog = $("#SQMCustomersdialogData");
    
    //Toolbar Buttons
    $("#btnSQMCustomersdialogEditData").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnSQMCustomersdialogCancelEdit").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#SQMCustomersdialogData").dialog({
        autoOpen: false,
        height: 345,
        width: 450,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (dialog.attr('Mode') == "v") {
                    $(this).dialog("close");
                }
                else {
                    var DoSuccessfully = false;
                    var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
                    var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
                    $.ajax({
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMBasic/CreateCustomers" : "/SQMBasic/EditCustomers"),
                        data: {
                            "BasicInfoGUID": gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID,
                            "OEMCustomerName": escape($.trim($("#txtOEMCustomerName").val())),
                            "BusinessCategory": escape($.trim($("#txtBusinessCategory").val())),
                            "RevenuePer": escape($.trim($("#txtRevenuePer").val())),
                            "MajorMaterials": escape($.trim($("#txtMajorMaterials").val())),
                            "MajorSupplier": escape($.trim($("#txtMajorSupplier").val())),
                            "PurchaseRevenuePer": escape($.trim($("#txtPurchaseRevenuePer").val()))
                          
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialog.attr('Mode') == "c")
                                    alert("Customers create successfully.");
                                else
                                    alert("Customers edit successfully.");
                            }
                            else {
                                if ((dialog.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#SQMCustomerslblDiaErrMsg").html(data);
                            }
                        },
                        error: function (xhr, textStatus, thrownError) {
                            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                        },
                        complete: function (jqXHR, textStatus) {
                            //$("#ajaxLoading").hide();
                        }
                    });
                    if (DoSuccessfully) {
                        $(this).dialog("close");
                        $("#btnSQMCustomersSearch").click();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialog.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialog.attr('SID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });
});

//change dialog UI
// c: Create, v: View, e: Edit
function CustomersDialogSetUIByMode(Mode) {
    var dialog = $("#SQMCustomersdialogData");
    var gridDataList = $("#SQMCustomersgridDataList");
    var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
    var BasicInfoRowId = gridDataListBasicInfoType.jqGrid('getGridParam', 'selrow');
    switch (Mode) {
        case "c": //Create
            $("#SQMCustomersdialogDataToolBar").hide();

            dialog.attr('ItemRowId', "");
            dialog.attr('BasicInfoGUID', gridDataListBasicInfoType.jqGrid('getRowData', BasicInfoRowId).BasicInfoGUID);

            $("#txtOEMCustomerName").val("");
            $("#txtOEMCustomerName").removeAttr('disabled');
            $("#txtBusinessCategory").val("");
            $("#txtBusinessCategory").removeAttr('disabled');
            $("#txtRevenuePer").val("");
            $("#txtRevenuePer").removeAttr('disabled');
            $("#txtMajorMaterials").val("");
            $("#txtMajorMaterials").removeAttr('disabled');
            $("#txtMajorSupplier").val("");
            $("#txtMajorSupplier").removeAttr('disabled');
            $("#txtPurchaseRevenuePer").val("");
            $("#txtPurchaseRevenuePer").removeAttr('disabled');

            $("#SQMCustomerslblDiaErrMsg").html("");

            break;
        case "v": //View
            $("#btnSQMCustomersdialogEditData").button("option", "disabled", false);
            $("#btnSQMCustomersdialogCancelEdit").button("option", "disabled", true);
            $("#SQMCustomersdialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('BasicInfoGUID', dataRow.BasicInfoGUID);

           
            $("#txtOEMCustomerName").val(dataRow.OEMCustomerName);
            $("#txtOEMCustomerName").attr("disabled", "disabled");
            $("#txtBusinessCategory").val(dataRow.BusinessCategory);
            $("#txtBusinessCategory").attr("disabled", "disabled");
            $("#txtRevenuePer").val(dataRow.RevenuePer);
            $("#txtRevenuePer").attr("disabled", "disabled");
            $("#txtMajorMaterials").val(dataRow.MajorMaterials);
            $("#txtMajorMaterials").attr("disabled", "disabled");
            $("#txtMajorSupplier").val(dataRow.MajorSupplier);
            $("#txtMajorSupplier").attr("disabled", "disabled");
            $("#txtPurchaseRevenuePer").val(dataRow.PurchaseRevenuePer);
            $("#txtPurchaseRevenuePer").attr("disabled", "disabled");

            $("#SQMCustomerslblDiaErrMsg").html("");

            break;
        default: //Edit("e")
            $("#btnSQMCustomersdialogEditData").button("option", "disabled", true);
            $("#btnSQMCustomersdialogCancelEdit").button("option", "disabled", false);
            $("#SQMCustomersdialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('BasicInfoGUID', dataRow.BasicInfoGUID);

            $("#txtOEMCustomerName").val(dataRow.OEMCustomerName);
           
            $("#txtBusinessCategory").val(dataRow.BusinessCategory);
            $("#txtBusinessCategory").removeAttr('disabled');
            $("#txtRevenuePer").val(dataRow.RevenuePer);
            $("#txtRevenuePer").removeAttr('disabled');
            $("#txtMajorMaterials").val(dataRow.MajorMaterials);
            $("#txtMajorMaterials").removeAttr('disabled');
            $("#txtMajorSupplier").val(dataRow.MajorSupplier);
            $("#txtMajorSupplier").removeAttr('disabled');
            $("#txtPurchaseRevenuePer").val(dataRow.PurchaseRevenuePer);
            $("#txtPurchaseRevenuePer").removeAttr('disabled');

            $("#SQMCustomerslblDiaErrMsg").html("");

            break;
    }
}