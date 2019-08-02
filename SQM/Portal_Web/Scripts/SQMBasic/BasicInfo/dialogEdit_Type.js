//initial dialog
$(function () {
    var dialog = $("#dialogDataBasicInfoType");

    //Toolbar Buttons
    $("#btndialogEditDataBasicInfoType").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogCancelEditBasicInfoType").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#dialogDataBasicInfoType").dialog({
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
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMBasic/CreateBasicInfoType" : "/SQMPVL/EditPVL"),
                        data: {
                            "BasicInfoGUID": dialog.attr("BasicInfoGUID"),
                            "TB_SQM_Vendor_TypeCID": escape($("#ddlVendorType").val()),
                            "TB_SQM_Commodity_SubTB_SQM_CommodityCID": escape($("#ddlCategory").val()),
                            "TB_SQM_Commodity_SubCID": escape($("#ddlCategorySub").val())
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialog.attr('Mode') == "c")
                                    alert("BasicInfoType create successfully.");
                                else
                                    alert("BasicInfoType edit successfully.");
                            }
                            else {
                                if ((dialog.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblDiaErrMsgBasicInfoType").html(data);
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
                        
                        var gridDataListBasicInfoType = $("#gridDataListBasicInfoType");
                        gridDataListBasicInfoType.jqGrid('clearGridData');

                        gridDataListBasicInfoType.jqGrid('setGridParam', { postData: { SearchText: '' } })
                        gridDataListBasicInfoType.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialog.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialog.attr('BasicInfoGUID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });
});

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByMode_BasicInfoType(Mode) {
    var dialog = $("#dialogDataBasicInfoType");
    var gridDataList = $("#gridDataListBasicInfoType");
    switch (Mode) {
        case "c": //Create
            $("#dialogDataToolBarBasicInfoType").hide();

            dialog.attr('ItemRowId', "");
            dialog.attr('BasicInfoGUID', "");

            $("#ddlVendorType").val("");
            $("#ddlVendorType").removeAttr('disabled');
            $("#ddlCategory").val("");
            $("#ddlCategory").removeAttr('disabled');
            $("#ddlCategorySub").val("");
            $("#ddlCategorySub").removeAttr('disabled');

            $("#lblDiaErrMsgBasicInfoType").html("");

            break;
        case "v": //View
            $("#btndialogEditDataBasicInfoType").button("option", "disabled", false);
            $("#btndialogCancelEditBasicInfoType").button("option", "disabled", true);
            $("#dialogDataToolBarBasicInfoType").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('BasicInfoGUID', dataRow.BasicInfoGUID);


            $("#ddlVendorType").val(dataRow.TB_SQM_Vendor_TypeCID);
            $("#ddlVendorType").attr('disabled', 'disabled');
            $("#ddlCategory").val(dataRow.TB_SQM_Commodity_SubTB_SQM_CommodityCID).change();
            $("#ddlCategory").attr('disabled', 'disabled');
            $("#ddlCategorySub").val(dataRow.TB_SQM_Commodity_SubCID);
            $("#ddlCategorySub").attr('disabled', 'disabled');

            $("#lblDiaErrMsgBasicInfoType").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditDataBasicInfoType").button("option", "disabled", true);
            $("#btndialogCancelEditBasicInfoType").button("option", "disabled", false);
            $("#dialogDataToolBarBasicInfoType").show();

            var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
            var dataRow = gridDataList.jqGrid('getRowData', RowId);
            dialog.attr('ItemRowId', RowId);
            dialog.attr('BasicInfoGUID', dataRow.BasicInfoGUID);

            $("#ddlVendorType").val(dataRow.TB_SQM_Vendor_TypeCID);
            $("#ddlVendorType").removeAttr('disabled');
            $("#ddlCategory").val(dataRow.TB_SQM_Commodity_SubTB_SQM_CommodityCID);
            $("#ddlCategory").removeAttr('disabled');
            $("#ddlCategorySub").val(dataRow.TB_SQM_Commodity_SubCID);
            $("#ddlCategorySub").removeAttr('disabled');

            $("#lblDiaErrMsgBasicInfoType").html("");

            break;
    }
}