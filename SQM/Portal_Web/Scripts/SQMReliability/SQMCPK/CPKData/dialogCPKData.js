$(function () {
    var dialogCPKData = $("#dialogDataCPKData");
    var gridDataListCPKData = $("#gridDataListCPKData");
    //Toolbar Buttons
    $("#btndialogEditDataCPKData").button({
        label: "Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btndialogCancelEditCPKData").button({
        label: "Cancel",
        icons: { primary: "ui-icon-close" }
    });

    $("#dialogDataCPKData").dialog({
        autoOpen: false,
        height: 400,
        width: 500,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                if (dialogCPKData.attr('Mode') == "v") {
                    $(this).dialog("close");
                }
                else {
                    var DoSuccessfully = false;
                    //var RowIdCPKData = gridDataListCPKData.jqGrid('getGridParam', 'selrow');
                    //var dataRowCPKData = gridDataListCPKData.jqGrid('getRowData', RowIdCPKData);
                    $.ajax({
                        url: __WebAppPathPrefix + ((dialogCPKData.attr('Mode') == "c") ? "/SQMReliability/CreateCPKData" : "/SQMReliability/EditCPKData"),
                        data: {
                            "reportID": $("#dialogDataCPKSub").attr('reportID')
                            , "Designator": $("#dialogDataCPKSub").attr('Designator')
                            , "CTQ": escape($.trim($("#txtCTQ").val()))
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                //if (dialogCPKData.attr('Mode') == "c")
                                //    alert("create successfully.");
                                //else
                                //    alert("edit successfully.");
                            }
                            else {
                                if ((dialogCPKData.attr('Mode') != "c") && (data == __LockIsNotValid)) {
                                    alert("Edit time too long, abort current editing.\n\n(Please restart editing if you wish to do it again)");
                                    DoSuccessfully = true;
                                }
                                else
                                    $("#lblDiaErrMsgCPKData").html(data);
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
                        $("#btnSearchCPKData").click();
                        ($("#spCTQNum").html() > 1 ) ?  $("#btnCreateCPKData").click():alert("數據鍵入完成") ;
                       
                        }
                    
                }
            },
            Cancel: function () { $(this).dialog("close"); }
        },
        close: function () {
            if (dialogCPKData.attr('Mode') == "e") {
                var r = ReleaseDataLock(dialogCPKData.attr('plantCode'));
                if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
            }
        }
    });

})

//change dialog UI
// c: Create, v: View, e: Edit
function DialogSetUIByModeCPKData(Mode) {
    var dialogCPKData = $("#dialogCPKDataCPKData");
    var gridDataListCPKData = $("#gridDataListCPKData");
    var RowIdCPKData = gridDataListCPKData.jqGrid('getGridParam', 'selrow');
    var dataRowCPKData = gridDataListCPKData.jqGrid('getRowData', RowIdCPKData);
    switch (Mode) {
        case "c": //Create
            $("#dialogDataToolBarCPKData").hide();

            dialogCPKData.attr('ItemRowId', $("#dialogDataCPKSub").attr('reportID'));
            dialogCPKData.attr('plantCode', $("#dialogDataCPKSub").attr('Designator'));

            $("#txtCTQ").val("");
            $("#txtCTQ").removeAttr('disabled');

            $("#lblDiaErrMsgCPKData").html("");

            break;
        case "v": //View
            $("#btndialogEditDataCPKData").button("option", "disabled", false);
            $("#btndialogCancelEditCPKData").button("option", "disabled", true);
            $("#dialogDataToolBarCPKData").show();

            dialogCPKData.attr('ItemRowId', RowIdCPKData);
            dialogCPKData.attr('plantCode', dataRowCPKData.plantCode);

            $("#txtCTQ").val(dataRowCPKData.CTQ);
            $("#txtCTQ").attr("disabled", "disabled");

            $("#lblDiaErrMsgCPKData").html("");

            break;
        default: //Edit("e")
            $("#btndialogEditDataCPKData").button("option", "disabled", true);
            $("#btndialogCancelEditCPKData").button("option", "disabled", false);
            $("#dialogDataToolBarCPKData").show();

            dialogCPKData.attr('ItemRowId', RowIdCPKData);
            dialogCPKData.attr('plantCode', dataRowCPKData.plantCode);

            $("#txtCTQ").val(dataRowCPKData.CTQ);
            $("#txtCTQ").removeAttr('disabled');

            $("#lblDiaErrMsgCPKData").html("");

            break;
    }
}