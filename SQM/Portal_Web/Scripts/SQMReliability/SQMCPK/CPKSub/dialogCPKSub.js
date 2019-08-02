$(function () {
    var dialogCPKSub = $("#dialogDataCPKSub");
    var gridDataListCPKSub = $("#gridDataListCPKSub");

    //Toolbar Buttons
    $("#btndialogEditDataCPKSub").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogCancelEditCPKSub").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });


    $('#ddlDesignator').change(function () {
        var plantCode = $("#dialogDataCPK").attr('plantCode');
        var mainID = $('#ddlDesignator').val();
        $.ajax({
            url: __WebAppPathPrefix + '/SQMReliability/GetLitNoData',
            data: { "MainID": mainID, "plantCode": plantCode },
            type: "post",
            dataType: 'json',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                if (0 == data.length) {
                    
                } else {
                    //$("#txtUnit").val();
                    //$("#txtNominal").val();
                    $("#txtminNominal").val(data[0]["lsl"]);
                    $("#txtmaxNominal").val(data[0]["usl"]);
                    $("#txtUpperControlLimit").val(data[0]["ucl"]);
                    $("#txtLowerControlLimit").val(data[0]["lcl"]);
                    $("#txtCTQNum").val(data[0]["sample"]);
                    $("#txtcenterline").val(data[0]["sl"]);
                    $("#txtCPKManager").val(data[0]["cpk"]);
                    $("#txtsection").val(data[0]["datum"]);
                    "Y" == data[0]["check_6u"] ? $("#ckbSixpointrise").prop('checked', true) : $("#ckbSixpointrise").prop('checked', false);
                    "Y" == data[0]["check_6d"] ? $("#ckbDropdown").prop('checked', true) : $("#ckbDropdown").prop('checked', false);
                    "Y" == data[0]["check_9m"] ? $("#ckbNineNearCenter").prop('checked', true) : $("#ckbNineNearCenter").prop('checked', false);
                }

            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    });

    $("#dialogDataCPKSub").dialog({
        autoOpen: false,
        height: 500,
        width: 500,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (dialogCPKSub.attr('Mode') == "v") {
                    $(this).dialog("close");
                }
                else {
                    var DoSuccessfully = false;
                    var RowIdCPKSub = gridDataListCPKSub.jqGrid('getGridParam', 'selrow');
                    var dataRowCPKSub = gridDataListCPKSub.jqGrid('getRowData', RowIdCPKSub);
                    $.ajax({
                        url: __WebAppPathPrefix + ((dialogCPKSub.attr('Mode') == "c") ? "/SQMReliability/CreateCPKSub" : "/SQMReliability/EditCPKSub"),
                        data: {
                            "SID": dataRowCPKSub.SID
                            , "plantCode": $("#dialogDataCPK").attr('plantCode')
                            , "spc_id": escape($.trim($("#ddlDesignator").val()))
                            , "Unit": escape($.trim($("#txtUnit").val()))
                            , "Nominal": escape($.trim($("#txtNominal").val()))
                            , "minNominal": escape($.trim($("#txtminNominal").val()))
                            , "maxNominal": escape($.trim($("#txtmaxNominal").val()))
                            , "UpperControlLimit": escape($.trim($("#txtUpperControlLimit").val()))
                            , "LowerControlLimit": escape($.trim($("#txtLowerControlLimit").val()))
                            , "CTQNum": escape($.trim($("#txtCTQNum").val()))
                            , "centerline": escape($.trim($("#txtcenterline").val()))
                            , "CPKManager": escape($.trim($("#txtCPKManager").val()))
                            , "section": escape($.trim($("#txtsection").val()))
                            , "Sixpointrise": ($('#ckbSixpointrise').prop('checked')) ? true : false
                            , "Dropdown": ($('#ckbDropdown').prop('checked')) ? true : false
                            , "NineNearCenter": ($('#ckbNineNearCenter').prop('checked')) ? true : false
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialogCPKSub.attr('Mode') == "c")
                                    alert("create successfully.");
                                else
                                    alert("edit successfully.");
                            }
                            else {
                                if ((dialogCPKSub.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblDiaErrMsgCPKSub").html(data);
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
                        $("#btnSearchCPKSub").click();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialogCPKSub.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialogCPKSub.attr('plantCode'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });

})

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByModeCPKSub(Mode) {
    var dialogCPKSub = $("#dialogCPKSubCPK");
    var gridDataListCPKSub = $("#gridDataListCPKSub");
    var RowIdCPKSub = gridDataListCPKSub.jqGrid('getGridParam', 'selrow');
    var dataRowCPKSub = gridDataListCPKSub.jqGrid('getRowData', RowIdCPKSub);
    switch (Mode) {
        case "c": //Create
            $("#dialogCPKSubToolBarCPKSub").hide();

            dialogCPKSub.attr('ItemRowId', "");
            dialogCPKSub.attr('plantCode', "");
            dialogCPKSub.attr('SID', "");

            $("#ddlDesignator option:first").prop("selected", 'selected').change();
            $("#ddlDesignator").removeAttr('disabled');
            $("#txtUnit").val("");
            $("#txtUnit").removeAttr('disabled');
            $("#txtNominal").val("");
            $("#txtNominal").removeAttr('disabled');
            //$("#txtminNominal").val("");
            $("#txtminNominal").attr("disabled", "disabled");
            //$("#txtmaxNominal").val("");
            $("#txtmaxNominal").attr("disabled", "disabled");
            //$("#txtTolerance").val("");
            $("#txtTolerance").attr("disabled", "disabled");
            //$("#txtUpperControlLimit").val("");
            $("#txtUpperControlLimit").attr("disabled", "disabled");
            //$("#txtLowerControlLimit").val("");
            $("#txtLowerControlLimit").attr("disabled", "disabled");
            //$("#txtCTQNum").val("");
            $("#txtCTQNum").attr("disabled", "disabled");
            //$("#txtcenterline").val("");
            $("#txtcenterline").attr("disabled", "disabled");
            //$("#txtCPKManager").val("");
            $("#txtCPKManager").attr("disabled", "disabled");
            //$("#txtsection").val("");
            $("#txtsection").attr("disabled", "disabled");
            //$("#ckbSixpointrise").prop('checked', false);
            $("#ckbSixpointrise").attr("disabled", "disabled");
            //$("#ckbDropdown").prop('checked', false);
            $("#ckbDropdown").attr("disabled", "disabled");
            //$("#ckbNineNearCenter").prop('checked', false);
            $("#ckbNineNearCenter").attr("disabled", "disabled");

            $("#lblDiaErrMsgCPKSub").html("");

            break;
        case "v": //View
            $("#btndialogEditDataCPKSub").button("option", "disabled", false);
            $("#btndialogCancelEditCPKSub").button("option", "disabled", true);
            $("#dialogCPKSubToolBarCPKSub").show();

            dialogCPKSub.attr('ItemRowId', RowIdCPKSub);
            dialogCPKSub.attr('plantCode', dataRowCPKSub.plantCode);
            dialogCPKSub.attr('SID', dataRowCPKSub.SID);

            $("#ddlDesignator").val(dataRowCPKSub.Designator).change();
            $("#ddlDesignator").attr("disabled", "disabled");
            $("#txtUnit").val(dataRowCPKSub.Unit);
            $("#txtUnit").attr("disabled", "disabled");
            $("#txtNominal").val(dataRowCPKSub.Nominal);
            $("#txtNominal").attr("disabled", "disabled");
            $("#txtminNominal").val(dataRowCPKSub.minNominal);
            $("#txtminNominal").attr("disabled", "disabled");
            $("#txtmaxNominal").val(dataRowCPKSub.maxNominal);
            $("#txtmaxNominal").attr("disabled", "disabled");
            $("#txtUpperControlLimit").val(dataRowCPKSub.UpperControlLimit);
            $("#txtUpperControlLimit").attr("disabled", "disabled");
            $("#txtLowerControlLimit").val(dataRowCPKSub.LowerControlLimit);
            $("#txtLowerControlLimit").attr("disabled", "disabled");
            $("#txtCTQNum").val(dataRowCPKSub.CTQNum);
            $("#txtCTQNum").attr("disabled", "disabled");
            $("#txtcenterline").val(dataRowCPKSub.centerline);
            $("#txtcenterline").attr("disabled", "disabled");
            $("#txtCPKManager").val(dataRowCPKSub.CPKManager);
            $("#txtCPKManager").attr("disabled", "disabled");
            $("#txtsection").val(dataRowCPKSub.section);
            $("#txtsection").attr("disabled", "disabled");
            //$("#ckbSixpointrise").prop('checked', dataRowCPKSub.Sixpointrise == "Y" ? true : false);
            //$("#ckbSixpointrise").attr("disabled", "disabled");
            //$("#ckbDropdown").prop('checked', dataRowCPKSub.Dropdown == "Y" ? true : false);
            //$("#ckbDropdown").attr("disabled", "disabled");
            //$("#ckbNineNearCenter").prop('checked', dataRowCPKSub.NineNearCenter == "Y" ? true : false);
            //$("#ckbNineNearCenter").attr("disabled", "disabled");

            $("#ckbSixpointrise").attr("disabled", "disabled");
            $("#ckbSixpointrise").prop('checked', dataRowCPKSub.Sixpointrise == "Y" ? true : false);
            $("#ckbDropdown").attr("disabled", "disabled");
            $("#ckbDropdown").prop('checked', dataRowCPKSub.Dropdown == "Y" ? true : false);
            $("#ckbNineNearCenter").attr("disabled", "disabled");
            $("#ckbNineNearCenter").prop('checked', dataRowCPKSub.NineNearCenter == "Y" ? true : false);

            $("#lblDiaErrMsgCPKSub").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditDataCPKSub").button("option", "disabled", true);
            $("#btndialogCancelEditCPKSub").button("option", "disabled", false);
            $("#dialogCPKSubToolBarCPKSub").show();

            dialogCPKSub.attr('ItemRowId', RowIdCPKSub);
            dialogCPKSub.attr('plantCode', dataRowCPKSub.plantCode);
            dialogCPKSub.attr('SID', dataRowCPKSub.SID);

            $("#ddlDesignator").val(dataRowCPKSub.Designator).change();
            $("#ddlDesignator").removeAttr('disabled');
            $("#txtUnit").val(dataRowCPKSub.Unit);
            $("#txtUnit").removeAttr('disabled');
            $("#txtNominal").val(dataRowCPKSub.Nominal);
            $("#txtNominal").removeAttr('disabled');
            $("#txtminNominal").val(dataRowCPKSub.minNominal);
            $("#txtminNominal").attr("disabled", "disabled");
            $("#txtmaxNominal").val(dataRowCPKSub.maxNominal);
            $("#txtmaxNominal").attr("disabled", "disabled");
            $("#txtUpperControlLimit").val(dataRowCPKSub.UpperControlLimit);
            $("#txtUpperControlLimit").attr("disabled", "disabled");
            $("#txtLowerControlLimit").val(dataRowCPKSub.LowerControlLimit);
            $("#txtLowerControlLimit").attr("disabled", "disabled");
            $("#txtCTQNum").val(dataRowCPKSub.CTQNum);
            $("#txtCTQNum").attr("disabled", "disabled");
            $("#txtcenterline").val(dataRowCPKSub.centerline);
            $("#txtcenterline").attr("disabled", "disabled");
            $("#txtCPKManager").val(dataRowCPKSub.CPKManager);
            $("#txtCPKManager").attr("disabled", "disabled");
            $("#txtsection").val(dataRowCPKSub.section);
            $("#txtsection").attr("disabled", "disabled");
            //$("#ckbSixpointrise").prop('checked', dataRowCPKSub.Sixpointrise == "true" ? true : false);
            //$("#ckbSixpointrise").attr("disabled", "disabled");
            //$("#ckbDropdown").prop('checked', dataRowCPKSub.Dropdown == "true" ? true : false);
            //$("#ckbDropdown").attr("disabled", "disabled");
            //$("#ckbNineNearCenter").prop('checked', dataRowCPKSub.NineNearCenter == "true" ? true : false);
            //$("#ckbNineNearCenter").attr("disabled", "disabled");
            $("#ckbSixpointrise").attr("disabled", "disabled");
            $("#ckbSixpointrise").prop('checked', dataRowCPKSub.Sixpointrise == "Y" ? true : false);
            $("#ckbDropdown").attr("disabled", "disabled");
            $("#ckbDropdown").prop('checked', dataRowCPKSub.Dropdown == "Y" ? true : false);
            $("#ckbNineNearCenter").attr("disabled", "disabled");
            $("#ckbNineNearCenter").prop('checked', dataRowCPKSub.NineNearCenter == "Y" ? true : false);

            $("#lblDiaErrMsgCPKSub").html("");

            break;
    }
}