$(function () {
    var diaQueryASNAttach = $('#dialogQueryASNFileAttach');

    $('#dia_btn_VMIQuery_ASNFileInfo_ViewLogs').button({
        label: "View Logs",
        icons: { primary: 'ui-icon-search' }
    });

    diaQueryASNAttach.dialog({
        autoOpen: false,
        height: 220,
        width: 400,
        resizable: false,
        modal: true,
        buttons: {
            Close: function () { $(this).dialog("close"); }
        },
        close: function () {
            __DialogIsShownNow = false;
        }
    });
});

function InitdialogASNFileAttach() {
    var diaASNAttach = $('#dialogQueryASNFileAttach');
    $('#dialog_span_VMIQuery_ASNFileInfo_Download').html(diaASNAttach.attr("FILENAME"));
    $('#dialog_txt_VMIQuery_ASNFileInfo_Remark').val(diaASNAttach.attr("REMARK"));
}