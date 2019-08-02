$(function () {
    var dialogCPK = $("#dialogDataCPK");
    var gridDataListCPK = $("#gridDataListCPK");
    //Toolbar Buttons
    $("#btndialogEditDataCPK").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogCancelEditCPK").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $.ajax({
        url: __WebAppPathPrefix + '/SQMReliability/GetLitNoList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].LitNo + '> ' + data[idx].LitNo + '</option>';
            }
            $('#ddlplantCode1').html(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });

    $("#dialogDataCPK").dialog({
        autoOpen: false,
        height: 400,
        width: 500,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (dialogCPK.attr('Mode') == "v") {
                    $(this).dialog("close");
                }
                else {
                    var DoSuccessfully = false;
                    var RowIdCPK = gridDataListCPK.jqGrid('getGridParam', 'selrow');
                    $.ajax({
                        url: __WebAppPathPrefix + ((dialogCPK.attr('Mode') == "c") ? "/SQMReliability/CreateCPK" : "/SQMReliability/EditCPK"),
                        data: {
                            "reportID":escape(dialogCPK.attr('reportID'))
                            ,"plantCode": escape($.trim($("#ddlplantCode").val()))
                            , "Description": escape($.trim($("#txtDescription").val()))
                            , "ToolNumber": escape($.trim($("#txtToolNumber").val()))
                            , "Inches": escape($.trim($("#txtInches").val()))
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialogCPK.attr('Mode') == "c")
                                    alert("create successfully.");
                                else
                                    alert("edit successfully.");
                            }
                            else {
                                if ((dialogCPK.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblDiaErrMsgCPK").html(data);
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
                        $("#btnSearchCPK").click();
                    }
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialogCPK.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialogCPK.attr('SID'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });

})

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByModeCPK(Mode) {
    var dialogCPK = $("#dialogDataCPK");
    var gridDataListCPK = $("#gridDataListCPK");
    var RowIdCPK = gridDataListCPK.jqGrid('getGridParam', 'selrow');
    var dataRowCPK = gridDataListCPK.jqGrid('getRowData', RowIdCPK);
    switch (Mode) {
        case "c": //Create
            $("#dialogDataToolBarCPK").hide();

            dialogCPK.attr('ItemRowId', "");
            dialogCPK.attr('ItemRowId', "");
            dialogCPK.attr('reportID', "");

            $("#txtreportID").val("");
            $("#txtreportID").attr("disabled", "disabled");
            $("#ddlReportType").val("");
            $("#ddlplantCode").removeAttr('disabled');
            $("#txtSupplier").val(dataRowCPK.ERP_VNAME);
            $("#txtSupplier").attr("disabled", "disabled");
            $("#txtDescription").val("");
            $("#txtDescription").removeAttr('disabled');
            $("#txtToolNumber").val("");
            $("#txtToolNumber").removeAttr('disabled');
            $("#txtInches").val("");
            $("#txtInches").removeAttr('disabled');

            $("#lblDiaErrMsgCPK").html("");

            break;
        case "v": //View
            $("#btndialogEditDataCPK").button("option", "disabled", false);
            $("#btndialogCancelEditCPK").button("option", "disabled", true);
            $("#dialogDataToolBarCPK").show();

            dialogCPK.attr('ItemRowId', RowIdCPK);
            dialogCPK.attr('reportID', dataRowCPK.reportID);

            $("#txtreportID").val(dataRowCPK.reportID);
            $("#txtreportID").attr("disabled", "disabled");
            $("#ddlplantCode").val(dataRowCPK.plantCode);
            $("#ddlplantCode").attr("disabled", "disabled");
            $("#txtSupplier").val(dataRowCPK.ERP_VNAME);
            $("#txtSupplier").attr("disabled", "disabled");
            $("#txtDescription").val(dataRowCPK.Description);
            $("#txtDescription").attr("disabled", "disabled");
            $("#txtToolNumber").val(dataRowCPK.ToolNumber);
            $("#txtToolNumber").attr("disabled", "disabled");
            $("#txtInches").val(dataRowCPK.Inches);
            $("#txtInches").attr("disabled", "disabled");
            
            $("#lblDiaErrMsgCPK").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditDataCPK").button("option", "disabled", true);
            $("#btndialogCancelEditCPK").button("option", "disabled", false);
            $("#dialogDataToolBarCPK").show();

            dialogCPK.attr('ItemRowId', RowIdCPK);
            dialogCPK.attr('reportID', dataRowCPK.reportID);

            $("#txtreportID").val(dataRowCPK.reportID);
            $("#txtreportID").attr("disabled", "disabled");
            $("#ddlplantCode").val(dataRowCPK.plantCode);
            $("#ddlplantCode").attr("disabled", "disabled");
            $("#txtSupplier").val(dataRowCPK.ERP_VNAME);
            $("#txtSupplier").attr("disabled", "disabled");
            $("#txtDescription").val(dataRowCPK.Description);
            $("#txtDescription").removeAttr('disabled');
            $("#txtToolNumber").val(dataRowCPK.ToolNumber);
            $("#txtToolNumber").removeAttr('disabled');
            $("#txtInches").val(dataRowCPK.Inches);
            $("#txtInches").removeAttr('disabled');

            $("#lblDiaErrMsgCPK").html("");

            break;
    }
}