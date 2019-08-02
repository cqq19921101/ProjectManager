$(function () {

    var diaUploadASNResult = $('#dialog_VMIProcess_UploadASNResult');

    //Init dialog
    diaUploadASNResult.dialog({
        autoOpen: false,
        height: 230,
        width: 650,
        resizable: false,
        modal: true,
        buttons: {
            Close: function () {
                $(this).dialog("close");
            }
        },
        close: function () {
            __DialogIsShownNow = false;
        }
    });

    $('#dialog_span_VMProcess_UploadASNResult_ASNNo').click(function () {
        diaUploadASNResult.dialog("close");
        window.location.href = __WebAppPathPrefix + '/VMIProcess/ToDoASN?sQueryASNNoFrom=' + diaUploadASNResult.attr('ASNNo');
    });
});