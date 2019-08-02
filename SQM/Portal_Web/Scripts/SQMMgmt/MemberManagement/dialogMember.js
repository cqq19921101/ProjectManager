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
        width: 510,
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
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SystemMgmt/CreateMember" : "/SystemMgmt/EditMember"),
                        data: {
                            "AccountGUID": dialog.attr("AccountGUID"),
                            "AccountID": escape($.trim($("#txtDiaAccountID").val())),
                            "MemberType": $('input[name=DiaMemberType]:checked').val() == "e" ? "External Member" : "Internal Member",
                            "NameInChinese": escape($.trim($("#txtDiaNameInChinese").val())),
                            "NameInEnglish": escape($.trim($("#txtDiaNameInEnglish").val())),
                            "PrimaryEmail": escape($.trim($("#txtDiaEmail").val()))
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialog.attr('Mode') == "c")
                                    alert("Member create successfully.");
                                else
                                    alert("Member edit successfully.");
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
                var r = ReleaseDataLock(dialog.attr('AccountGUID'));
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
            dialog.attr('AccountGUID', "");

            $("#txtDiaAccountID").val("");
            $("#txtDiaAccountID").removeAttr('disabled');
            $("#radDiaMemberTypeExternal").attr('checked', true);
            $("#radDiaMemberTypeExternal").removeAttr('disabled');
            $("#radDiaMemberTypeInternal").removeAttr('disabled');

            $("#txtDiaNameInChinese").val("");
            $("#txtDiaNameInEnglish").val("");
            $("#txtDiaEmail").val("");

            $("#txtDiaNameInChinese").removeAttr('disabled');
            $("#txtDiaNameInEnglish").removeAttr('disabled');
            $("#txtDiaEmail").removeAttr('disabled');

            $("#lblDiaErrMsg").html("");
            $("#btnDiaReadInternalUserInfo").hide();
            $("#lblDiaErrMsg").html("");

            break;
        case "v": //View
            $("#btndialogEditData").button("option", "disabled", false);
            $("#btndialogCancelEdit").button("option", "disabled", true);
            $("#dialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('AccountGUID', dataRow.AccountGUID);

            $("#txtDiaAccountID").val(dataRow.AccountID);
            $("#txtDiaAccountID").attr("disabled", "disabled");

            $("#radDiaMemberTypeExternal").attr("disabled", "disabled");
            $("#radDiaMemberTypeInternal").attr("disabled", "disabled");
            $("#btnDiaReadInternalUserInfo").hide();
            var CheckedRadioButton;
            if (dataRow.MemberType == "External Member") { CheckedRadioButton = $("#radDiaMemberTypeExternal"); }
            else { CheckedRadioButton = $("#radDiaMemberTypeInternal"); }
            CheckedRadioButton.attr('checked', true);

            $("#txtDiaNameInChinese").attr("disabled", "disabled");
            $("#txtDiaNameInEnglish").attr("disabled", "disabled");
            $("#txtDiaEmail").attr("disabled", "disabled");

            $("#txtDiaNameInChinese").val(dataRow.NameInChinese);
            $("#txtDiaNameInEnglish").val(dataRow.NameInEnglish);
            $("#txtDiaEmail").val(dataRow.PrimaryEmail);
            $("#lblDiaErrMsg").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditData").button("option", "disabled", true);
            $("#btndialogCancelEdit").button("option", "disabled", false);
            $("#dialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('AccountGUID', dataRow.AccountGUID);

            $("#txtDiaAccountID").val(dataRow.AccountID);
            $("#txtDiaAccountID").attr("disabled", "disabled");

            $("#radDiaMemberTypeExternal").removeAttr('disabled');
            $("#radDiaMemberTypeInternal").removeAttr('disabled');

            var CheckedRadioButton;
            if (dataRow.MemberType == "External Member") {
                CheckedRadioButton = $("#radDiaMemberTypeExternal");
                $("#btnDiaReadInternalUserInfo").hide();

                $("#txtDiaNameInChinese").removeAttr('disabled');
                $("#txtDiaNameInEnglish").removeAttr('disabled');
                $("#txtDiaEmail").removeAttr('disabled');
            } else {
                CheckedRadioButton = $("#radDiaMemberTypeInternal");
                $("#btnDiaReadInternalUserInfo").show();

                $("#txtDiaNameInChinese").attr("disabled", "disabled");
                $("#txtDiaNameInEnglish").attr("disabled", "disabled");
                $("#txtDiaEmail").attr("disabled", "disabled");
            }
            CheckedRadioButton.attr('checked', true);

            $("#txtDiaNameInChinese").val(dataRow.NameInChinese);
            $("#txtDiaNameInEnglish").val(dataRow.NameInEnglish);
            $("#txtDiaEmail").val(dataRow.PrimaryEmail);
            $("#lblDiaErrMsg").html("");

            break;
    }
}

$(function () {
    $("input[name='DiaMemberType']").change(function () {
        if ($(this).val() == "i") {
            $("#txtDiaNameInChinese").attr("disabled", "disabled");
            $("#txtDiaNameInEnglish").attr("disabled", "disabled");
            $("#txtDiaEmail").attr("disabled", "disabled");
            $("#txtDiaNameInChinese").val("");
            $("#txtDiaNameInEnglish").val("");
            $("#txtDiaEmail").val("");
            $("#btnDiaReadInternalUserInfo").show();
        }
        else {
            $("#txtDiaNameInChinese").removeAttr('disabled');
            $("#txtDiaNameInEnglish").removeAttr('disabled');
            $("#txtDiaEmail").removeAttr('disabled');
            $("#btnDiaReadInternalUserInfo").hide();
        }
    });
});

$(function () {
    $("#btnDiaReadInternalUserInfo").click(function () {
        var AccountID = $.trim($("#txtDiaAccountID").val());
        $.ajax({
            url: __WebAppPathPrefix + "/SystemMgmt/GetInternalUserInfoByAccountID",
            data: { "AccountID": AccountID },
            type: "post",
            dataType: 'json',
            success: function (data) {
                if (data.ErrMsg == "") {
                    $("#txtDiaNameInChinese").val(data.Member.NameInChinese);
                    $("#txtDiaNameInEnglish").val(data.Member.NameInEnglish);
                    $("#txtDiaEmail").val(data.Member.PrimaryEmail);
                    $("#lblDiaErrMsg").html("");
                }
                else {
                    $("#txtDiaNameInChinese").val("");
                    $("#txtDiaNameInEnglish").val("");
                    $("#txtDiaEmail").val("");
                    $("#lblDiaErrMsg").html(data.ErrMsg);
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                //$("#ajaxLoading").hide();
            }
        });
    });
});