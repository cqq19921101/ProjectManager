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
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMPRO/CreateMember" : "/SQMPRO/EditMember"),
                        data: {
                            "HRType": escape($.trim($("#ddlCName option:selected").val())),
                            "CName": escape($.trim($("#ddlCName option:selected").text())),
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
                                    $("#lblDiaErrMsg").html(data);
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

function LoadTotal() {
    $.ajax({
        url: __WebAppPathPrefix + '/SQMPRO/GetTotalData',
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
function DialogSetUIByMode(Mode) {
    var dialog = $("#dialogData");
    var gridDataList = $("#gridDataList");
    switch (Mode) {
        case "c": //Create
            $("#dialogDataToolBar").hide();

            dialog.attr('ItemRowId', "");

            $("#ddlCName").removeAttr('disabled');
            $("#txtEmployeeQty").removeAttr('disabled');
            $("#txtEmployeePlanned").removeAttr('disabled');
            $("#txtAverageJobSeniority").removeAttr('disabled');

            $("#txtEmployeeQty").val("");
            $("#txtEmployeePlanned").val("");
            $("#txtAverageJobSeniority").val("");

            $("#lblDiaErrMsg").html("");

            break;
        case "v": //View
            $("#btndialogEditData").button("option", "disabled", false);
            $("#btndialogCancelEdit").button("option", "disabled", true);
            $("#dialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);

            $("#ddlCName").val(dataRow.HRType);
            //$("#ddlCName").text(dataRow.CName);
            $("#txtEmployeeQty").val(dataRow.EmployeeQty);
            $("#txtEmployeePlanned").val(dataRow.EmployeePlanned);
            $("#txtAverageJobSeniority").val(dataRow.AverageJobSeniority);

            $("#ddlCName").attr("disabled", "disabled");
            $("#txtEmployeeQty").attr("disabled", "disabled");
            $("#txtEmployeePlanned").attr("disabled", "disabled");
            $("#txtAverageJobSeniority").attr("disabled", "disabled");

            $("#lblDiaErrMsg").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditData").button("option", "disabled", true);
            $("#btndialogCancelEdit").button("option", "disabled", false);
            $("#dialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);

            $("#ddlCName").val(dataRow.HRType);
            //$("#ddlCName").text(dataRow.CName);
            $("#txtEmployeeQty").val(dataRow.EmployeeQty);
            $("#txtEmployeePlanned").val(dataRow.EmployeePlanned);
            $("#txtAverageJobSeniority").val(dataRow.AverageJobSeniority);

            $("#txtEmployeeQty").removeAttr('disabled');
            $("#txtEmployeePlanned").removeAttr('disabled');
            $("#txtAverageJobSeniority").removeAttr('disabled');

            $("#lblDiaErrMsg").html("");

            break;
    }
}