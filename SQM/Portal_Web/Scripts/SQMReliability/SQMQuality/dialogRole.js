//initial dialog
$(function () {
    var dialog = $("#dialogData");
    
    //Toolbar Buttons
    $("#btndialogEditData").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogCancelEdit").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#txtOQCDate").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        minDate: 0,
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtOQCDate").datepicker("setDate", '-31d');

    $("#txtDeliveryDate").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtDeliveryDate").datepicker("setDate", '-31d');

    $("#txtDateCode").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtDateCode").datepicker("setDate", '-31d');

    $.ajax({
        url: __WebAppPathPrefix + '/SQMReliability/GetReportTypeList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].SID + '> ' + data[idx].ReportName + '</option>';
            }
            $('#ddlReportType').html(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });

    $("#dialogData").dialog({
        autoOpen: false,
        height: 570,
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
                    $.ajax({
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMReliability/CreateQuality" : "/SQMReliability/EditQuality"),
                        data: {
                            "ReportSID": dialog.attr("ReportSID"),
                            "ReportNo": escape($.trim($("#txtReportNo").val())),
                            "Supplier": escape($.trim($("#txtSupplier").val())),
                            "DeliveryDate": escape($.trim($("#txtDeliveryDate").val())),
                            "Qty": escape($.trim($("#txtQty").val())),
                            "SupplierNo": escape($.trim($("#txtSupplierNo").val())),
                            "LiteNo": escape($.trim($("#txtLiteNo").val())),
                            "LotNo": escape($.trim($("#txtLotNo").val())),
                            "DateCode": escape($.trim($("#txtDateCode").val())),
                            "OQCDate": escape($.trim($("#txtOQCDate").val())),
                            "MFGLocation": escape($.trim($("#txtMFGLocation").val())),
                            "SupplierRemark": escape($.trim($("#txtSupplierRemark").val())),
                            "isChange": escape($.trim($("#txtisChange").val())),
                            "ChangeNote": escape($.trim($("#txtChangeNote").val())),
                            "Equipment": escape($.trim($("#txtEquipment").val())),
                            "Material": escape($.trim($("#txtMaterial").val())),
                            "Inspector": escape($.trim($("#txtInspector").val())),
                            "ReportType": escape($.trim($("#ddlReportType").val())),
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialog.attr('Mode') == "c")
                                    alert(" create successfully.");
                                else
                                    alert(" edit successfully.");
                            }
                            else {
                                if ((dialog.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblDiaErrMsg").html(data);
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
                        $("#btnSearch").click();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialog.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialog.attr('SubFuncGUID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });
});

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByMode(Mode) {
    var dialog = $("#dialogData");
    var gridDataList = $("#gridDataList");
    switch (Mode) {
        case "c": //Create
            $("#dialogDataToolBar").hide();

            dialog.attr('ItemRowId', "");
            dialog.attr('ReportSID', "");

            $("#ddlReportType option:first").prop("selected", 'selected');
            $("#ddlReportType").removeAttr('disabled');

            $("#txtSupplier").val("");
            $("#txtSupplier").attr("disabled", "disabled");
            $("#txtReportNo").val("");
            $("#txtReportNo").attr("disabled", "disabled");
            $("#txtDeliveryDate").val("");
            $("#txtDeliveryDate").removeAttr('disabled');
            $("#txtQty").val("");
            $("#txtQty").removeAttr('disabled');
            $("#txtSupplierNo").val("");
            $("#txtSupplierNo").removeAttr('disabled');
            $("#txtLiteNo").val("");
            $("#txtLiteNo").removeAttr('disabled');
            $("#txtLotNo").val("");
            $("#txtLotNo").removeAttr('disabled');
            $("#txtDateCode").val("");
            $("#txtDateCode").removeAttr('disabled');
            $("#txtOQCDate").val("");
            $("#txtOQCDate").removeAttr('disabled');
            $("#txtMFGLocation").val("");
            $("#txtMFGLocation").removeAttr('disabled');
            $("#txtSupplierRemark").val("");
            $("#txtSupplierRemark").removeAttr('disabled');
            $("#txtisChange option:first").prop("selected", 'selected').change();
            $("#txtisChange").removeAttr('disabled');
            $("#txtChangeNote").val("");
            $("#txtChangeNote").removeAttr('disabled');
            $("#txtEquipment").val("");
            $("#txtEquipment").removeAttr('disabled');
            $("#txtMaterial").val("");
            $("#txtMaterial").removeAttr('disabled');
            $("#txtInspector").val("");
            $("#txtInspector").removeAttr('disabled');
           

            $("#lblDiaErrMsg").html("");

            break;
        case "v": //View
            $("#btndialogEditData").button("option", "disabled", false);
            $("#btndialogCancelEdit").button("option", "disabled", true);
            $("#dialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('ReportSID', dataRow.ReportSID);

            $("#ddlReportType").val(dataRow.ReportType);
            $("#ddlReportType").attr("disabled", "disabled");
         
            $("#txtReportNo").val(dataRow.ReportNo);
            $("#txtReportNo").attr("disabled", "disabled");
            $("#txtSupplier").val(dataRow.Supplier);
            $("#txtSupplier").attr("disabled", "disabled");
            $("#txtDeliveryDate").val(dataRow.DeliveryDate);
            $("#txtDeliveryDate").attr("disabled", "disabled");
            $("#txtQty").val(dataRow.Qty);
            $("#txtQty").attr("disabled", "disabled");
            $("#txtSupplierNo").val(dataRow.SupplierNo);
            $("#txtSupplierNo").attr("disabled", "disabled");
            $("#txtLiteNo").val(dataRow.LiteNo);
            $("#txtLiteNo").attr("disabled", "disabled");
            $("#txtLotNo").val(dataRow.LotNo);
            $("#txtLotNo").attr("disabled", "disabled");
            $("#txtDateCode").val(dataRow.DateCode);
            $("#txtDateCode").attr("disabled", "disabled");
            $("#txtOQCDate").val(dataRow.OQCDate);
            $("#txtOQCDate").attr("disabled", "disabled");
            $("#txtMFGLocation").val(dataRow.MFGLocation);
            $("#txtMFGLocation").attr("disabled", "disabled");
            $("#txtSupplierRemark").val(dataRow.SupplierRemark);
            $("#txtSupplierRemark").attr("disabled", "disabled");
            $("#txtisChange").val(dataRow.isChange == "True"?"1":"0").change();
            $("#txtisChange").attr("disabled", "disabled");
            $("#txtChangeNote").val(dataRow.ChangeNote);
            $("#txtChangeNote").attr("disabled", "disabled");
            $("#txtEquipment").val(dataRow.Equipment);
            $("#txtEquipment").attr("disabled", "disabled");
            $("#txtMaterial").val(dataRow.Material);
            $("#txtMaterial").attr("disabled", "disabled");
            $("#txtInspector").val(dataRow.Inspector);
            $("#txtInspector").attr("disabled", "disabled");
            $("#txtApproveder").val(dataRow.Approveder);
            $("#txtApproveder").attr("disabled", "disabled");
            $("#lblDiaErrMsg").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditData").button("option", "disabled", true);
            $("#btndialogCancelEdit").button("option", "disabled", false);
            $("#dialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('ReportSID', dataRow.ReportSID);

            $("#ddlReportType").val(dataRow.ReportType);
            $("#ddlReportType").attr("disabled", "disabled");
  
            $("#txtReportNo").val(dataRow.ReportNo);
            $("#txtReportNo").attr("disabled", "disabled");
            $("#txtSupplier").val(dataRow.Supplier);
            $("#txtSupplier").attr("disabled", "disabled");
            $("#txtDeliveryDate").val(dataRow.DeliveryDate);
            $("#txtDeliveryDate").removeAttr('disabled');
            $("#txtQty").val(dataRow.Qty);
            $("#txtQty").removeAttr('disabled');
            $("#txtSupplierNo").val(dataRow.SupplierNo);
            $("#txtSupplierNo").removeAttr('disabled');
            $("#txtLiteNo").val(dataRow.LiteNo);
            $("#txtLiteNo").removeAttr('disabled');
            $("#txtLotNo").val(dataRow.LotNo);
            $("#txtLotNo").removeAttr('disabled');
            $("#txtDateCode").val(dataRow.DateCode);
            $("#txtDateCode").removeAttr('disabled');
            $("#txtOQCDate").val(dataRow.OQCDate);
            $("#txtOQCDate").removeAttr('disabled');
            $("#txtMFGLocation").val(dataRow.MFGLocation);
            $("#txtMFGLocation").removeAttr('disabled');
            $("#txtSupplierRemark").val(dataRow.SupplierRemark);
            $("#txtSupplierRemark").removeAttr('disabled');
            $("#txtisChange").val(dataRow.isChange == "True" ? "1" : "0").change();
            $("#txtisChange").removeAttr('disabled');
            $("#txtChangeNote").val(dataRow.ChangeNote);
            $("#txtChangeNote").removeAttr('disabled');
            $("#txtEquipment").val(dataRow.Equipment);
            $("#txtEquipment").removeAttr('disabled');
            $("#txtMaterial").val(dataRow.Material);
            $("#txtMaterial").removeAttr('disabled');
            $("#txtInspector").val(dataRow.Inspector);
            $("#txtInspector").removeAttr('disabled');
            $("#txtApproveder").val(dataRow.Approveder);
            $("#txtApproveder").attr("disabled", "disabled");
            $("#lblDiaErrMsg").html("");

            break;
    }
}