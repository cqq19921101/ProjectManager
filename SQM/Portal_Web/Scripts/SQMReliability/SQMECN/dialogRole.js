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
    $("#txtEnvironment").change(function () {
        if (this.value == "1") {
            $("#fileUploadDesignChange").show();
        } else {
            $("#fileUploadDesignChange").hide();
        }
    });
    //txtChangeType
    $.ajax({
        url: __WebAppPathPrefix + '/SQMReliability/GetChangeTypeList',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].SID + '>' +  data[idx].ChangeName + '</option>';
            }
            $('#txtChangeType').append(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
    //txtChangeItemType
    $.ajax({
        url: __WebAppPathPrefix + '/SQMReliability/GetChangeItemTypeList',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].SID + '>' + data[idx].ChangeItem + '</option>';
            }
            $('#txtChangeItemType').append(options);
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
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMReliability/CreateECN" : "/SQMReliability/EditECN"),
                        data: {
                            "SID": dialog.attr("SID"),
                            "Supplier": escape($.trim($("#txtSupplier").val())),
                            "Phone": escape($.trim($("#txtPhone").val())),
                            "OriginatorName": escape($.trim($("#txtOriginatorName").val())),
                            "Spec": escape($.trim($("#txtSpec").val())),
                            "Description": escape($.trim($("#txtDescription").val())),
                            "ChangeItemType": escape($.trim($("#txtChangeItemType").val())),
                            "ChangeType": escape($.trim($("#txtChangeType").val())),
                            "ProposedChange": escape($.trim($("#txtProposedChange").val())),
                            "ProposedDate": escape($.trim($("#txtProposedDate").val())),
                            "ChangeReason": escape($.trim($("#txtChangeReason").val())),
                            "DesignChange": escape($.trim($("#txtDesignChange").val())),
                            "Consume": escape(($('#ckbConsume').prop('checked')) ? 1 : 0),
                            "Scrap": escape(($('#ckbScrap').prop('checked')) ? 1 : 0),
                            "Rework": escape(($('#ckbRework').prop('checked')) ? 1 : 0),
                            "Sort": escape(($('#ckbSort').prop('checked')) ? 1 : 0),
                            "WIP": escape($.trim($("#txtWIP").val())),
                            "QtyInStock": escape($.trim($("#txtQtyInStock").val())),
                            "Environment": escape($.trim($("#txtEnvironment").val())),
                            "PPAP": escape($.trim($("#txtPPAP").val())),
                            "SupplierApprovalFGUID": escape($.trim($("#dialogData").attr("SupplierApprovalFGUID"))),
                            "Title": escape($.trim($("#txtTitle").val())),
                            "RequestDate": escape($.trim($("#txtRequestDate").val()))
                            ,"DesignChangeFGUID":escape($.trim($("#dialogData").attr("DesignChangeFGUID")))
                            ,"ProposedChangeFGUID":escape($.trim($("#dialogData").attr("ProposedChangeFGUID")))
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialog.attr('Mode') == "c")
                                    alert("ECN create successfully.");
                                else
                                    alert("ECN edit successfully.");
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

            $("#txtSupplier").val("");
            $("#txtSupplier").attr("disabled", "disabled");
            $("#txtPhone").val("");
            $("#txtPhone").attr("disabled", "disabled");
            $("#txtOriginatorName").val("");
            $("#txtOriginatorName").attr("disabled", "disabled");
            $("#txtSpec").val("");
            $("#txtSpec").removeAttr('disabled');
            $("#txtDescription").val("");
            $("#txtDescription").removeAttr('disabled');
            $("#txtChangeItemType option:first").prop("selected", 'selected');
            $("#txtChangeItemType").removeAttr('disabled');
            $("#txtChangeType option:first").prop("selected", 'selected');
            $("#txtChangeType").removeAttr('disabled');
            $("#txtProposedChange").val("");
            $("#txtProposedChange").removeAttr('disabled');
            $("#txtProposedDate").val("");
            $("#txtProposedDate").removeAttr('disabled');
            $("#txtChangeReason").val("");
            $("#txtChangeReason").removeAttr('disabled');
            $("#txtDesignChange option:first").prop("selected", 'selected');
            $("#txtDesignChange").removeAttr('disabled');
            $("#ckbConsume").prop('checked', false);
            $("#ckbConsume").removeAttr('disabled');
            $("#ckbScrap").prop('checked', false);
            $("#ckbScrap").removeAttr('disabled');
            $("#ckbRework").prop('checked', false);
            $("#ckbRework").removeAttr('disabled');
            $("#ckbSort").prop('checked', false);
            $("#ckbSort").removeAttr('disabled');
            $("#txtWIP").val("");
            $("#txtWIP").removeAttr('disabled');
            $("#txtQtyInStock").val("");
            $("#txtQtyInStock").removeAttr('disabled');
            $("#txtEnvironment option:first").prop("selected", 'selected').change();
            $("#txtEnvironment").removeAttr('disabled');
            $("#txtPPAP option:first").prop("selected", 'selected');
            $("#txtPPAP").removeAttr('disabled');
            $("#txtTitle").val("");
            $("#txtTitle").removeAttr('disabled');
            $("#txtRequestDate").val("");
            $("#txtRequestDate").removeAttr('disabled');

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

            $("#txtSupplier").val(dataRow.Supplier);
            $("#txtSupplier").attr("disabled", "disabled");
            $("#txtPhone").val(dataRow.Phone);
            $("#txtPhone").attr("disabled", "disabled");
            $("#txtOriginatorName").val(dataRow.OriginatorName);
            $("#txtOriginatorName").attr("disabled", "disabled");
            $("#txtSpec").val(dataRow.Spec);
            $("#txtSpec").attr("disabled", "disabled");
            $("#txtDescription").val(dataRow.Description);
            $("#txtDescription").attr("disabled", "disabled");
            $("#txtChangeItemType").val(dataRow.ChangeItemTypeSID);
            $("#txtChangeItemType").attr("disabled", "disabled");
            $("#txtChangeType").val(dataRow.ChangeTypeSID);
            $("#txtChangeType").attr("disabled", "disabled");
            $("#txtProposedChange").val(dataRow.ProposedChange);
            $("#txtProposedChange").attr("disabled", "disabled");
            $("#txtProposedDate").val(dataRow.ProposedDate);
            $("#txtProposedDate").attr("disabled", "disabled");
            $("#txtChangeReason").val(dataRow.ChangeReason);
            $("#txtChangeReason").attr("disabled", "disabled");
            $("#txtDesignChange").val(dataRow.DesignChange == 'Yes' ? 1 : 0);
            $("#txtDesignChange").attr("disabled", "disabled");
            $("#ckbConsume").prop('checked', dataRow.Consume == "true" ? true : false);
            $("#ckbConsume").attr("disabled", "disabled");
            $("#ckbScrap").prop('checked', dataRow.Scrap == "true" ? true : false);
            $("#ckbScrap").attr("disabled", "disabled");
            $("#ckbRework").prop('checked', dataRow.Rework == "true" ? true : false);
            $("#ckbRework").attr("disabled", "disabled");
            $("#ckbSort").prop('checked', dataRow.Sort == "true" ? true : false);
            $("#ckbSort").attr("disabled", "disabled");
            $("#txtWIP").val(dataRow.WIP);
            $("#txtWIP").attr("disabled", "disabled");
            $("#txtQtyInStock").val(dataRow.QtyInStock);
            $("#txtQtyInStock").attr("disabled", "disabled");
            $("#txtEnvironment").val(dataRow.Environment == 'Yes' ? 1 : 0).change();
            $("#txtEnvironment").attr("disabled", "disabled");
            $("#txtPPAP").val(dataRow.PPAP == 'Yes' ? 1 : 0);
            $("#txtPPAP").attr("disabled", "disabled");
            $("#txtTitle").val(dataRow.Title);
            $("#txtTitle").attr("disabled", "disabled");
            $("#txtRequestDate").val(dataRow.RequestDate);
            $("#txtRequestDate").attr("disabled", "disabled");
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

            $("#txtSupplier").val(dataRow.Supplier);
            $("#txtSupplier").attr("disabled", "disabled");
            $("#txtPhone").val(dataRow.Phone);
            $("#txtPhone").attr("disabled", "disabled");
            $("#txtOriginatorName").val(dataRow.OriginatorName);
            $("#txtOriginatorName").attr("disabled", "disabled");
            $("#txtSpec").val(dataRow.Spec);
            $("#txtSpec").removeAttr('disabled');
            $("#txtDescription").val(dataRow.Description);
            $("#txtDescription").removeAttr('disabled');
            $("#txtChangeItemType").val(dataRow.ChangeItemTypeSID);
            $("#txtChangeItemType").removeAttr('disabled');
            $("#txtChangeType").val(dataRow.ChangeTypeSID);
            $("#txtChangeType").removeAttr('disabled');
            $("#txtProposedChange").val(dataRow.ProposedChange);
            $("#txtProposedChange").removeAttr('disabled');
            $("#txtProposedDate").val(dataRow.ProposedDate);
            $("#txtProposedDate").removeAttr('disabled');
            $("#txtChangeReason").val(dataRow.ChangeReason);
            $("#txtChangeReason").removeAttr('disabled');
            $("#txtDesignChange").val(dataRow.DesignChange == 'Yes' ? 1 : 0);
            $("#txtDesignChange").removeAttr('disabled');
            $("#ckbConsume").prop('checked', dataRow.Consume == "true" ? true : false);
            $("#ckbConsume").removeAttr('disabled');
            $("#ckbScrap").prop('checked', dataRow.Scrap == "true" ? true : false);
            $("#ckbScrap").removeAttr('disabled');
            $("#ckbRework").prop('checked', dataRow.Rework == "true" ? true : false);
            $("#ckbRework").removeAttr('disabled');
            $("#ckbSort").prop('checked', dataRow.Sort == "true" ? true : false);
            $("#ckbSort").removeAttr('disabled');
            $("#txtWIP").val(dataRow.WIP);
            $("#txtWIP").removeAttr('disabled');
            $("#txtQtyInStock").val(dataRow.QtyInStock);
            $("#txtQtyInStock").removeAttr('disabled');
            $("#txtEnvironment").val(dataRow.Environment == 'Yes' ? 1 : 0).change();
            $("#txtEnvironment").removeAttr('disabled');
            $("#txtPPAP").val(dataRow.PPAP == 'Yes' ? 1 : 0);
            $("#txtPPAP").removeAttr('disabled');
            $("#txtTitle").val(dataRow.Title);
            $("#txtTitle").removeAttr('disabled');
            $("#txtRequestDate").val(dataRow.RequestDate);
            $("#txtRequestDate").removeAttr('disabled');

            $("#lblDiaErrMsg").html("");

            $("#fileUploadProposedChange").show();
            break;
    }
}