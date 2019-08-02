$(function () {
    var dialogMap = $("#dialogDataMap");
    var gridDataListMap = $("#gridDataListMap");
    //Toolbar Buttons
    $("#btndialogEditDataMap").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogCancelEditMap").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#dialogDataMap").dialog({
        autoOpen: false,
        height: 400,
        width: 500,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (dialogMap.attr('Mode') == "v") {
                    $(this).dialog("close");
                }
                else {
                    var DoSuccessfully = false;
                    var RowIdMap = gridDataListMap.jqGrid('getGridParam', 'selrow');
                    $.ajax({
                        url: __WebAppPathPrefix + ((dialogMap.attr('Mode') == "c") ? "/SQMBasic/CreateAQLPlantMap" : "/SQMBasic/EditAQLPlantMap"),
                        data: {
                            "SID": $("#dialogDataMap").attr('SID')
                            , "SSID": $("#dialogData").attr('SSID')
                            , "AQLNum": escape($.trim($("#txtAQLNum").val()))
                            , "AQLType": escape($.trim($("#txtAQLType").val()))
                            , "AQL": escape($.trim($("#txtAQL").val()))
                            , "CR": escape($.trim($("#txtCR").val()))
                            , "MA": escape($.trim($("#txtMA").val()))
                            , "MI": escape($.trim($("#txtMI").val()))
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialogMap.attr('Mode') == "c")
                                    alert("create successfully.");
                                else
                                    alert("edit successfully.");
                            }
                            else {
                                if ((dialogMap.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblDiaErrMsgMap").html(data);
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
                        //$("#btnSearchMap").click();
                        ReflashMap();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialogMap.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialogMap.attr('SID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });

})

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByModeMap(Mode) {
    var dialogMap = $("#dialogDataMap");
    var gridDataListMap = $("#gridDataListMap");
    var RowIdMap = gridDataListMap.jqGrid('getGridParam', 'selrow');
    var dataRowMap = gridDataListMap.jqGrid('getRowData', RowIdMap);
    switch (Mode) {
        case "c": //Create
            $("#dialogDataToolBarMap").hide();

            dialogMap.attr('ItemRowId', "");
            dialogMap.attr('SID', "");

            $("#txtAQLNum").val("");
            $("#txtAQLNum").removeAttr('disabled');
            $("#txtAQLType option:first").prop("selected", 'selected').change();
            $("#txtAQLType").removeAttr('disabled');
            $("#txtAQL").val("");
            $("#txtAQL").removeAttr('disabled');

            $("#txtCR").val("");
            $("#txtCR").removeAttr('disabled');
            $("#txtMA").val("");
            $("#txtMA").removeAttr('disabled');
            $("#txtMI").val("");
            $("#txtMI").removeAttr('disabled');

            $("#lblDiaErrMsgMap").html("");

            break;
        case "v": //View
            $("#btndialogEditDataMap").button("option", "disabled", false);
            $("#btndialogCancelEditMap").button("option", "disabled", true);
            $("#dialogDataToolBarMap").show();

            dialogMap.attr('ItemRowId', RowIdMap);
            dialogMap.attr('SID', dataRowMap.SID);

            $("#txtAQLNum").val(dataRowMap.AQLNum);
            $("#txtAQLNum").attr("disabled", "disabled");
            $("#txtAQLType").val(GetValue(dataRowMap.AQLType));
            $("#txtAQLType").attr("disabled", "disabled");
            $("#txtAQL").val(dataRowMap.AQL);
            $("#txtAQL").attr("disabled", "disabled");

            $("#txtCR").val(dataRowMap.CR);
            $("#txtCR").attr("disabled", "disabled");
            $("#txtMA").val(dataRowMap.MA);
            $("#txtMA").attr("disabled", "disabled");
            $("#txtMI").val(dataRowMap.MI);
            $("#txtMI").attr("disabled", "disabled");


            $("#lblDiaErrMsgMap").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditDataMap").button("option", "disabled", true);
            $("#btndialogCancelEditMap").button("option", "disabled", false);
            $("#dialogDataToolBarMap").show();

            dialogMap.attr('ItemRowId', RowIdMap);
            dialogMap.attr('SID', dataRowMap.SID);

            $("#txtAQLNum").val(dataRowMap.AQLNum);
            $("#txtAQLNum").removeAttr('disabled');
            $("#txtAQLType").val(GetValue(dataRowMap.AQLType));
            $("#txtAQLType").removeAttr('disabled');
            $("#txtAQL").val(dataRowMap.AQL);
            $("#txtAQL").removeAttr('disabled');

            $("#txtCR").val(dataRowMap.CR);
            $("#txtCR").removeAttr('disabled');
            $("#txtMA").val(dataRowMap.MA);
            $("#txtMA").removeAttr('disabled');
            $("#txtMI").val(dataRowMap.MI);
            $("#txtMI").removeAttr('disabled');

            $("#lblDiaErrMsgMap").html("");

            break;
    }
}
function GetValue(type) {
    switch (type) {
        case "正常":
            return 0;
            break;
        case "加嚴":
            return 1;
            break;
        case "減量":
            return 2;
            break;
    }
}
