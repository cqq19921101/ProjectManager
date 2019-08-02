$(function () {
    var gridDataListFile = $("#gridDataListFile");

    jQuery("#btnBackFile").click(function () {
        gridDataListFile.jqGrid('clearGridData');

        $("#quality").show();
        $("#inspFile").hide();
        $("#tbMain1File").hide();

    });

})