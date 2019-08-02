$(function () {
    $('#btnSearchApplyHistory').click(function () {
        $(this).removeClass('ui-state-focus');

        ReloadApplicationgridDataList();
    });

    $('#btnQueryVendorEmail').click(function () {
        $(this).removeClass('ui-state-focus');

        var Account = escape($.trim($('#txtAccount').val()));
        var BUVendorCode = escape($.trim($("#txtBUVendorCode").val()));

        if (Account != '' && BUVendorCode != '') {
            $.ajax({
                url: __WebAppPathPrefix + '/VMIProcess/VendorEmailQueryEmail',
                data: {
                    Account: Account,
                    BUVendorCode: BUVendorCode
                },
                type: "post",
                dataType: 'text',
                async: false, // if need page refresh, please remark this option
                success: function (data) {
                    if (data != '') {
                        $('#dialogVendorAccountInfo').prop('currentEmail', data);
                        $('#dialogVendorAccountInfo').prop('account', Account);
                        $('#dialogVendorAccountInfo').prop('BUVendorCode', BUVendorCode);
                        $('#dialogVendorAccountInfo').prop('action', 'query');
                        $('#dialogVendorAccountInfo').dialog('open');
                    }
                    else {
                        alert('Account or BU Vendor Code not exists. Please check it again, thanks.');
                    }
                },
                error: function (xhr, textStatus, thrownError) {
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, textStatus) {
                }
            });
        }
        else {
            alert('Account & BU Vendor Code cannot be empty, thanks.');
        }
    });
});

function ReloadApplicationgridDataList() {
    var gridApplication = $('#gridApplication');
    var Status = escape($.trim($('#ddlStatus option:selected').val()));
    var Account = escape($.trim($('#txtAccount').val()));
    var BUVendorCode = escape($.trim($("#txtBUVendorCode").val()));

    gridApplication.jqGrid('clearGridData');
    gridApplication.jqGrid('setGridParam', {
        postData: {
            Status: Status,
            Account: Account,
            BUVendorCode: BUVendorCode
        }
    });
    gridApplication.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}