$(function () {
    var diaDownloadSplash = $('#dialogDownloadSplash');

    //Init dialog
    diaDownloadSplash.dialog({
        autoOpen: false,
        height: 10,
        width: 10,
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
})