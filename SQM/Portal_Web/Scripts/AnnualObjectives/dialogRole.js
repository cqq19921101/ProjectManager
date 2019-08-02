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
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/AnnualObjectives/Create" : "/AnnualObjectives/Edit"),
                        data: {
                            "SID": dialog.attr("SID"),
                            "CID": escape($.trim($("#ddlCID").val())),
                            "CCID": escape($.trim($("#ddlCCID").val())),
                            "AType": escape($.trim($("#ddlType").val())),
                            "MaterialType": escape($.trim($("#ddlMaterialType").val())),
                            "Q1": escape($.trim($("#txtQ1").val())),
                            "Q2": escape($.trim($("#txtQ2").val())),
                            "Q3": escape($.trim($("#txtQ3").val())),
                            "Q4": escape($.trim($("#txtQ4").val()))
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialog.attr('Mode') == "c")
                                    alert("Annual create successfully.");
                                else
                                    alert("Annual edit successfully.");
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
});

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByMode(Mode) {
    var dialog = $("#dialogData");
    var gridDataList = $("#gridDataList");
    switch (Mode) {
        case "c": //Create
            $("#dialogDataToolBar").hide();

            dialog.attr('ItemRowId', "");
            dialog.attr('SID', "");

        
           
            $("#ddlCID").val(1);
            $("#ddlCID").removeAttr('disabled');
            $("#ddlCCID").val(1);
            $("#ddlCCID").removeAttr('disabled');
            $("#ddlType").val(1);
            $("#ddlType").removeAttr('disabled');
   
            $("#txtQ1").val("");
            $("#txtQ1").removeAttr('disabled');
            $("#txtQ2").val("");
            $("#txtQ2").removeAttr('disabled');
            $("#txtQ3").val("");
            $("#txtQ3").removeAttr('disabled');
            $("#txtQ4").val("");
            $("#txtQ4").removeAttr('disabled');

            $("#lblDiaErrMsg").html("");

            break;
        case "v": //View
            $("#btndialogEditData").button("option", "disabled", false);
            $("#btndialogCancelEdit").button("option", "disabled", true);
            $("#dialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('SID', dataRow.SID);

           
        
            $("#ddlCID").val(dataRow.CID);
            $("#ddlCID").attr("disabled", "disabled");
            $('#ddlCID').change();
            $("#ddlCCID").val(dataRow.CCID);
            $("#ddlCCID").attr("disabled", "disabled");
            $("#ddlType").val(dataRow.AType);
            $("#ddlType").attr("disabled", "disabled");
            $("#ddlMaterialType").val(dataRow.MaterialType);
            $("#ddlMaterialType").attr("disabled", "disabled");
            $("#txtQ1").val(dataRow.Q1);
            $("#txtQ1").attr("disabled", "disabled");
            $("#txtQ2").val(dataRow.Q2);
            $("#txtQ2").attr("disabled", "disabled");
            $("#txtQ3").val(dataRow.Q3);
            $("#txtQ3").attr("disabled", "disabled");
            $("#txtQ4").val(dataRow.Q4);
            $("#txtQ4").attr("disabled", "disabled");

            $("#lblDiaErrMsg").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditData").button("option", "disabled", true);
            $("#btndialogCancelEdit").button("option", "disabled", false);
            $("#dialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('SID', dataRow.SID);

            $("#ddlCID").val(dataRow.CID)
            $("#ddlCID").attr("disabled", "disabled");
            $("#ddlCCID").val(dataRow.CCID)
            $("#ddlCCID").attr("disabled", "disabled");
            $("#ddlType").val(dataRow.AType);
            $("#ddlType").attr("disabled", "disabled");
            $("#ddlMaterialType").val(dataRow.MaterialType);
            $("#ddlMaterialType").attr("disabled", "disabled");
            $("#txtQ1").val(dataRow.Q1);
            $("#txtQ1").removeAttr('disabled');
            $("#txtQ2").val(dataRow.Q2);
            $("#txtQ2").removeAttr('disabled');
            $("#txtQ3").val(dataRow.Q3);
            $("#txtQ3").removeAttr('disabled');
            $("#txtQ4").val(dataRow.Q4);
            $("#txtQ4").removeAttr('disabled');
            

            $("#lblDiaErrMsg").html("");

            break;
    }
}