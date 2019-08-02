//initial dialog
$(function () {
    var Mapdialog = $("#MapdialogData");
    var dialog = $("#CriticismdialogData");
    var Infodialog = $("#InfodialogData");
    //Toolbar Buttons
    $("#btnCriticismdialogEditData").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnCriticismdialogCancelEdit").button({
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
        height: 345,
        width: 450,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (Mapdialog.attr('Mode') == "v") {
                    $(this).dialog("close");
                }
                else {
                    var DoSuccessfully = false;
                    $.ajax({
                        url: __WebAppPathPrefix + ((Mapdialog.attr('Mode') == "c") ? "/SQMBasic/CreateCriticismMap" : "/SQMBasic/EditCriticismMap"),
                        data: {
                            "CriticismID": Mapdialog.attr("CriticismID"),
                            "CriticismName": escape($.trim($("#txtCriticmName").val()))
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialog.attr('Mode') == "c")
                                    alert("Criticism create successfully.");
                                else
                                    alert("Criticism edit successfully.");
                            }
                            else {
                                if ((dialog.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblCriticismDiaErrMsg").html(data);
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
                        $("#btnCriticismSearch").click();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialog.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialog.attr('CriticismID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });
    $("#CriticismdialogData").dialog({
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
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMBasic/CreateCriticism" : "/SQMBasic/EditCriticism"),
                        data: {
                            "CriticismID": dialog.attr("CriticismID"),
                            "CriticismCategory": escape($.trim($("#ddlCriticismCategoryD").val())),
                            "CriticismUnit": escape($.trim($("#ddlCriticismUnitD").val())),
                            "ItemNO": escape($.trim($("#ddlItemNOD").val())),
                            "CriticismItem": escape($.trim($("#ddlItemNOD").find("option:selected").text())),
                            "ItemPartition": escape($.trim($("#txtItemPartition").val())),
                            "FitnessScore": escape($.trim($("#txtFitnessScore").val())),
                            "ActualScore": escape($.trim($("#txtActualScore").val())),
                            "ScoringRatio": escape($.trim($("#txtScoringRatio").val().replace("%",""))),
                            "Evaluation": escape($.trim($("#txtEvaluation").val()))
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialog.attr('Mode') == "c")
                                    alert("Criticism create successfully.");
                                else
                                    alert("Criticism edit successfully.");
                            }
                            else {
                                if ((dialog.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblCriticismDiaErrMsg").html(data);
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
                        $("#btnCriticismSearch").click();
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
    $("#InfodialogData").dialog(
      {
          autoOpen: false,
          height: 345,
          width: 800,
          resizable: false,
          modal: true,
          close: function () {
              if (dialog.attr('Mode') == "e") {
                  var r = ReleaseDataLock(dialog.attr('SID'));
                  if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
              }
          }
      }
    );
    jQuery("#txtActualScore").change(function () {
        if ($("#txtActualScore").val() && $("#txtFitnessScore").val()) {
            $("#txtScoringRatio").val(parseInt($("#txtFitnessScore").val() / $("#txtActualScore").val() * 100) + "%");
        } else {
            $("#txtScoringRatio").val("")
        }
    });
    jQuery("#txtFitnessScore").change(function () {
        if ($("#txtActualScore").val() && $("#txtFitnessScore").val()) {
            $("#txtScoringRatio").val(parseInt($("#txtFitnessScore").val() / $("#txtActualScore").val() * 100) + "%");
        } else {
            $("#txtScoringRatio").val("")
        }
    });
});

//change dialog UI
// c: Create, v: View, e: Edit
function CriticismDialogSetUIByMode(Mode) {
    var dialog = $("#CriticismdialogData");
    var gridDataList = $("#CriticismgridDataList");
    switch (Mode) {
        case "c": //Create
            $("#CriticismdialogDataToolBar").hide();
            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('CriticismID', dataRow.CriticismID);

            $("#ddlCriticismCategoryD").change();
            $("#ddlCriticismCategoryD").removeAttr('disabled');
          
            $("#ddlCriticismUnitD").removeAttr('disabled');

            $("#ddlItemNOD").removeAttr('disabled');
            $("#txtItemPartition").val("");
            $("#txtItemPartition").removeAttr('disabled');
            $("#txtFitnessScore").val("");
            $("#txtFitnessScore").removeAttr('disabled');
            $("#txtScoringRatio").val("");
            $("#txtScoringRatio").attr("disabled", "disabled");
            $("#txtActualScore").val("");
            $("#txtActualScore").removeAttr('disabled');
            $("#txtEvaluation").val("");
            $("#txtEvaluation").removeAttr('disabled');


            $("#lblCriticismDiaErrMsg").html("");

            break;
        case "v": //View
            $("#btnCriticismdialogEditData").button("option", "disabled", false);
            $("#btnCriticismdialogCancelEdit").button("option", "disabled", true);
            $("#CriticismdialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('CriticismID', dataRow.CriticismID);

            $("#ddlCriticismCategoryD").val(dataRow.CriticismCategory.trim());
            $("#ddlCriticismCategoryD").attr("disabled", "disabled");
            $("#ddlCriticismUnitD").val(dataRow.CriticismUnit.trim());
            $("#ddlCriticismUnitD").attr("disabled", "disabled");
            $("#ddlItemNOD").val(dataRow.ItemNO.trim());
            $("#ddlItemNOD").attr("disabled", "disabled");
            $("#txtItemPartition").val(dataRow.ItemPartition);
            $("#txtItemPartition").attr("disabled", "disabled");
            $("#txtFitnessScore").val(dataRow.FitnessScore);
            $("#txtFitnessScore").attr("disabled", "disabled");
            $("#txtScoringRatio").val(dataRow.ScoringRatio);
            $("#txtScoringRatio").attr("disabled", "disabled");
            $("#txtActualScore").val(dataRow.ActualScore);
            $("#txtActualScore").attr("disabled", "disabled");
            $("#txtEvaluation").val(dataRow.Evaluation);
            $("#txtEvaluation").attr("disabled", "disabled");
         

            $("#lblCriticismDiaErrMsg").html("");

            break;
        //default: //Edit("e")
        //    $("#btndialogEditData").button("option", "disabled", true);
        //    $("#btndialogCancelEdit").button("option", "disabled", false);
        //    $("#dialogDataToolBar").show();

        //    var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
        //    var dataRow = gridDataList.jqGrid('getRowData', RowId);
        //    dialog.attr('ItemRowId', RowId);
        //    dialog.attr('CriticismID', dataRow.CriticismID);

        //    $("#ddlCriticismCategoryD").val(dataRow.CriticismCategory.trim());
        //    $("#ddlCriticismCategoryD").attr("disabled", "disabled");
        //    $("#ddlCriticismUnitD").val(dataRow.CriticismUnit.trim());
        //    $("#ddlCriticismUnitD").attr("disabled", "disabled");
        //    $("#ddlItemNOD").val(dataRow.ItemNO.trim());
        //    $("#ddlItemNOD").attr("disabled", "disabled");
        //    $("#txtItemPartition").val(dataRow.ItemPartition);
        //    $("#txtItemPartition").removeAttr('disabled');
        //    $("#txtFitnessScore").val(dataRow.FitnessScore);
        //    $("#txtFitnessScore").removeAttr('disabled');
        //    $("#txtScoringRatio").val(dataRow.ScoringRatio);
        //    $("#txtScoringRatio").removeAttr('disabled');
        //    $("#txtActualScore").val(dataRow.ActualScore);
        //    $("#txtActualScore").removeAttr('disabled');
        //    $("#txtEvaluation").val(dataRow.Evaluation);
        //    $("#txtEvaluation").removeAttr('disabled');

        //    $("#lblDiaErrMsg").html("");

            //break;
    }
}


//change Mapdialog UI
// c: Create, v: View, e: Edit
function MapDialogSetUIByMode(Mode) {
    var dialog = $("#MapdialogData");
    var gridDataList = $("#CriticismgridDataList");
    switch (Mode) {
        case "c": //Create
            $("#MapdialogDataToolBar").hide();
            dialog.attr('ItemRowId', "");
            dialog.attr('CriticismID', "");
 
            $("#txtCriticmName").val("");
            $("#txtCriticmName").removeAttr('disabled');

            $("#lblMapDiaErrMsg").html("");

            break;
        case "v": //View
            $("#btnMapdialogEditData").button("option", "disabled", false);
            $("#btnMapdialogCancelEdit").button("option", "disabled", true);
            $("#MapdialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('CriticismID', dataRow.CriticismID);

            $("#txtCriticmName").val(dataRow.CriticismName);
            $("#txtCriticmName").attr("disabled", "disabled");


            $("#lblMapDiaErrMsg").html("");

            break;
        default: //Edit("e")
            $("#btnMapdialogEditData").button("option", "disabled", true);
            $("#btnMapdialogCancelEdit").button("option", "disabled", false);
            $("#MapdialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('CriticismID', dataRow.CriticismID);

            $("#txtCriticmName").val(dataRow.CriticismName);
            $("#txtCriticmName").removeAttr('disabled');

            $("#lblMapDiaErrMsg").html("");

            break;
    }
}

//change Infodialog UI
// c: Create, v: View, e: Edit
function InfoDialogSetUIByMode() {
    var gridDataList = $("#CriticismgridDataList");

    var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
    var dataRow = gridDataList.jqGrid('getRowData', RowId);
    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/LoadCriticismInfoJSonWithFilter',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        data: { SearchText: dataRow.CriticismID },
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            for (var idx in data.Rows) {
                $("#lblFactoryName").text(data.Rows[idx].FactoryName);
                $("#lblFactoryAddress").text(data.Rows[idx].FactoryAddress);
                $("#lblCNAME").text(data.Rows[idx].CNAME);
                $("#lblPHONE").text(data.Rows[idx].PHONE);
                $("#lblQSADateCriticism").text(data.Rows[idx].QSADateCriticism);
                $("#lblQPADateCriticism").text(data.Rows[idx].QPADateCriticism);
                $("#lblQSAScore").text(data.Rows[idx].QSAScore);
                $("#lblQPAScore").text(data.Rows[idx].QPAScore);
                $("#lblQSAEvaluation").text(data.Rows[idx].QSAEvaluation);
                $("#lblQPAEvaluation").text(data.Rows[idx].QPAEvaluation);
            }
         
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });


    var InfogridDataList = $("#InfogridDataList");
    InfogridDataList.jqGrid('clearGridData');
    InfogridDataList.jqGrid('setGridParam', { postData: { CriticismID: dataRow.CriticismID } })
    InfogridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}
