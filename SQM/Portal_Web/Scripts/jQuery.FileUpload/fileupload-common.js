$(function () {
    $.init_fileupload = function (formId, maxNumberOfFiles, maxFileSize, acceptFileTypes) {
        InitOption = {
            dataType: 'json',
            autoUpload: true,
            maxNumberOfFiles: maxNumberOfFiles,
            maxFileSize: maxFileSize,
            acceptFileTypes: acceptFileTypes,
            processfail: function (e, data) {
                if (data.files.error) {
                    alert('The file ' + data.files[0].name + ' could not be uploaded: ' + data.files[0].error);
                    $(this).find('tbody.files tr.processing').remove();
                    return false;
                }
            }
        }

        $('#' + formId).fileupload(InitOption);
    }

    $.reInit_fileupalod = function (formId) {
        $('#' + formId).find('table>tbody.files').empty();
    }

    $.get_uploadedFileInfo = function (formId) {
        var FileInfo = []

        $('form#' + formId + ' table tr a').each(function (index) {
            var File = new Object();
            File.FileType = $(this).attr("FileType");
            File.FileName = unescape($(this).text());
            File.Url = $(this).attr("Url");
            File.FS_GUID = $(this).attr("FS_GUID");

            FileInfo.push(File);
        });

        return FileInfo;
    }
})