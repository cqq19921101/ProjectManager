//initial dialog
function getHRdialog() {
    var dialog = $("#dialogSQMHRData");
    
    //Toolbar Buttons
    $("#btndialogSQMHREditData").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogSQMHRCancelEdit").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#dialogSQMHRData").dialog({
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
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMHR/CreateMember" : "/SQMHR/EditMember"),
                        data: {
                            "BasicInfoGUID": $("#gridDataListBasicInfoType").jqGrid('getRowData', $("#gridDataListBasicInfoType").jqGrid('getGridParam', 'selrow')).BasicInfoGUID,
                            "HRType": escape($.trim($("#ddlSQMHRCName option:selected").val())),
                            "CName": escape($.trim($("#ddlSQMHRCName option:selected").text())),
                            "EmployeeQty": escape($.trim($("#txtEmployeeQty").val())),
                            "EmployeePlanned": escape($.trim($("#txtEmployeePlanned").val())),
                            "AverageJobSeniority": escape($.trim($("#txtAverageJobSeniority").val())),
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialog.attr('Mode') == "c")
                                    alert("create successfully.");
                                else
                                    alert("edit successfully.");
                                LoadTotal();
                            }
                            else {
                                if ((dialog.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblDiaHRErrMsg").html(data);
                            }
                        },
                        error: function (xhr, textStatus, thrownError) {
                            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                        },
                        complete: function (jqXHR, textStatus) {
                        }
                    });
                    if (DoSuccessfully) {
                        $(this).dialog("close");
                        $("#btnSQMHRSearch").click();
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
}

function LoadTotal() {
    var gridSQMHRDataList = $("#gridSQMHRDataList");

    $.ajax({
        url: __WebAppPathPrefix + '/SQMHR/GetTotalData',
        data: { BasicInfoGUID: $("#gridDataListBasicInfoType").jqGrid('getRowData', $("#gridDataListBasicInfoType").jqGrid('getGridParam', 'selrow')).BasicInfoGUID },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            var TotalEmployeeQty;
            var TotalEmployeePlanned;
            for (var idx in data) {
                TotalEmployeeQty = data[idx].EmployeeQty;
                TotalEmployeePlanned = data[idx].EmployeePlanned;
            }
            $('#txtTotalEmployees').val(TotalEmployeeQty);
            $('#txtNumberPlanned').val(TotalEmployeePlanned);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByModeSQMHR(Mode) {
    var dialog = $("#dialogSQMHRData");
    var gridSQMHRDataList = $("#gridSQMHRDataList");
    switch (Mode) {
        case "c": //Create
            $("#btndialogSQMHREditData").hide();
            $("#btndialogSQMHRCancelEdit").hide();
            $("#dialogSQMHRDataToolBar").hide();

            dialog.attr('ItemRowId', "");
            $("#ddlSQMHRCName").removeAttr('disabled');
            $("#txtEmployeeQty").removeAttr('disabled');
            $("#txtEmployeePlanned").removeAttr('disabled');
            $("#txtAverageJobSeniority").removeAttr('disabled');

            $("#txtEmployeeQty").val("");
            $("#txtEmployeePlanned").val("");
            $("#txtAverageJobSeniority").val("");

            $("#lblDiaHRErrMsg").html("");

            break;
        case "v": //View
            $("#btndialogSQMHREditData").show();
            $("#btndialogSQMHRCancelEdit").show();
            $("#btndialogSQMHREditData").button("option", "disabled", false);
            $("#btndialogSQMHRCancelEdit").button("option", "disabled", true);
            $("#dialogSQMHRDataToolBar").show();

            var RowId = gridSQMHRDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridSQMHRDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);

            $("#ddlSQMHRCName").val(dataRow.HRType);
            //$("#ddlCName").text(dataRow.CName);
            $("#txtEmployeeQty").val(dataRow.EmployeeQty);
            $("#txtEmployeePlanned").val(dataRow.EmployeePlanned);
            $("#txtAverageJobSeniority").val(dataRow.AverageJobSeniority);

            $("#ddlSQMHRCName").attr("disabled", "disabled");
            $("#txtEmployeeQty").attr("disabled", "disabled");
            $("#txtEmployeePlanned").attr("disabled", "disabled");
            $("#txtAverageJobSeniority").attr("disabled", "disabled");

            $("#lblDiaHRErrMsg").html("");
            break;
        default: //Edit("e")
            $("#btndialogSQMHREditData").show();
            $("#btndialogSQMHRCancelEdit").show();
            $("#btndialogSQMHREditData").button("option", "disabled", true);
            $("#btndialogSQMHRCancelEdit").button("option", "disabled", false);
            $("#dialogSQMHRDataToolBar").show();

            var RowId = gridSQMHRDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridSQMHRDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            $("#ddlSQMHRCName").val(dataRow.HRType);
            //$("#ddlCName").text(dataRow.CName);
            $("#txtEmployeeQty").val(dataRow.EmployeeQty);
            $("#txtEmployeePlanned").val(dataRow.EmployeePlanned);
            $("#txtAverageJobSeniority").val(dataRow.AverageJobSeniority);

            $("#txtEmployeeQty").removeAttr('disabled');
            $("#txtEmployeePlanned").removeAttr('disabled');
            $("#txtAverageJobSeniority").removeAttr('disabled');

            $("#lblDiaHRErrMsg").html("");

            break;
    }
}