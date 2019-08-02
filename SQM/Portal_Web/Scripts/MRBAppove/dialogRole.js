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
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/MRBAppove/Create" : "/MRBAppove/Edit"),
                        data: {
                            "SID": dialog.attr("SID"),
                     
                            "DepartmentType": escape($.trim($("#txtDepartmentType").val())),
                         
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialog.attr('Mode') == "c")
                                    alert("Contact create successfully.");
                                else
                                    alert("Contact edit successfully.");
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

    var Mapdialog = $("#MapdialogData");

    //Toolbar Buttons
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
                        url: __WebAppPathPrefix + ((Mapdialog.attr('Mode') == "c") ? "/MRBAppove/CreateMap" : "/MRBAppove/EditMap"),
                        data: {
                            "SID": Mapdialog.attr("SID"),
                            "SSID": Mapdialog.attr("SSID"),
                            "Name": escape($.trim($("#txtName").val())),
                            "Email": escape($.trim($("#txtEmail").val()))
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (Mapdialog.attr('Mode') == "c")
                                    alert("Map create successfully.");
                                else
                                    alert("Map edit successfully.");
                            }
                            else {
                                if ((Mapdialog.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#MaplblDiaErrMsg").html(data);
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
                        $("#btnMapSearch").click();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (Mapdialog.attr('Mode') == "e") {
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

        
            $("#txtDepartmentType").val("");
            $("#txtDepartmentType").removeAttr('disabled');


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

           
        
            $("#txtDepartmentType").val(dataRow.DepartmentType);
            $("#txtDepartmentType").attr("disabled", "disabled");
  

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

            $("#txtDepartmentType").val(dataRow.DepartmentType);
            $("#txtDepartmentType").removeAttr('disabled');
      

            $("#lblDiaErrMsg").html("");

            break;
    }
}

function MapDialogSetUIByMode(Mode) {
    var dialog = $("#MapdialogData");
    var gridDataList = $("#MapgridDataList");
    switch (Mode) {
        case "c": //Create
            $("#MapdialogDataToolBar").hide();
            dialog.attr('ItemRowId', "");
         


            $("#txtName").val("");
            $("#txtName").removeAttr('disabled');
            $("#txtEmail").val("");
            $("#txtEmail").removeAttr('disabled');
         
            $("#MaplblDiaErrMsg").html("");

            break;
        case "v": //View
            $("#btnMapdialogEditData").button("option", "disabled", false);
            $("#btnMapdialogCancelEdit").button("option", "disabled", true);
            $("#MapdialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('SID', dataRow.SID);



            $("#txtName").val(dataRow.Name);
            $("#txtName").attr("disabled", "disabled");
            $("#txtEmail").val(dataRow.Email);
            $("#txtEmail").attr("disabled", "disabled");
       
            $("#MaplblDiaErrMsg").html("");

            break;
        default: //Edit("e")
            $("#btnMapdialogEditData").button("option", "disabled", true);
            $("#btnMapdialogCancelEdit").button("option", "disabled", false);
            $("#MapdialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('SID', dataRow.SID);

            $("#txtName").val(dataRow.Name);
            $("#txtName").removeAttr('disabled');
            $("#txtEmail").val(dataRow.Email);
            $("#txtEmail").removeAttr('disabled');
        

            $("#MaplblDiaErrMsg").html("");

            break;
    }
}