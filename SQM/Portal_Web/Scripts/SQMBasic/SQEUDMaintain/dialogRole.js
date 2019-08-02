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
        height: 600,
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
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMBasic/CreateUD" : "/SQMBasic/EditUD"),
                        data: {
                            "SID": dialog.attr("SID"),
                            "UD": escape($.trim($("#txtUD").val())),
                            "Plan": escape($.trim($("#txtPlan").val())),
                            "UDType": escape($.trim($("#ddlUDType").val())),
                            "ReMark": escape($.trim($("#txtReMark").val())),
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialog.attr('Mode') == "c")
                                    alert("UD create successfully.");
                                else
                                    alert("UD edit successfully.");
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
            Cancel: function () {
                $(this).dialog("close");
                $("#dialogData").attr("DesignChangeFGUID", "");
                $("#dialogData").attr("ProposedChangeFGUID", "");
            }
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
            dialog.attr('SID', "");

            $("#txtUD").val("");
            $("#txtUD").removeAttr('disabled');
            $("#txtPlan").val("");
            $("#txtPlan").removeAttr('disabled');
            $("#ddlUDType  option:first").prop("selected", 'selected').change();
            $("#ddlUDType").removeAttr('disabled');
            $("#txtReMark").val("");
            $("#txtReMark").removeAttr('disabled');

            $("#lblDiaErrMsg").html("");

            $("#fileUploadProposedChange").show();
            break;
        case "v": //View
            $("#btndialogEditData").button("option", "disabled", false);
            $("#btndialogCancelEdit").button("option", "disabled", 'Yes');
            $("#dialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('SID', dataRow.SID);

            $("#txtUD").val(dataRow.UD);
            $("#txtUD").attr("disabled", "disabled");
            $("#txtPlan").val(dataRow.Plan);
            $("#txtPlan").attr("disabled", "disabled");
            $("#ddlUDType").val(dataRow.UDType);
            $("#ddlUDType").attr("disabled", "disabled");
            $("#txtReMark").val(dataRow.ReMark);
            $("#txtReMark").attr("disabled", "disabled");

            $("#lblDiaErrMsg").html("");

            $("#fileUploadProposedChange").hide();
            $("#fileUploadDesignChange").hide();
            break;
        default: //Edit("e")
            $("#btndialogEditData").button("option", "disabled", 'Yes');
            $("#btndialogCancelEdit").button("option", "disabled", false);
            $("#dialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('SID', dataRow.SID);

            $("#txtUD").val(dataRow.UD);
            $("#txtUD").removeAttr('disabled');
            $("#txtPlan").val(dataRow.Plan);
            $("#txtPlan").removeAttr('disabled');
            $("#ddlUDType").val(dataRow.UDType);
            $("#ddlUDType").removeAttr('disabled');
            $("#txtReMark").val(dataRow.ReMark);
            $("#txtReMark").removeAttr('disabled');

            $("#lblDiaErrMsg").html("");

            $("#fileUploadProposedChange").show();
            break;
    }
}