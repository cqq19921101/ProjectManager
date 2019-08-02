$(function () {
    //Toolbar Buttons
    $("#btnSearchMap").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCreateMap").button({
        label: "Create",
        icons: { primary: "ui-icon-plus" }
    });
    $("#btnViewEditMap").button({
        label: "View/Edit",
        icons: { primary: "ui-icon-pencil" }
    });
    $("#btnDeleteMap").button({
        label: "Delete",
        icons: { primary: "ui-icon-trash" }
    });
    $("#btnBack").button({
        label: "Back",
        icons: { primary: "ui-icon-pencil" }
    });

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });
    //Data List
    var gridDataListMap = $("#gridDataListMap");
    gridDataListMap.jqGrid({
        url: __WebAppPathPrefix + '/SQMBasic/LoadAQLPlantMap',
        postData: {
            SearchText: ""
            , SSID: ""
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
            '樣本數',
            '抽樣等級',
            '抽樣數',
            'Critical數量',
            'Major數量',
            'Minor數量'
        ],
        colModel: [
            { name: 'SID', index: 'SID', width: 100, sorttype: 'text', hidden: true },
            { name: 'SSID', index: 'SSID', width: 150, sortable: true, sorttype: 'text', hidden: true },
            { name: 'AQLNum', index: 'AQLNum', width: 150, sortable: true, sorttype: 'text' },
            { name: 'AQLType', index: 'AQLType', width: 150, sortable: true, sorttype: 'text', formatter: AQLTypeTrans },
            { name: 'AQL', index: 'AQL', width: 150, sortable: true, sorttype: 'text' },
            { name: 'CR', index: 'CR', width: 150, sortable: true, sorttype: 'text' },
            { name: 'MA', index: 'MA', width: 150, sortable: true, sorttype: 'text' },
            { name: 'MI', index: 'MI', width: 150, sortable: true, sorttype: 'text' },
        ],
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'SID',
        viewrecords: true,
        loadonce: true,
        mtype: 'POST',
        pager: '#gridListPagerMap',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        }
    });
    gridDataListMap.jqGrid('navGrid', '#gridListPagerMap', { edit: false, add: false, del: false, search: false, refresh: false });

    //$('#tbMain1Code').show();
    //$('#dialogDataCode').show();

})
function AQLTypeTrans(cellValue, options, rowdata, action) {
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