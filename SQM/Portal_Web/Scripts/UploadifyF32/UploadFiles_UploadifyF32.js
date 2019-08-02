$(function () {
    var UploadifySwfPath = __WebAppPathPrefix + "/Scripts/UploadifyF32/uploadify.swf";
    var UploadDestPath = __WebAppPathPrefix + "/Uploadify/Upload";
    var DownloadPath_Local = __WebAppPathPrefix + "/Uploadify/OutputInStream"

    $.UploadFiles_UPloadifyF32_Type1_RemoveRow = function (FUComponentId, TRId) {
        $("#" + TRId).remove();
        $("#" + FUComponentId).uploadify('disable', false);
    }

    $.UploadFiles_UPloadifyF32_Type1_RetrieveFileInfo = function (FUFileListId) {
        var FileInfo = [];

        $("#" + FUFileListId + "_table" + " tr").each(function () {
            var File = new Object();
            File.FileType = $(this).attr("FileType");
            File.FileName = unescape($(this).attr("FileName"));
            File.Url = $(this).attr("Url");
            File.FS_GUID = $(this).attr("FS_GUID");

            FileInfo.push(File);
        });

        return FileInfo;
    }

    $.UploadFiles_UPloadifyF32_Type1_SetInitDisable = function (FUFileList) {
        FUFileList.attr("InitDisable", "__InitDisable");
    }

    //$.UploadFiles_UPloadifyF32_Type1_SetRunTimeDisable = function (FUComponent) {
    //    FUComponent.uploadify('disable', true);
    //}

    $.UploadFiles_UPloadifyF32_Type1_ReInit = function (FUFileList) {
        var FileListTable_id = FUFileList.attr("id") + "_table";
        FUFileList.empty();
        FUFileList.append("<table id='" + FileListTable_id + "'></table>");
        FUFileList.attr("FileIndex", 0);
    }

    $.UploadFiles_UPloadifyF32_Type1_AddFileIntoFileList = function (FUFileList, FUComponent, FileListTable_id, SubFolderName, FileType, FileName, FileUrl, FS_GUID, Removable) {
        AddFileIntoFileList(FUFileList, FUComponent, FileListTable_id, SubFolderName, FileType, FileName, FileUrl, FS_GUID, Removable);
    }

    function AddFileIntoFileList(FUFileList, FUComponent, FileListTable_id, SubFolderName, FileType, FileName, FileUrl, FS_GUID, Removable) {
        var NewIndex = parseInt(FUFileList.attr("FileIndex")) + 1
        FUFileList.attr("FileIndex", NewIndex);

        var NewRowHtml = "";
        var NewTRId = FileListTable_id + "_TR" + NewIndex;
        var OnClickScript = "$.UploadFiles_UPloadifyF32_Type1_RemoveRow('" + FUComponent.attr('id') + "', '" + NewTRId + "');"

        NewRowHtml += "<tr id='" + NewTRId + "' FileType='" + FileType + "' FileName='" + escape(FileName) + "' Url='" + FileUrl + "' FS_GUID='" + FS_GUID + "' >" //File Type: 0:new upload (local url), 1:existing
            + "<td>";

        if (FileType == 0) {
            var LocalUrl = DownloadPath_Local + "?SubPath=" + SubFolderName + "&FileName=" + encodeURI(FileName);
            NewRowHtml += "<a href='" + LocalUrl + "' target='_blank'>" + FileName + "</a>";
        }
        else {
            var FileStreamUrl = __WebAppPathPrefix + FileUrl + "?DataGUID=" + FS_GUID;
            NewRowHtml += "<a href='" + FileStreamUrl + "' target='_blank'>" + FileName + "</a>";
        }

        if (Removable == "1") {
            NewRowHtml += "</td>"
                + "<td>&nbsp;" + "<a href=\"javascript: " + OnClickScript + ";\" >remove</a>" + "</td>"
                + "</tr>";
        }
        else
            NewRowHtml += "</td>"
                    + "<td></td>"
                    + "</tr>";

        var FileListAppendPoint;
        if ($('#' + FileListTable_id + ' > tbody').length == 0)
            FileListAppendPoint = $('#' + FileListTable_id);
        else
            FileListAppendPoint = $('#' + FileListTable_id + ' > tbody:last')
        FileListAppendPoint.append(NewRowHtml);
    }

    $.UploadFiles_UPloadifyF32_Type1_Init = function (
            FUFileList,
            FUComponent,
            MaxNumberOfFiles,   // 0: unlimited
            FileSizeLimit,      //Ex. "4GB"
            SubFolderName      //sub folder name for local temp storage
        ) {
        UploadFiles_UPloadifyF32_Type1_Init_Main(FUFileList, FUComponent, MaxNumberOfFiles, FileSizeLimit, SubFolderName, "");
    }

    $.UploadFiles_UPloadifyF32_Type1_Init2 = function (
            FUFileList,
            FUComponent,
            MaxNumberOfFiles,   // 0: unlimited
            FileSizeLimit,      //Ex. "4GB"
            SubFolderName,      //sub folder name for local temp storage
            ButtonText
        ) {
        UploadFiles_UPloadifyF32_Type1_Init_Main(FUFileList, FUComponent, MaxNumberOfFiles, FileSizeLimit, SubFolderName, ButtonText);
    }

    function UploadFiles_UPloadifyF32_Type1_Init_Main(
            FUFileList,
            FUComponent,
            MaxNumberOfFiles,   // 0: unlimited
            FileSizeLimit,      //Ex. "4GB"
            SubFolderName,      //sub folder name for local temp storage
            ButtonText
        ) {
        var i = 1;
        var FileListTable_id = FUFileList.attr("id") + "_table";
        FUFileList.empty();
        FUFileList.append("<table id='" + FileListTable_id + "'></table>");

        FUFileList.attr("FileIndex", 0)
        //for (var iCnt = 0; iCnt < FileInfoArrayCaller.length; iCnt++) {
        //    AddFileIntoFileList(FUFileList, FUComponent, FileListTable_id, SubFolderName,
        //        FileInfoArrayCaller[iCnt].FileType,
        //        FileInfoArrayCaller[iCnt].FileName,
        //        FileInfoArrayCaller[iCnt].Url,
        //        FileInfoArrayCaller[iCnt].FS_GUID
        //        );
        //}

        var udp = UploadDestPath;
        if (SubFolderName != "")
            udp = UploadDestPath + "?SubPath=" + SubFolderName;

        var InitOptions = {
            'swf': UploadifySwfPath,
            'uploader': udp,
            //'queueID': 'fileQueue',
            'auto': true,
            'multi': false,
            'fileSizeLimit': FileSizeLimit,
            'onSelect': function (file) {
                FUComponent.uploadify('disable', true);
            },

            'onUploadSuccess': function (file, data, response) {
                if ($('#' + FileListTable_id + ' tr').length < MaxNumberOfFiles)
                    AddFileIntoFileList(
                        FUFileList,
                        FUComponent,
                        FileListTable_id,
                        SubFolderName,
                        "0",
                        file.name,
                        "",
                        "",
                        "1"
                        );

                if (MaxNumberOfFiles == 0)
                    FUComponent.uploadify('disable', false);
                else {
                    if ($('#' + FileListTable_id + ' tr').length < MaxNumberOfFiles)
                        FUComponent.uploadify('disable', false);
                }
            },
            'onUploadProgress': function (file, bytesUploaded, bytesTotal, totalBytesUploaded, totalBytesTotal) {
                //To be complete
                //var NewIndex = $("#@(UploadControlSet[iUCS, 1])").attr("FileIndex");
                //var size = Math.pow(10, 0);
                //var p = Math.round(((totalBytesUploaded / totalBytesTotal) * 100) * size) / size;

                //$("#" + '@(UploadControlSet[iUCS, 1])' + pfxPBID + NewIndex).progressbar("value", p);
            },
            'onUploadError': function (file, errorCode, errorMsg, errorString) {
                FUComponent.uploadify('disable', false);
                alert('The file ' + file.name + ' could not be uploaded: ' + errorString);
            },
            'onSWFReady': function () {
                if (FUFileList.attr("InitDisable") == "__InitDisable") {
                    FUComponent.uploadify('disable', true);
                    FUFileList.removeAttr('InitDisable');
                }
                else {
                    if (MaxNumberOfFiles > 0)
                        if ($('#' + FileListTable_id + ' tr').length >= MaxNumberOfFiles)
                            FUComponent.uploadify('disable', true);
                }
            }
        }

        if (ButtonText.length > 0)
            InitOptions = $.extend(InitOptions, { 'buttonText': ButtonText });

        FUComponent.uploadify(InitOptions);
    }
});