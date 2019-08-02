$(function () {
    var diaModifyASNAttach = $('#dialogModifyASNFileAttach');
    var diaModifyASNFileDetail = $('#dialog_VMIQuery_ModifyASNFileDetail');
    var VMI_ModifyASNFileDetailgridDataList = $('#VMI_Query_ModifyASNFileDetail_gridDataList');

    //Init Function Button
    //Button
    $('#dia_btn_VMIQuery_ModifyASNFileDetail_Add').button({
        label: "Add",
        icons: { primary: 'ui-icon-pencil' }
    });

    $('#dia_btn_VMIQuery_ModifyASNFileDetail_Delete').button({
        label: "Delete",
        icons: { primary: 'ui-icon-minus' }
    });

    $('#dia_btn_VMIQuery_ModifyASNFileDetail_ViewLogs').button({
        label: "View Logs",
        icons: { primary: 'ui-icon-search' }
    });

    //After Init. to Show Menu Function Button
    $('#dialog_VMIQuery_ModifyASNFileDetail_tbTopToolBar').show();

    //Init dialog
    diaModifyASNFileDetail.dialog({
        autoOpen: false,
        height: 500,
        width: 500,
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

    //Init JQgrid
    VMI_ModifyASNFileDetailgridDataList.jqGrid({
        url: __WebAppPathPrefix + "/VMIProcess/QueryToDoASNFileDetailInfo",
        //postData: { },
        mtype: "POST",
        datatype: "local",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        colNames: ["FileID",
                    "File Name",
                    "Update Date",
                    "Remark"],
        colModel: [
                    { name: "FS_GUID", index: "FS_GUID", width: 50, sortable: true, sorttype: "text", hidden: true },
                    {
                        name: "FS_FILENAME", index: "FS_FILENAME", width: 120, sortable: true, sorttype: "text", classes: "jqGridColumnDataAsLinkWithBlue",
                        formatter: function (cellvalue, option, rowobject) {
                            return cellvalue;
                        }
                    },
                    { name: "UPLOADDATETIME", index: "UPLOADDATETIME", width: 160, sortable: true, sorttype: "text" },
                    { name: "REMARK", index: "REMARK", width: 150, sortable: true, sorttype: "text" }

        ],
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            var $this = $(this);
            var FileName = $this.jqGrid('getCell', rowid, 'FS_FILENAME');
            var FSGUID = $this.jqGrid('getCell', rowid, 'FS_GUID');
            var REMARK = $this.jqGrid('getCell', rowid, 'REMARK');
            if (FSGUID != "") {
                if (iCol == 2) {
                    __DialogIsShownNow = false;
                    if (!__DialogIsShownNow) {
                        __DialogIsShownNow = true;
                        diaModifyASNAttach.attr("FILENAME", $.trim(FileName));
                        diaModifyASNAttach.attr("FS_GUID", $.trim(FSGUID));
                        diaModifyASNAttach.attr("REMARK", $.trim(REMARK));
                        diaModifyASNAttach.attr('Mode', 'U');
                        InitdialogModifyASNFileAttach("U");
                        diaModifyASNAttach.dialog("option", "title", "File Attach").dialog("open");
                    }
                }
            }
        },
        multiselect: true,
        shrinkToFit: false,
        scrollrows: true,
        width: 480,
        height: 280,
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: "ASNLINE",
        viewrecords: true,
        loadonce: true,
        pager: '#VMI_Query_ModifyASNFileDetail_gridListPager',
        loadComplete: function () {
            var $this = $(this);

            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '') {
                    setTimeout(function () {
                        $this.triggerHandler('reloadGrid');
                    }, 50);
                }
        }
    });

    VMI_ModifyASNFileDetailgridDataList.jqGrid('navGrid', '#VMI_Query_ModifyASNFileDetail_gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });


    $('#dia_btn_VMIQuery_ModifyASNFileDetail_Add').click(function () {
        $(this).removeClass('ui-state-focus');
        diaModifyASNAttach.attr('Mode', 'C');
        InitdialogModifyASNFileAttach('C');

        /**** Init jQuery fileupload 1 ****/
        $.init_fileupload(
            'fileupload_uploadFileAttach', // fileUpload Control ID
            1, // maxNumberOfFiles (null: nolimit)
            30000000, // maxFileSize (ex: 10000000 // 10 MB; 0: nolimit)
            null // acceptFileTypes (ex: /(\.|\/)(gif|jpe?g|png)$/i)
            );

        diaModifyASNAttach.dialog("option", "title", "Upload").dialog("open");
    });

    $('#dia_btn_VMIQuery_ModifyASNFileDetail_Delete').click(function () {
        $(this).removeClass('ui-state-focus');
        var VMI_ModifyASNFileDetailgridDataList = $('#VMI_Query_ModifyASNFileDetail_gridDataList');
        var arrSelRowID = VMI_ModifyASNFileDetailgridDataList.jqGrid("getGridParam", "selarrrow");
        var AttachGUIDLines = "";

        if (arrSelRowID.length > 0) {
            for (var i = 0 ; i < arrSelRowID.length; i++) {
                AttachGUIDLines += VMI_ModifyASNFileDetailgridDataList.jqGrid('getRowData', arrSelRowID[i]).FS_GUID + ",";
            }

            //delete success 
            if (confirm("Delete the selected lines?")) {
                DeleteSelectedModifyAttachLine(AttachGUIDLines);
            }
        }
        else {
            alert("Please select a data to delete.");
        }

    });
});

function DeleteSelectedModifyAttachLine(AttachGUIDLines) {

    var diaQueryASNManage = $('#dialog_VMIQuery_QueryASNManage');

    $.ajax({
        url: __WebAppPathPrefix + '/VMIProcess/DeleteToDoASNAttachFileInfo',
        data: {
            ASN_NUM: escape($.trim(diaQueryASNManage.prop("ASN_NUM"))),
            AttachGUIDs: escape($.trim(AttachGUIDLines))
        },
        type: "post",
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.Result == true) {
                alert(data.Message);
                ReloadModifyASNFileDetailInfogridDataList();
            }
            else {
                alert(data.Message);
            }
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
            //HideAjaxLoading();
        }
    });
}