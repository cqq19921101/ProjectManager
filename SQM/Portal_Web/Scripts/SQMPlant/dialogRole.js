//initial dialog
$(function () {
    var dialog = $("#SQMPlantdialogData");
    
    //Toolbar Buttons
    $("#btnSQMPlantdialogEditData").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnSQMPlantdialogCancelEdit").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#SQMPlantdialogData").dialog({
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
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMPlant/CreatePlant" : "/SQMPlant/EditPlant"),
                        data: {
                            "PlantCode": escape($.trim($("#txtPlant").val())),
                            'MemberGUID': escape($.trim($("#txtMG").val())),
                            "plant_name": "",
                            "NAME": "",
                            "EMAIL": "",
                          
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialog.attr('Mode') == "c")
                                    alert("Plant create successfully.");
                                else
                                    alert("Plant edit successfully.");
                            }
                            else {
                                if ((dialog.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#SQMPlantlblDiaErrMsg").html(data);
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
                        $("#btnSQMPlantSearch").click();
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
function PlantDialogSetUIByMode(Mode) {
    var dialog = $("#SQMPlantdialogData");
    var gridDataList = $("#SQMPlantgridDataList");
    switch (Mode) {
        case "c": //Create
           

            $("#txtPlant").val("");
            $("#txtPlant").removeAttr('disabled');
            $("#txtMG").val("");
            $("#txtMG").removeAttr('disabled');

  

            $("#SQMPlantlblDiaErrMsg").html("");

            break;
        case "v": //View
            $("#btnSQMPlantdialogEditData").button("option", "disabled", false);
            $("#btnSQMPlantdialogCancelEdit").button("option", "disabled", true);
            $("#SQMPlantdialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('SID', dataRow.SID);

           
            $("#txtProvider").val(dataRow.Provider);
            $("#txtProvider").attr("disabled", "disabled");
            $("#txtVendorCode").val(dataRow.Vendor);
            $("#txtVendorCode").attr("disabled", "disabled");
            $("#ddlJob").val(dataRow.jobID)
            $("#ddlJob").attr("disabled", "disabled");
            $("#txtName").val(dataRow.Name);
            $("#txtName"). attr("disabled", "disabled");
            $("#txtPhone").val(dataRow.Phone);
            $("#txtPhone").attr("disabled", "disabled");
            $("#txtFixedTelephone").val(dataRow.FixedTelephone);
            $("#txtFixedTelephone").attr("disabled", "disabled");
            $("#txtEmail").val(dataRow.Email);
            $("#txtEmail").attr("disabled", "disabled");

            $("#SQMPlantlblDiaErrMsg").html("");

            break;
        default: //Edit("e")
            $("#btnSQMPlantdialogEditData").button("option", "disabled", true);
            $("#btnSQMPlantdialogCancelEdit").button("option", "disabled", false);
            $("#SQMPlantdialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('SID', dataRow.SID);

            $("#txtProvider").val(dataRow.Provider);
            $("#txtProvider").attr("disabled", "disabled");
            $("#txtVendorCode").val(dataRow.Vendor);
            $("#txtVendorCode").attr("disabled", "disabled");
            $("#ddlJob").val(dataRow.jobID)
            $("#ddlJob").removeAttr('disabled');
            $("#txtName").val(dataRow.Name);
            $("#txtName").removeAttr('disabled');
            $("#txtPhone").val(dataRow.Phone);
            $("#txtPhone").removeAttr('disabled');
            $("#txtFixedTelephone").val(dataRow.FixedTelephone);
            $("#txtFixedTelephone").removeAttr('disabled');
            $("#txtEmail").val(dataRow.Email);
            $("#txtEmail").removeAttr('disabled');

            $("#SQMPlantlblDiaErrMsg").html("");

            break;
    }
}