$(function () {
    //var gridVDS = $('#gridVDS');
    //var diaToDoVDSManage = $('#dialog_VMIProcess_ToDoVDSManage');

    //Init Function Button
    //$('#btnQueryDemand').button({
    //    label: "Query",
    //    icons: { primary: 'ui-icon-search' }
    //});

    $('#btnOpenQueryPlantDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    $('#btnOpenQueryVendorCodeDialog').button({
        icons: { primary: 'ui-icon-search' }
    });

    //$('#btnOpenQueryBuyerCodeDialog').button({
    //    icons: { primary: 'ui-icon-search' }
    //});

    //$('#btnVDSCommit').button({
    //    label: 'Commit',
    //    icons: { primary: 'ui-icon-check' }
    //});

    //$('#btnVDSCommitAll').button({
    //    label: 'Commit All',
    //    icons: { primary: 'ui-icon-check' }
    //});

    //$('#btnExportAll').button({
    //    label: 'Export All',
    //    icons: { primary: 'ui-icon-arrowreturnthick-1-n' }
    //});

    //$('#btnExportVDS').button({
    //    label: 'Export VDS Only',
    //    icons: { primary: 'ui-icon-arrowreturnthick-1-n' }
    //});

    // Query character to upper
    $('input#txtPlant').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });
    $('input#txtVendorCode').on('keydown keyup', function () {
        $(this).val($(this).val().toUpperCase());
    });
    //$('input#txtBuyerCode').on('keydown keyup', function () {
    //    $(this).val($(this).val().toUpperCase());
    //});

    
});

function getCurrentDateTime() {
    var d = new Date,
        dformat = [d.getFullYear(),
            (d.getMonth() + 1).padLeft(),
            d.getDate().padLeft()].join('/') +
                    ' ' +
                  [d.getHours().padLeft(),
                    d.getMinutes().padLeft()/*,
                    d.getSeconds().padLeft()*/].join(':');
    return dformat;
}