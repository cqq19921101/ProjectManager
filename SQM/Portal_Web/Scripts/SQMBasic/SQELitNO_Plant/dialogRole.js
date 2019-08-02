//initial dialog
$(function () {
    var dialog = $("#dialogData");
    $('#ddlLitNo').change(function () {
        $.ajax({
            url: __WebAppPathPrefix + '/SQMBasic/GetLitNoUnit',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
            data: { LitNo: $('#ddlLitNo').val() },
            type: "post",
            dataType: 'json',
            async: false,
            success: function (data) {
                //var options = '<option value=-1 Selected>All</option>';

                for (var idx in data) {
                    $('#txtStandard1').val(data[idx].Unit1);
                    $('#txtStandard2').val(data[idx].Unit2);

                }


            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    });
    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetLitNo',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].LitNo + '>' + data[idx].LitNo + '</option>';
            }
            $('#ddlLitNo').append(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
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
            $('#ddlTB_SQM_AQL_PLANT_SID').append(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
    $("#dialogData").dialog({
        autoOpen: false,
        height: 600,
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
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMBasic/CreateLitNO_Plant" : "/SQMBasic/EditLitNO_Plant"),
                        data: {
                            "SID": dialog.attr("SID"),
                            "LitNo": escape($.trim($("#ddlLitNo").val())),
                            "Standard1": escape($.trim($("#txtStandard1").val())),
                            "Standard2": escape($.trim($("#txtStandard2").val())),
                            "Type": escape($.trim($("#txtType").val())),
                            "TB_SQM_AQL_PLANT_SID": escape($.trim($("#ddlTB_SQM_AQL_PLANT_SID").val())),
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
            Cancel: function () {
                $(this).dialog("close");
                $("#dialogData").attr("DesignChangeFGUID", "");
                $("#dialogData").attr("ProposedChangeFGUID", "");
            }
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
    var dialog = $("#dialogData");
    var gridDataList = $("#gridDataList");
    switch (Mode) {
        case "c": //Create
            $("#dialogDataToolBar").hide();

            dialog.attr('ItemRowId', "");
            dialog.attr('SID', "");

            $("#ddlLitNo option:first").prop("selected", 'selected').change();
            $("#ddlLitNo").removeAttr('disabled');
            $("#txtStandard1").val("");
            $("#txtStandard1").removeAttr('disabled');
            $("#txtStandard2").val("");
            $("#txtStandard2").removeAttr('disabled');
            $("#txtType").val("");
            $("#txtType").removeAttr('disabled');
            $("#ddlTB_SQM_AQL_PLANT_SID option:first").prop("selected", 'selected').change();
            $("#ddlTB_SQM_AQL_PLANT_SID").removeAttr('disabled');

            $("#lblDiaErrMsg").html("");

            $("#fileUploadProposedChange").show();
            break;
        case "v": //View
            $("#btndialogEditData").button("option", "disabled", false);
            $("#btndialogCancelEdit").button("option", "disabled", 'Yes');
            $("#dialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('SID', dataRow.SID);

            $("#ddlLitNo").val(dataRow.LitNo);
            $("#ddlLitNo").attr("disabled", "disabled");
            $("#txtStandard1").val(dataRow.Standard1);
            $("#txtStandard1").attr("disabled", "disabled");
            $("#txtStandard2").val(dataRow.Standard2);
            $("#txtStandard2").attr("disabled", "disabled");
            $("#txtType").val(dataRow.Type);
            $("#txtType").attr("disabled", "disabled");
            $("#ddlTB_SQM_AQL_PLANT_SID").val(dataRow.TB_SQM_AQL_PLANT_SID);
            $("#ddlTB_SQM_AQL_PLANT_SID").attr("disabled", "disabled");

            $("#lblDiaErrMsg").html("");

            $("#fileUploadProposedChange").hide();
            $("#fileUploadDesignChange").hide();
            break;
        default: //Edit("e")
            $("#btndialogEditData").button("option", "disabled", 'Yes');
            $("#btndialogCancelEdit").button("option", "disabled", false);
            $("#dialogDataToolBar").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('SID', dataRow.SID);

            $("#ddlLitNo").val(dataRow.LitNo);
            $("#ddlLitNo").attr("disabled", "disabled");
            $("#txtStandard1").val(dataRow.Standard1);
            $("#txtStandard1").removeAttr('disabled');
            $("#txtStandard2").val(dataRow.Standard2);
            $("#txtStandard2").removeAttr('disabled');
            $("#txtType").val(dataRow.Type);
            $("#txtType").removeAttr('disabled');
            $("#ddlTB_SQM_AQL_PLANT_SID").val(dataRow.TB_SQM_AQL_PLANT_SID);
            $("#ddlTB_SQM_AQL_PLANT_SID").removeAttr('disabled');

            $("#lblDiaErrMsg").html("");

            $("#fileUploadProposedChange").show();
            break;
    }
}