$(function () {
    $('#dialogRefAndDownload').dialog({
        autoOpen: false,
        resizable: false,
        modal: true,
        width: 500,
        height: 550,
        open: function (event, ui) {
            var ID = $(this).prop('ID');
            var Category = $(this).prop('Category');
            var PublishDate = $(this).prop('PublishDate');
            var Subject = $(this).prop('Subject');
            
            $.ajax({
                url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinDetail',
                type: "post",
                data: {
                    ID: escape(ID)
                },
                dataType: 'json',
                async: false, // if need page refresh, please remark this option
                success: function (data) {
                    var attachments = '';
                    for (var idx in data) {
                        if (idx == 0) {
                            $('#tdCategory').text(Category);
                            $('#tdPublishDate').text(PublishDate);
                            $('#tdSubject').text(Subject);
                            // Body Text
                            $('#tdBodyText').html(data[idx].BodyText);
                        }
                        attachments += '<a class="downloadAttachment" style="color: blue;" title="Click me to download!" href="#" id="' + ID + "_" + data[idx].SortID + '">' + data[idx].FileTitle + '</a>&nbsp;';
                    }
                    $('#tdAttachments').append(attachments);
                    $('.downloadAttachment').click(function () {
                        var info = $(this).attr('id').split('_');
                        var ID = info[0];
                        var SortID = info[1];
                        $.ajax({
                            url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinAttachment',
                            data: {
                                ID: escape(ID),
                                SortID: escape(SortID)
                            },
                            type: "post",
                            dataType: 'json',
                            async: false,
                            success: function (data) {
                                if (data.Result) {
                                    if (data.FileKey != "") {
                                        $("#dialogDownloadSplash_FileKey").val(data.FileKey);
                                        $("#dialogDownloadSplash_FileName").val(data.FileName);
                                        setTimeout(function () {
                                            $("#dialogDownloadSplash_Form").attr('action', __WebAppPathPrefix + '/VMIBulletin/RetrieveFileByFileKey').submit();
                                            $("#dialogDownloadSplash").dialog("close");
                                        }, 10);
                                    }
                                }
                                else if (data.FileName != "") {
                                    alert(data.FileName);
                                }
                                else {
                                    alert("Error for downloading file, please contact administrator manager.");
                                }
                            },
                            error: function (xhr, textStatus, thrownError) {
                                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                            }
                        });
                    });
                },
                error: function (xhr, textStatus, thrownError) {
                    $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
                },
                complete: function (jqXHR, textStatus) {
                }
            });
        },
        close: function () {
            $('#tdCategory').text('');
            $('#tdPublishDate').text('');
            $('#tdSubject').text('');
            // Body Text
            $('#tdBodyText').html('');
            $('#tdAttachments').html('');
        }
    });
})