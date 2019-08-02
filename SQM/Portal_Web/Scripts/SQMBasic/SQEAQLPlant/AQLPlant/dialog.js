$(function () {
    var dialog = $("#dialogData");
    var gridDataList = $("#gridDataList");
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
                    var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
                    $.ajax({
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMBasic/CreateAQLPlant" : "/SQMBasic/EditAQLPlant"),
                        data: {
                            "SID": gridDataList.jqGrid('getRowData', RowId).SID
                            , "PlantName": escape($.trim($("#txtPlantName").val()))
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
                var r = ReleaseDataLock(dialog.attr('SID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });

})

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByMode(Mode) {
    var dialog = $("#dialogData");
    var gridDataList = $("#gridDataList");
    var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
    var dataRow = gridDataList.jqGrid('getRowData', RowId);
    switch (Mode) {
        case "c": //Create
            $("#dialogDataToolBar").hide();

            dialog.attr('ItemRowId', "");
            dialog.attr('SID', "");

            $("#txtPlantName").val("");
            $("#txtPlantName").removeAttr('disabled');


            $("#lblDiaErrMsg").html("");

            break;
        case "v": //View
            $("#btndialogEditData").button("option", "disabled", false);
            $("#btndialogCancelEdit").button("option", "disabled", true);
            $("#dialogDataToolBar").show();

            dialog.attr('ItemRowId', RowId);
            dialog.attr('SID', dataRow.SID);

            $("#txtPlantName").val(dataRow.PlantName);
            $("#txtPlantName").attr("disabled", "disabled");

            $("#lblDiaErrMsg").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditData").button("option", "disabled", true);
            $("#btndialogCancelEdit").button("option", "disabled", false);
            $("#dialogDataToolBar").show();

            dialog.attr('ItemRowId', RowId);
            dialog.attr('SID', dataRow.SID);

            $("#txtPlantName").val(dataRow.PlantName);
            $("#txtPlantName").removeAttr('disabled');

            $("#lblDiaErrMsg").html("");

            break;
    }
}