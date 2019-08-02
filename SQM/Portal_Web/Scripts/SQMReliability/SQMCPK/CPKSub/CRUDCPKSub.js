$(function () {
    var dialogCPKSub = $("#dialogDataCPKSub");
    var gridDataListCPKSub = $("#gridDataListCPKSub");
    var gridDataListCPKData = $("#gridDataListCPKData");
    jQuery("#btnShowCPKData").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataListCPKSub.jqGrid('getGridParam', 'selrow');
        if (RowId) {   //single select
            var dataRow = gridDataListCPKSub.jqGrid('getRowData', RowId);
            gridDataListCPKData.jqGrid('clearGridData');
            gridDataListCPKData.jqGrid('setGridParam', { postData: { reportID: $("#spreportID").html(), Designator: dataRow.Designator } })
            gridDataListCPKData.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
            $("#dialogDataCPKSub").attr('reportID', $("#spreportID").html());
            $("#dialogDataCPKSub").attr('Designator', dataRow.Designator);
            $("#dialogDataCPKSub").attr('UpperControlLimit', dataRow.UpperControlLimit);
            $("#dialogDataCPKSub").attr('LowerControlLimit', dataRow.LowerControlLimit);
            $("#dialogDataCPKSub").attr('Nominal', dataRow.Nominal);
            $("#dialogDataCPKSub").attr('CTQNum', dataRow.CTQNum);
            $("#spDesignator").html(dataRow.Designator);
            $("#spUnit").html(dataRow.Unit);
            $("#spNominal").html(dataRow.Nominal);
            $("#spUpperControlLimit").html(dataRow.maxNominal);
            $("#spLowerControlLimit").html(dataRow.minNominal);

            $('#inspCPKSub').hide();
            $('#inspCPKData').show();
            $('#tbMain1CPKData').show();


        } else { alert("Please select a row data to show."); }

    });

    jQuery("#btnBack").click(function () {
        $(this).removeClass('ui-state-focus');
        gridDataListCPKSub.jqGrid('clearGridData');

        $('#inspCPK').show();
        $('#inspCPKSub').hide();
        $('#tbMain1CPKSub').hide();

    });

    jQuery("#btnCreateCPKSub").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListCPKSub.jqGrid('getGridParam', 'multiselect')) {   //single select
            dialogCPKSub.attr('Mode', "c");
            DialogSetUIByModeCPKSub(dialogCPKSub.attr('Mode'));
            dialogCPKSub.dialog("option", "title", "Create").dialog("open");
        }
    });

    jQuery("#btnViewEditCPKSub").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListCPKSub.jqGrid('getGridParam', 'multiselect')) {   //single select
            SetToViewModeCPKSub();
        }
    });

    jQuery("#btndialogEditDataCPKSub").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataListCPKSub.jqGrid('getGridParam', 'selrow');
        if (RowId) {
            //var r = AcquireDataLock(dialog.attr('VendorCPK'))
            if (true) {
                dialogCPKSub.attr('Mode', "e");
                DialogSetUIByModeCPKSub(dialogCPKSub.attr('Mode'));
                dialogCPKSub.dialog("option", "title", "Edit").dialog("open");
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

    jQuery("#btndialogCancelEditCPKSub").click(function () {
        $(this).removeClass('ui-state-focus');
        var r = ReleaseDataLock(dialogCPKSub.attr('plantCode'));
        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewModeCPKSub();
    });

    function SetToViewModeCPKSub() {
        var RowIdCPK = gridDataListCPKSub.jqGrid('getGridParam', 'selrow');
        if (RowIdCPK) {
            dialogCPKSub.attr('Mode', "v");
            DialogSetUIByModeCPKSub(dialogCPKSub.attr('Mode'));
            dialogCPKSub.dialog("option", "title", "View").dialog("open");
        } else { alert("Please select a row data to edit."); }
    }

    jQuery("#btnDeleteCPKSub").click(function () {
        $(this).removeClass('ui-state-focus');
        if (!gridDataListCPKSub.jqGrid('getGridParam', 'multiselect')) {   //single select
            var RowIdCPK = gridDataListCPKSub.jqGrid('getGridParam', 'selrow');
            if (RowIdCPK) {
                //var DataKey = (gridDataListCPKSub.jqGrid('getRowData', RowId)).SubFuncGUID;
                //var r = AcquireDataLock(DataKey)
                if (true) {
                    if (confirm("Confirm to delete selected member (" + gridDataListCPKSub.jqGrid('getRowData', RowIdCPK).Designator + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMReliability/DeleteCPKSub",
                            data: {
                                "reportID":$("#dialogDataCPK").attr('reportID')
                                ,"SID": gridDataListCPKSub.jqGrid('getRowData', RowIdCPK).SID
                            },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    $("#btnSearchCPKSub").click();
                                    alert(" delete successfully.");
                                }
                                else {
                                    alert(" delete fail due to:\n\n" + data);
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