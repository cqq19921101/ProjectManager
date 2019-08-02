$(function () {
    var dialogInsp = $("#dialogDataInsp");
    var gridDataListInsp = $("#gridDataListInsp");
    //Toolbar Buttons
    $("#btndialogEditDataInsp").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogCancelEditInsp").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#dialogDataInsp").dialog({
        autoOpen: false,
        height: 400,
        width: 500,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (dialogInsp.attr('Mode') == "v") {
                    $(this).dialog("close");
                }
                else {
                    var DoSuccessfully = false;
                    var RowIdInsp = gridDataListInsp.jqGrid('getGridParam', 'selrow');
                    $.ajax({
                        url: __WebAppPathPrefix + ((dialogInsp.attr('Mode') == "c") ? "/SQMReliability/CreateQualityInsp" : "/SQMReliability/EditQualityInsp"),
                        data: {
                            "ReportSID": $("#gridDataListInsp").attr("ReportSID")
                            ,"Insptype": $("#gridDataListInsp").attr("Insptype")
                            , "InspCodeID": $('#ddlInspCode').val()
                            , "InspDescID": $('#ddlInspDesc').val()
                            ,"CR": escape($.trim($("#txtCR").val()))
                             , "MA": escape($.trim($("#txtMA").val()))
                             , "MI": escape($.trim($("#txtMI").val()))
                            , "UCL": escape($.trim($("#txtUCL").val()))
                             , "LCL": escape($.trim($("#txtLCL").val()))
                             , "InspTool": escape($.trim($("#txtInspTool").val()))
                              , "AQL": escape($.trim($("#txtAQL").val()))
                             , "InspNum": escape($.trim($("#txtInspNum").val()))
                            , "InspResult": escape($.trim($("#txtInspResult").val()))
                            , "Judge": escape($.trim($("#txtJudge").val()))
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialogInsp.attr('Mode') == "c")
                                    alert("create successfully.");
                                else
                                    alert("edit successfully.");
                            }
                            else {
                                if ((dialogInsp.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblDiaErrMsgInsp").html(data);
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
                        $("#btnSearchInsp").click();
                        $("#btnSearchInspVar").click();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialogInsp.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialogInsp.attr('SID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });
    //InspCode ddl Change
    $('#ddlInspCode').change(function () {
        $.ajax({
            url: __WebAppPathPrefix + '/SQMReliability/GetInspDescList',
            data: { MainID: $('#ddlInspCode').val() },
            type: "post",
            dataType: 'json',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                if (data.length > 0) {
                    $("#txtInspResult  option:first").prop("selected", 'selected');
                    //if (data[0].Insptype == 'Attributes') {
                        $("#trddlInspDesc").show();
                        var options = '';
                        for (var idx in data) {
                            options += '<option value=' + data[idx].SID + '> ' + data[idx].Name + '</option>';
                        }
                        $('#ddlInspDesc').html(options);
                    //} else {
                        
                    //    $('#ddlInspDesc').html('');
                    //}
                } else {
                    $("#trddlInspDesc").hide();
                    $('#ddlInspDesc').val('null');
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
        InspDescChange();
    });
    //InspDesc ddl Change -> hide/show
    $("#ddlInspDesc").change(function () {
        InspDescChange();
    });
    //InspCode ddl
   

    //InspDesc ddl
    InspDescddl();
 
})

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByModeInsp(Mode) {
    var dialogInsp = $("#dialogInspCode");
    var gridDataListInsp = $("#gridDataListInsp");
    var RowIdInsp = gridDataListInsp.jqGrid('getGridParam', 'selrow');
    var dataRowInsp = gridDataListInsp.jqGrid('getRowData', RowIdInsp);
    $("#tbVar").hide();
    switch (Mode) {
        case "c": //Create
            $("#dialogInspToolBarInsp").hide();

            dialogInsp.attr('ItemRowId', "");
            dialogInsp.attr('ItemRowId', "");
            dialogInsp.attr('SID', "");

            $("#ddlInspCode option:first").prop("selected", 'selected').change();;
            $("#ddlInspCode").removeAttr('disabled');
            $("#ddlInspDesc option:first").prop("selected", 'selected').change();
            $("#ddlInspDesc").removeAttr('disabled');

            $("#txtInspNum").val($("#gridDataList").attr("AQL"));
            $("#txtInspNum").removeAttr('disabled');
            $("#txtStandard").val("");
            $("#txtStandard").attr("disabled", "disabled");

            $("#txtUCL").val("");
            $("#txtUCL").removeAttr('disabled');
            $("#txtLCL").val("");
            $("#txtLCL").removeAttr('disabled');
            $("#txtInspTool").val("");
            $("#txtInspTool").removeAttr('disabled');
            $("#txtAQL").val("");
            $("#txtAQL").removeAttr('disabled');

            $("#txtInspResult option:first").prop("selected", 'selected').change();
            $("#txtInspResult").removeAttr('disabled');
            $("#txtJudge option:first").prop("selected", 'selected').change();
            $("#txtJudge").removeAttr('disabled');

            $("#txtCR").val("");
            $("#txtCR").removeAttr('disabled');
            $("#txtMA").val("");
            $("#txtMA").removeAttr('disabled');
            $("#txtMI").val("");
            $("#txtMI").removeAttr('disabled');

            $("#txtOther").val("");
            $("#txtOther").attr("disabled", "disabled");


            if ($('#ckbCustom').prop('checked')) {
                $("tr[id^='cus']").hide();
                $("#normal").show();
            } else {
                $("tr[id^='cus']").show();
                $("#normal").hide();
            }

            $("#lblDiaErrMsgInsp").html("");

            break;
        case "v": //View
            $("#btndialogEditDataInsp").button("option", "disabled", false);
            $("#btndialogCancelEditInsp").button("option", "disabled", true);
            $("#dialogInspToolBarInsp").show();

            dialogInsp.attr('ItemRowId', RowIdInsp);
            dialogInsp.attr('SID', dataRowInsp.SID);


            $("#ddlInspCode").val(dataRowInsp.CodeSID).change();
            $("#ddlInspCode").attr("disabled", "disabled");
            $("#ddlInspDesc").val(dataRowInsp.DescSID).change();
            $("#ddlInspDesc").attr("disabled", "disabled");

            $("#txtInspNum").val(dataRowInsp.InspNum);
            $("#txtInspNum").attr("disabled", "disabled");
            $("#txtStandard").val(dataRowInsp.Standard);
            $("#txtStandard").attr("disabled", "disabled");

            $("#txtInspResult").val(dataRowInsp.InspResult);
            $("#txtInspResult").attr("disabled", "disabled");
            $("#txtJudge").val(dataRowInsp.Judge);
            $("#txtJudge").attr("disabled", "disabled");


            $("#txtCR").val(dataRowInsp.CR);
            $("#txtCR").attr("disabled", "disabled");
            $("#txtMA").val(dataRowInsp.MA);
            $("#txtMA").attr("disabled", "disabled");
            $("#txtMI").val(dataRowInsp.MI);
            $("#txtMI").attr("disabled", "disabled");

            $("#txtOther").val(dataRowInsp.Other);
            $("#txtOther").attr("disabled", "disabled");

            $("#tbAttr").show();
            if (dataRowInsp.isOther=="true") {
                $("tr[id^='cus']").hide();
                $("#normal").show();
            } else {
                $("tr[id^='cus']").show();
                $("#normal").hide();
            }

            $("#lblDiaErrMsgInsp").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditDataInsp").button("option", "disabled", true);
            $("#btndialogCancelEditInsp").button("option", "disabled", false);
            $("#dialogInspToolBarInsp").show();

            dialogInsp.attr('ItemRowId', RowIdInsp);
            dialogInsp.attr('SID', dataRowInsp.SID);

            $("#ddlInspCode").val(dataRowInsp.CodeSID).change();;
            $("#ddlInspCode").attr("disabled", "disabled");
            $("#ddlInspDesc").val(dataRowInsp.DescSID).change();
            $("#ddlInspDesc").attr("disabled", "disabled");

            $("#txtInspNum").val(dataRowInsp.InspNum);
            $("#txtInspNum").removeAttr('disabled');
            $("#txtStandard").val(dataRowInsp.Standard);
            $("#txtStandard").attr("disabled", "disabled");

            $("#txtInspResult").val(dataRowInsp.InspResult);
            $("#txtInspResult").removeAttr('disabled');
            $("#txtJudge").val(dataRowInsp.Judge);
            $("#txtJudge").removeAttr('disabled');


            $("#txtCR").val(dataRowInsp.CR);
            $("#txtCR").removeAttr('disabled');
            $("#txtMA").val(dataRowInsp.MA);
            $("#txtMA").removeAttr('disabled');
            $("#txtMI").val(dataRowInsp.MI);
            $("#txtMI").removeAttr('disabled');

            $("#txtOther").val(dataRowInsp.Other);
            $("#txtOther").attr("disabled", "disabled");

            $("#tbAttr").show();
            if (dataRowInsp.isOther == "true") {
                $("tr[id^='cus']").hide();
                $("#normal").show();
            } else {
                $("tr[id^='cus']").show();
                $("#normal").hide();
            }

            $("#lblDiaErrMsgInsp").html("");

            break;
    }
}
function DialogSetUIByModeInspVar(Mode) {
    var dialogInspVar = $("#dialogDataInspVar");
    var gridDataListInspVar = $("#gridDataListInspVar");
    var RowIdInspVar = gridDataListInspVar.jqGrid('getGridParam', 'selrow');
    var dataRowInspVar = gridDataListInspVar.jqGrid('getRowData', RowIdInspVar);
    switch (Mode) {
        case "v": //View
            $("#btndialogEditDataInspVar").button("option", "disabled", false);
            $("#btndialogCancelEditInspVar").button("option", "disabled", true);
            $("#dialogInspToolBarInspVar").show();

            dialogInspVar.attr('ItemRowId', RowIdInspVar);
            dialogInspVar.attr('ReportSID', dataRowInspVar.ReportSID);
            dialogInspVar.attr('Insptype', dataRowInspVar.Insptype);
            dialogInspVar.attr('InspCodeID', dataRowInspVar.InspCodeID);
            dialogInspVar.attr('InspDescID', dataRowInspVar.InspDescID);


            $("#vtxtUCL").val(dataRowInspVar.UCL);
            $("#vtxtUCL").attr("disabled", "disabled");
            $("#vtxtLCL").val(dataRowInspVar.LCL);
            $("#vtxtLCL").attr("disabled", "disabled");
            $("#vtxtInspTool").val(dataRowInspVar.InspTool);
            $("#vtxtInspTool").attr("disabled", "disabled");
            $("#vtxtAQL").val(dataRowInspVar.AQL);
            $("#vtxtAQL").attr("disabled", "disabled");
            $("#vtxtInspNum").val(dataRowInspVar.InspNum);
            $("#vtxtInspNum").attr("disabled", "disabled");

            $("#vtxtJudge").val(dataRowInspVar.Judge);
            $("#vtxtJudge").attr("disabled", "disabled");

            $("#lblDiaErrMsgInspVar").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditDataInspVar").button("option", "disabled", true);
            $("#btndialogCancelEditInspVar").button("option", "disabled", false);
            $("#dialogInspToolBarInspVar").show();

            dialogInspVar.attr('ItemRowId', RowIdInspVar);
            dialogInspVar.attr('ReportSID', dataRowInspVar.ReportSID);
            dialogInspVar.attr('Insptype', dataRowInspVar.Insptype);
            dialogInspVar.attr('InspCodeID', dataRowInspVar.InspCodeID);
            dialogInspVar.attr('InspDescID', dataRowInspVar.InspDescID);


            $("#vtxtUCL").val(dataRowInspVar.UCL);
            $("#vtxtUCL").removeAttr('disabled');
            $("#vtxtLCL").val(dataRowInspVar.LCL);
            $("#vtxtLCL").removeAttr('disabled');
            $("#vtxtInspTool").val(dataRowInspVar.InspTool);
            $("#vtxtInspTool").removeAttr('disabled');
            $("#vtxtAQL").val(dataRowInspVar.AQL);
            $("#vtxtAQL").removeAttr('disabled');
            $("#vtxtInspNum").val(dataRowInspVar.InspNum);
            $("#vtxtInspNum").removeAttr('disabled');

            $("#vtxtJudge").val(dataRowInspVar.Judge);
            $("#vtxtJudge").removeAttr('disabled');


            $("#lblDiaErrMsgInspVar").html("");

            break;
    }
}

function InspDescChange() {
    $.ajax({
        url: __WebAppPathPrefix + '/SQMReliability/GetInspIfisOther',
        data: {
            SID: $('#ddlInspCode').val()
            , SSID: $('#ddlInspDesc').val()
        },
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            if (data.length > 0) {
                $("#gridDataListInsp").attr("Insptype", data[0].Insptype)
                $("#tbCommSub").show();
                if (data[0].Insptype == 'Attributes') {
                    $("#tbAttr").show();
                    $("#tbAttrSub").show();
                    $("#tbResult").show();
                    $("#tbVar").hide();
                    if (data[0].isOther) {
                        $("tr[id^='cus']").hide();
                        $("#normal").show();
                        $("#txtOther").val(data[0].Other);
                    } else {
                        $("tr[id^='cus']").show();
                        $("#normal").hide();
                        $("#txtCR").val(data[0].CR);
                        $("#txtMA").val(data[0].MA);
                        $("#txtMI").val(data[0].MI);
                    }
                   
                    $("#txtStandard").val(data[0].Standard);
                }else if (data[0].Insptype == 'Variables') {
                    $("#tbAttr").hide();
                    $("#tbAttrSub").hide();
                    $("#tbResult").hide();
                    $("#tbVar").show();
                    $("#txtUCL").val(data[0].UCL);
                    $("#txtLCL").val(data[0].LCL);
                    $("#txtInspTool").val(data[0].InspTool);
                    $("#txtAQL").val(data[0].AQL);
                   
                } else {
                    $("#tbAttr").hide();
                    $("#tbAttrSub").hide();
                    $("#tbResult").hide();
                    $("#tbVar").hide();
                }
            } else {
                $("#tbCommSub").hide();
                $("input[id^='txt']").val("");
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

function InspDescddl() {
    $.ajax({
        url: __WebAppPathPrefix + '/SQMReliability/GetInspDescList',
        data: { MainID: ($('#ddlInspCode').val() == "") ? $("#ddlInspCode option:first").prop("selected", 'selected') : $('#ddlInspCode').val() },
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].SID + '> ' + data[idx].Name + '</option>';
            }
            $('#ddlInspDesc').html(options);
            InspDescChange();
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}