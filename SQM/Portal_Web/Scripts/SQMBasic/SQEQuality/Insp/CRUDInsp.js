$(function () {
    var gridDataListInsp = $("#gridDataListInsp");

    jQuery("#btnBackInsp").click(function () {
        $(this).removeClass('ui-state-focus');
        gridDataListInsp.jqGrid('clearGridData');

        $("#quality").show();
        $("#inspInsp").hide();
        $("#tbMain1Insp").hide();
        $("#inspInspVar").hide();
        $("#tbMain1InspVar").hide();
        
        $("#ddlSInspCode  option:first").prop("selected", 'selected').change();
        $("#ddlSInspCodeVar  option:first").prop("selected", 'selected').change();

    });

})