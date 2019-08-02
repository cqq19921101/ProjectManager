$(function () {
    //Toolbar Buttons
    $("#btnSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCreate").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnViewEdit").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnDelete").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });
    $("#btnDisbaled").button({
        label: "Disable/Enable",
        icons: { primary: "ui-icon-trash" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });
    //Data List
    var gridDataListCode = $("#gridDataList");
    gridDataListCode.jqGrid({
        url: __WebAppPathPrefix + '/SQMBasic/LoadAQLPlantRule',
        postData: {
            SearchText: "",
            IsShow:""
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
            'SID',
            'SSID',
            '抽樣計畫',
            'SSSID',
            '等級',
            '檢驗批數',
            '批退數',
            '允收數',
            '转换后等级',
            'PlantSID',
            '轉換後抽樣計畫',
            'IsShow'
        ],
        colModel: [
            { name: 'SID', index: 'SID', width: 100, sorttype: 'text', hidden: true },
            { name: 'SSID', index: 'SSID', width: 100, sortable: true, sorttype: 'text', hidden: true },
            { name: 'PlantName', index: 'PlantName', width: 100, sortable: true, sorttype: 'text' },
            { name: 'SSSID', index: 'SSSID', width: 100, sortable: true, sorttype: 'text', hidden: true },
            { name: 'AQLType', index: 'AQLType', width: 100, sortable: true, sorttype: 'text',formatter:MapAQLType},
            { name: 'CheckNum', index: 'CheckNum', width: 100, sortable: true, sorttype: 'text' },
            { name: 'RetreatingNum', index: 'RetreatingNum', width: 100, sortable: true, sorttype: 'text' },
            { name: 'AcceptanceNum', index: 'AcceptanceNum', width: 100, sortable: true, sorttype: 'text' },
            { name: 'AfterAQLType', index: 'AfterAQLType', width: 100, sortable: true, sorttype: 'text', formatter: AQLTypeTrans },
            { name: 'PlantSID', index: 'PlantSID', width: 100, sortable: true, sorttype: 'text', hidden: true },
            { name: 'AfterPlantName', index: 'AfterPlantName', width: 100, sortable: true, sorttype: 'text' },
            { name: 'IsShow', index: 'IsShow', width: 100, sorttype: 'text', hidden: true },
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'Name',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridListPager',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataListCode.jqGrid('navGrid', '#gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    $('#tbMain1').show();

    $('#dialogData').hide();
    $('#dialogDataMap').hide();
    $('#divMap').hide();

    

})
function AQLTypeTrans(cellValue, options, rowdata, action) {
    switch (rowdata.AfterAQLType) {
        case 0:
            return "正常";
            break;
        case 1:
            return "加嚴";
            break;
        case 2:
            return "減量";
            break;
    }
}
function MapAQLType(cellValue, options, rowdata, action) {
    var aqltype;

    switch (rowdata.AQLType) {
        case 0:
            return "正常";
            break;
        case 1:
            return "加嚴";
            break;
        case 2:
            return "減量";
            break;
    }
}