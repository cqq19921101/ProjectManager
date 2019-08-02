//initial dialog
$(function () {
    var dialog = $("#dialogSQMCertificationData");
    
    //Toolbar Buttons
    $("#btndialogSQMCertificationEditData").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogSQMCertificationCancelEdit").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#txtCertificationdate").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            } catch (err) {
                $(this).datepicker("setDate", '-31d');
            }
        }
    });
    $("#txtCertificationdate").datepicker("setDate", '-31d');

    $("#txtEnableEndDate").datepicker({
        changeMonth: true,
        dateFormat: 'yy/mm/dd',
        onClose: function (selectedDate) {
            try {
                $.datepicker.parseDate('yy/mm/dd', $(this).val());
            }
            catch (err) {
                $(this).datepicker("setDate", '0d');
            }
        }
    });
    $("#txtEnableEndDate").datepicker("setDate", '0d');

    $("#dialogSQMCertificationData").dialog({
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
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMCertification/CreateMember" : "/SQMCertification/EditMember"),
                        data: {
                            "BasicInfoGUID": $("#gridDataListBasicInfoType").jqGrid('getRowData', $("#gridDataListBasicInfoType").jqGrid('getGridParam', 'selrow')).BasicInfoGUID,
                            "CertificationType": escape($.trim($("#ddlSQMCertificationCName option:selected").val())),
                            "CName": escape($.trim($("#ddlSQMCertificationCName option:selected").text())),
                            "CNameInput": escape($.trim($("#txtSQMCertificationCNameInput").val())),
                            "CertificationAuthority": escape($.trim($("#txtCertificationAuthority").val())),
                            "CertificationNum": escape($.trim($("#txtCertificationNum").val())),
                            "CertificationDate": escape($.trim($("#txtCertificationdate").val())),
                            "ValidDate": escape($.trim($("#txtEnableEndDate").val()))
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
                                    $("#lblDiaErrMsgCertification").html(data);
                            }
                        },
                        error: function (xhr, textStatus, thrownError) {
                            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                        },
                        complete: function (jqXHR, textStatus) {
                        }
                    });
                    if (DoSuccessfully) {
                        $(this).dialog("close");
                        $("#btnSQMCertificationSearch").click();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialog.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialog.attr('SubFuncGUID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });
});

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByMode(Mode) {
    var dialog = $("#dialogSQMCertificationData");
    var gridSQMCertificationDataList = $("#gridSQMCertificationDataList");
    switch (Mode) {
        case "c": //Create
            $("#dialogSQMCertificationDataToolBar").hide();

            dialog.attr('ItemRowId', "");

            $("#ddlSQMCertificationCName").removeAttr('disabled');
            $("#txtCertificationAuthority").removeAttr('disabled');
            $("#txtCertificationNum").removeAttr('disabled');
            $("#txtCertificationdate").removeAttr('disabled');
            $("#txtEnableEndDate").removeAttr('disabled');

            $("#txtCertificationAuthority").val("");
            $("#txtCertificationNum").val("");

            $("#uploadpart").hide();
            //$("#UpDateFile").hide();
            //$("#btnProcessIntro").hide();

            $("#lblDiaErrMsgCertification").html("");

            break;
        case "v": //View
            $("#btndialogSQMCertificationEditData").button("option", "disabled", false);
            $("#btndialogSQMCertificationCancelEdit").button("option", "disabled", true);
            $("#dialogSQMCertificationDataToolBar").show();

            var RowId = gridSQMCertificationDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridSQMCertificationDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);

            $("#ddlSQMCertificationCName").val(dataRow.CertificationType);
            $("#txtSQMCertificationCNameInput").val(dataRow.CName);
            $("#txtCertificationAuthority").val(dataRow.CertificationAuthority);
            $("#txtCertificationNum").val(dataRow.CertificationNum);
            $("#txtCertificationdate").val(dataRow.CertificationDate);
            $("#txtEnableEndDate").val(dataRow.ValidDate);

            $("#ddlSQMCertificationCName").attr("disabled", "disabled");
            $("#txtSQMCertificationCNameInput").hide();
            $("#txtCertificationAuthority").attr("disabled", "disabled");
            $("#txtCertificationNum").attr("disabled", "disabled");
            $("#txtCertificationdate").attr("disabled", "disabled");
            $("#txtEnableEndDate").attr("disabled", "disabled");

            $("#uploadpart").hide();
            //$("#UpDateFile").show();
            //$("#btnProcessIntro").show();

            $("#UpDateFile").attr("disabled", "disabled");
            $("#btnProcessIntro").attr("disabled", "disabled");

            $("#lblDiaErrMsgCertification").html("");

            break;
        default: //Edit("e")
            $("#btndialogSQMCertificationEditData").button("option", "disabled", true);
            $("#btndialogSQMCertificationCancelEdit").button("option", "disabled", false);
            $("#dialogSQMCertificationDataToolBar").show();

            var RowId = gridSQMCertificationDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridSQMCertificationDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);

            $("#ddlSQMCertificationCName").val(dataRow.CertificationType);
            $("#txtSQMCertificationCNameInput").val(dataRow.CName);
            $("#txtCertificationAuthority").val(dataRow.CertificationAuthority);
            $("#txtCertificationNum").val(dataRow.CertificationNum);
            $("#txtCertificationdate").val(dataRow.CertificationDate);
            $("#txtEnableEndDate").val(dataRow.ValidDate);

            $("#txtCertificationAuthority").removeAttr('disabled');
            $("#txtCertificationNum").removeAttr('disabled');
            $("#txtCertificationdate").removeAttr('disabled');
            $("#txtEnableEndDate").removeAttr('disabled');

            $("#uploadpart").show();
            //$("#UpDateFile").show();
            //$("#btnProcessIntro").show();

            $("#UpDateFile").removeAttr('disabled');
            $("#btnProcessIntro").removeAttr('disabled');

            $("#lblDiaErrMsgCertification").html("");

            break;
    }
}

$(function () {
    $('#ddlSQMCertificationCName').change(function () {
        var txtProcess = $("#ddlSQMCertificationCName option:selected").text();
        if(txtProcess == "Other")
        {
            $("#txtSQMCertificationCNameInput").show();
            $("#txtSQMCertificationCNameInput").val("");
        }
        else
        {
            $("#txtSQMCertificationCNameInput").hide();
            $("#txtSQMCertificationCNameInput").val(txtProcess);
        }
    });
});