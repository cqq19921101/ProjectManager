$(function () {
    $('#dialogMaterialDoc').dialog({
        autoOpen: false,
        height: 370,
        width: 200,
        resizable: false,
        modal: true,
        open: function (event, ui) {
            $this = $(this);
            $.ajax({
                url: __WebAppPathPrefix + '/VMIQuery/GetMaterialDoc',
                data: {
                    plant: escape($this.prop('plant')),
                    vendor: escape($this.prop('vendor')),
                    material: escape($this.prop('material')),
                    date: escape($this.prop('date'))
                },
                type: "post",
                dataType: 'json',
                async: false, // if need page refresh, please remark this option
                success: function (data) {
                    $('#lbDate').text($this.prop('date'));
                    $('#lbMaterial').text($this.prop('material'));
                    $('#materialDocList').text('');
                    $(data).each(function (index, value) {
                        var materialDocLists = $('#materialDocList').text();
                        if (materialDocLists != '') {
                            $('#materialDocList').text(materialDocLists + '\n' + value);
                        }
                        else {
                            $('#materialDocList').text(value);
                        }
                        
                    });
                },
                error: function (xhr, textStatus, thrownError) {
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, textStatus) {
                }
            });
        }
    });
});