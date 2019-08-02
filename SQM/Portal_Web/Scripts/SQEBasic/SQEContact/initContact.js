$(function () {
    var gridDataList = $("#ContactgridDataList");
    //Toolbar Buttons
    $("#btnShowContact").button({
        label: "btnShowContact",
        icons: { primary: "ui-icon-search" }
    });
    $("#btnCombSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    //ReliInfo


    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQMBasic/LoadReliInfo',
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
        pager: '#ContactgridListPager',
        //sort by reload
        loadComplete: function () {
            var $this = $(this);
            if ($this.jqGrid('getGridParam', 'datatype') === 'json')
                if ($this.jqGrid('getGridParam', 'sortname') !== '')
                    setTimeout(function () { $this.triggerHandler('reloadGrid'); }, 50);
        },
        //送簽、查看
        onSelectRow: function (id) {

            var $this = $(this);
            var selRow = $this.jqGrid('getGridParam', 'selrow');

            if (selRow) {
                var rowData = $this.jqGrid('getRowData', selRow);

            }
        }
    });

    gridDataList.jqGrid('navGrid', '#ContactgridListPager', { edit: false, add: false, del: false, search: false, refresh: false });

    //plantname ddl
    $.ajax({
        url: __WebAppPathPrefix + '/SQMBasic/GetPlantList',
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

    $("#ContacttbMain1").show();
    $("#ContactgridDataList").show();
    $("#ContactInfo").hide()

}
);
