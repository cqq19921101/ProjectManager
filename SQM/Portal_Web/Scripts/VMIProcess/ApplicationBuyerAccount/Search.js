$(function () {
    $('#btnQueryABA').click(function () {
        $(this).removeClass('ui-state-focus');
        ReloadABAgridDataList();
    });

    $("#btnDiaReadInternalUserInfo").click(function () {
        var AccountID = $.trim($("#txtBUYER_NAME").val());
        $.ajax({
            url: __WebAppPathPrefix + "/SystemMgmt/GetInternalUserInfoByAccountID",
            data: { "AccountID": AccountID },
            type: "post",
            dataType: 'json',
            success: function (data) {
                if (data.ErrMsg == "") {                    
                    $("#lbBuyerName").text(data.Member.NameInEnglish);
                }
                else {
                    $("#lbBuyerName").text("");
                    alert(data.ErrMsg.replace('<br>', '\n'));
                }
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    });
});

function ReloadABAgridDataList() {
    var gridABA = $('#gridABA');

    gridABA.jqGrid('clearGridData');
    gridABA.jqGrid('setGridParam', {
        postData: {
            BUYER_NAME: escape($.trim($('#txtBuyerName').val())),
            SITE_ID: escape($.trim($('#ddlSite option:selected').val())),
            CREATE_USER_NAME: escape($.trim($("#txtIssuer").val())),
            STATUS: escape($.trim($('#ddlStatus option:selected').val()))
        }
    });

    gridABA.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}