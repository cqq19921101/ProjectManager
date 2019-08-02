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
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQECPK/CreateCPK" : "/SQECPK/EditCPK"),
                        data: {
                            "spc_id": $("#dialogData").attr('spc_id')
                             , "LitNo": escape($.trim($("#txtLitNo").val()))
                             , "spc_item": escape($.trim($("#txtDesignator").val()))
                            
                             , "lsl": escape($.trim($("#txtminNominal").val()))
                             , "usl": escape($.trim($("#txtmaxNominal").val()))
                             , "ucl": escape($.trim($("#txtUpperControlLimit").val()))
                             , "lcl": escape($.trim($("#txtLowerControlLimit").val()))
                             , "sample": escape($.trim($("#txtCTQNum").val()))
                             , "sl": escape($.trim($("#txtcenterline").val()))
                             , "cpk": escape($.trim($("#txtCPKManager").val()))
                             , "datum": escape($.trim($("#txtsection").val()))
                             , "check_6u": ($('#ckbSixpointrise').prop('checked')) ? "Y" :"N"
                             , "check_6d": ($('#ckbDropdown').prop('checked')) ? "Y": "N"
                             , "check_9m": ($('#ckbNineNearCenter').prop('checked')) ?"Y" : "N"
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialog.attr('Mode') == "c")
                                    alert("SubFunc create successfully.");
                                else
                                    alert("SubFunc edit successfully.");
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
                var r = ReleaseDataLock(dialog.attr('spc_id'));
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
            $("#dialogToolBar").hide();

            dialog.attr('ItemRowId', "");
            dialog.attr('spc_id', "");

            $("#txtDesignator").val("");
            $("#txtDesignator").removeAttr('disabled');
            $("#txtLitNo").val("");
            $("#txtLitNo").removeAttr('disabled');
        
            $("#txtminNominal").val("");
            $("#txtminNominal").removeAttr('disabled');
            $("#txtmaxNominal").val("");
            $("#txtmaxNominal").removeAttr('disabled');
            $("#txtTolerance").val("");
            $("#txtTolerance").removeAttr('disabled');
            $("#txtUpperControlLimit").val("");
            $("#txtUpperControlLimit").removeAttr('disabled');
            $("#txtLowerControlLimit").val("");
            $("#txtLowerControlLimit").removeAttr('disabled');
            $("#txtCTQNum").val("");
            $("#txtCTQNum").removeAttr('disabled');
            $("#txtcenterline").val("");
            $("#txtcenterline").removeAttr('disabled');
            $("#txtCPKManager").val("");
            $("#txtCPKManager").removeAttr('disabled');
            $("#txtsection").val("");
            $("#txtsection").removeAttr('disabled');
            $("#ckbSixpointrise").prop('checked', false);
            $("#ckbSixpointrise").removeAttr('disabled');
            $("#ckbDropdown").prop('checked', false);
            $("#ckbDropdown").removeAttr('disabled');
            $("#ckbNineNearCenter").prop('checked', false);
            $("#ckbNineNearCenter").removeAttr('disabled');

            $("#lblDiaErrMsg").html("");

            break;
        case "v": //View
            $("#btndialogEditData").button("option", "disabled", false);
            $("#btndialogCancelEdit").button("option", "disabled", true);
            $("#dialogToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRowCPKSub = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('spc_id', dataRowCPKSub.spc_id);

            $("#txtDesignator").val(dataRowCPKSub.spc_item);
            $("#txtDesignator").attr("disabled", "disabled");
            $("#txtLitNo").val(dataRowCPKSub.LitNo);
            $("#txtLitNo").attr("disabled", "disabled");
          
            $("#txtminNominal").val(dataRowCPKSub.sl);
            $("#txtminNominal").attr("disabled", "disabled");
            $("#txtmaxNominal").val(dataRowCPKSub.usl);
            $("#txtmaxNominal").attr("disabled", "disabled");
            $("#txtUpperControlLimit").val(dataRowCPKSub.ucl);
            $("#txtUpperControlLimit").attr("disabled", "disabled");
            $("#txtLowerControlLimit").val(dataRowCPKSub.lcl);
            $("#txtLowerControlLimit").attr("disabled", "disabled");
            $("#txtCTQNum").val(dataRowCPKSub.sample);
            $("#txtCTQNum").attr("disabled", "disabled");
            $("#txtcenterline").val(dataRowCPKSub.lsl);
            $("#txtcenterline").attr("disabled", "disabled");
            $("#txtCPKManager").val(dataRowCPKSub.cpk);
            $("#txtCPKManager").attr("disabled", "disabled");
            $("#txtsection").val(dataRowCPKSub.datum);
            $("#txtsection").attr("disabled", "disabled");
            if (dataRowCPKSub.check_6u == 'Y') {
                $("#ckbSixpointrise").attr('checked', 'checked');
                $("#ckbSixpointrise").attr("disabled", "disabled");
            } else {
                $("#ckbSixpointrise").attr("disabled", "disabled");
            }
            if (dataRowCPKSub.check_6d == 'Y') {
                $("#ckbDropdown").attr('checked', 'checked');
                $("#ckbDropdown").attr("disabled", "disabled");
            } else {
                $("#ckbDropdown").attr("disabled", "disabled");
            }
            if (dataRowCPKSub.check_9m == 'Y') {
                $("#ckbNineNearCenter").attr('checked', 'checked');
                $("#ckbNineNearCenter").attr("disabled", "disabled");
            } else {
                $("#ckbNineNearCenter").attr("disabled", "disabled");
            }

            $("#lblDiaErrMsg").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditData").button("option", "disabled", true);
            $("#btndialogCancelEdit").button("option", "disabled", false);
            $("#dialogToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRowCPKSub = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('spc_id', dataRowCPKSub.spc_id);

            $("#txtDesignator").val(dataRowCPKSub.spc_item);
            $("#txtDesignator").removeAttr('disabled');
            $("#txtLitNo").val(dataRowCPKSub.LitNo);
            $("#txtLitNo").removeAttr('disabled');
        
            $("#txtminNominal").val(dataRowCPKSub.sl);
            $("#txtminNominal").removeAttr('disabled');
            $("#txtmaxNominal").val(dataRowCPKSub.usl);
            $("#txtmaxNominal").removeAttr('disabled');
            $("#txtUpperControlLimit").val(dataRowCPKSub.ucl);
            $("#txtUpperControlLimit").removeAttr('disabled');
            $("#txtLowerControlLimit").val(dataRowCPKSub.lcl);
            $("#txtLowerControlLimit").removeAttr('disabled');
            $("#txtCTQNum").val(dataRowCPKSub.sample);
            $("#txtCTQNum").removeAttr('disabled');
            $("#txtcenterline").val(dataRowCPKSub.lsl);
            $("#txtcenterline").removeAttr('disabled');
            $("#txtCPKManager").val(dataRowCPKSub.cpk);
            $("#txtCPKManager").removeAttr('disabled');
            $("#txtsection").val(dataRowCPKSub.datum);
            $("#txtsection").removeAttr('disabled');
            if (dataRowCPKSub.check_6u == 'Y') {
                $("#ckbSixpointrise").attr('checked', 'checked');
                $("#ckbSixpointrise").removeAttr('disabled');
            } else {
                $("#ckbSixpointrise").removeAttr('disabled');
            }
            if (dataRowCPKSub.check_6d == 'Y') {
                $("#ckbDropdown").attr('checked', 'checked');
                $("#ckbDropdown").removeAttr('disabled');
            } else {
                $("#ckbDropdown").removeAttr('disabled');
            }
            if (dataRowCPKSub.NineNearCenter == 'Y') {
                $("#ckbNineNearCenter").attr('checked', 'checked');
                $("#ckbNineNearCenter").removeAttr('disabled');
            } else {
                $("#ckbNineNearCenter").removeAttr('disabled');
            }

            $("#lblDiaErrMsg").html("");

            break;
    }
}