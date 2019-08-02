$(function () {
    var gridDataList = $("#gridPVL");
    var dialog = $("#dialogDataPVL");

    //$("#btnSubmit").click(function () {
    //    $(this).removeClass('ui-state-focus');
    //    if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
    //    {   //single select
    //        SetToViewMode_BasicInfoType();
    //    }
    //});


    jQuery("#btnSubmit").click(function () {
        $(this).removeClass('ui-state-focus');
        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');

        if (RowId)
        {
            if (RowId == "1" || RowId == "2" || RowId == "3") {
                dialog.attr('Mode', "e");
                DialogSetUIByMode_BasicInfoType(dialog.attr('Mode'));
                dialog.dialog("option", "title", "Confirm").dialog("open");
            }

            else {

                var DataKey = (gridDataList.jqGrid('getRowData', RowId)).BasicInfoGUID;
                //var r = AcquireDataLock(DataKey)
                //if (r == "ok") {
                if (true) {
                    if (confirm("Confirm to submit selected?")) {
                        $.ajax({
                            url: __WebAppPathPrefix + "/SQMBasic/PVLContact",
                            data: { "BasicInfoGUID": gridDataList.jqGrid('getRowData', RowId).BasicInfoGUID, "TB_SQM_Vendor_TypeCID": gridDataList.jqGrid('getRowData', RowId).TB_SQM_Vendor_TypeCID },
                            type: "post",
                            dataType: 'text',
                            async: false,
                            success: function () {
                                alert("Commit successfully.");
                                var gridDataList = $("#gridPVL");
                                gridDataList.jqGrid('clearGridData');
                                gridDataList.jqGrid('setGridParam', { postData: { SearchText: "" } })
                                gridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                                $("#btnSuspend").hide();
                                $("#btnCommit").hide();
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
                        var r = ReleaseDataLock(DataKey);
                        if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
                    }
                }
                //else {
                //    switch (r) {
                //        case "timeout": $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); break;
                //        case "l": alert("Data already lock by other user."); break;
                //        default: alert("Data lock fail or application error."); break;
                //    }
                //}

                //alert("請選擇供應商！");
            }

        }
        else
        {
            alert("請選擇供應商！");
        }
        
       
        //}
    });

    //$("#btndialogEditDataBasicInfoType").click(function () {
    //    $(this).removeClass('ui-state-focus');
    //    var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
    //    if (RowId) {
    //        //var r = AcquireDataLock(dialog.attr('BasicInfoTypeGUID'));
    //        var r = "ok";
    //        if (r == "ok") {
    //            dialog.attr('Mode', "e");
    //            DialogSetUIByMode_BasicInfoType(dialog.attr('Mode'));
    //            dialog.dialog("option", "title", "Edit").dialog("open");
    //        }
    //        else {
    //            switch (r) {
    //                case "timeout": $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); break;
    //                case "l": alert("Data already lock by other user."); break;
    //                default: alert("Data lock fail or application error."); break;
    //            }
    //        }
    //    } else { alert("Please select a row data to edit."); }
    //});

    //$("#btndialogCancelEditBasicInfoType").click(function () {
    //    $(this).removeClass('ui-state-focus');
    //    var r = ReleaseDataLock(dialog.attr('BasicInfoTypeGUID'));
    //    if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); else SetToViewMode_BasicInfoType();
    //});

    //function SetToViewMode_BasicInfoType()
    //{
    //    var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
    //    if (RowId) {
    //        dialog.attr('Mode', "v");
    //        DialogSetUIByMode_BasicInfoType(dialog.attr('Mode'));
    //        dialog.dialog("option", "title", "View").dialog("open");
    //    } else { alert("Please select a row data to edit."); }
    //}

    //$("#btnDeleteBasicInfoType").click(function () {
    //    $(this).removeClass('ui-state-focus');
    //    if (!gridDataList.jqGrid('getGridParam', 'multiselect'))
    //    {   //single select
    //        var RowId = gridDataList.jqGrid('getGridParam', 'selrow');
    //        if (RowId) {
    //            var DataKey = (gridDataList.jqGrid('getRowData', RowId)).BasicInfoGUID;
    //            var r = AcquireDataLock(DataKey)
    //            if (r == "ok") {
    //                if (confirm("Confirm to delete selected BasicInfoType (" + gridDataList.jqGrid('getRowData', RowId).BasicInfoTypeGUID + ")?\n\n(Note. Data cannot be recovered once deleted)")) {
    //                    $.ajax({
    //                        url: __WebAppPathPrefix + "/SQMBasic/DeleteBasicInfoType",
    //                        data: { "BasicInfoGUID": gridDataList.jqGrid('getRowData', RowId).BasicInfoGUID },
    //                        type: "post",
    //                        dataType: 'text',
    //                        async: false,
    //                        success: function (data) {
    //                            if (data == "") {
    //                                //$("#btnSearch").click();
    //                                alert("BasicInfoType delete successfully.");
    //                            }
    //                            else {
    //                                alert("BasicInfoType delete fail due to:\n\n" + data);
    //                            }
    //                        },
    //                        error: function (xhr, textStatus, thrownError) {
    //                            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
    //                        },
    //                        complete: function (jqXHR, textStatus) {
    //                            //$("#ajaxLoading").hide();
    //                        }
    //                    });
    //                }
    //                else {
    //                    var r = ReleaseDataLock(DataKey);
    //                    if (r == "timeout") $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut);
    //                }
    //            }
    //            else {
    //                switch (r) {
    //                    case "timeout": $.CommonUIUtility_SessionTimeOut(__UrlForTimeOut); break;
    //                    case "l": alert("Data already lock by other user."); break;
    //                    default: alert("Data lock fail or application error."); break;
    //                }
    //            }
    //        } else { alert("Please select a row data to delete."); }
    //    }
    //});



});

$(function () {
    $('#btnSubmit').button({
        label: '選擇',
        icons: { primary: 'ui-icon-arrowthickstop-1-s' }
    });

    $("#btnSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $('#btnNSubmit').button({
        label: '取消',
        icons: { primary: 'ui-icon-arrowthickstop-1-s' }
    });

});


$(function () {

    $("#btnSearch").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridPVL = $("#gridPVL");
        gridPVL.jqGrid('clearGridData');

        gridPVL.jqGrid('setGridParam', {
            postData: {
                VTNAME: escape($("#ddlVendorType").val()),
                CNAME: escape($("#ddlCategory").val()),
                CSNAME: escape($("#ddlCategorySub").val())
            }
        })
        gridPVL.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');

        //ReloadPVLgridDataList();
    });
});




function ReloadPVLgridDataList() {
    var gridDataListBasicInfoType = $("#gridPVL");
    $.ajax({
        url: __WebAppPathPrefix + "/SQMBasic/GetPVL",
        data: {
            //"BasicInfoGUID": dialog.attr("BasicInfoGUID"),
            VTNAME: escape($("#ddlVendorType").val()),
            CNAME: escape($("#ddlCategory").val()),
            CSNAME: escape($("#ddlCategorySub").val())
            //"TB_SQM_Commodity_SubCID": escape($("#ddlCategorySub").val()),
            //"IsChoose": escape($("input:radio[name='A']:checked").val())
        },
        type: "post",
        dataType: 'text',
        async: false,
        success: function (data) {
            if (data == "") {
                alert("此供應商尚未設定對應SQE,請通知SQE維護資料");
            }
            else
            {
                switch (data) {
                    case "搜尋條件不能為空,請確認後輸入！":
                        alert("搜尋條件不能為空,請確認後輸入！");
                        break;

                }
            }
        }
    });
    gridDataListBasicInfoType.jqGrid('navGrid', '#gridPVLListPager', { edit: false, add: false, del: false, search: false, refresh: false });
}










 