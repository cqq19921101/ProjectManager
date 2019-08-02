$(function () {
    $("#btnSearchFile").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCreateFile").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnViewEditFile").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnDeleteFile").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });
    $("#btnBackFile").button({
        label: "Back",
        icons: { primary: "ui-icon-pencil" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    var gridDataListFile = $("#gridDataListFile");
    gridDataListFile.jqGrid({
        url: __WebAppPathPrefix + '/SQMReliability/LoadQualityFileJSonWithFilter',
        postData: {
            SearchText: ""
            , ReportSID: ""
        },
        type: "post",
        datatype: "json",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        width: "auto",
        height: "auto",
        colNames: [
                    'ReportSID'
                    , '文件編號'
                    , '文件名稱'
                    , '文件結果'
                    , 'FGuid'
        ],
        colModel: [
            { name: 'ReportSID', index: 'ReportSID', width: 100, sorttype: 'text', hidden: true },
            { name: 'DocNo', index: 'DocNo', width: 150, sortable: true, sorttype: 'text' },
            { name: 'DocName', index: 'DocName', width: 150, sortable: true, sorttype: 'text',formatter: fileLink },
            { name: 'DocInspResult', index: 'DocFileResult', width: 150, sortable: true, sorttype: 'text' },
            { name: 'FGuid', index: 'FGuid', width: 100, sorttype: 'text', hidden: true },
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'Name',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridListPagerFile',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataListFile.jqGrid('navGrid', '#gridListPagerFile', { edit: false, add: false, del: false, search: false, refresh: false });

})
function fileLink(cellValue, options, rowdata, action) {
    if (rowdata.DocName == null) {
        return "None";
    } else {
        return "<a href='" + __WebAppPathPrefix + "/SQMBasic/DownloadSQMFile?DataKey=" + rowdata.DocNo + "' style='color: blue;'>" + rowdata.DocName + "</a>";
    }
}