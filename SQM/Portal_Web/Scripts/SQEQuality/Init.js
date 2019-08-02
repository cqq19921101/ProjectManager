$(function () {
    //Toolbar Buttons
    $("#btnSearch").button({
        label: "Search",
        icons: { primary: "ui-icon-search" }
    });
    //$("#btnCreate").button({
    //    label: "Create",
    //    icons: { primary: "ui-icon-plus" }
    //});
    //$("#btnViewEdit").button({
    //    label: "View/Edit",
    //    icons: { primary: "ui-icon-pencil" }
    //});
    //$("#btnDelete").button({
    //    label: "Delete",
    //    icons: { primary: "ui-icon-trash" }
    //});

    //取消focus時的虛線框
    $("button").focus(function () { this.blur(); });
    $(':radio').focus(function () { this.blur(); });

    //Data List
    var cn = ['ReportSID',
              '報告單號',
              '供應商',
    '出貨日', '送貨數量', '製造商料號', '料號', '生產批號', '生產日期', '檢驗日', '製作地', '批次', '是否變更製程', '變更說明', '設備', '原料', '評管員', '核准', '檢驗結果', '附件'];
    var cm = [
            { name: 'ReportSID', index: 'SubFuncGUID', width: 200, sortable: false, hidden: true },
            { name: 'ReportNo', index: 'ReportNo', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Supplier', index: 'Supplier', width: 150, sortable: true, sorttype: 'text' },
            { name: 'DeliveryDate', index: 'DeliveryDate', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Qty', index: 'Qty', width: 150, sortable: true, sorttype: 'text' },
            { name: 'SupplierNo', index: 'SupplierNo', width: 150, sortable: true, sorttype: 'text' },
            { name: 'LiteNo', index: 'LiteNo', width: 150, sortable: true, sorttype: 'text' },
            { name: 'LotNo', index: 'LotNo', width: 150, sortable: true, sorttype: 'text' },
            { name: 'DateCode', index: 'DateCode', width: 150, sortable: true, sorttype: 'text' },
            { name: 'OQCDate', index: 'OQCDate', width: 150, sortable: true, sorttype: 'date', formatter: "date", formatoptions: { newformat: "Y/m/d" } },
            { name: 'MFGLocation', index: 'MFGLocation', width: 150, sortable: true, sorttype: 'text' },
            { name: 'SupplierRemark', index: 'SupplierRemark', width: 150, sortable: true, sorttype: 'text' },
            { name: 'isChange', index: 'isChange', width: 150, sortable: true, sorttype: 'text' },
            { name: 'ChangeNote', index: 'ChangeNote', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Equipment', index: 'Equipment', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Material', index: 'Material', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Inspector', index: 'Inspector', width: 150, sortable: true, sorttype: 'text' },
            { name: 'Approveder', index: 'Approveder', width: 150, sortable: true, sorttype: 'text' },
            {
                name: '', index: '', width: 150, sortable: true, sorttype: 'text', classes: "jqGridColumnDataAsLinkWithBlue", formatter: function (cellvalue, options, rowObject) {
                    return "點擊查看";
                }
            },
            { name: '', index: '', width: 150, sortable: true, sorttype: 'text', classes: "jqGridColumnDataAsLinkWithBlue", formatter: function (cellvalue, options, rowObject) { return "點擊查看"; } }

    ];


    var gridDataList = $("#gridDataList");
    gridDataList.jqGrid({
        url: __WebAppPathPrefix + '/SQEQuality/LoadQualityJSonWithFilter',
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
        colNames: cn,
        colModel: cm,
        rowNum: 10,
        //rowList: [10, 20, 30],
        sortname: 'ReportNo',
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
        },
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            $this = $(this);
            if (iCol == 18) {

                $("#quality").hide();
                $("#inspInsp").show();
                $("#tbMain1Insp").show();

                var gridDataListInsp = $("#gridDataListInsp");
                gridDataListInsp.attr("ReportSID", $this.jqGrid('getCell', rowid, 'ReportSID'));
                gridDataListInsp.jqGrid('setGridParam', { postData: { ReportSID: $this.jqGrid('getCell', rowid, 'ReportSID') } });
                gridDataListInsp.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
            }
            if (iCol == 19) {

                $("#quality").hide();
                $("#inspFile").show();
                $("#tbMain1File").show();

                var gridDataListFile = $("#gridDataListFile");
                gridDataListFile.attr("ReportSID", $this.jqGrid('getCell', rowid, 'ReportSID'));
                gridDataListFile.jqGrid('setGridParam', { postData: { ReportSID: $this.jqGrid('getCell', rowid, 'ReportSID') } });
                gridDataListFile.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
            }
        }
    });
    gridDataList.jqGrid('navGrid', '#gridListPager', { edit: false, add: false, del: false, search: false, refresh: false });


    $('#tbMain1').show();
    $('#dialogData').show();
    $("#inspInsp").hide();
    $("#inspFile").hide();
});
