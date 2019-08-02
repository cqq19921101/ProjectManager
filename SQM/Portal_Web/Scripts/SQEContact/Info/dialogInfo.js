//initial dialog
$(function () {
    var dialog = $("#SQEContactdialogData");

    //Toolbar Buttons
    $("#btnSQEContactdialogEditData").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnSQEContactdialogCancelEdit").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#SQEContactdialogData").dialog({
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
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQEContact/CreateContact" : "/SQEContact/EditContact"),
                        data: {
                            "MemberGUID": dialog.attr("MemberGUID"),
                            "SID": dialog.attr("SID"),
                            "Provider": escape($.trim($("#txtProvider").val())),
                            "Vendor": escape($.trim($("#txtVendorCode").val())),
                            "job": escape($.trim($("#ddlJob").val())),
                            "Name": escape($.trim($("#txtName").val())),
                            "Phone": escape($.trim($("#txtPhone").val())),
                            "FixedTelephone": escape($.trim($("#txtFixedTelephone").val())),
                            "Email": escape($.trim($("#txtEmail").val()))
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
                                    $("#SQEContactlblDiaErrMsg").html(data);
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
                        $("#btnSQEContactSearch").click();
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
function ContactDialogSetUIByMode(Mode) {
    var dialog = $("#SQEContactdialogData");
    var gridDataList = $("#SQEContactgridDataList");
    switch (Mode) {
        case "c": //Create
            $("#SQEContactdialogDataToolBar").hide();

            dialog.attr('ItemRowId', "");
            dialog.attr('SID', "");

            $("#txtProvider").val("");
            $("#txtProvider").attr("disabled", "disabled");
            $("#txtVendorCode").val("");
            $("#txtVendorCode").removeAttr('disabled');
            $("#ddlJob").val(1);
            $("#ddlJob").removeAttr('disabled');
            $("#txtName").val("");
            $("#txtName").removeAttr('disabled');
            $("#txtPhone").val("");
            $("#txtPhone").removeAttr('disabled');
            $("#txtFixedTelephone").val("");
            $("#txtFixedTelephone").removeAttr('disabled');
            $("#txtEmail").val("");
            $("#txtEmail").removeAttr('disabled');

            $("#SQEContactlblDiaErrMsg").html("");

            break;
        case "v": //View
            $("#btnSQEContactdialogEditData").button("option", "disabled", false);
            $("#btnSQEContactdialogCancelEdit").button("option", "disabled", true);
            $("#SQEContactdialogDataToolBar").show();

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
            $("#txtName").attr("disabled", "disabled");
            $("#txtPhone").val(dataRow.Phone);
            $("#txtPhone").attr("disabled", "disabled");
            $("#txtFixedTelephone").val(dataRow.FixedTelephone);
            $("#txtFixedTelephone").attr("disabled", "disabled");
            $("#txtEmail").val(dataRow.Email);
            $("#txtEmail").attr("disabled", "disabled");

            $("#SQEContactlblDiaErrMsg").html("");

            break;
        default: //Edit("e")
            $("#btnSQEContactdialogEditData").button("option", "disabled", true);
            $("#btnSQEContactdialogCancelEdit").button("option", "disabled", false);
            $("#SQEContactdialogDataToolBar").show();

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

            $("#SQEContactlblDiaErrMsg").html("");

            break;
    }
}