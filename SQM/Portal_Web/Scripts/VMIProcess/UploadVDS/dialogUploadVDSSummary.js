$(function () {
    var dialogUploadVDSSummary = $('#dialogUploadVDSSummary');

    dialogUploadVDSSummary.dialog({
        width: 600,
        autoOpen: false,
        modal: true,
        resizable: false,
        buttons: {
            Close: function () {
                $(this).dialog('close');
            }
        },
        close: function () {
            $('#dialogUploadVDSSummary #result').text('');

            $('#tdErrorFileDownload').children().remove();

            $('#dialogUploadVDSSummary table.defaultTable tr').eq(2).find('td').each(function (index, element) {
                $(element).find('p').each(function (index, element) {
                    switch (index) {
                        case 0:
                            $(element).text('Total Lines:0');
                            break;
                        case 1:
                            $(element).text('Correct Lines:0');
                            break;
                        case 2:
                            $(element).text('Error Lines:0');
                            break;
                    }
                });
            });
        }
    });
})