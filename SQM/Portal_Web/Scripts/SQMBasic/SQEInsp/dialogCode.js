$(function () {
    var dialogCode = $("#dialogDataCode");
    var gridDataListCode = $("#gridDataListCode");
    //Toolbar Buttons
    $("#btndialogEditDataCode").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogCancelEditCode").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#dialogDataCode").dialog({
        autoOpen: false,
        height: 400,
        width: 500,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (dialogCode.attr('Mode') == "v") {
                    $(this).dialog("close");
                }
                else {
                    var DoSuccessfully = false;
                    var RowIdCode = gridDataListCode.jqGrid('getGridParam', 'selrow');
                    $.ajax({
                        url: __WebAppPathPrefix + ((dialogCode.attr('Mode') == "c") ? "/SQMBasic/CreateInsp" : "/SQMBasic/EditInsp"),
                        data: {
                            "SID": gridDataListCode.jqGrid('getRowData', RowIdCode).SID
                            , "LitNo": escape($.trim($("#txtLitNo").val()))
                            , "Name": escape($.trim($("#txtInspCode").val()))
                            , "Insptype": escape($.trim($("#ddlInsptype").val()))

                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialogCode.attr('Mode') == "c")
                                    alert("create successfully.");
                                else
                                    alert("edit successfully.");
                            }
                            else {
                                if ((dialogCode.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblDiaErrMsgCode").html(data);
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
                        $("#btnSearchCode").click();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialogCode.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialogCode.attr('SID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });

})

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByModeCode(Mode) {
    var dialogCode = $("#dialogDataCode");
    var gridDataListCode = $("#gridDataListCode");
    var RowIdCode = gridDataListCode.jqGrid('getGridParam', 'selrow');
    var dataRowCode = gridDataListCode.jqGrid('getRowData', RowIdCode);
    switch (Mode) {
        case "c": //Create
            $("#dialogDataToolBarCode").hide();

            dialogCode.attr('ItemRowId', "");
            dialogCode.attr('ItemRowId', "");
            dialogCode.attr('SID', "");

            $("#ddlInsptype option:first").prop("selected", 'selected');
            $("#ddlInsptype").removeAttr('disabled');
            $("#txtInspCode").val("");
            $("#txtInspCode").removeAttr('disabled'); 
            $("#txtLitNo").val("");
            $("#txtLitNo").removeAttr('disabled');

            $("#lblDiaErrMsgCode").html("");

            break;
        case "v": //View
            $("#btndialogEditDataCode").button("option", "disabled", false);
            $("#btndialogCancelEditCode").button("option", "disabled", true);
            $("#dialogDataToolBarCode").show();

            dialogCode.attr('ItemRowId', RowIdCode);
            dialogCode.attr('SID', dataRowCode.SID);

            $("#ddlInsptype").val(dataRowCode.Insptype)
            $("#ddlInsptype").attr("disabled", "disabled");
            $("#txtInspCode").val(dataRowCode.Name);
            $("#txtInspCode").attr("disabled", "disabled");
            $("#txtLitNo").val(dataRowCode.LitNo);
            $("#txtLitNo").attr("disabled", "disabled");

            $("#lblDiaErrMsgCode").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditDataCode").button("option", "disabled", true);
            $("#btndialogCancelEditCode").button("option", "disabled", false);
            $("#dialogDataToolBarCode").show();

            dialogCode.attr('ItemRowId', RowIdCode);
            dialogCode.attr('SID', dataRowCode.SID);

            $("#ddlInsptype").val(dataRowCode.Insptype)
            $("#ddlInsptype").removeAttr('disabled');
            $("#txtInspCode").val(dataRowCode.Name);
            $("#txtInspCode").removeAttr('disabled');
            $("#txtLitNo").val(dataRowCode.LitNo);
            $("#txtLitNo").attr("disabled", "disabled");
            $("#lblDiaErrMsgCode").html("");

            break;
    }
}