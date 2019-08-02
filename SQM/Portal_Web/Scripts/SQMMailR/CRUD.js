function VDSToolBar() {
    // Init Toolbar
    var diaToDVDSManage_tbTopToolBar = $('#dialog_VMIProcess_ToDoVDSManage_tbTopToolBar');
    var tabsVDS = $('#ToDoVDSManageTabs');
    var diaToDVDSManage = $('#dialog_VMIProcess_ToDoVDSManage');
    var PLANT = $.trim(diaToDVDSManage.prop('PLANT'));
    var VENDOR = $.trim(diaToDVDSManage.prop('VENDOR'));
    var BUYER = $.trim(diaToDVDSManage.prop('BUYER'));

    diaToDVDSManage_tbTopToolBar.show();

    $('#btnVDSCommit').click(function () {
        VDSCommit(tabsVDS, PLANT, VENDOR, BUYER, false);
        $(this).blur();
    });

    $('#btnVDSCommitAll').click(function () {
        VDSCommit(tabsVDS, PLANT, VENDOR, BUYER, true);
        $(this).blur();
    });

    $('#btnExportAll').click(function () {
        VDSExportVDS('ExportAll', PLANT, VENDOR, BUYER);
        $(this).blur();
    });

    $('#btnExportVDS').click(function () {
        VDSExportVDS('ExportVDS', PLANT, VENDOR, BUYER);
        $(this).blur();
    });
}

