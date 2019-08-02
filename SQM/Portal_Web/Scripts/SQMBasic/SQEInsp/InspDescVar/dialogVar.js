$(function () {
    var dialogDescVar = $("#dialogDataDescVar");
    var gridDataListDescVar = $("#gridDataListDescVariables");
    //Toolbar Buttons
    $("#btndialogEditDataDescVar").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogCancelEditDescVar").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#dialogDataDescVar").dialog({
        autoOpen: false,
        height: 400,
        width: 500,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (dialogDescVar.attr('Mode') == "v") {
                    $(this).dialog("close");
                }
                else {
                    var DoSuccessfully = false;
                    var RowIdDescVar = gridDataListDescVar.jqGrid('getGridParam', 'selrow');
                    $.ajax({
                        url: __WebAppPathPrefix + ((dialogDescVar.attr('Mode') == "c") ? "/SQMBasic/CreateInspMap" : "/SQMBasic/EditInspMap"),
                        data: {
                            "SID": $("#dialogDataCode").attr('SID')
                            , "SSID": gridDataListDescVar.jqGrid('getRowData', RowIdDescVar).SSID
                            //, "InspCode": escape($.trim($("#vtxtInspCode").val()))
                            , "InspDesc": escape($.trim($("#vtxtInspDesc").val()))
                            , "InspNum": escape($.trim($("#vtxtInspNum").val()))
                            , "Insptype":escape($("#dialogDataCode").attr('Insptype'))
                            , "UCL": escape($.trim($("#vtxtUCL").val()))
                            , "LCL": escape($.trim($("#vtxtLCL").val()))
                            , "AQL": escape($.trim($("#vtxtAQL").val()))
                            , "InspTool": escape($.trim($("#vtxtInspTool").val()))
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialogDescVar.attr('Mode') == "c")
                                    alert("create successfully.");
                                else
                                    alert("edit successfully.");
                            }
                            else {
                                if ((dialogDescVar.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblDiaErrMsgDescVar").html(data);
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
                        $("#btnSearchDescVar").click();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialogDescVar.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialogDescVar.attr('SID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });
    Changedddl();
    jQuery("#ddlInsptype").change(function () {
        Changedddl();
    })

})

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByModeDescVar(Mode) {
    var dialogDescVar = $("#dialogDescCode");
    var gridDataListDescVar = $("#gridDataListDescVariables");
    var RowIdDescVar = gridDataListDescVar.jqGrid('getGridParam', 'selrow');
    var dataRowDescVar = gridDataListDescVar.jqGrid('getRowData', RowIdDescVar);
    switch (Mode) {
        case "c": //Create
            $("#dialogDescToolBarDescVar").hide();

            dialogDescVar.attr('ItemRowId', "");
            dialogDescVar.attr('ItemRowId', "");
            dialogDescVar.attr('SID', "");

            $("#vtxtInspDesc").val("");
            $("#vtxtInspDesc").removeAttr('disabled');
            $("#vtxtUCL").val("");
            $("#vtxtUCL").removeAttr('disabled');
            $("#vtxtLCL").val("");
            $("#vtxtLCL").removeAttr('disabled');
            $("#vtxtInspTool").val("");
            $("#vtxtInspTool").removeAttr('disabled');
            $("#vtxtAQL").val("");
            $("#vtxtAQL").removeAttr('disabled');
            $("#vtxtInspNum").val("");
            $("#vtxtInspNum").removeAttr('disabled');


            $("#lblDiaErrMsgDescVar").html("");

            break;
        case "v": //View
            $("#btndialogEditDataDescVar").button("option", "disabled", false);
            $("#btndialogCancelEditDescVar").button("option", "disabled", true);
            $("#dialogDescToolBarDescVar").show();

            dialogDescVar.attr('ItemRowId', RowIdDescVar);
            dialogDescVar.attr('SID', dataRowDescVar.SID);

            $("#vtxtInspDesc").val(dataRowDescVar.InspDesc);
            $("#vtxtInspDesc").attr("disabled", "disabled");
            $("#vtxtUCL").val(dataRowDescVar.UCL);
            $("#vtxtUCL").attr("disabled", "disabled");
            $("#vtxtLCL").val(dataRowDescVar.LCL);
            $("#vtxtLCL").attr("disabled", "disabled");
            $("#vtxtInspTool").val(dataRowDescVar.InspTool);
            $("#vtxtInspTool").attr("disabled", "disabled");
            $("#vtxtAQL").val(dataRowDescVar.AQL);
            $("#vtxtAQL").attr("disabled", "disabled");
            $("#vtxtInspNum").val(dataRowDescVar.InspNum);
            $("#vtxtInspNum").attr("disabled", "disabled");

            $("#lblDiaErrMsgDescVar").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditDataDescVar").button("option", "disabled", true);
            $("#btndialogCancelEditDescVar").button("option", "disabled", false);
            $("#dialogDescToolBarDescVar").show();

            dialogDescVar.attr('ItemRowId', RowIdDescVar);
            dialogDescVar.attr('SID', dataRowDescVar.SID);


            $("#vtxtInspDesc").val(dataRowDescVar.InspDesc);
            $("#vtxtInspDesc").removeAttr('disabled');
            $("#vtxtUCL").val(dataRowDescVar.UCL);
            $("#vtxtUCL").removeAttr('disabled');
            $("#vtxtLCL").val(dataRowDescVar.LCL);
            $("#vtxtLCL").removeAttr('disabled');
            $("#vtxtInspTool").val(dataRowDescVar.InspTool);
            $("#vtxtInspTool").removeAttr('disabled');
            $("#vtxtAQL").val(dataRowDescVar.AQL);
            $("#vtxtAQL").removeAttr('disabled');
            $("#vtxtInspNum").val(dataRowDescVar.InspNum);
            $("#vtxtInspNum").removeAttr('disabled');

            $("#lblDiaErrMsgDescVar").html("");

            break;
    }
}
//init
function Changedddl() {
    if ($('#ddlInsptype').val() == 'Variables') {
        $("#tbVar").show();
        $("#tbAttr").hide();
    } else {
        $("#tbVar").hide();
        $("#tbAttr").show();
    }
}
