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

    $("#dialogData").dialog({
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
                    $.ajax({
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMMaterialMgmt/CreateSQMSCAR" : "/SQMMaterialMgmt/EditSQMSCAR"),
                        data: {
                            "SID": dialog.attr("SID"),
                            "anomalousTime": escape($.trim($("#txtAnomalousTime").val())),
                            "LitNo": escape($.trim($("#txtLitNo").val())),
                            "Model": escape($.trim($("#txtModel").val())),
                            "BitNo": escape($.trim($("#txtBitNo").val())),
                            "BitNum": escape($.trim($("#txtBitNum").val())),
                            "badnessNum": escape($.trim($("#txtbadnessNum").val())),
                            "RejectRatio": escape($.trim($("#txtRejectRatio").val())),
                            "Abnormal": escape($.trim($("#ddlAbnormal").val())),
                            "VenderCode": escape($.trim($("#txtVendorCode").val())),
                            "badnessNote": escape($.trim($("#txtbadnessNote").val())),
                            "badnessPic": escape($.trim($("#dialogData").attr("SCARD1FGUID")))
                         
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialog.attr('Mode') == "c")
                                    alert("SCAR create successfully.");
                                else
                                    alert("SCAR edit successfully.");
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
                var r = ReleaseDataLock(dialog.attr('SID'));
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
    var Abnormal;
    var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
    var dataRow = gridDataList.jqGrid('getRowData', RowId);
    switch (dataRow.Abnormal) {
        case "進料":
            Abnormal = 0;
            break;
        case "製程":
            Abnormal = 1;
            break;
        case "OQC":
            Abnormal = 2;
            break;
        case "客訴":
            Abnormal = 3;
            break;
        case "可靠測試":
            Abnormal = 4;
            break;
        default:

    }
    switch (Mode) {
        case "c": //Create
            $("#dialogDataToolBar").hide();

            dialog.attr('ItemRowId', "");
            dialog.attr('SID', "");

            $("#txtAnomalousTime").val("");
            $("#txtAnomalousTime").removeAttr('disabled');
            $("#txtLitNo").val("");
            $("#txtLitNo").removeAttr('disabled');
            $("#txtModel").val("");
            $("#txtModel").removeAttr('disabled');
            $("#txtBitNo").val("");
            $("#txtBitNo").removeAttr('disabled');
            $("#txtBitNum").val("");
            $("#txtBitNum").removeAttr('disabled');
            $("#txtbadnessNum").val("");
            $("#txtbadnessNum").removeAttr('disabled');
            $("#txtRejectRatio").val("");
            $("#txtRejectRatio").removeAttr('disabled');
            $("#ddlAbnormal").val("");
            $("#ddlAbnormal").removeAttr('disabled');
            $("#txtVenderCode").val("");
            $("#txtVenderCode").removeAttr('disabled');
            $("#txtbadnessNote").val("");
            $("#txtbadnessNote").removeAttr('disabled');
            $("#txtbadnessPic").val("");
            $("#txtbadnessPic").removeAttr('disabled');
            $("#lblDiaErrMsg").html("");

            break;
        case "v": //View
            $("#btndialogEditData").button("option", "disabled", false);
            $("#btndialogCancelEdit").button("option", "disabled", true);
            $("#dialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('SID', dataRow.SID);

            $("#txtAnomalousTime").val(dataRow.anomalousTime);
            $("#txtAnomalousTime").attr("disabled", "disabled");
            $("#txtLitNo").val(dataRow.LitNo);
            $("#txtLitNo").attr("disabled", "disabled");
            $("#txtModel").val(dataRow.model);
            $("#txtModel").attr("disabled", "disabled");
            $("#txtBitNo").val(dataRow.BitNo);
            $("#txtBitNo").attr("disabled", "disabled");
            $("#txtBitNum").val(dataRow.BitNum);
            $("#txtBitNum").attr("disabled", "disabled");
            $("#txtbadnessNum").val(dataRow.badnessNum);
            $("#txtbadnessNum").attr("disabled", "disabled");
            $("#txtRejectRatio").val(dataRow.rejectRatio);
            $("#txtRejectRatio").attr("disabled", "disabled");
            $("#ddlAbnormal").val(Abnormal);
            $("#ddlAbnormal").attr("disabled", "disabled");
            $("#txtVenderCode").val(dataRow.VenderCode);
            $("#txtVenderCode").attr("disabled", "disabled");
            $("#txtbadnessNote").val(dataRow.badnessNote);
            $("#txtbadnessNote").attr("disabled", "disabled");
            $("#txtbadnessPic").val(dataRow.badnessPic);
            $("#txtbadnessPic").attr("disabled", "disabled");

            $("#lblDiaErrMsg").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditData").button("option", "disabled", true);
            $("#btndialogCancelEdit").button("option", "disabled", false);
            $("#dialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('SID', dataRow.SID);

            $("#txtAnomalousTime").val(dataRow.anomalousTime);
            $("#txtAnomalousTime").removeAttr('disabled');;
            $("#txtLitNo").val(dataRow.LitNo);
            $("#txtLitNo").removeAttr('disabled');;
            $("#txtModel").val(dataRow.model);
            $("#txtModel").removeAttr('disabled');;
            $("#txtBitNo").val(dataRow.BitNo);
            $("#txtBitNo").removeAttr('disabled');;
            $("#txtBitNum").val(dataRow.BitNum);
            $("#txtBitNum").removeAttr('disabled');;
            $("#txtbadnessNum").val(dataRow.badnessNum);
            $("#txtbadnessNum").removeAttr('disabled');;
            $("#txtRejectRatio").val(dataRow.rejectRatio);
            $("#txtRejectRatio").removeAttr('disabled');;
            $("#ddlAbnormal").val(Abnormal);
            $("#ddlAbnormal").removeAttr('disabled');;
            $("#txtVenderCode").val(dataRow.VenderCode);
            $("#txtVenderCode").removeAttr('disabled');;
            $("#txtbadnessNote").val(dataRow.badnessNote);
            $("#txtbadnessNote").removeAttr('disabled');;
            $("#txtbadnessPic").val(dataRow.badnessPic);
            $("#txtbadnessPic").removeAttr('disabled');;

            $("#lblDiaErrMsg").html("");

            break;
    }
}