$(function () {
    $("#btnSearch").click(function () {
        $(this).removeClass('ui-state-focus');
        var gridDataList = $("#gridDataList");
        gridDataList.jqGrid('clearGridData');

        gridDataList.jqGrid('setGridParam', { postData: { SearchText: escape($.trim($("#txtFilterText").val())) } })
        gridDataList.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
    });
    var diaSBUVendor = $('#dialog_VMI_QuerySBUVendor');
     $('#btnOpenQueryVendorCodeDialog').click(function () {
        $(this).removeClass('ui-state-focus');
        if (!__DialogIsShownNow) {
            __DialogIsShownNow = true;
            __SelectorName = '#txtVendorCode';

            InitdialogSBUVendor();
            ReloadDiaSBUVendorCodegridDataList();

            diaSBUVendor.show();
            diaSBUVendor.dialog("open");
        }
     });
     function InitdialogSBUVendor() {
         $('#dialog_VMI_txt_SBU_VDN').val("");
     }
     function ReloadDiaSBUVendorCodegridDataList() {
         var diaSBUVendorgridData = $('#dialog_VMI_SBUVendor_gridDataList');
         var diatxtSBUVDN = $('#dialog_VMI_txt_SBU_VDN');

         diaSBUVendorgridData.jqGrid('clearGridData');
         diaSBUVendorgridData.jqGrid('setGridParam', { postData: { ERP_VND: escape($.trim(diatxtSBUVDN.val())) } });
         diaSBUVendorgridData.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
     }
});