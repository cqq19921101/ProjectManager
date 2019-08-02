$(function () {
    $('#btnQueryAVA').click(function () {
        $(this).removeClass('ui-state-focus');
        ReloadANAgridDataList();
    });
});

function ReloadANAgridDataList() {
    var gridAVA = $('#gridAVA');

    gridAVA.jqGrid('clearGridData');
    gridAVA.jqGrid('setGridParam', {
        postData: {
            COMPANY_NAME: escape($.trim($("#txtCompanyName").val())),
            SITE_ID: escape($.trim($('#ddlSite option:selected').val())),
            VENDOR_CODE: escape($.trim($("#txtVendorCode").val())),
            ISSUER: escape($.trim($('#txtIssuer').val())),
            STATUS: escape($.trim($('#ddlStatus option:selected').val()))
        }
    });

    gridAVA.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
}