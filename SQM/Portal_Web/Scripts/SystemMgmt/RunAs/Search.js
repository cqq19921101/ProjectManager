$(function () {
    $("#btnSearch").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataList = $("#gridDataList");
        gridDataList.jqGrid('clearGridData');

        var sMemberType = "";
        if ($('#radFilterMemberTypeInternalOnly').is(':checked'))
            sMemberType = "2";
        else if ($('#radFilterMemberTypeExternalOnly').is(':checked'))
            sMemberType = "1";

        $.ajax({
            url: __WebAppPathPrefix + '/SystemMgmt/LoadMemberJSonWithFilter',
            data: { SearchText: $("#txtFilterText").val(), MemberType: sMemberType },
            type: "post",
            dataType: 'json',
            async: false,
            success: function (data) {
                for (var iCnt = 0; iCnt < data.Rows.length; iCnt++) {
                    var datarow = {
                        AccountGUID: data.Rows[iCnt].AccountGUID,
                        AccountID: data.Rows[iCnt].AccountID,
                        MemberType: data.Rows[iCnt].MemberType,
                        NameInChinese: data.Rows[iCnt].NameInChinese,
                        NameInEnglish: data.Rows[iCnt].NameInEnglish,
                        PrimaryEmail: data.Rows[iCnt].PrimaryEmail,
                        PasswordHash: data.Rows[iCnt].PasswordHash
                    };
                    var su = gridDataList.jqGrid('addRowData', iCnt.toString(), datarow, 'last');
                }
                gridDataList.triggerHandler('reloadGrid');
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
                //HideAjaxLoading();
            }
        });
    });
});