﻿//initial dialog
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
                        url: __WebAppPathPrefix + ((dialog.attr('Mode') == "c") ? "/SQMGrade/Create" : "/SQMGrade/Edit"),
                        data: {
                            "SID": dialog.attr("SID"),
                     
                            "CID": escape($.trim($("#ddlCID").val())),
                            "CCID": escape($.trim($("#ddlCCID").val())),
                            "NAME": escape($.trim($("#txtNAME").val())),
                            "Grade": escape($.trim($("#txtGRADE").val())),
                            "RowID": escape($.trim($("#txtRowID").val()))
                           
                        },
                        type: "post",
                        dataType: 'text',
                        async: false,
                        success: function (data) {
                            if (data == "") {
                                DoSuccessfully = true;
                                if (dialog.attr('Mode') == "c")
                                    alert("Grade create successfully.");
                                else
                                    alert("Grade edit successfully.");
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

        
           
            $("#ddlCID").val(1);
            $("#ddlCID").removeAttr('disabled');
            $("#ddlCCID").val(1);
            $("#ddlCCID").removeAttr('disabled');
            $("#txtNAME").val("");
            $("#txtNAME").removeAttr('disabled');
            $("#txtGRADE").val("");
            $("#txtGRADE").removeAttr('disabled');
            $("#txtRowID").val("");
            $("#txtRowID").removeAttr('disabled');
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

           
        
            $("#ddlCID").val(dataRow.CID);
            $("#ddlCID").attr("disabled", "disabled");
            $("#ddlCCID").val(dataRow.CCID);
            $("#ddlCCID").attr("disabled", "disabled");
            $("#txtNAME").val(dataRow.NAME);
            $("#txtNAME").attr("disabled", "disabled");
            $("#txtGRADE").val(dataRow.Grade);
            $("#txtGRADE").attr("disabled", "disabled");
            $("#txtRowID").val(dataRow.RowID);
            $("#txtRowID").attr("disabled", "disabled");


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

            $("#ddlCID").val(dataRow.CID);
            $("#ddlCID").attr("disabled", "disabled");
            $("#ddlCCID").val(dataRow.CCID);
            $("#ddlCCID").attr("disabled", "disabled");
            $("#txtNAME").val(dataRow.NAME);
            $("#txtNAME").removeAttr('disabled');
            $("#txtGRADE").val(dataRow.Grade);
            $("#txtGRADE").removeAttr('disabled');
            $("#txtRowID").val(dataRow.RowID);
            $("#txtRowID").removeAttr('disabled');
            

            $("#lblDiaErrMsg").html("");

            break;
    }
}