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
    $("#btnCombSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
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
        url: __WebAppPathPrefix + '/SQEReliability/LoadReliInfo',
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
        width: 500,
        height: "auto",
        colNames: [
            //'VendorCode',
            '部門代碼',
                   '部門名稱',
                    //'PlantCode',
                    '供應商名稱',
                   //'SourcerGUID',
                   //'SourcerName',
                   'MemberGUID',
                   '供應商經理'
				  //'RDGUID',
				  // 'RDNAME',
                  // 'RDSGUID',
                  //  'RDSNAME'
        ],
        colModel: [
            //{ name: 'VendorCode', index: 'VendorCode', width: 200, sortable: false },
             { name: 'Plant', index: 'Plant', width: 200, sortable: true, sorttype: 'text' },
            { name: 'plant_name', index: 'plant_name', width: 200, sortable: true, sorttype: 'text' },
            //{ name: 'PlantCode', index: 'PlantCode', width: 150, sortable: true, sorttype: 'text', hidden: true },
            //{ name: 'SourcerGUID', index: 'SourcerGUID', width: 150, sortable: true, sorttype: 'text', hidden: true },
            { name: 'ERP_VNAME', index: 'ERP_VNAME', width: 250, sortable: false },
            //{ name: 'SourcerName', index: 'SourcerName', width: 150, sortable: true, sorttype: 'text' },
            { name: 'MemberGUID', index: 'MemberGUID', width: 150, sortable: true, sorttype: 'text', hidden: true },
            { name: 'NameInChinese', index: 'NameInChinese', width: 150, sortable: true, sorttype: 'text' },
            //{ name: 'RDGUID', index: 'RDGUID', width: 150, sortable: true, sorttype: 'text', hidden: true },
            //{ name: 'RDNAME', index: 'RDNAME', width: 150, sortable: true, sorttype: 'text' },
            //{ name: 'RDSGUID', index: 'RDSGUID', width: 150, sortable: true, sorttype: 'text', hidden: true },
            //{ name: 'RDSNAME', index: 'RDSNAME', width: 150, sortable: true, sorttype: 'text' },

        ],

        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'MemberGUID',
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
        },
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

                if (rowData.Status == "None" || rowData.Status == "Reject") {
                    $("#btnCommitReliability").show();
                    $("#btnUploadInfo").hide();
                } else if (rowData.Status == "Pending") {
                    $("#btnCommitReliability").hide();
                    $("#btnUploadInfo").show();
                } else if (rowData.Status == "Finished") {
                    $("#btnCommitReliability").hide();
                    $("#btnUploadInfo").hide();
                }

                if (rowData.Status == "Finished" && rowData.FileStatus == "Finished") {
                    $('#btnCommitFile').show();
                }
            }
        }
    });

    gridDataList.jqGrid('navGrid', '#ReliabilitygridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    //plantname ddl
    $.ajax({
        url: __WebAppPathPrefix + '/SQEReliability/GetPlantList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '<option value="">-- 請選擇 --</option>';
            for (var idx in data) {
                options += '<option value=' + data[idx].PlantCode + '>' + data[idx].PlantCode + ' ' + data[idx].PlantName + '</option>';
            }
            $('#ddlPlant').append(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
    //$("#ddlPlant option:first").prop("selected", 'selected');

    $('#ddlCategory').change(function () {
        $.ajax({
            url: __WebAppPathPrefix + '/SQMBasic/GetCommoditySubList',
            data: { MainID: $('#ddlCategory').val() },
            type: "post",
            dataType: 'json',
            async: false, // if need page refresh, please remark this option
            success: function (data) {
                var options = '';
                for (var idx in data) {
                    options += '<option value=' + data[idx].CID + '>' + data[idx].CID + ' ' + data[idx].CNAME + '</option>';
                }
                $('#ddlCategorySub').html(options);
            },
            error: function (xhr, textStatus, thrownError) {
                $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    });
    //材料一級菜單
    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetCommodityList',// url: __WebAppPathPrefix + '/VMIBulletin/GetBulletinCategoryList',
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].CID + '>' + data[idx].CID + ' ' + data[idx].CNAME + '</option>';
            }
            $('#ddlCategory').append(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });

    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetCommoditySubList',
        data: { MainID: ($('#ddlCategory').val() == "") ? $("#ddlCategory option:first").prop("selected", 'selected') : $('#ddlCategory').val() },
        type: "post",
        dataType: 'json',
        async: false, // if need page refresh, please remark this option
        success: function (data) {
            var options = '';
            for (var idx in data) {
                options += '<option value=' + data[idx].CID + '>' + data[idx].CID + ' ' + data[idx].CNAME + '</option>';
            }
            $('#ddlCategorySub').html(options);
        },
        error: function (xhr, textStatus, thrownError) {
            $.CommonUIUtility_AjaxErrorHandler(xhr, textStatus, thrownError, __UrlForTimeOut);
        },
        complete: function (jqXHR, textStatus) {
        }
    });


    $('#ReliabilitytbMain1').show();
    $('#MapdialogData').show();
    $('#ReliabilitydialogData').show();

    //$("#btnReliabilityViewEdit").hide();
    //$("#btnShowReliability").hide();
    $("#btnUploadInfo").hide();
    $("#btnCommitReliability").hide();
    $('#btnCommitFile').hide();
    //$('#InfodialogData').show();
    $('#SearchReliInfo').hide();
    $("#ReliInfo").hide();

    var gridDataListInfo = $("#ReliInfogridDataList");
    gridDataListInfo.jqGrid({
        url: __WebAppPathPrefix + '/SQEReliability/LoadReliability',
        postData: { MemberGUID: "" },
        type: "post",
        datatype: "json",
        jsonReader: {
            root: "Rows",
            page: "Page",
            total: "Total",
            records: "Records",
            repeatitems: false
        },
        width: 1000,
        height: "auto",
        colNames: ['ReliabititySID'
                    , 'MemberGUID'
                    , 'FGUID'
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
                    //, '簽核狀態'
                    //, '文件簽核狀態'

        ],
        colModel: [
                { name: 'ReliabititySID', index: 'ReliabititySID', width: 200, sortable: false, hidden: true },
                { name: 'MemberGUID', index: 'MemberGUID', width: 200, sortable: false, hidden: true },
                { name: 'FGUID', index: 'FGUID', width: 200, sortable: false, hidden: true },
                { name: 'TB_SQM_CommodityCID', index: 'TB_SQM_CommodityCID', width: 50, sorttype: 'text', hidden: true },
                { name: 'CommodityName', index: 'CommodityName', width: 150, sortable: true, sorttype: 'text' },
                { name: 'TB_SQM_Commodity_SubCID', index: 'TB_SQM_Commodity_SubCID', width: 50, sorttype: 'text', hidden: true },

                { name: 'Commodity_SubName', index: 'Commodity_SubName', width: 150, sortable: true, sorttype: 'text' },
                { name: 'TestProjet', index: 'TestProjet', width: 200, sortable: true, sorttype: 'text', classes: "jqGridColumnDataAsLinkWithBlue" },
                { name: 'PlannedTestTime', index: 'PlannedTestTime', width: 150, sortable: true, sorttype: 'date', formatter: "date", formatoptions: { newformat: "Y/m/d" } },
                { name: 'ActualTestTime', index: 'ActualTestTime', width: 150, sortable: true, sorttype: 'date', formatter: "date", formatoptions: { newformat: "Y/m/d" } },
                { name: 'TestResult', index: 'TestResult', width: 150, sortable: true, sorttype: 'text' },

                { name: 'TestPeople', index: 'TestPeople', width: 100, sortable: true, sorttype: 'text' },
                { name: 'Note', index: 'Note', width: 100, sortable: true, sorttype: 'text' },
                { name: 'insertime', index: 'insertime', width: 250, sortable: true, sorttype: 'date', formatter: "date", formatoptions: { srcformat: "Y/m/d H:i:s", newformat: "Y/m/d H:i:s" } },
                { name: 'FileName', index: 'FileName', width: 200, sortable: true, sorttype: 'text', formatter: fileLink }
                //{ name: 'Status', index: 'Status', width: 150, sortable: true, sorttype: 'text' },
                //{ name: 'FileStatus', index: 'FileStatus', width: 150, sortable: true, sorttype: 'text' }
        ],
        rowNum: 20,
        //rowList: [10, 20, 30],
        sortname: 'ReliabilitySID',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#ReliInfogridListPager',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
            //var rowcount=parseInt($this.getGridParam("records"));
            //if (rowcount<0) {
            //    $("btnReliInfoCreate").css("display", "block")
            //} else {
            //    $("btnReliInfoCreate").css("display", "none")
            //}
        },
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            $this = $(this);
            if (iCol == 7) {
                var historydialog = $("#ReliabilityHistoryDialogData");
                var gridDataListHistory = $("#ReliabilityHistorygridDataList");
                gridDataListHistory.jqGrid('clearGridData');
                gridDataListHistory.jqGrid('setGridParam', { postData: { SID: $this.jqGrid('getCell', rowid, 'ReliabititySID') } });
                gridDataListHistory.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                historydialog.dialog("option", "title", "History").dialog("open");

                // Remove auto focus
                $('.ui-dialog :button').blur();
                $.ui.dialog.prototype._keepFocus = $.noop
            }
        },
        onSelectRow: function (id) {
            var $this = $(this);
            var selRow = $this.jqGrid('getGridParam', 'selrow');

            //if (selRow) {
            //    var rowData = $this.jqGrid('getRowData', selRow);
            //    //$('#ihTB_SQM_Vendor_TypeCID').val(rowData.TB_SQM_Vendor_TypeCID);

            //    if (rowData) {
            //        $("#btnUploadInfo").show();
            //    } else {
            //        $("#btnUploadInfo").hide();
            //    }
            //    //if (rowData.ReliabilitycaseID) {
            //    //    $("#btnReliabilityViewEdit").show();
            //    //    $("#btnShowReliability").show();
            //    //} else {
            //    //    $("#btnReliabilityViewEdit").hide();
            //    //    $("#btnShowReliability").show();
            //    //}
            //}
        }
    });
    gridDataListInfo.jqGrid('navGrid', '#ReliInfogridListPager', { edit: false, add: false, del: false, search: false, refresh: false });
}
);

function InitReli() {

    $('#Reliability').hide();
    $('#ReliInfo').show();
    $('#ReliInfotbMain1').show();
    $('#ReliabilitydialogData').show();
    $('#SearchReliInfo').show();

}

function fileLink(cellValue, options, rowdata, action) {
    if (rowdata.FileName == null) {
        return "None";
    } else {
        return "<a href='/VMIP2/SQMBasic/DownloadSQMFile?DataKey=" + rowdata.FGUID + "' style='color: blue;'>" + rowdata.FileName + "</a>";
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
//    gridDataListHistory.jqGrid('setGridParam', { postData: { SID: sid } })
//    gridDataListHistory.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
//    historydialog.dialog("option", "title", "History").dialog("open");
//    //todo : filehistory dialog
//}