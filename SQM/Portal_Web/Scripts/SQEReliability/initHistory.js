$(function () {
    $("#ReliabilityHistoryDialogData").dialog({
        autoOpen: false,
        height: 500,
        width: 1050,
        resizable: false,
        modal: true,
        buttons: {
            OK: function () {
                $(this).dialog("close");
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });
    var gridDataListHistory = $("#ReliabilityHistorygridDataList");
    gridDataListHistory.jqGrid({
        url: __WebAppPathPrefix + '/SQMReliability/LoadReliabilityHistory',
        postData: { SID: "" },
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
        colNames: ['SID'
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
        ],
        colModel: [
                { name: 'SID', index: 'SID', width: 200, sortable: false, hidden: true },
                { name: 'FGUID', index: 'FGUID', width: 200, sortable: false, hidden: true },
                { name: 'TB_SQM_CommodityCID', index: 'TB_SQM_CommodityCID', width: 50, sorttype: 'text', hidden: true },
                { name: 'CommodityName', index: 'CommodityName', width: 100, sortable: true, sorttype: 'text' },
                { name: 'TB_SQM_Commodity_SubCID', index: 'TB_SQM_Commodity_SubCID', width: 50, sorttype: 'text', hidden: true },

                { name: 'Commodity_SubName', index: 'Commodity_SubName', width: 100, sortable: true, sorttype: 'text' },
                { name: 'TestProjet', index: 'TestProjet', width: 150, sortable: true, sorttype: 'text'},
                { name: 'PlannedTestTime', index: 'PlannedTestTime', width: 100, sortable: true, sorttype: 'date', formatter: "date", formatoptions: { newformat: "Y/m/d" } },
                { name: 'ActualTestTime', index: 'ActualTestTime', width: 100, sortable: true, sorttype: 'date', formatter: "date", formatoptions: { newformat: "Y/m/d" } },
                { name: 'TestResult', index: 'TestResult', width: 100, sortable: true, sorttype: 'text' },

                { name: 'TestPeople', index: 'TestPeople', width: 100, sortable: true, sorttype: 'text' },
                { name: 'Note', index: 'Note', width: 100, sortable: true, sorttype: 'text' },
                { name: 'insertime', index: 'insertime', width: 150, sortable: true, sorttype: 'date', formatter: "date", formatoptions: { srcformat: "Y/m/d H:i:s", newformat: "Y/m/d H:i:s" } },
                { name: 'FileName', index: 'FileName', width: 100, sortable: true, sorttype: 'text', formatter: fileLink },
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'ReliabititySID',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#ReliabilityFilegridListPager',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        },
    });

})