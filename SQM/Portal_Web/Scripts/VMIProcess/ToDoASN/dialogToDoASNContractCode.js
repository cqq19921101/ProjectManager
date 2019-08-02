$(function () {
    var diaToDoASNContract = $('#dialog_VMIProcess_ToDoASNContractCode');
    var VMI_ToDoASNImportDetailgridDataList = $('#VMI_Process_ToDoASNImportDetail_gridDataList');

    //Init dialog
    diaToDoASNContract.dialog({
        autoOpen: false,
        height: 150,
        width: 300,
        resizable: false,
        modal: true,
        buttons: {
            Close: function () {
                $(this).dialog("close");
            }
        },
        close: function () {
            __DialogIsShownNow = false;
            VMI_ToDoASNImportDetailgridDataList.jqGrid('setCell', diaToDoASNContract.attr("RowID"), 'lCUSTOMSSEAL', $('#dialog_dropbox_VMIProcess_ToDoASNContractCode_CustomesSeal').val());
        }
    });
});