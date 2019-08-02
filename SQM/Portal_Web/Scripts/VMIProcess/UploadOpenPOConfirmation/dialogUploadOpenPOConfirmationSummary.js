$(function () {
    var dialogUploadOpenPOConfirmationSummary = $('#dialogUploadOpenPOConfirmationSummary');

    dialogUploadOpenPOConfirmationSummary.dialog({
        width: 400,
        autoOpen: false,
        modal: true,
        resizable: false,
        buttons: {
            Close: function () {
                $(this).dialog('close');
            }
        },
        close: function () {
            $('#dialogUploadOpenPOConfirmationSummary #result').text('');
            dialogUploadOpenPOConfirmationSummary.find('#message').text('');
            dialogUploadOpenPOConfirmationSummary.find('#tdErrorFileDownload').children().remove();
        }
    });
});