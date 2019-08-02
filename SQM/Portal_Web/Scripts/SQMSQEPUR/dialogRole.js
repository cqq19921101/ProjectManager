//initial dialog
$(function () {
    var dialog = $("#SQMSQEPURdialogData");
    
    //Toolbar Buttons
    $("#btnSQMSQEPURdialogEditData").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnSQMSQEPURdialogCancelEdit").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#SQMSQEPURdialogData").dialog({
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
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMSQEPUR/CreateSQMSQEPUR" : "/SQMSQEPUR/EditSQMSQEPUR"),
                        data: {
                            "PlantCode": escape($.trim($("#txtPlant").val())),
                            'VendorCode': escape($.trim($("#txtVendorCode").val())),
                            "SourcerGUID": escape($.trim($("#txtSource").val())),
                            "SQEGUID": escape($.trim($("#txtMember").val())),
                            "RDGUID": escape($.trim($("#txtRD").val())),
                            "RDSGUID": escape($.trim($("#txtRDS").val())),
                            "UpdateDatetime": "",
                            "UpdateUser": "",
                          
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
                                    $("#SQMSQEPURlblDiaErrMsg").html(data);
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
                        $("#btnSQMSQEPURSearch").click();
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
function SQMSQEDialogSetUIByMode(Mode) {
    var dialog = $("#SQMSQEPURdialogData");
    var gridDataList = $("#SQMSQEPURgridDataList");
    switch (Mode) {
        case "c": //Create
           

            $("#txtPlant").val("");
            $("#txtPlant").removeAttr('disabled');
            $("#txtVendorCode").val("");
            $("#txtVendorCode").removeAttr('disabled');
            $("#txtSource").val("");
            $("#txtSource").removeAttr('disabled');
            $("#txtMember").val("");
            $("#txtMember").removeAttr('disabled');
            $("#txtRD").val("");
            $("#txtRD").removeAttr('disabled');
            $("#txtRDS").val("");
            $("#txtRDS").removeAttr('disabled');
            $("#txtSourceName").val("");
            $("#txtSourceName").removeAttr('disabled');
            $("#txtMemberName").val("");
            $("#txtMemberName").removeAttr('disabled');
            $("#txtRDName").val("");
            $("#txtRDName").removeAttr('disabled');
            $("#txtRDSName").val("");
            $("#txtRDSName").removeAttr('disabled');
  

            $("#SQMSQEPURlblDiaErrMsg").html("");

            break;
        case "v": //View
            $("#btnSQMSQEPURdialogEditData").button("option", "disabled", false);
            $("#btnSQMSQEPURdialogCancelEdit").button("option", "disabled", true);
            $("#SQMSQEPURdialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            //dialog.attr('SID', dataRow.SID);

            $("#txtPlant").val(dataRow.PlantCode);
            $("#txtPlant").removeAttr('disabled');
            $("#txtVendorCode").val(dataRow.VendorCode);
            $("#txtVendorCode").removeAttr('disabled');
            $("#txtSource").val(dataRow.SourcerGUID);
            $("#txtSource").removeAttr('disabled');
            $("#txtMember").val(dataRow.SQEGUID);
            $("#txtMember").removeAttr('disabled');
            $("#txtRD").val(dataRow.RDGUID);
            $("#txtRD").removeAttr('disabled');
            $("#txtRDS").val(dataRow.RDSGUID);
            $("#txtRDS").removeAttr('disabled');
            $("#txtSourceName").val(dataRow.SourcerName);
            $("#txtSourceName").removeAttr('disabled');
            $("#txtMemberName").val(dataRow.SQENAME);
            $("#txtMemberName").removeAttr('disabled');
            $("#txtRDName").val(dataRow.RDNAME);
            $("#txtRDName").removeAttr('disabled');
            $("#txtRDSName").val(dataRow.RDSNAME);
            $("#txtRDSName").removeAttr('disabled');


            //$("#txtProvider").val(dataRow.Provider);
            //$("#txtProvider").attr("disabled", "disabled");
            //$("#txtVendorCode").val(dataRow.Vendor);
            //$("#txtVendorCode").attr("disabled", "disabled");
            //$("#SQMSQEPURlblDiaErrMsg").html("");

            break;
        default: //Edit("e")
            $("#btnSQMSQEPURdialogEditData").button("option", "disabled", true);
            $("#btnSQMSQEPURdialogCancelEdit").button("option", "disabled", false);
            $("#SQMSQEPURdialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            //dialog.attr('SID', dataRow.SID);
            $("#txtPlant").val(dataRow.PlantCode);
            $("#txtPlant").attr("disabled", "disabled");
            $("#txtVendorCode").val(dataRow.VendorCode);
            $("#txtVendorCode").attr("disabled", "disabled");
            $("#txtSource").val(dataRow.SourcerGUID);
            //$("#txtSource").attr("disabled", "disabled");
            $("#txtMember").val(dataRow.SQEGUID);
            //$("#txtMember").attr("disabled", "disabled");
            $("#txtRD").val(dataRow.RDGUID);
            //$("#txtRD").removeAttr('disabled');
            $("#txtRDS").val(dataRow.RDSGUID);
            //$("#txtRDS").removeAttr('disabled');
            $("#txtSourceName").val(dataRow.SourcerName);
            
            $("#txtMemberName").val(dataRow.SQENAME);
         
            $("#txtRDName").val(dataRow.RDNAME);

            $("#txtRDSName").val(dataRow.RDSNAME);
            

            $("#SQMSQEPURlblDiaErrMsg").html("");

            break;
    }
}