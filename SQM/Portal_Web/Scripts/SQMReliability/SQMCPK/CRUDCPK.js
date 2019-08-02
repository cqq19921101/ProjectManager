$(function () {
    var dialogCPK = $("#dialogDataCPK");
    var gridDataListCPK = $("#gridDataListCPK");
    var gridDataListCPKSub = $("#gridDataListCPKSub");
    jQuery("#btnShowCPKSub").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataListCPK.jqGrid('getGridParam', 'selrow');
        if (RowId) {   //single select
            var dataRow = gridDataListCPK.jqGrid('getRowData', RowId);
            gridDataListCPKSub.jqGrid('clearGridData');
            gridDataListCPKSub.jqGrid('setGridParam', { postData: { plantCode: dataRow.plantCode } })
            gridDataListCPKSub.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
            $("#dialogDataCPK").attr('reportID', dataRow.reportID);
            $("#dialogDataCPK").attr('plantCode', dataRow.plantCode);
            $("#spreportID").html(dataRow.reportID)

            $('#inspCPK').hide();
            $('#inspCPKSub').show();
            $('#tbMain1CPKSub').show();


            $.ajax({
                url: __WebAppPathPrefix + '/SQMReliability/GetLitNoDataList',
                data: { "plantCode": $("#dialogDataCPK").attr('plantCode') },
                type: "post",
                dataType: 'json',
                async: false, // if need page refresh, please remark this option
                success: function (data) {
                    var options = '';
                    for (var idx in data) {
                        options += '<option value=' + data[idx].spc_item + '> ' + data[idx].spc_item + '</option>';
                    }
                    $('#ddlDesignator').html(options);
                },
                error: function (xhr, textStatus, thrownError) {
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, textStatus) {
                }
            });

        } else { alert("Please select a row data to show."); }

    });

    jQuery("#btnCreateCPK").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListCPK.jqGrid('getGridParam', 'multiselect')) {   //single select
            dialogCPK.attr('Mode', "c");
            DialogSetUIByModeCPK(dialogCPK.attr('Mode'));
            dialogCPK.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnViewEditCPK").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListCPK.jqGrid('getGridParam', 'multiselect')) {   //single select
            SetToViewModeCPK();
        }
    });

    jQuery("#btndialogEditDataCPK").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataListCPK.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('VendorCPK'))
            if (true) {
                dialogCPK.attr('Mode', "e");
                DialogSetUIByModeCPK(dialogCPK.attr('Mode'));
                dialogCPK.dialog("option", "title", "Edit").dialog("open");
            }
            else {
                switch (r) {
                    case "timeout": $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); break;
                    case "l": alert("Data already lock by other user."); break;
                    default: alert("Data lock fail or application error."); break;
                }
            }
        } else { alert("Please select a row data to edit."); }
    });

    jQuery("#btndialogCancelEditCPK").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialogCPK.attr('BasicInfoGUID'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewModeCPK();
    });

    function SetToViewModeCPK() {
        var RowIdCPK = gridDataListCPK.jqGrid('getGridParam', 'selrow');
        if (RowIdCPK) {
            dialogCPK.attr('Mode', "v");
            DialogSetUIByModeCPK(dialogCPK.attr('Mode'));
            dialogCPK.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnDeleteCPK").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListCPK.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowIdCPK = gridDataListCPK.jqGrid('getGridParam', 'selrow');
            if (RowIdCPK) {
                //var DataKey = (gridDataListCPK.jqGrid('getRowData', RowId)).SubFuncGUID;
                //var r = AcquireDataLock(DataKey)
                if (true) {
                    if (confirm("Confirm to delete selected member (" + gridDataListCPK.jqGrid('getRowData', RowIdCPK).reportID + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMReliability/DeleteCPK",
                            data: {
                                "plantCode":gridDataListCPK.jqGrid('getRowData', RowIdCPK).plantCode,
                                "reportID": gridDataListCPK.jqGrid('getRowData', RowIdCPK).reportID
                            },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSearchCPK").click();
                                    alert("delete successfully.");
                                }
                                else {
                                    alert("delete fail due to:\n\n" + data);
                                }
                            },
                            error: function (xhr, textStatus, thrownError) {
                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                            },
                            complete: function (jqXHR, textStatus) {
                                //$("#ajaxLoading").hide();
                            }
                        });
                    }
                    else {
                        var rP = ReleaseDataLock(DataKey);
                        if (rP == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
                    }
                }
                else {
                    switch (rP) {
                        case "timeout": $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); break;
                        case "l": alert("Data already lock by other user."); break;
                        default: alert("Data lock fail or application error."); break;
                    }
                }
            } else { alert("Please select a row data to delete."); }
        }
    });

})