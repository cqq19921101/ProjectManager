$(function () {
    var gridDataListInsp = $("#gridDataListInsp");

    jQuery("#btnBackInsp").click(function () {
        gridDataListInsp.jqGrid('clearGridData');

        $("#quality").show();
        $("#inspInsp").hide();
        $("#tbMain1Insp").hide();

    });

})