$(function () {
    $('#dialogPerInvRptConsignResult').dialog({
        autoOpen: false,
        resizable: false,
        modal: true,
        width: 600,
        height: 500,
        close: function () {
            $('#tdFiles').html('');
        }
    });
});