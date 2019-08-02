//initial dialog
$(function () {
    var dialog = $("#dialogSQMProcessData");
    
    //Toolbar Buttons
    $("#btndialogSQMProcessEditData").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogSQMProcessCancelEdit").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#dialogSQMProcessData").dialog({
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
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMProcess/CreateMember" : "/SQMProcess/EditMember"),
                        data: {
                            "BasicInfoGUID": $("#gridDataListBasicInfoType").jqGrid('getRowData', $("#gridDataListBasicInfoType").jqGrid('getGridParam', 'selrow')).BasicInfoGUID,
                            "ProcessType": escape($.trim($("#ddlSQMProcessCName option:selected").val())),
                            "CName": escape($.trim($("#ddlSQMProcessCName option:selected").text())),
                            "CNameInput": escape($.trim($("#txtSQMProcessCNameInput").val())),
                            "ProcessName": escape($.trim($("#txtProcessName").val())),
                            "OwnOrOut": $('input[name=DiaMemberType]:checked').val() == "own" ? "0" : "1",
                            "ExternalSupplierName": escape($.trim($("#txtExternalSupplierName").val())),
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
                            }
                            else {
                                if ((dialog.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblDiaErrMsgOfprocess").html(data);
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
                        $("#btnSQMProcessSearch").click();
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
function DialogSetUIByModeSQMProcess(Mode) {
    var dialog = $("#dialogSQMProcessData");
    var gridSQMProcessDataList = $("#gridSQMProcessDataList");
    switch (Mode) {
        case "c": //Create
            $("#btndialogSQMProcessEditData").hide();
            $("#btndialogSQMProcessCancelEdit").hide();
            $("#dialogSQMProcessDataToolBar").hide();

            dialog.attr('ItemRowId', "");

            $("#ddlSQMProcessCName").removeAttr('disabled');
            $("#txtProcessName").removeAttr('disabled');
            //$("#txtOwnOrOut").removeAttr('disabled');
            $("#txtExternalSupplierName").removeAttr('disabled');

            $("#radOwnProcess").attr('checked', true);
            $("#radOwnProcess").removeAttr('disabled');
            $("#radOutProcess").removeAttr('disabled');

            $("#txtProcessName").val("");
            //$("#txtOwnOrOut").val("");
            $("#txtExternalSupplierName").val("");

            $("#lblDiaErrMsgOfprocess").html("");

            break;
        case "v": //View
            $("#btndialogSQMProcessEditData").show();
            $("#btndialogSQMProcessCancelEdit").show();
            $("#btndialogSQMProcessEditData").button("option", "disabled", false);
            $("#btndialogSQMProcessCancelEdit").button("option", "disabled", true);
            $("#dialogSQMProcessDataToolBar").show();

            var RowId = gridSQMProcessDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridSQMProcessDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);

            $("#ddlSQMProcessCName").val(dataRow.ProcessType);
            $("#txtSQMProcessCNameInput").val(dataRow.CName);
            $("#txtProcessName").val(dataRow.ProcessName);
            //$("#txtOwnOrOut").val(dataRow.OwnOrOut);
            $("#txtExternalSupplierName").val(dataRow.ExternalSupplierName);

            $("#radOwnProcess").attr("disabled", "disabled");
            $("#radOutProcess").attr("disabled", "disabled");
            var CheckedRadioButton;
            if (dataRow.OwnOrOut == "0") { CheckedRadioButton = $("#radOwnProcess"); }
            else { CheckedRadioButton = $("#radOutProcess"); }
            CheckedRadioButton.attr('checked', true);

            $("#ddlSQMProcessCName").attr("disabled", "disabled");
            $("#txtSQMProcessCNameInput").hide();
            $("#txtProcessName").attr("disabled", "disabled");
            //$("#txtOwnOrOut").attr("disabled", "disabled");
            $("#txtExternalSupplierName").attr("disabled", "disabled");

            $("#lblDiaErrMsgOfprocess").html("");

            break;
        default: //Edit("e")
            $("#btndialogSQMProcessEditData").show();
            $("#btndialogSQMProcessCancelEdit").show();
            $("#btndialogSQMProcessEditData").button("option", "disabled", true);
            $("#btndialogSQMProcessCancelEdit").button("option", "disabled", false);
            $("#dialogSQMProcessDataToolBar").show();

            var RowId = gridSQMProcessDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridSQMProcessDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);

            $("#ddlSQMProcessCName").val(dataRow.ProcessType);
            $("#txtSQMProcessCNameInput").val(dataRow.CName);
            $("#txtProcessName").val(dataRow.ProcessName);
            //$("#txtOwnOrOut").val(dataRow.OwnOrOut);
            $("#txtExternalSupplierName").val(dataRow.ExternalSupplierName);

            $("#radOwnProcess").removeAttr('disabled');
            $("#radOutProcess").removeAttr('disabled');

            var CheckedRadioButton;
            if (dataRow.OwnOrOut == "0") { CheckedRadioButton = $("#radOwnProcess"); }
            else { CheckedRadioButton = $("#radOutProcess"); }
            CheckedRadioButton.attr('checked', true);

            $("#txtProcessName").removeAttr('disabled');
            //$("#txtOwnOrOut").removeAttr('disabled');
            $("#txtExternalSupplierName").removeAttr('disabled');

            $("#lblDiaErrMsgOfprocess").html("");

            break;
    }
}

$(function () {
    $('#ddlSQMProcessCName').click(function () {
        var txtProcess = $("#ddlSQMProcessCName option:selected").text();
        if(txtProcess == "Other")
        {
            $("#txtSQMProcessCNameInput").show();
            $("#txtSQMProcessCNameInput").val("");
        }
        else
        {
            $("#txtSQMProcessCNameInput").hide();
            $("#txtSQMProcessCNameInput").val(txtProcess);
        }
    });
});