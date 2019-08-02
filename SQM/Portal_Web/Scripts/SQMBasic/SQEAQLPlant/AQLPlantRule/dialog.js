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

    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetPlantNameList',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].SID + '>' + data[idx].PlantName + '</option>';
            }
            $('#ddlPlantName').append(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });

    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetPlantNameList',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].SID + '>' + data[idx].PlantName + '</option>';
            }
            $('#ddlAfterPlantName').append(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });

    $("#ddlPlantName").change(function () {
        $.ajax({
            url: __WebAppPathPrefix + '/SQMBasic/GetPlantMapList',
            data: { MainID: $('#ddlPlantName').val() },
            type: "post",
            dataType: 'json',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                var options = '';
                for (var idx in data) {
                    options += '<option value=' + data[idx].AQLType + '>' + GiveValue(data[idx].AQLType) + '</option>';
                }
                $('#ddlAQLType').html(options);
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    })
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
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMBasic/CreateAQLPlantRule" : "/SQMBasic/EditAQLPlantRule"),
                        data: {
                            "SID": gridDataList.jqGrid('getRowData', RowId).SID
                            , "SSID": escape($.trim($("#ddlPlantName").val()))
                            , "SSSID": escape($.trim($("#ddlAQLType").val()))
                            , "PlantName": escape($.trim($("#txtPlantName").val()))
                            , "CheckNum": escape($.trim($("#txtCheckNum").val()))
                            , "RetreatingNum": escape($.trim($("#txtRetreatingNum").val()))
                            , "AcceptanceNum": escape($.trim($("#txtAcceptanceNum").val()))
                            , "AQLType": escape($.trim($("#ddlAfterAQLType").val()))
                            , "PlantSID": escape($.trim($("#ddlAfterPlantName").val()))
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

            $("#ddlPlantName  option:first").prop("selected", 'selected').change();
            $("#ddlPlantName").removeAttr('disabled');
            $("#ddlAQLType  option:first").prop("selected", 'selected').change();
            $("#ddlAQLType").removeAttr('disabled');
            $("#txtCheckNum").val("");
            $("#txtCheckNum").removeAttr('disabled');
            $("#txtRetreatingNum").val("");
            $("#txtRetreatingNum").removeAttr('disabled');
            $("#txtAcceptanceNum").val("");
            $("#txtAcceptanceNum").removeAttr('disabled');
            $("#ddlAfterAQLType option:first").prop("selected", 'selected');
            $("#ddlAfterAQLType").removeAttr('disabled');
            $("#ddlAfterPlantName option:first").prop("selected", 'selected');
            $("#ddlAfterPlantName").removeAttr('disabled');


            $("#lblDiaErrMsg").html("");

            break;
        case "v": //View
            $("#btndialogEditData").button("option", "disabled", false);
            $("#btndialogCancelEdit").button("option", "disabled", true);
            $("#dialogDataToolBar").show();

            dialog.attr('ItemRowId', RowId);
            dialog.attr('SID', dataRow.SID);

            $("#ddlPlantName").val(dataRow.SSID).change();
            $("#ddlPlantName").attr("disabled", "disabled");
            $("#ddlAQLType").val(dataRow.SSSID);
            $("#ddlAQLType").attr("disabled", "disabled");
            $("#txtCheckNum").val(dataRow.CheckNum);
            $("#txtCheckNum").attr("disabled", "disabled");
            $("#txtRetreatingNum").val(dataRow.RetreatingNum);
            $("#txtRetreatingNum").attr("disabled", "disabled");
            $("#txtAcceptanceNum").val(dataRow.AcceptanceNum);
            $("#txtAcceptanceNum").attr("disabled", "disabled");
            $("#ddlAfterAQLType").val(GetValue(dataRow.AfterAQLType));
            $("#ddlAfterAQLType").attr("disabled", "disabled");
            $("#ddlAfterPlantName").val(dataRow.PlantSID);
            $("#ddlAfterPlantName").attr("disabled", "disabled");

            $("#lblDiaErrMsg").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditData").button("option", "disabled", true);
            $("#btndialogCancelEdit").button("option", "disabled", false);
            $("#dialogDataToolBar").show();

            dialog.attr('ItemRowId', RowId);
            dialog.attr('SID', dataRow.SID);

            $("#ddlPlantName").val(dataRow.SSID).change();
            $("#ddlPlantName").removeAttr('disabled');
            $("#ddlAQLType").val(dataRow.SSSID);
            $("#ddlAQLType").removeAttr('disabled');
            $("#txtCheckNum").val(dataRow.CheckNum);
            $("#txtCheckNum").removeAttr('disabled');
            $("#txtRetreatingNum").val(dataRow.RetreatingNum);
            $("#txtRetreatingNum").removeAttr('disabled');
            $("#txtAcceptanceNum").val(dataRow.AcceptanceNum);
            $("#txtAcceptanceNum").removeAttr('disabled');
            $("#ddlAfterAQLType").val(GetValue(dataRow.AfterAQLType));
            $("#ddlAfterAQLType").removeAttr('disabled');
            $("#ddlAfterPlantName").val(dataRow.PlantSID);
            $("#ddlAfterPlantName").removeAttr('disabled');

            $("#lblDiaErrMsg").html("");

            break;
    }
}

function GiveValue(sid) {
    switch (sid) {
        case 0:
            return "正常";
            break;
        case 1:
            return "加嚴";
            break;
        case 2:
            return "減量";
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