$(function () {
    var dialogDesc = $("#dialogDataDesc");
    var gridDataListDesc = $("#gridDataListDesc");
    //Toolbar Buttons
    $("#btndialogEditDataDesc").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogCancelEditDesc").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#dialogDataDesc").dialog({
        autoOpen: false,
        height: 400,
        width: 500,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (dialogDesc.attr('Mode') == "v") {
                    $(this).dialog("close");
                }
                else {
                    var DoSuccessfully = false;
                    var RowIdDesc = gridDataListDesc.jqGrid('getGridParam', 'selrow');
                    $.ajax({
                        url: __WebAppPathPrefix + ((dialogDesc.attr('Mode') == "c") ? "/SQMBasic/CreateInspMap" : "/SQMBasic/EditInspMap"),
                        data: {
                            "SID": $("#dialogDataCode").attr('SID')
                            , "SSID": gridDataListDesc.jqGrid('getRowData', RowIdDesc).SSID
                            , "InspCode": escape($.trim($("#txtInspCode").val()))
                            , "InspDesc": escape($.trim($("#txtInspDesc").val()))
                            , "Standard": escape($.trim($("#txtStandard").val()))
                            , "CR": escape($.trim($("#txtCR").val()))
                            , "MA": escape($.trim($("#txtMA").val()))
                            , "MI": escape($.trim($("#txtMI").val()))
                            , "Other": escape($.trim($("#txtOther").val()))
                            , "InspNum": escape($.trim($("#txtInspNum").val()))
                            , "isOther": ($('#ckbCustom').prop('checked')) ? 1 : 0
                            , "Insptype": escape($("#dialogDataCode").attr('Insptype'))
                            , "UCL": escape($.trim($("#txtUCL").val()))
                            , "LCL": escape($.trim($("#txtLCL").val()))
                            , "AQL": escape($.trim($("#txtAQL").val()))
                            , "InspTool": escape($.trim($("#txtInspTool").val()))
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialogDesc.attr('Mode') == "c")
                                    alert("create successfully.");
                                else
                                    alert("edit successfully.");
                            }
                            else {
                                if ((dialogDesc.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblDiaErrMsgDesc").html(data);
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
                        $("#btnSearchDesc").click();
                        $("#btnSearchDescVar").click();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialogDesc.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialogDesc.attr('SID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });

    jQuery("#ckbCustom").change(function () {
        if ($('#ckbCustom').prop('checked')) {
            $("tr[id^='cus']").hide();
            $("#normal").show();
        } else {
            $("tr[id^='cus']").show();
            $("#normal").hide();
        }
    })

})

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByModeDesc(Mode) {
    var dialogDesc = $("#dialogDescCode");
    var gridDataListDesc = $("#gridDataListDesc");
    var RowIdDesc = gridDataListDesc.jqGrid('getGridParam', 'selrow');
    var dataRowDesc = gridDataListDesc.jqGrid('getRowData', RowIdDesc);
    switch (Mode) {
        case "c": //Create
            $("#dialogDescToolBarDesc").hide();

            dialogDesc.attr('ItemRowId', "");
            dialogDesc.attr('ItemRowId', "");
            dialogDesc.attr('SID', "");

            $("#ckbCustom").prop('checked', false);
            $("#ckbCustom").removeAttr('disabled');
            $("#ddlInsptype").val("Attributes");
            $("#tbcomm").show();

            $("#txtInspCode").val("");
            $("#txtInspCode").removeAttr('disabled');
            $("#txtInspDesc").val("");
            $("#txtInspDesc").removeAttr('disabled');
            $("#txtInspNum").val("");
            $("#txtInspNum").removeAttr('disabled');
            $("#txtStandard").val("");
            $("#txtStandard").removeAttr('disabled');

            $("#txtCR").val("");
            $("#txtCR").removeAttr('disabled');
            $("#txtMA").val("");
            $("#txtMA").removeAttr('disabled');
            $("#txtMI").val("");
            $("#txtMI").removeAttr('disabled');

            $("#txtOther").val("");
            $("#txtOther").removeAttr('disabled');


            if ($('#ckbCustom').prop('checked')) {
                $("tr[id^='cus']").hide();
                $("#normal").show();
            } else {
                $("tr[id^='cus']").show();
                $("#normal").hide();
            }

            $("#lblDiaErrMsgDesc").html("");

            break;
        case "v": //View
            $("#btndialogEditDataDesc").button("option", "disabled", false);
            $("#btndialogCancelEditDesc").button("option", "disabled", true);
            $("#dialogDescToolBarDesc").show();

            dialogDesc.attr('ItemRowId', RowIdDesc);
            dialogDesc.attr('SID', dataRowDesc.SID);

            $("#ckbCustom").prop('checked', dataRowDesc.isOther == "true" ? true : false);
            $("#ckbCustom").attr("disabled", "disabled");
            $("#tbcomm").hide();
            if (dataRowDesc.Insptype == 'Attributes') {
                $("#tbAttr").show();
                $("#tbVar").hide();
            } else {
                $("#tbAttr").hide();
                $("#tbVar").show();
            }


            $("#txtInspCode").val(dataRowDesc.Name);
            $("#txtInspCode").attr("disabled", "disabled");
            $("#txtInspDesc").val(dataRowDesc.InspDesc);
            $("#txtInspDesc").attr("disabled", "disabled");
            $("#txtInspNum").val(dataRowDesc.InspNum);
            $("#txtInspNum").attr("disabled", "disabled");
            $("#txtStandard").val(dataRowDesc.Standard);
            $("#txtStandard").attr("disabled", "disabled");

            $("#txtCR").val(dataRowDesc.CR);
            $("#txtCR").attr("disabled", "disabled");
            $("#txtMA").val(dataRowDesc.MA);
            $("#txtMA").attr("disabled", "disabled");
            $("#txtMI").val(dataRowDesc.MI);
            $("#txtMI").attr("disabled", "disabled");

            $("#txtOther").val(dataRowDesc.Other);
            $("#txtOther").attr("disabled", "disabled");


            if ($('#ckbCustom').prop('checked')) {
                $("tr[id^='cus']").hide();
                $("#normal").show();
            } else {
                $("tr[id^='cus']").show();
                $("#normal").hide();
            }

            $("#lblDiaErrMsgDesc").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditDataDesc").button("option", "disabled", true);
            $("#btndialogCancelEditDesc").button("option", "disabled", false);
            $("#dialogDescToolBarDesc").show();

            dialogDesc.attr('ItemRowId', RowIdDesc);
            dialogDesc.attr('SID', dataRowDesc.SID);

            $("#ckbCustom").prop('checked', dataRowDesc.isOther == "true" ? true : false);
            $("#ckbCustom").removeAttr('disabled');
            if (dataRowDesc.Insptype == 'Attributes') {
                $("#tbAttr").show();
                $("#tbVar").hide();
            } else {
                $("#tbAttr").hide();
                $("#tbVar").show();
            }

            $("#txtInspCode").val(dataRowDesc.Name);
            $("#txtInspCode").removeAttr('disabled');
            $("#txtInspDesc").val(dataRowDesc.InspDesc);
            $("#txtInspDesc").removeAttr('disabled');
            $("#txtInspNum").val(dataRowDesc.InspNum);
            $("#txtInspNum").removeAttr('disabled');
            $("#txtStandard").val(dataRowDesc.Standard);
            $("#txtStandard").removeAttr('disabled');

            $("#txtCR").val(dataRowDesc.CR);
            $("#txtCR").removeAttr('disabled');
            $("#txtMA").val(dataRowDesc.MA);
            $("#txtMA").removeAttr('disabled');
            $("#txtMI").val(dataRowDesc.MI);
            $("#txtMI").removeAttr('disabled');

            $("#txtOther").val(dataRowDesc.Other);
            $("#txtOther").removeAttr('disabled');


            if ($('#ckbCustom').prop('checked')) {
                $("tr[id^='cus']").hide();
                $("#normal").show();
            } else {
                $("tr[id^='cus']").show();
                $("#normal").hide();
            }

            $("#lblDiaErrMsgDesc").html("");

            break;
    }
}