﻿//initial dialog
$(function () {
    var Mapdialog = $("#MapdialogData");
    var dialog = $("#ReliabilitydialogData");
    //var Infodialog = $("#InfodialogData");
    //Toolbar Buttons
    $("#btnReliabilitydialogEditData").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnReliabilitydialogCancelEdit").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });
    $("#btnMapdialogEditData").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnMapdialogCancelEdit").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });


    $("#MapdialogData").dialog({
        autoOpen: false,
        height: 450,
        width: 560,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (Mapdialog.attr('Mode') == "v" || Mapdialog.attr('Mode') == "u") {
                    $(this).dialog("close");
                }
                else {
                    var DoSuccessfully = false;
                    $.ajax({
                        url: __WebAppPathPrefix + ((Mapdialog.attr('Mode') == "c") ? "/SQMBasic/CreateReliability" : "/SQMBasic/UpdateReliInfo"),
                        data: {
                            "ReliabilitySID": Mapdialog.attr("ReliabititySID"),
                            'ActualTestTime':escape($.trim($("#txtActualTestTime").val())),
                            'TestResult':escape($.trim($("#txtTestResult").val())),
                            'TestPeople':escape($.trim($("#txtTestPeople").val())),
                            'Note':escape($.trim($("#txtNote").val()))
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialog.attr('Mode') == "c")
                                    alert("Reliability create successfully.");
                                else
                                    alert("Reliability edit successfully.");
                            }
                            else {
                                if ((dialog.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblMapDiaErrMsg").html(data);
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
                        $("#btnReliInfoSearch").click();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialog.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialog.attr('SID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });



    $("#ReliabilitydialogData").dialog({
        autoOpen: false,
        height: 400,
        width: 500,
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
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMBasic/CreateReliInfo" : "/SQMBasic/UpdateReliInfo"),
                        data: {
                            'ReliabilitySID': $("#ReliInfogridDataList").attr("ReliabititySID"),
                            //'TB_SQM_CommodityCID':
                            //'TB_SQM_CommodityCID': escape($.trim($("#ddlCategory").val())),
                            //'TB_SQM_Commodity_SubCID':
                            //'TB_SQM_Commodity_SubCID': escape($.trim($("#ddlCategorySub").val())),
                            'TestProjet': escape($.trim($("#txtTestProjet").val())),
                            'PlannedTestTime': escape($.trim($("#txtPlannedTestTime").val())),
                            'ActualTestTime':escape($.trim($("#txtActualTestTime").val())),
                            'TestResult':escape($.trim($("#txtTestResult").val())),
                            'TestPeople':escape($.trim($("#txtTestPeople").val())),
                            'Note':escape($.trim($("#txtNote").val()))
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialog.attr('Mode') == "c")
                                    alert("Reliability create successfully.");
                                else
                                    alert("Reliability edit successfully.");
                            }
                            else {
                                if ((dialog.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblReliabilityDiaErrMsg").html(data);
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
                        $("#btnReliInfoSearch").click();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialog.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialog.attr('SID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });
    //$("#InfodialogData").dialog(
    //  {
    //      autoOpen: false,
    //      height: 345,
    //      width: 800,
    //      resizable: false,
    //      modal: true,
    //      close: function () {
    //          if (dialog.attr('Mode') == "e") {
    //              var r = ReleaseDataLock(dialog.attr('SID'));
    //              if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
    //          }
    //      }
    //  }
    //);

});

//change dialog UI
// c: Create, v: View, e: Edit
function ReliabilityDialogSetUIByMode(Mode) {
    var dialog = $("#ReliabilitydialogData");
    var gridDataList = $("#ReliInfogridDataList");
    switch (Mode) {
        //case "c": //Create
        //    $("#ReliabilitydialogDataToolBar").hide();
        //    var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        //    var dataRow = gridDataList.jqGrid('getRowData', RowId);
        //    dialog.attr('ItemRowId', RowId);
        //    dialog.attr('SID', "");

        //    $("#ddlCategory").val("A");
        //    $("#ddlCategory").removeAttr('disabled');
        //    $("#ddlCategorySub").val();
        //    $("#ddlCategorySub").removeAttr('disabled');

        //    //$("#txtReliabilitySID").val(dataRow.ReliabilitySID);
        //    //$("#txtReliabilitySID").removeAttr('disabled');
        //    $("#txtCommodityName").val("");
        //    $("#txtCommodityName").removeAttr('disabled');
        //    $("#txtCommodity_SubName").val("");
        //    $("#txtCommodity_SubName").removeAttr('disabled');
        //    $("#txtTestProjet").val("");
        //    $("#txtTestProjet").removeAttr('disabled');
        //    $("#txtPlannedTestTime").val("");
        //    $("#txtPlannedTestTime").removeAttr('disabled');


        //    //$("txtActualTestTime").val("");
        //    //$("txtActualTestTime").removeAttr('disabled');
        //    //$("txtTestResult").val("");
        //    //$("txtTestResult").removeAttr('disabled');
        //    //$("txtTestPeople").val("");
        //    //$("txtTestPeople").removeAttr('disabled');
        //    //$("txtNote").val("");
        //    //$("txtNote").removeAttr('disabled');
        //    //$("#txtFile").hide();
        //    //$("#UpDateFile").hide();
        //    //$("#btnUploadSureInfo").hide();



        //    $("#lblDiaErrMsg").html("");

        //    break;
        case "v": //View

            $("#btnReliabilitydialogEditData").button("option", "disabled", false);
            $("#btnReliabilitydialogCancelEdit").button("option", "disabled", true);
            $("#ReliabilitydialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('SID', dataRow.ReliabilitySID);

            $("#ddlCategory").val(dataRow.TB_SQM_CommodityCID).change();
            $("#ddlCategory").attr('disabled', 'disabled');
            $("#ddlCategorySub").val(dataRow.TB_SQM_Commodity_SubCID);
            $("#ddlCategorySub").attr('disabled', 'disabled');

            //$("#ddlReliabilityCategoryD").val(dataRow.ReliabilityCategory.trim());
            //$("#ddlReliabilityCategoryD").attr("disabled", "disabled");
            //$("#ddlReliabilityUnitD").val(dataRow.ReliabilityUnit.trim());
            //$("#ddlReliabilityUnitD").attr("disabled", "disabled");
            //$("#ddlItemNOD").val(dataRow.ItemNO.trim());
            //$("#ddlItemNOD").attr("disabled", "disabled");

            //$("#txtReliabilitySID").val(dataRow.ReliabilitySID);
            //$("#txtReliabilitySID").attr("disabled", "disabled");
            $("#txtCommodityName").val(dataRow.CommodityName);
            $("#txtCommodityName").attr("disabled", "disabled");
            $("#txtCommodity_SubName").val(dataRow.Commodity_SubName);
            $("#txtCommodity_SubName").attr("disabled", "disabled");
            $("#txtTestProjet").val(dataRow.TestProjet);
            $("#txtTestProjet").attr("disabled", "disabled");
            $("#txtPlannedTestTime").val(dataRow.PlannedTestTime);
            $("#txtPlannedTestTime").attr("disabled", "disabled");

            $("#txtActualTestTime").val(dataRow.ActualTestTime);
            $("#txtActualTestTime").attr("disabled", "disabled");
            $("#txtTestResult").val(dataRow.TestResult);
            $("#txtTestResult").attr("disabled", "disabled");
            $("#txtTestPeople").val(dataRow.TestPeople);
            $("#txtTestPeople").attr("disabled", "disabled");
            $("#txtNote").val(dataRow.Note);
            $("#txtNote").attr("disabled", "disabled");

            $("#txtFile").hide();
            $("#UpDateFile").hide();
            $("#btnUploadSureInfo").hide();

            $("#lblReliabilityDiaErrMsg").html("");
            break;
            //case "u"://Upload File
            //    var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            //    var dataRow = gridDataList.jqGrid('getRowData', RowId);
            //    dialog.attr('ItemRowId', RowId);
            //    dialog.attr('SID', dataRow.ReliabilitySID);

            //    $("#ReliabilitydialogDataToolBar").hide();

            //    $("#ddlCategory").val(dataRow.TB_SQM_CommodityCID).change();
            //    $("#ddlCategory").attr('disabled', 'disabled');
            //    //$("#ddlCategory").hide();
            //    //$("#lblCategory").hide();
            //    $("#ddlCategorySub").val(dataRow.TB_SQM_Commodity_SubCID);
            //    $("#ddlCategorySub").attr('disabled', 'disabled');
            //    //$("#ddlCategorySub").hide();

            //    $("#txtCommodityName").val(dataRow.CommodityName);
            //    $("#txtCommodityName").attr("disabled", "disabled");
            //    //$("#txtCommodityName").hide();
            //    //$("#lblCommodityName").hide();
            //    $("#txtCommodity_SubName").val(dataRow.Commodity_SubName);
            //    $("#txtCommodity_SubName").attr("disabled", "disabled");
            //    //$("#txtCommodity_SubName").hide();
            //    //$("#lblCommodity_SubName").hide();
            //    $("#txtTestProjet").val(dataRow.TestProjet);
            //    $("#txtTestProjet").attr("disabled", "disabled");
            //    //$("#txtTestProjet").hide();
            //    //$("#lblTestProjet").hide();
            //    $("#txtPlannedTestTime").val(dataRow.PlannedTestTime);
            //    $("#txtPlannedTestTime").attr("disabled", "disabled");
            //    //$("#txtPlannedTestTime").hide();
            //    //$("#lblPlannedTestTime").hide();

            //    $("#txtFile").show();
            //    $("#UpDateFile").show();
            //    $("#btnUploadSureInfo").show();

            //    $("#UpDateFile").removeAttr('disabled');
            //    $("#btnUploadSureInfo").removeAttr('disabled');

            //    $("#lblDiaErrMsg").html("");

            //    break;
        default: //Edit("e")
            $("#btnReliabilitydialogEditData").button("option", "disabled", true);
            $("#btnReliabilitydialogCancelEdit").button("option", "disabled", false);
            $("#ReliabilitydialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('SID', dataRow.ReliabilitySID);


            //$("#ddlCategory").val(dataRow.TB_SQM_CommodityCID);
            //$("#ddlCategory").removeAttr('disabled');
            //$("#ddlCategorySub").val(dataRow.TB_SQM_Commodity_SubCID);
            //$("#ddlCategorySub").removeAttr('disabled');

            $("#ddlCategory").val(dataRow.TB_SQM_CommodityCID).change();
            $("#ddlCategory").attr('disabled', 'disabled');
            $("#ddlCategorySub").val(dataRow.TB_SQM_Commodity_SubCID);
            $("#ddlCategorySub").attr('disabled', 'disabled');

            //$("#txtReliabilitySID").val(dataRow.ReliabilitySID);
            //$("#txtReliabilitySID").removeAttr('disabled');
            $("#txtCommodityName").val(dataRow.CommodityName);
            $("#txtCommodityName").removeAttr('disabled');
            $("#txtCommodity_SubName").val(dataRow.Commodity_SubName);
            $("#txtCommodity_SubName").removeAttr('disabled');
            $("#txtTestProjet").val(dataRow.TestProjet);
            $("#txtTestProjet").attr("disabled", "disabled");
            $("#txtPlannedTestTime").val(dataRow.PlannedTestTime);
            $("#txtPlannedTestTime").attr("disabled", "disabled");


            $("#txtActualTestTime").val(dataRow.ActualTestTime);
            $("#txtActualTestTime").removeAttr('disabled');
            $("#txtTestResult").val((dataRow.TestResult=="")?"Pass":dataRow.TestResult);
            $("#txtTestResult").removeAttr('disabled');
            $("#txtTestPeople").val(dataRow.TestPeople);
            $("#txtTestPeople").removeAttr('disabled');
            $("#txtNote").val(dataRow.Note);
            $("#txtNote").removeAttr('disabled');

            $("#txtFile").hide();
            $("#UpDateFile").hide();
            $("#btnUploadSureInfo").hide();

            $("#lblReliabilityDiaErrMsg").html("");

            break;
    }
}


//change Mapdialog UI
// c: Create, v: View, e: Edit
function RMapDialogSetUIByMode(Mode) {
    var dialog = $("#MapdialogData");
    var gridDataList = $("#ReliInfogridDataList");
    switch (Mode) {
        case "c": //Create
            $("#MapdialogDataToolBar").hide();
            dialog.attr('ItemRowId', "");
            dialog.attr('ReliabititySID', "");

            $("#ddlCategory option:first").prop("selected", 'selected');
            $("#ddlCategory").removeAttr('disabled');
            $("#ddlCategorySub option:first").prop("selected", 'selected');
            $("#ddlCategorySub").removeAttr('disabled');

            $("#txtCommodityName").val("");
            $("#txtCommodityName").removeAttr('disabled');
            $("#txtCommodity_SubName").val("");
            $("#txtCommodity_SubName").removeAttr('disabled');
            $("#txtTestProjet").val("");
            $("#txtTestProjet").removeAttr('disabled');
            $("#txtPlannedTestTime").val("");
            $("#txtPlannedTestTime").removeAttr('disabled');


            $("#txtActualTestTime").val("");
            $("#txtActualTestTime").hide();
            $("#lblActualTestTime").hide();
            $("#txtTestResult").val("");
            $("#txtTestResult").hide();
            $("#lblTestResult").hide();
            $("#txtTestPeople").val("");
            $("#txtTestPeople").hide();
            $("#lblTestPeople").hide();
            $("#txtNote").val("");
            $("#txtNote").hide();
            $("#lblNote").hide();
            $("#txtFile").hide();
            $("#UpDateFile").hide();
            $("#txtinsertime").hide();
            $("#btnUploadSureInfo").hide();

            $("#lblMapDiaErrMsg").html("");

            break;
            //case "v": //View
            //    $("#btnMapdialogEditData").button("option", "disabled", false);
            //    $("#btnMapdialogCancelEdit").button("option", "disabled", true);
            //    $("#MapdialogDataToolBar").show();

            //    var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            //    var dataRow = gridDataList.jqGrid('getRowData', RowId);
            //    dialog.attr('ItemRowId', RowId);
            //    dialog.attr('SID', dataRow.SID);

            //    $("#txtReliabilityName").val(dataRow.ReliabilityName);
            //    $("#txtReliabilityName").attr("disabled", "disabled");

            //    $("#txtFile").hide();
            //    $("#UpDateFile").hide();
            //    $("#btnUploadSureInfo").hide();

            //    $("#lblMapDiaErrMsg").html("");

            //    break;
        case "e": //Edit("e")
            $("#btnMapdialogEditData").button("option", "disabled", true);
            $("#btnMapdialogCancelEdit").button("option", "disabled", false);
            $("#MapdialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('ReliabititySID', dataRow.ReliabititySID);

            $("#ddlCategory").val(dataRow.TB_SQM_CommodityCID).change();
            $("#ddlCategory").attr('disabled', 'disabled');
            $("#ddlCategorySub").val(dataRow.TB_SQM_Commodity_SubCID);
            $("#ddlCategorySub").attr('disabled', 'disabled');

            $("#txtCommodityName").val(dataRow.CommodityName);
            $("#txtCommodityName").attr('disabled', 'disabled');
            $("#txtCommodity_SubName").val(dataRow.Commodity_SubName);
            $("#txtCommodity_SubName").attr('disabled', 'disabled');
            $("#txtTestProjet").val(dataRow.TestProjet);
            $("#txtTestProjet").attr('disabled', 'disabled');
            $("#txtPlannedTestTime").val(dataRow.PlannedTestTime);
            $("#txtPlannedTestTime").attr('disabled', 'disabled');

            $("#txtActualTestTime").val(dataRow.ActualTestTime);
            $("#txtActualTestTime").removeAttr('disabled');
            $("#txtActualTestTime").show();
            $("#lblActualTestTime").show();
            $("#txtTestResult").removeAttr('disabled');
            dataRow.TestResult == "" ? $("#txtTestResult option:first").prop("selected", 'selected') : $("#txtTestResult").val(dataRow.TestResult);
            $("#txtTestResult").show();
            $("#lblTestResult").show();
            $("#txtTestPeople").val(dataRow.TestPeople);
            $("#txtTestPeople").removeAttr('disabled');
            $("#txtTestPeople").show();
            $("#lblTestPeople").show();
            $("#txtNote").val(dataRow.Note);
            $("#txtNote").removeAttr('disabled');
            $("#txtNote").show();
            $("#lblNote").show();
            $("#txtinsertime").val(dataRow.insertime);
            $("#txtinsertime").removeAttr('disabled');
            $("#txtinsertime").show();
            $("#lblinsertime").show();

            $("#txtFile").show();
            $("#UpDateFile").show();
            $("#btnUploadSureInfo").show();

            $("#lblMapDiaErrMsg").html("");

            break;
        default://View
            $("#btnMapdialogEditData").button("option", "disabled", false);
            $("#btnMapdialogCancelEdit").button("option", "disabled", true);
            $("#MapdialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('ReliabititySID', dataRow.ReliabititySID);

            $("#ddlCategory").val(dataRow.TB_SQM_CommodityCID).change();
            $("#ddlCategory").attr("disabled", "disabled");
            $("#ddlCategorySub").val(dataRow.TB_SQM_Commodity_SubCID);
            $("#ddlCategorySub").attr("disabled", "disabled");

            $("#txtCommodityName").val(dataRow.CommodityName);
            $("#txtCommodityName").attr("disabled", "disabled");
            $("#txtCommodity_SubName").val(dataRow.Commodity_SubName);
            $("#txtCommodity_SubName").attr("disabled", "disabled");
            $("#txtTestProjet").val(dataRow.TestProjet);
            $("#txtTestProjet").attr("disabled", "disabled");
            $("#txtPlannedTestTime").val(dataRow.PlannedTestTime);
            $("#txtPlannedTestTime").attr("disabled", "disabled");

            $("#txtActualTestTime").val(dataRow.ActualTestTime);
            $("#txtActualTestTime").attr("disabled", "disabled");
            $("#txtActualTestTime").show();
            $("#lblActualTestTime").show();
            $("#txtTestResult").val(dataRow.TestResult);
            $("#txtTestResult").attr("disabled", "disabled");
            $("#txtTestResult").show();
            $("#lblTestResult").show();
            $("#txtTestPeople").val(dataRow.TestPeople);
            $("#txtTestPeople").attr("disabled", "disabled");
            $("#txtTestPeople").show();
            $("#lblTestPeople").show();
            $("#txtNote").val(dataRow.Note);
            $("#txtNote").attr("disabled", "disabled");
            $("#txtNote").show();
            $("#lblNote").show();
            $("#txtinsertime").val(dataRow.insertime);
            $("#txtinsertime").attr("disabled", "disabled");
            $("#txtinsertime").show();
            $("#lblinsertime").show();

            $("#txtFile").hide();
            $("#UpDateFile").hide();
            $("#btnUploadSureInfo").hide();


            $("#lblMapDiaErrMsg").html("");

            break;
    }
}
