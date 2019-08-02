$(function () {
    //Toolbar Buttons
    $("#btnReliabilitySearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnReliabilityCreate").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnReliabilityViewEdit").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnReliabilityDelete").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });
    $("#btnReliInfoCreate").button({
        label: "CreatReliability",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnShowReliability").button({
        label: "ShowReliability",
        icons: { primary: "ui-icon-search" }
    });
    //ReliInfo
    $("#btnBack").button({
        label: "Back",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnBack").click(function () {
        //var gridDataList = $("#ReliabilitygridDataList");
        //var gridDataList = $("#ReliInfogridDataList");
        //$("#btnReliabilitySearch").click();
        var gridDataList = $("#ReliInfogridDataList");
        gridDataList.jqGrid('clearGridData');
        $("#ReliInfo").hide();
        $("#Reliability").show();
    });
    $("#btnReliInfoSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnReliInfoCreate").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnReliInfoViewEdit").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnReliInfoDelete").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });
    $("#btnCreatReliInfo").button({
        label: "CreatReliInfo",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnShowReliInfo").button({
        label: "ShowReliInfo",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCommitReliability").button({
        label: "Commit",
        icons: { primary: "ui-icon-arrowthickstop-1-s" }
    });
    $("#btnCommitFile").button({
        label: "FileCommit",
        icons: { primary: "ui-icon-arrowthickstop-1-s" }
    });
    $("#btnUploadInfo").button({
        label: "Upload",
        icons: { primary: "ui-icon-pencil" }
    });
    $('#ddlReliabilityUnitD').change()
    $('#ddlItemNOD').change()


    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var gridDataList = $("#ReliabilitygridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMReliability/LoadReliability',
        postData: { SearchText: "" },
        type: "post",
        datatype: "json",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        width: 1300,
        height: "auto",
        colNames: [
                     'ReliabititySID'
                    , 'MemberGUID'
                    , 'FGUID'
                    , 'FSID'
                    , 'TB_SQM_CommodityCID'
                    , '材料類型'
                    , 'TB_SQM_Commodity_SubCID'

                    , '材料子類型'
                    , '可靠度性實驗項目'
                    , '計劃實驗時間'
                    , '實際實驗時間'
                    , '實際實驗結果'

                    , '檢測人員'
                    , '備註'
                    , '編輯時間'
                    , '附件'
                    , '簽核狀態'
                    , '簽核備註'
                    , '文件簽核狀態'
                    , '文件簽核備註'
        ],
        colModel: [
                { name: 'ReliabititySID', index: 'ReliabititySID', width: 200, sortable: false, hidden: true },
                { name: 'MemberGUID', index: 'MemberGUID', width: 200, sortable: false, hidden: true },
                { name: 'FGUID', index: 'FGUID', width: 200, sortable: false, hidden: true },
                { name: 'FSID', index: 'FSID', width: 200, sortable: false, hidden: true },
                { name: 'TB_SQM_CommodityCID', index: 'TB_SQM_CommodityCID', width: 50, sorttype: 'text', hidden: true },
                { name: 'CommodityName', index: 'CommodityName', width: 100, sortable: true, sorttype: 'text' },
                { name: 'TB_SQM_Commodity_SubCID', index: 'TB_SQM_Commodity_SubCID', width: 50, sorttype: 'text', hidden: true },

                { name: 'Commodity_SubName', index: 'Commodity_SubName', width: 100, sortable: true, sorttype: 'text' },
                { name: 'TestProjet', index: 'TestProjet', width: 150, sortable: true, sorttype: 'text'},// classes: "jqGridColumnDataAsLinkWithBlue"
                { name: 'PlannedTestTime', index: 'PlannedTestTime', width: 100, sortable: true, sorttype: 'date', formatter: "date", formatoptions: { newformat: "Y/m/d" } },
                { name: 'ActualTestTime', index: 'ActualTestTime', width: 100, sortable: true, sorttype: 'date', formatter: "date", formatoptions: { newformat: "Y/m/d" } },
                { name: 'TestResult', index: 'TestResult', width: 100, sortable: true, sorttype: 'text' },

                { name: 'TestPeople', index: 'TestPeople', width: 100, sortable: true, sorttype: 'text' },
                { name: 'Note', index: 'Note', width: 100, sortable: true, sorttype: 'text' },
                { name: 'insertime', index: 'insertime', width: 150, sortable: true, sorttype: 'date', formatter: "date", formatoptions: {srcformat:"Y/m/d H:i:s", newformat: "Y/m/d H:i:s" } },
                { name: 'FileName', index: 'FileName', width: 100, sortable: true, sorttype: 'text' ,formatter: fileLink},
                { name: 'Status', index: 'Status', width: 100, sortable: true, sorttype: 'text' },
                { name: 'StateNote', index: 'StateNote', width: 100, sortable: true, sorttype: 'text'},
                { name: 'FileStatus', index: 'FileStatus', width: 100, sortable: true, sorttype: 'text'},
                { name: 'FileStateNote', index: 'FileStateNote', width: 100, sortable: true, sorttype: 'text'},

        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'ReliabititySID',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#ReliabilitygridListPager',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
            var status = $this.jqGrid('getRowData', '1').Status;
            var filestatus = $this.jqGrid('getRowData', '1').FileStatus;
            var note = $this.jqGrid('getRowData', '1').StateNote;
            var filenote = $this.jqGrid('getRowData', '1').FileStateNote;
            //$("#spStatus").html(typeof (status) == "undefined" ? 'None' : status);
            //$("#spFileStatus").html(typeof (filestatus) == "undefined" ? 'None' : filestatus);
            //$("#spStatusNote").html(note == "" ? 'None' : note);
            //$("#spFileStatusNote").html(filenote == "" ? 'None' : filenote);
        },
        ////onCellSelect: function (rowid, iCol, cellcontent, e) {
        ////    $this = $(this);
        ////    if (iCol == 8) {
        ////        var historydialog = $("#ReliabilityHistoryDialogData");
        ////        var gridDataListHistory = $("#ReliabilityHistorygridDataList");
        ////        gridDataListHistory.jqGrid('clearGridData');
        ////        gridDataListHistory.jqGrid('setGridParam', { postData: { SID: $this.jqGrid('getCell', rowid, 'ReliabititySID') } });
        ////        gridDataListHistory.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
        ////        historydialog.dialog("option", "title", "History").dialog("open");

        ////        // Remove auto focus
        ////        $('.ui-dialog :button').blur();
        ////        $.ui.dialog.prototype._keepFocus=$.noop
        ////    }
        ////},
        //送簽、查看
        onSelectRow: function (id) {
            //var $this = $(this);
            //var selRow = $this.jqGrid('getGridParam', 'selrow');

            //if (selRow) {
            //    var rowData = $this.jqGrid('getRowData', selRow);
                
            //    if (rowData) {
            //        $("#btnCommitReliability").show();
            //    } else {
            //        $("#btnCommitReliability").hide();
            //    }
            //    //if (rowData.ReliabilitycaseID) {
            //    //    $("#btnReliabilityViewEdit").show();
            //    //    $("#btnShowReliability").show();
            //    //} else {
            //    //    $("#btnReliabilityViewEdit").hide();
            //    //    $("#btnShowReliability").show();
            //    //}
            //}

            var $this = $(this);
            var selRow = $this.jqGrid('getGridParam', 'selrow');

            if (selRow) {
                var rowData = $this.jqGrid('getRowData', selRow);

                //if (rowData.Status == "None" || rowData.Status == "Reject") {
                //    //$("#btnCommitReliability").show();
                //    $("#btnUploadInfo").hide();
                //} else if (rowData.Status == "Pending") {
                //    //$("#btnCommitReliability").hide();
                //    $("#btnUploadInfo").hide();
                //} else if (rowData.Status == "Finished") {
                //    //$("#btnCommitReliability").hide();
                //    $("#btnUploadInfo").show();
                //}
                //if (rowData.Status == "None") {
                //    $("#btnCommitReliability").show();
                //} else {
                //    $("#btnCommitReliability").hide();
                //}
                //if (rowData.Status == "Pending") {
                //    $('#btnCommitFile').show();
                //    $("#btnCommitReliability").hide();
                //}

                //if (rowData.Status == "Finished" && rowData.FileStatus != "Finished") {
                //    $('#btnCommitFile').show();
                //} else {
                //    $('#btnCommitFile').hide();
                //}
                //if (rowData.Status == "Finished" ) {
                //    $("#btnUploadInfo").show();
                //} else {
                //    $("#btnUploadInfo").hide();
                //}
            
                if (rowData.Status == "None" || rowData.Status == "Reject") {
                    $("#btnCommitReliability").show();
                    $("#btnReliabilityViewEdit").show();
                    $("#btnReliabilityDelete").show();
                } else if (rowData.Status == "Pending") {
                    $("#btnCommitReliability").show();
                    $("#btnReliabilityViewEdit").hide();
                    $("#btnReliabilityDelete").hide();
                }
                else {
                    $("#btnCommitReliability").hide();
                    $("#btnReliabilityViewEdit").hide();
                    $("#btnReliabilityDelete").hide();
                }
                if (rowData.Status == "approve" && rowData.FileStatus != "Pending") {
                    $("#btnUploadInfo").show();
                    $("#btnCommitFile").show();
                } else {
                    $("#btnUploadInfo").hide();
                    $("#btnCommitFile").hide();
                }
                if (rowData.TestResult == "") {
                    $("#fileUploadPart").hide();
                } else {
                    $("#fileUploadPart").show(); 
                }
            }
        }
    });

    gridDataList.jqGrid('navGrid', '#ReliabilitygridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#ReliabilitytbMain1').show();
    $('#MapdialogData').show();
    $('#ReliabilitydialogData').show();

    //$("#btnReliabilityViewEdit").hide();
    //$("#btnShowReliability").hide();
    $("#btnUploadInfo").hide();
    $("#btnCommitReliability").show();
    $('#btnCommitFile').hide();
    //$('#InfodialogData').show();
    $('#SearchReliInfo').hide();

}
);
function fileLink(cellValue, options, rowdata, action) {
    if (rowdata.FileName == null) {
        return "None";
    } else {
        return "<a href='" + __WebAppPathPrefix + "/SQMBasic/DownloadSQMFile?DataKey=" + rowdata.FGUID + "' style='color: blue;'>" + rowdata.FileName + "</a>";
    }
}
//function projLink(cellValue, options, rowdata, action) {
//    return "<a onclick='loadFileHistory(this.id)' id='" + rowdata.ReliabititySID + "' href='javascript:;' style='color: blue;'>" + rowdata.TestProjet + "</a>";
//}
//function loadFileHistory(sid) {
//    //alert(sid);
//    var historydialog = $("#ReliabilityHistoryDialogData");
//    var gridDataListHistory = $("#ReliabilityHistorygridDataList");
//    gridDataListHistory.jqGrid('clearGridData');
//    gridDataListHistory.jqGrid('setGridParam', { postData: { SID:sid} })
//    gridDataListHistory.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
//    historydialog.dialog("option", "title", "History").dialog("open");
//    //todo : filehistory dialog
//}